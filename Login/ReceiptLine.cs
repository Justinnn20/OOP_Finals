using System;

namespace UMakCoop.Models
{
    /// <summary>One purchased item, copied from the cart at the moment of sale so the price never changes later.</summary>
    public class ReceiptLine
    {
        public ReceiptLine(int productId, string productName, string size, decimal unitPrice, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("quantity");
            if (unitPrice < 0m) throw new ArgumentOutOfRangeException("unitPrice");
            ProductId = productId;
            ProductName = productName;
            Size = string.IsNullOrWhiteSpace(size) ? null : size;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string Size { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        public decimal LineTotal
        {
            get { return UnitPrice * Quantity; }
        }
    }
}
