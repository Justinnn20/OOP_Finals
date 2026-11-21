using System;
using System.Collections.Generic;
using System.Linq;

namespace Login
{
    /// <summary>
    /// Represents an Order in the system (the Order Scroll).
    /// Holds items, discounts, order status, and totals calculations.
    /// </summary>
    public class Order
    {
        private static int _nextOrderNumber = 1045;

        public string OrderId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string StudentId { get; set; } = string.Empty;

        public List<OrderItem> Items { get; } = new List<OrderItem>();

        public string DiscountType { get; set; } = "None";
        public decimal DiscountRate { get; set; } = 0.00m;

        public decimal Subtotal => Items.Sum(i => i.LineTotal);

        public decimal DiscountAmount => Math.Round(Subtotal * DiscountRate, 2, MidpointRounding.AwayFromZero);

        public decimal Total => Math.Max(0m, Subtotal - DiscountAmount);

        public int TotalItemCount => Items.Sum(i => i.Quantity);

        public bool IsEmpty => Items.Count == 0;

        public Order()
        {
            OrderId = $"ORD-{_nextOrderNumber++}";
            Status = OrderStatus.Active;
            CreatedAt = DateTime.Now;
        }

        public Order(string orderId)
        {
            OrderId = orderId;
            Status = OrderStatus.Active;
            CreatedAt = DateTime.Now;
        }

        public OrderItem? FindItem(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return null;
            return Items.FirstOrDefault(i => string.Equals(i.Sku, sku.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void AddItem(string sku, string productName, decimal unitPrice, int quantity = 1, string size = "")
        {
            if (quantity <= 0) return;

            var existing = FindItem(sku);
            if (existing != null)
            {
                existing.Increment(quantity);
            }
            else
            {
                Items.Add(new OrderItem(sku, productName, unitPrice, quantity, size));
            }
        }

        public bool RemoveItem(string sku)
        {
            var item = FindItem(sku);
            if (item != null)
            {
                return Items.Remove(item);
            }
            return false;
        }

        public void IncreaseQuantity(string sku, int amount = 1)
        {
            var item = FindItem(sku);
            if (item != null)
            {
                item.Increment(amount);
            }
        }

        public void DecreaseQuantity(string sku, int amount = 1)
        {
            var item = FindItem(sku);
            if (item != null)
            {
                bool remains = item.Decrement(amount);
                if (!remains)
                {
                    Items.Remove(item);
                }
            }
        }

        public void SetItemQuantity(string sku, int newQty)
        {
            var item = FindItem(sku);
            if (item != null)
            {
                if (newQty <= 0)
                {
                    Items.Remove(item);
                }
                else
                {
                    item.Quantity = newQty;
                }
            }
        }

        public void ApplyDiscount(string discountType, decimal rate)
        {
            DiscountType = discountType;
            DiscountRate = Math.Max(0m, Math.Min(1m, rate));
        }

        public void Hold()
        {
            Status = OrderStatus.Held;
        }

        public void Resume()
        {
            Status = OrderStatus.Active;
        }

        public void Complete()
        {
            Status = OrderStatus.Completed;
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        public void Clear()
        {
            Items.Clear();
            DiscountType = "None";
            DiscountRate = 0m;
        }
    }
}

