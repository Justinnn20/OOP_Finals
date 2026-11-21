using System;
using System.Collections.Generic;
using System.Text;
using UMakCoop.Services;

namespace UMakCoop.Models
{
    public class DiscountSummaryLine
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>End-of-day totals for one date. Only Completed receipts count toward sales; voids are counted separately.</summary>
    public class DailySummary
    {
        private const int Width = 40;

        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public int ItemsSold { get; set; }
        public int VoidedCount { get; set; }
        public decimal GrossSales { get; set; }      // before discounts
        public decimal DiscountTotal { get; set; }
        public decimal NetSales { get; set; }        // what the customers actually paid
        public decimal CashSales { get; set; }
        public decimal GCashSales { get; set; }
        public IList<DiscountSummaryLine> Discounts { get; set; }

        public string ToText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("DAILY SALES SUMMARY");
            sb.AppendLine(Date.ToString("dddd, MMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture));
            sb.AppendLine(new string('-', Width));
            sb.AppendLine(Row("Completed orders", OrderCount.ToString()));
            sb.AppendLine(Row("Items sold", ItemsSold.ToString()));
            sb.AppendLine(Row("Voided receipts", VoidedCount.ToString()));
            sb.AppendLine(new string('-', Width));
            sb.AppendLine(Row("Gross sales", Money.Peso(GrossSales)));
            sb.AppendLine(Row("Less: discounts", "-" + Money.Peso(DiscountTotal)));
            sb.AppendLine(Row("NET SALES", Money.Peso(NetSales)));
            sb.AppendLine(new string('-', Width));
            sb.AppendLine(Row("Cash", Money.Peso(CashSales)));
            sb.AppendLine(Row("GCash", Money.Peso(GCashSales)));
            sb.AppendLine(new string('-', Width));
            sb.AppendLine("Discounts given");
            if (Discounts == null || Discounts.Count == 0)
                sb.AppendLine("  none");
            else
                foreach (DiscountSummaryLine d in Discounts)
                    sb.AppendLine(Row("  " + d.Name + " (" + d.Count + ")", "-" + Money.Peso(d.Amount)));
            return sb.ToString();
        }

        private static string Row(string left, string right)
        {
            return left + new string(' ', Math.Max(1, Width - left.Length - right.Length)) + right;
        }
    }
}
