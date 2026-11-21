using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UMakCoop.Models;

namespace UMakCoop.Services
{
    /// <summary>
    /// Saves every receipt to one readable .json file and loads them back when the app starts,
    /// so transactions survive closing the app. No extra references needed (it uses the small JsonLite reader/writer).
    /// Prices, discount rate and payment are stored as they were at the time of sale (receipts are snapshots).
    /// </summary>
    public class ReceiptStore
    {
        private const int FormatVersion = 1;

        public ReceiptStore(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; private set; }

        /// <summary>%AppData%\UMakCoop\transactions.json</summary>
        public static string DefaultPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    "UMakCoop", "transactions.json");
            }
        }

        // ---------------------------------------------------------------- save

        public void Save(IEnumerable<Receipt> receipts)
        {
            var file = new JsonObject()
                .Add("version", FormatVersion)
                .Add("receipts", receipts.Select(r => (object)ToJson(r)).ToList());

            string folder = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(folder)) Directory.CreateDirectory(folder);

            // Write to a temp file first, then copy over the real one, so a crash mid-save cannot wipe the old data.
            string temp = FilePath + ".tmp";
            File.WriteAllText(temp, JsonLite.Write(file), new UTF8Encoding(false));

            File.Copy(temp, FilePath, true);
            File.Delete(temp);
        }

        private static JsonObject ToJson(Receipt r)
        {
            var cash = r.Payment as CashPayment;
            var gcash = r.Payment as GCashPayment;
            return new JsonObject()
                .Add("receiptNo", r.ReceiptNo)
                .Add("issuedAt", r.IssuedAt.ToString("o", CultureInfo.InvariantCulture))
                .Add("cashier", r.CashierName)
                .Add("student", r.StudentName)
                .Add("discountName", r.Discount.Name)
                .Add("discountRate", r.Discount.Rate)
                .Add("discountRequiresId", r.Discount.RequiresIdNumber)
                .Add("discountIdLabel", r.Discount.IdLabel)
                .Add("discountIdNumber", r.DiscountIdNumber)
                .Add("paymentMethod", r.Payment.Method.ToString())
                .Add("cashReceived", cash != null ? cash.CashReceived : 0m)
                .Add("gcashReference", gcash != null ? gcash.ReferenceNumber : null)
                .Add("status", r.Status.ToString())
                .Add("voidedBy", r.VoidedBy)
                .Add("voidedAt", r.VoidedAt.HasValue ? r.VoidedAt.Value.ToString("o", CultureInfo.InvariantCulture) : null)
                .Add("voidReason", r.VoidReason)
                .Add("lines", r.Lines.Select(l => (object)new JsonObject()
                    .Add("productId", l.ProductId)
                    .Add("name", l.ProductName)
                    .Add("size", l.Size)
                    .Add("unitPrice", l.UnitPrice)
                    .Add("quantity", l.Quantity)).ToList());
        }

        // ---------------------------------------------------------------- load

        /// <summary>
        /// Returns the saved receipts (empty if there is no file yet). If the file is damaged it is renamed to
        /// transactions.json.corrupt-yyyyMMddHHmmss (never overwritten) and an InvalidDataException says where it went.
        /// </summary>
        public IList<Receipt> Load()
        {
            if (!File.Exists(FilePath)) return new List<Receipt>();

            try
            {
                var file = (JsonObject)JsonLite.Parse(File.ReadAllText(FilePath, Encoding.UTF8));

                var result = new List<Receipt>();
                foreach (object item in file.List("receipts")) result.Add(FromJson((JsonObject)item));
                return result;
            }
            catch (IOException)
            {
                throw;   // locked or unreadable: do not touch the file, let the caller report it
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string backup = FilePath + ".corrupt-" + DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                File.Move(FilePath, backup);
                throw new InvalidDataException("The saved transactions file was damaged (" + ex.Message +
                                               "). It was kept as " + backup + " and the list was started empty.");
            }
        }

        private static Receipt FromJson(JsonObject j)
        {
            var lines = j.List("lines").Cast<JsonObject>()
                .Select(l => new ReceiptLine(l.Int("productId"), l.Str("name"), l.Str("size"), l.Dec("unitPrice"), l.Int("quantity")))
                .ToList();
            var discount = new RecordedDiscount(j.Str("discountName"), j.Dec("discountRate"), j.Bool("discountRequiresId"), j.Str("discountIdLabel"));

            decimal subtotal = lines.Sum(l => l.LineTotal);
            decimal total = subtotal - discount.Compute(subtotal);

            Payment payment = j.Str("paymentMethod") == PaymentMethod.GCash.ToString()
                ? (Payment)new GCashPayment(total, j.Str("gcashReference"))
                : new CashPayment(total, j.Dec("cashReceived"));

            DateTime issuedAt = ParseTime(j.Str("issuedAt"));
            var receipt = new Receipt(j.Str("receiptNo"), issuedAt, j.Str("cashier"), j.Str("student"), lines, discount, j.Str("discountIdNumber"), payment);

            if (j.Str("status") == ReceiptStatus.Voided.ToString())
            {
                string at = j.Str("voidedAt");
                receipt.RestoreVoided(j.Str("voidedBy"), at != null ? ParseTime(at) : issuedAt, j.Str("voidReason"));
            }
            return receipt;
        }

        private static DateTime ParseTime(string text)
        {
            return DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        /// <summary>A discount as it was on the day of the sale, even if the rates in the app change later.</summary>
        private class RecordedDiscount : Discount
        {
            private readonly string _name, _idLabel;
            private readonly decimal _rate;
            private readonly bool _requiresId;

            public RecordedDiscount(string name, decimal rate, bool requiresId, string idLabel)
            {
                _name = name; _rate = rate; _requiresId = requiresId; _idLabel = idLabel ?? "ID no.";
            }

            public override string Name { get { return _name; } }
            public override decimal Rate { get { return _rate; } }
            public override bool RequiresIdNumber { get { return _requiresId; } }
            public override string IdLabel { get { return _idLabel; } }
        }
    }
}
