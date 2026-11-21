namespace UMakCoop.Models
{
    // OOP: Inheritance and polymorphism. NoDiscount reuses the Discount contract
    // while providing a zero-rate implementation.
    public class NoDiscount : Discount
    {
        public override string Name { get { return "None"; } }
        public override decimal Rate { get { return 0m; } }
    }
}
