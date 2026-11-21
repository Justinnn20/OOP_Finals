using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UMakCoop.Models;
using UMakCoop.Services;

namespace UMakCoop.Controllers
{
    /// <summary>Seals sales, keeps the list of receipts, searches it, and voids.</summary>
    public class TransactionController
    {
        private readonly List<Receipt> _all = new List<Receipt>();
        private readonly IStockService _stock;
        private readonly ReceiptStore _store;
        private DateTime _sequenceDate = DateTime.MinValue;
        private int _sequence;

        /// <summary>
        /// The normal way: transactions are saved to %AppData%\UMakCoop\transactions.json and loaded again
        /// when the app starts. MainForm only needs: new TransactionController(stockService).
        /// </summary>
        public TransactionController(IStockService stock) : this(stock, new ReceiptStore(ReceiptStore.DefaultPath))
        {
        }

        /// <summary>For testing: nothing is saved, transactions are lost when the app closes.</summary>
        public static TransactionController CreateInMemory(IStockService stock)
        {
            return new TransactionController(stock, null);
        }

        /// <summary>
        /// With a store, saved receipts are loaded now and every sale / void is saved straight away.
        /// Use this overload to save somewhere other than the default file.
        /// </summary>
        public TransactionController(IStockService stock, ReceiptStore store)
        {
            _stock = stock;
            _store = store;
            if (_store != null) LoadSaved();
        }

        /// <summary>
        /// Set when loading or saving the file failed (file locked, disk full, damaged file). The sale itself still
        /// went through. Null when everything is fine. Views show it so the cashier knows.
        /// </summary>
        public string StorageWarning { get; private set; }

        public event EventHandler<Receipt> ReceiptIssued;
        public event EventHandler<Receipt> ReceiptVoided;

        public IList<Receipt> All { get { return _all.AsReadOnly(); } }

        public decimal TodaySales
        {
            get
            {
                return _all.Where(r => r.Status == ReceiptStatus.Completed && r.IssuedAt.Date == DateTime.Today)
                           .Sum(r => r.Total);
            }
        }

        public int TodayCount
        {
            get
            {
                return _all.Count(r => r.Status == ReceiptStatus.Completed && r.IssuedAt.Date == DateTime.Today);
            }
        }

        public Receipt Checkout(IList<ReceiptLine> lines, Discount discount, string discountIdNumber, Payment payment,
                                string cashierName, string studentName)
        {
            if (lines == null || lines.Count == 0)
                throw new InvalidOperationException("The order is empty.");

            if (discount.RequiresIdNumber && string.IsNullOrWhiteSpace(discountIdNumber))
                throw new InvalidOperationException("Enter the " + discount.IdLabel + " to use the " + discount.Name + " discount.");

            string error;
            if (!payment.TryValidate(out error))
                throw new InvalidOperationException(error);

            // The same GCash reference must never pay for two sales.
            var gcash = payment as GCashPayment;
            if (gcash != null && _all.Any(r => r.Status == ReceiptStatus.Completed &&
                    r.Payment is GCashPayment &&
                    ((GCashPayment)r.Payment).ReferenceNumber == gcash.ReferenceNumber))
                throw new InvalidOperationException("That GCash reference number was already used for another sale.");

            // Check every product first so a failed sale never leaves stock half-deducted.
            foreach (var group in lines.GroupBy(l => l.ProductId))
            {
                if (!_stock.HasStock(group.Key, group.Sum(l => l.Quantity)))
                    throw new InvalidOperationException("Not enough stock for " + group.First().ProductName + ".");
            }

            var receipt = new Receipt(NextReceiptNo(), DateTime.Now, cashierName, studentName, lines, discount, discountIdNumber, payment);

            foreach (var line in lines) _stock.Deduct(line.ProductId, line.Quantity);

            _all.Add(receipt);
            Persist();
            var handler = ReceiptIssued;
            if (handler != null) handler(this, receipt);
            return receipt;
        }

