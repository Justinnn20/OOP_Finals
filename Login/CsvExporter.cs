using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UMakCoop.Models;

namespace UMakCoop.Services
{
    /// <summary>Writes receipts to a .csv that opens cleanly in Excel (one row per receipt).</summary>
    public static class CsvExporter
    {
        private static readonly string[] Header =
        {
            "Receipt No", "Date", "Time", "Cashier", "Student", "Items", "Item details",
            "Subtotal", "Discount", "Discount amount", "Discount ID no", "Total",
            "Payment", "GCash reference", "Cash received", "Change",
            "Status", "Voided by", "Voided at", "Void reason"
        };

        public static void Write(string path, IEnumerable<Receipt> receipts)
        {
            // UTF-8 with BOM so Excel shows the peso sign correctly.
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                writer.WriteLine(string.Join(",", Header));
                foreach (Receipt r in receipts) writer.WriteLine(string.Join(",", Row(r)));
            }
        }

        private static IEnumerable<string> Row(Receipt r)
        {
            var cash = r.Payment as CashPayment;
            var gcash = r.Payment as GCashPayment;

            yield return Text(r.ReceiptNo);
            yield return r.IssuedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            yield return r.IssuedAt.ToString("HH:mm", CultureInfo.InvariantCulture);
            yield return Text(r.CashierName);
            yield return Text(r.StudentName ?? "Walk-in");
            yield return r.ItemCount.ToString(CultureInfo.InvariantCulture);
            yield return Text(string.Join("; ", r.Lines.Select(l =>
                l.Quantity + " x " + l.ProductName + (l.Size != null ? " (" + l.Size + ")" : ""))));
            yield return Num(r.Subtotal);
            yield return Text(r.DiscountAmount > 0m ? r.Discount.ToString() : "None");
            yield return Num(r.DiscountAmount);
            yield return Text(r.DiscountIdNumber ?? "");
            yield return Num(r.Total);
            yield return Text(r.Payment.Method.ToString());
            yield return Text(gcash != null ? gcash.ReferenceNumber : "");
            yield return cash != null ? Num(cash.CashReceived) : "";
            yield return cash != null ? Num(cash.Change) : "";
            yield return r.Status.ToString();
            yield return Text(r.VoidedBy ?? "");
            yield return r.VoidedAt.HasValue ? r.VoidedAt.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) : "";
            yield return Text(r.VoidReason ?? "");
        }

        private static string Num(decimal value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        /// <summary>Quotes the cell, and defuses text that Excel would run as a formula (=, +, -, @).</summary>
        private static string Text(string value)
        {
            value = value ?? "";
            if (value.Length > 0 && "=+-@\t\r".IndexOf(value[0]) >= 0) value = "'" + value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
