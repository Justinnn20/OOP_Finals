using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UMakCoop.Services;

namespace UMakCoop.Models
{
    /// <summary>
    /// The permanent record of one sale. Everything is captured at checkout, so an old receipt never changes
    /// even if prices or discounts change later. The Transactions list is a list of Receipts.
    /// </summary>
    public class Receipt
    {
        private const int Width = 40;
        private readonly List<ReceiptLine> _lines;

        public Receipt(string receiptNo, DateTime issuedAt, string cashierName, string studentName,
                       IEnumerable<ReceiptLine> lines, Discount discount, string discountIdNumber, Payment payment)
        {
            _lines = new List<ReceiptLine>(lines);
            if (_lines.Count == 0) throw new ArgumentException("A receipt needs at least one line.", "lines");

            ReceiptNo = receiptNo;
            IssuedAt = issuedAt;
            CashierName = cashierName;
            StudentName = string.IsNullOrWhiteSpace(studentName) ? null : studentName;
            Discount = discount;
            DiscountIdNumber = string.IsNullOrWhiteSpace(discountIdNumber) ? null : discountIdNumber.Trim();
            Payment = payment;

            Subtotal = _lines.Sum(l => l.LineTotal);
            DiscountAmount = discount.Compute(Subtotal);

            if (payment.AmountDue != Total)
                throw new ArgumentException("The payment amount does not match the order total.", "payment");
        }

        public string ReceiptNo { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public string CashierName { get; private set; }
        public string StudentName { get; private set; }
        public Discount Discount { get; private set; }

        /// <summary>Senior Citizen / PWD ID number. Null when no ID discount was used.</summary>
        public string DiscountIdNumber { get; private set; }
        public Payment Payment { get; private set; }

        public IList<ReceiptLine> Lines { get { return _lines.AsReadOnly(); } }
        public decimal Subtotal { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal Total { get { return Subtotal - DiscountAmount; } }
        public int ItemCount { get { return _lines.Sum(l => l.Quantity); } }

        public ReceiptStatus Status { get; private set; }
        public string VoidedBy { get; private set; }
        public DateTime? VoidedAt { get; private set; }
        public string VoidReason { get; private set; }

        internal void MarkVoided(string by, string reason)
        {
            RestoreVoided(by, DateTime.Now, reason);
        }

        /// <summary>Also used when loading saved receipts, so the original void time is kept.</summary>
        internal void RestoreVoided(string by, DateTime at, string reason)
        {
            Status = ReceiptStatus.Voided;
            VoidedBy = by;
            VoidedAt = at;
            VoidReason = reason;
        }

        /// <summary>Plain-text version for the popup and the printer. Show it in a monospaced font.</summary>
        public string ToText(bool isReprint)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Center("UNIVERSITY OF MAKATI COOP"));
            sb.AppendLine(Center("Bookstore"));
            sb.AppendLine(Center(ReceiptNo));
            sb.AppendLine(Center(IssuedAt.ToString("MMM d, yyyy h:mm tt", CultureInfo.InvariantCulture)));
            sb.AppendLine(Center("Cashier: " + CashierName));
            if (StudentName != null) sb.AppendLine(Center("Student: " + StudentName));
            if (isReprint) sb.AppendLine(Center("*** REPRINT ***"));
            sb.AppendLine(new string('-', Width));

            foreach (var line in _lines)
            {
                string size = line.Size != null ? " (" + line.Size + ")" : "";
                sb.AppendLine(Row(line.Quantity + " x " + line.ProductName + size, Money.Peso(line.LineTotal)));
                if (line.Quantity > 1) sb.AppendLine("    @ " + Money.Peso(line.UnitPrice));
            }

            sb.AppendLine(new string('-', Width));
            sb.AppendLine(Row("Subtotal", Money.Peso(Subtotal)));
            if (DiscountAmount > 0m) sb.AppendLine(Row("Discount (" + Discount + ")", "-" + Money.Peso(DiscountAmount)));
            if (DiscountAmount > 0m && DiscountIdNumber != null) sb.AppendLine(Row(Discount.IdLabel, DiscountIdNumber));
            sb.AppendLine(Row("TOTAL", Money.Peso(Total)));
            sb.AppendLine(Row("Paid via", Payment.Method.ToString()));
            sb.AppendLine(Payment.ReceiptLine);
            if (Payment.Method == PaymentMethod.Cash) sb.AppendLine(Row("Change", Money.Peso(Payment.Change)));
            sb.AppendLine(new string('-', Width));

            if (Status == ReceiptStatus.Voided)
            {
                sb.AppendLine(Center("*** VOIDED ***"));
                sb.AppendLine(Center("by " + VoidedBy));
                sb.AppendLine(Center("Reason: " + VoidReason));
            }
            else
            {
                sb.AppendLine(Center("Thank you! Keep this receipt"));
                sb.AppendLine(Center("for exchanges."));
            }
            return sb.ToString();
        }

        private static string Row(string left, string right)
        {
            int room = Width - right.Length - 1;
            if (left.Length > room) left = left.Substring(0, Math.Max(0, room));
            return left + new string(' ', Math.Max(1, Width - left.Length - right.Length)) + right;
        }

        private static string Center(string text)
        {
            return text.Length >= Width ? text : new string(' ', (Width - text.Length) / 2) + text;
        }
    }
}
