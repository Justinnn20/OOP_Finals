using System;
using System.Collections.Generic;
using System.Linq;

namespace Login
{
    /// <summary>
    /// Product information registered in the store catalog.
    /// </summary>
    public class CatalogProduct
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
    }

    /// <summary>
    /// Controller managing the Order Scroll (Cart) business logic,
    /// Barcode parsing, quantity stepper (+/-), Hold Order queue, and Clear Cart.
    /// </summary>
    public class CartController
    {
        private readonly Dictionary<string, CatalogProduct> _catalog = new(StringComparer.OrdinalIgnoreCase);

        public Order ActiveOrder { get; private set; }
        public List<Order> HeldOrders { get; } = new List<Order>();

        public event Action? CartChanged;
        public event Action<string>? StatusMessage;
        public event Action<Order>? OrderHeld;
        public event Action<Order>? OrderRestored;

        public int HeldOrderCount => HeldOrders.Count;

        public CartController()
        {
            ActiveOrder = new Order();
            InitializeCatalog();
        }

        private void InitializeCatalog()
        {
            RegisterProduct("UM-001", "Calculus Textbook", "Books & Manuals", 450.00m);
            RegisterProduct("UM-002", "Blouse (Female)", "Uniforms", 380.00m, "Size M");
            RegisterProduct("UM-003", "Yellow Pad", "School Supplies", 30.00m);
            RegisterProduct("UM-004", "PE Uniform", "Uniforms", 350.00m, "Size M");
            RegisterProduct("UM-005", "UMak Polo", "Uniforms", 450.00m, "Size L");
            RegisterProduct("UM-006", "CCIS Lab Manual", "Books & Manuals", 120.00m);
            RegisterProduct("UM-007", "Blue Book", "School Supplies", 20.00m);
            RegisterProduct("UM-008", "ID Holder", "School Supplies", 45.00m);
            RegisterProduct("UM-009", "UMak Lanyard", "School Supplies", 60.00m);
            RegisterProduct("UM-010", "PE Jogging Pants", "Uniforms", 400.00m, "Size M");
            RegisterProduct("UM-011", "UMak Spiral Notebook", "School Supplies", 35.00m);
            RegisterProduct("UM-012", "Ballpen Set (3pcs)", "School Supplies", 65.00m);
            RegisterProduct("UM-013", "University Pin", "School Supplies", 50.00m);
            RegisterProduct("UM-014", "College Manual", "Books & Manuals", 280.00m);
            RegisterProduct("UM-015", "Computer Programming Book", "Books & Manuals", 550.00m);
        }

        public void RegisterProduct(string sku, string name, string category, decimal price, string size = "")
        {
            var prod = new CatalogProduct
            {
                Sku = sku.Trim().ToUpperInvariant(),
                Name = name.Trim(),
                Category = category.Trim(),
                Price = price,
                Size = size.Trim()
            };
            _catalog[prod.Sku] = prod;
        }

        public CatalogProduct? FindProduct(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return null;
            if (_catalog.TryGetValue(sku.Trim(), out var product))
            {
                return product;
            }
            return null;
        }

        public bool AddToCart(string sku, int quantity = 1)
        {
            var product = FindProduct(sku);
            if (product == null)
            {
                StatusMessage?.Invoke($"Product with SKU '{sku}' not found in catalog.");
                return false;
            }

            ActiveOrder.AddItem(product.Sku, product.Name, product.Price, quantity, product.Size);
            CartChanged?.Invoke();
            return true;
        }

        public void AddToCart(string sku, string name, decimal price, int quantity = 1, string size = "")
        {
            ActiveOrder.AddItem(sku, name, price, quantity, size);
            CartChanged?.Invoke();
        }

        /// <summary>
        /// Parses raw barcode/SKU input (e.g. "3*UM-007" or "UM-007") using BarcodeParser
        /// and adds the resolved items into the cart.
        /// </summary>
        public bool ProcessBarcodeInput(string rawInput)
        {
            var result = BarcodeParser.Parse(rawInput);
            if (!result.IsSuccess)
            {
                StatusMessage?.Invoke(result.ErrorMessage);
                return false;
            }

            var product = FindProduct(result.Sku);
            if (product == null)
            {
                StatusMessage?.Invoke($"SKU '{result.Sku}' not found in system catalog.");
                return false;
            }

            ActiveOrder.AddItem(product.Sku, product.Name, product.Price, result.Quantity, product.Size);
            StatusMessage?.Invoke($"Added {result.Quantity}x {product.Name} to cart.");
            CartChanged?.Invoke();
            return true;
        }

        public void IncreaseQuantity(string sku, int amount = 1)
        {
            ActiveOrder.IncreaseQuantity(sku, amount);
            CartChanged?.Invoke();
        }

        public void DecreaseQuantity(string sku, int amount = 1)
        {
            ActiveOrder.DecreaseQuantity(sku, amount);
            CartChanged?.Invoke();
        }

        public void RemoveItem(string sku)
        {
            ActiveOrder.RemoveItem(sku);
            CartChanged?.Invoke();
        }

        public void SetDiscount(string discountType, decimal rate)
        {
            ActiveOrder.ApplyDiscount(discountType, rate);
            CartChanged?.Invoke();
        }

        /// <summary>
        /// Holds the current active order, adds it to the held orders queue,
        /// and starts a fresh order.
        /// </summary>
        public bool HoldCurrentOrder()
        {
            if (ActiveOrder.IsEmpty)
            {
                StatusMessage?.Invoke("The order scroll is empty. Nothing to hold.");
                return false;
            }

            ActiveOrder.Hold();
            HeldOrders.Add(ActiveOrder);
            var held = ActiveOrder;

            ActiveOrder = new Order();
            OrderHeld?.Invoke(held);
            CartChanged?.Invoke();
            StatusMessage?.Invoke($"Order #{held.OrderId} has been held ({HeldOrders.Count} held).");
            return true;
        }

        /// <summary>
        /// Resumes the most recently held order or toggles held orders.
        /// </summary>
        public bool RestoreLatestHeldOrder()
        {
            if (HeldOrders.Count == 0)
            {
                StatusMessage?.Invoke("No held orders to restore.");
                return false;
            }

            // If current cart has items, we can hold it or warn
            if (!ActiveOrder.IsEmpty)
            {
                ActiveOrder.Hold();
                HeldOrders.Add(ActiveOrder);
            }

            int lastIdx = HeldOrders.Count - 1;
            var orderToRestore = HeldOrders[lastIdx];
            HeldOrders.RemoveAt(lastIdx);

            orderToRestore.Resume();
            ActiveOrder = orderToRestore;

            OrderRestored?.Invoke(orderToRestore);
            CartChanged?.Invoke();
            StatusMessage?.Invoke($"Resumed held Order #{orderToRestore.OrderId}.");
            return true;
        }

        /// <summary>
        /// Clears all items and resets the active order.
        /// </summary>
        public void ClearCart()
        {
            if (ActiveOrder.IsEmpty) return;

            ActiveOrder.Clear();
            CartChanged?.Invoke();
            StatusMessage?.Invoke("Order scroll cleared.");
        }
    }
}

