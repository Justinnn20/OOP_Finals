namespace UMakCoop.Models
{
    /// <summary>20% for Senior Citizens. The cashier must enter the Senior Citizen ID number.</summary>
    // OOP: Child class — inheritance and polymorphism. SeniorDiscount inherits
    // from the Discount parent class and supplies senior-specific rules.
    public class SeniorDiscount : Discount
    {
        public override string Name { get { return "Senior Citizen"; } }
        public override decimal Rate { get { return 0.20m; } }
        public override bool RequiresIdNumber { get { return true; } }
        public override string IdLabel { get { return "Senior Citizen ID no."; } }
    }
}
