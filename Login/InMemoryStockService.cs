using System.Collections.Generic;

namespace UMakCoop.Services
{
    /// <summary>For testing only. Delete once ProductController implements IStockService.</summary>
    public class InMemoryStockService : IStockService
    {
        private readonly Dictionary<int, int> _stock = new Dictionary<int, int>
        {
            { 1, 15 }, { 2, 350 }, { 3, 3 }, { 4, 120 }
        };

        public bool HasStock(int productId, int quantity)
        {
            int stock;
            return _stock.TryGetValue(productId, out stock) && stock >= quantity;
        }

        public void Deduct(int productId, int quantity)
        {
            _stock[productId] -= quantity;
        }

        public void Restore(int productId, int quantity)
        {
            int stock;
            _stock.TryGetValue(productId, out stock);
            _stock[productId] = stock + quantity;
        }
    }
}
