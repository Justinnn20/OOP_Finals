using System;
using System.Collections.Generic;
using System.Linq;

namespace Login.Models
{
    // OOP: Encapsulation. The order owns its items and calculates totals
    // through properties and methods instead of exposing calculation logic elsewhere.
    public class Order
    {
        public string OrderId { get; }
        // OOP: Composition. An Order is composed of multiple OrderItem objects.
        public List<OrderItem> Items { get; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; } = DateTime.Now;
        public decimal DiscountPercent { get; set; } = 0m;
        public string DiscountName { get; set; } = "None";
        public string PaymentMethod { get; set; } = "Cash";
        public decimal CashReceived { get; set; } = 0m;

        public decimal Subtotal => Items.Sum(i => i.TotalPrice);
        public decimal DiscountAmount => Math.Round(Subtotal * DiscountPercent, 2);
        public decimal Total => Math.Max(0, Subtotal - DiscountAmount);
        public decimal Change => Math.Max(0, CashReceived - Total);
        public int TotalItemsCount => Items.Sum(i => i.Quantity);

        public Order()
        {
            OrderId = "ORD-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        public Order(string customId)
        {
            OrderId = customId;
        }

        public OrderItem? FindItem(string sku, string? variant = null)
        {
            return Items.FirstOrDefault(i =>
                string.Equals(i.Product.Sku, sku, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(i.Product.Variant, variant, StringComparison.OrdinalIgnoreCase));
        }

        public void AddItem(Product product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;

            var existing = FindItem(product.Sku, product.Variant);
            if (existing != null)
            {
                existing.Increment(quantity);
            }
            else
            {
                Items.Add(new OrderItem(product, quantity));
            }
        }

        public bool RemoveItem(OrderItem item)
        {
            return Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
            DiscountPercent = 0m;
            DiscountName = "None";
            CashReceived = 0m;
            Status = OrderStatus.Pending;
        }
    }
}

