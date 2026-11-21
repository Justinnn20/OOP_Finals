namespace UMakCoop.Models
{
    /// <summary>20% for Persons with Disability. The cashier must enter the PWD ID number.</summary>
    public class PwdDiscount : Discount
    {
        public override string Name { get { return "PWD"; } }
        public override decimal Rate { get { return 0.20m; } }
        public override bool RequiresIdNumber { get { return true; } }
        public override string IdLabel { get { return "PWD ID no."; } }
    }
}
