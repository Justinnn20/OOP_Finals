namespace Login.Models
{
    public class Product
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Variant { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Product() { }

        public Product(string sku, string name, string category, decimal price, string? variant = null, int stockQuantity = 100)
        {
            Sku = sku;
            Name = name;
            Category = category;
            Price = price;
            Variant = variant;
            StockQuantity = stockQuantity;
        }

        public string DisplayDetails => !string.IsNullOrWhiteSpace(Variant)
            ? $"{Variant} · ₱{Price:N0} each"
            : $"₱{Price:N0} each";
    }
}