        public IList<Receipt> Search(string text, ReceiptStatus? status, bool todayOnly)
        {
            IEnumerable<Receipt> query = _all;

            if (!string.IsNullOrWhiteSpace(text))
            {
                string term = text.Trim();
                query = query.Where(r =>
                    r.ReceiptNo.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (r.StudentName ?? "").IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    r.CashierName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (r.DiscountIdNumber ?? "").IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (r.DiscountAmount > 0m && r.Discount.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    GCashReferenceOf(r).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (status.HasValue) query = query.Where(r => r.Status == status.Value);
            if (todayOnly) query = query.Where(r => r.IssuedAt.Date == DateTime.Today);

            return query.OrderByDescending(r => r.IssuedAt).ToList();
        }

        /// <summary>Admin only. A reason is required so every void can be explained later.</summary>
        public void Void(string receiptNo, string voidedBy, bool isAdmin, string reason)
        {
            if (!isAdmin)
                throw new UnauthorizedAccessException("Only an Admin can void a transaction.");
            if (string.IsNullOrWhiteSpace(reason))
                throw new InvalidOperationException("Enter a reason for the void.");

            var receipt = _all.FirstOrDefault(r => r.ReceiptNo == receiptNo);
            if (receipt == null)
                throw new InvalidOperationException("Receipt " + receiptNo + " was not found.");
            if (receipt.Status == ReceiptStatus.Voided)
                throw new InvalidOperationException(receiptNo + " is already voided.");

            foreach (var line in receipt.Lines) _stock.Restore(line.ProductId, line.Quantity);

            receipt.MarkVoided(voidedBy, reason);
            Persist();
            var handler = ReceiptVoided;
            if (handler != null) handler(this, receipt);
        }

        /// <summary>Totals for one date (by the date of the sale). Voided receipts are counted apart and add no sales.</summary>
        public DailySummary Summarize(DateTime date)
        {
            var day = _all.Where(r => r.IssuedAt.Date == date.Date).ToList();
            var done = day.Where(r => r.Status == ReceiptStatus.Completed).ToList();

            return new DailySummary
            {
                Date = date.Date,
                OrderCount = done.Count,
                ItemsSold = done.Sum(r => r.ItemCount),
                VoidedCount = day.Count - done.Count,
                GrossSales = done.Sum(r => r.Subtotal),
                DiscountTotal = done.Sum(r => r.DiscountAmount),
                NetSales = done.Sum(r => r.Total),
                CashSales = done.Where(r => r.Payment.Method == PaymentMethod.Cash).Sum(r => r.Total),
                GCashSales = done.Where(r => r.Payment.Method == PaymentMethod.GCash).Sum(r => r.Total),
                Discounts = done.Where(r => r.DiscountAmount > 0m)
                                .GroupBy(r => r.Discount.Name)
                                .Select(g => new DiscountSummaryLine { Name = g.Key, Count = g.Count(), Amount = g.Sum(r => r.DiscountAmount) })
                                .OrderBy(d => d.Name)
                                .ToList()
            };
        }

        private static string GCashReferenceOf(Receipt r)
        {
            var gcash = r.Payment as GCashPayment;
            return gcash != null ? gcash.ReferenceNumber : "";
        }

        private void LoadSaved()
        {
            try
            {
                _all.AddRange(_store.Load().OrderBy(r => r.IssuedAt));
                ContinueTodaysNumbering();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                StorageWarning = "Could not read the saved transactions (" + ex.Message + "). Close any program using the file and restart.";
            }
            catch (InvalidDataException ex)
            {
                StorageWarning = ex.Message;
            }
        }

        private void Persist()
        {
            if (_store == null) return;
            try
            {
                _store.Save(_all);
                StorageWarning = null;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                StorageWarning = "The change was made but could not be saved to " + _store.FilePath + " (" + ex.Message + ").";
            }
        }

        /// <summary>After a restart, today's next receipt number continues from the highest saved one.</summary>
        private void ContinueTodaysNumbering()
        {
            int highest = 0;
            foreach (Receipt r in _all.Where(x => x.IssuedAt.Date == DateTime.Today))
            {
                int dash = r.ReceiptNo.LastIndexOf('-');
                int number;
                if (dash >= 0 && int.TryParse(r.ReceiptNo.Substring(dash + 1), out number) && number > highest)
                    highest = number;
            }
            if (highest > 0)
            {
                _sequenceDate = DateTime.Today;
                _sequence = highest;
            }
        }

        private string NextReceiptNo()
        {
            DateTime today = DateTime.Today;
            if (_sequenceDate != today)
            {
                _sequenceDate = today;
                _sequence = 0;
            }
            _sequence++;
            return "UC-" + today.ToString("yyyyMMdd") + "-" + _sequence.ToString("D4");
        }
    }
}
