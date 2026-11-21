namespace UMakCoop.Models
{
    public class NoDiscount : Discount
    {
        public override string Name { get { return "None"; } }
        public override decimal Rate { get { return 0m; } }
    }
}
