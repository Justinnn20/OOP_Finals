using System;

namespace Login.Models
{
    public class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public OrderItem(Product product, int quantity = 1)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = Math.Max(1, quantity);
            UnitPrice = product.Price;
        }

        public void Increment(int amount = 1)
        {
            if (amount > 0)
            {
                Quantity += amount;
            }
        }

        public void Decrement(int amount = 1)
        {
            if (amount > 0)
            {
                Quantity = Math.Max(0, Quantity - amount);
            }
        }

        public void SetQuantity(int quantity)
        {
            Quantity = Math.Max(0, quantity);
        }
    }
}

