using System;

namespace Login
{
    /// <summary>
    /// Represents an individual product line item inside an Order / Cart.
    /// Encapsulates pricing, quantity, and line total calculations.
    /// </summary>
    public class OrderItem
    {
        private int _quantity;
        private decimal _unitPrice;

        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Unit price cannot be negative.");
                _unitPrice = value;
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be at least 1.");
                _quantity = value;
            }
        }

        public decimal LineTotal => UnitPrice * Quantity;

        public OrderItem()
        {
            _quantity = 1;
            _unitPrice = 0m;
        }

        public OrderItem(string sku, string productName, decimal unitPrice, int quantity = 1, string size = "")
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be empty.", nameof(sku));
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.", nameof(productName));
            if (unitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Price cannot be negative.");
            if (quantity < 1)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");

            Sku = sku.Trim().ToUpperInvariant();
            ProductName = productName.Trim();
            _unitPrice = unitPrice;
            _quantity = quantity;
            Size = size?.Trim() ?? string.Empty;
        }

        public void Increment(int amount = 1)
        {
            if (amount <= 0) return;
            Quantity += amount;
        }

        public bool Decrement(int amount = 1)
        {
            if (amount <= 0) return true;
            if (Quantity - amount <= 0)
            {
                return false; // Signals to caller that item should be removed or reached zero
            }
            Quantity -= amount;
            return true;
        }
    }
}

