namespace UMakCoop.Models
{
    // OOP: Child class — inheritance and polymorphism. NoDiscount inherits from
    // the Discount parent class while providing a zero-rate implementation.
    public class NoDiscount : Discount
    {
        public override string Name { get { return "None"; } }
        public override decimal Rate { get { return 0m; } }
    }
}
