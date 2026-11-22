using System;
using System.Collections.Generic;

namespace Login
{
    public class TransactionItem
    {
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class TransactionRecord
    {
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierName { get; set; }
        public string StudentId { get; set; }
        public string DiscountType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public decimal CashReceived { get; set; }
        public decimal Change { get; set; }
        public List<TransactionItem> Items { get; set; } = new List<TransactionItem>();
    }

    // In-memory muna. Mawawala ito kapag nag-close ang app.
    public static class TransactionStore
    {
        private static readonly List<TransactionRecord> _records = new List<TransactionRecord>();

        public static IReadOnlyList<TransactionRecord> Records => _records;

        public static void Add(TransactionRecord record) => _records.Add(record);
    }
}