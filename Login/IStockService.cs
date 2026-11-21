namespace UMakCoop.Services
{
    /// <summary>
    /// Contract between Checkout (Module 6) and Inventory (Module 3).
    /// The Product/Inventory owner implements this. Module 6 only calls it.
    /// </summary>
    public interface IStockService
    {
        bool HasStock(int productId, int quantity);
        void Deduct(int productId, int quantity);   // when a sale is sealed
        void Restore(int productId, int quantity);  // when a receipt is voided
    }
}
