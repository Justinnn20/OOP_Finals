using System;
using System.Collections.Generic;
using System.Linq;
using Login.Models;

namespace Login.Controllers
{
    public class CartController
    {
        private readonly List<Product> _catalog = new();
        private readonly List<Order> _heldOrders = new();
        private readonly BarcodeParser _barcodeParser = new();

        public Order CurrentOrder { get; private set; } = new();
        public IReadOnlyList<Order> HeldOrders => _heldOrders.AsReadOnly();
        public IReadOnlyList<Product> Catalog => _catalog.AsReadOnly();

        public event Action<Order>? OrderUpdated;
        public event Action<Order>? OrderHeld;
        public event Action<Order>? OrderRecalled;
        public event Action? OrderCleared;
        public event Action<string>? Notification;

        public CartController()
        {
            SeedCatalog();
        }

        private void SeedCatalog()
        {
            _catalog.Add(new Product("UM-001", "PE Uniform", "Uniforms", 350m, "Size M", 100));
            _catalog.Add(new Product("UM-002", "ID Holder", "School Supplies", 45m, null, 150));
            _catalog.Add(new Product("UM-003", "UMak Polo", "Uniforms", 450m, "Size L", 80));
            _catalog.Add(new Product("UM-004", "UMak Lanyard", "School Supplies", 60m, null, 200));
            _catalog.Add(new Product("UM-005", "PE Jogging Pants", "Uniforms", 400m, "Size M", 90));
            _catalog.Add(new Product("UM-006", "College Manual", "Books & Manuals", 280m, null, 50));
            _catalog.Add(new Product("UM-007", "UMak Spiral Notebook", "School Supplies", 35m, null, 300));
            _catalog.Add(new Product("UM-008", "Ballpen Set (3pcs)", "School Supplies", 65m, null, 120));
            _catalog.Add(new Product("UM-009", "University Pin", "School Supplies", 50m, null, 250));
            _catalog.Add(new Product("UM-010", "Computer Programming Book", "Books & Manuals", 550m, null, 40));
        }

        public Product? FindProduct(string sku)
        {
            return _catalog.FirstOrDefault(p =>
                string.Equals(p.Sku, sku, StringComparison.OrdinalIgnoreCase));
        }

        public void AddItem(Product product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;
            CurrentOrder.AddItem(product, quantity);
            CurrentOrder.Status = OrderStatus.Pending;
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public bool AddItemByBarcode(string input, out string message)
        {
            var parseResult = _barcodeParser.Parse(input);
            if (!parseResult.IsSuccess)
            {
                message = parseResult.ErrorMessage ?? "Invalid barcode format.";
                Notification?.Invoke(message);
                return false;
            }

            var product = FindProduct(parseResult.Barcode);
            if (product == null)
            {
                message = $"SKU/Barcode '{parseResult.Barcode}' was not found in catalog.";
                Notification?.Invoke(message);
                return false;
            }

            AddItem(product, parseResult.Quantity);
            message = $"Added {parseResult.Quantity}x '{product.Name}' to scroll.";
            Notification?.Invoke(message);
            return true;
        }

        public void IncrementItem(OrderItem item)
        {
            item.Increment(1);
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public void DecrementItem(OrderItem item)
        {
            item.Decrement(1);
            if (item.Quantity <= 0)
            {
                CurrentOrder.RemoveItem(item);
            }
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public void RemoveItem(OrderItem item)
        {
            CurrentOrder.RemoveItem(item);
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public void ApplyDiscount(decimal percent, string name)
        {
            CurrentOrder.DiscountPercent = percent;
            CurrentOrder.DiscountName = name;
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public void SetPaymentMethod(string method)
        {
            CurrentOrder.PaymentMethod = method;
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public void SetCashReceived(decimal amount)
        {
            CurrentOrder.CashReceived = Math.Max(0, amount);
            OrderUpdated?.Invoke(CurrentOrder);
        }

        public bool HoldOrder(out string message)
        {
            if (CurrentOrder.Items.Count == 0)
            {
                message = "The scroll is empty. Cannot hold an empty order.";
                Notification?.Invoke(message);
                return false;
            }

            CurrentOrder.Status = OrderStatus.Held;
            _heldOrders.Add(CurrentOrder);
            var held = CurrentOrder;

            CurrentOrder = new Order();
            OrderHeld?.Invoke(held);
            OrderUpdated?.Invoke(CurrentOrder);

            message = $"Order {held.OrderId} held successfully. ({_heldOrders.Count} held)";
            Notification?.Invoke(message);
            return true;
        }

        public bool RecallOrder(Order heldOrder, out string message)
        {
            if (!_heldOrders.Contains(heldOrder))
            {
                message = "Held order not found.";
                return false;
            }

            _heldOrders.Remove(heldOrder);
            heldOrder.Status = OrderStatus.Pending;
            CurrentOrder = heldOrder;

            OrderRecalled?.Invoke(heldOrder);
            OrderUpdated?.Invoke(CurrentOrder);

            message = $"Restored order {heldOrder.OrderId}.";
            Notification?.Invoke(message);
            return true;
        }

        public void ClearCart()
        {
            CurrentOrder.Clear();
            OrderCleared?.Invoke();
            OrderUpdated?.Invoke(CurrentOrder);
            Notification?.Invoke("The scroll has been cleared.");
        }

        public bool Checkout(out string message)
        {
            if (CurrentOrder.Items.Count == 0)
            {
                message = "The scroll is blank. Choose an item to begin.";
                return false;
            }

            if (string.Equals(CurrentOrder.PaymentMethod, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                if (CurrentOrder.CashReceived < CurrentOrder.Total)
                {
                    decimal shortBy = CurrentOrder.Total - CurrentOrder.CashReceived;
                    message = $"Insufficient cash received.\nNeed: ₱{CurrentOrder.Total:N2}\nReceived: ₱{CurrentOrder.CashReceived:N2}\nShort by: ₱{shortBy:N2}";
                    return false;
                }
            }

            CurrentOrder.Status = OrderStatus.Completed;
            decimal total = CurrentOrder.Total;
            decimal change = CurrentOrder.Change;
            string orderId = CurrentOrder.OrderId;

            message = $"Order {orderId} completed successfully!\nMethod: {CurrentOrder.PaymentMethod}\nTotal: ₱{total:N2}\nChange: ₱{change:N2}";

            CurrentOrder = new Order();
            OrderUpdated?.Invoke(CurrentOrder);
            return true;
        }
    }
}

