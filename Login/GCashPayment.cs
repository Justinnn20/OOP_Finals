using System.Linq;

namespace UMakCoop.Models
{
    public class GCashPayment : Payment
    {
        public const int ReferenceLength = 13;

        public GCashPayment(decimal amountDue, string referenceNumber) : base(amountDue)
        {
            ReferenceNumber = (referenceNumber ?? string.Empty).Trim();
        }

        public string ReferenceNumber { get; private set; }

        public override PaymentMethod Method { get { return PaymentMethod.GCash; } }

        public override decimal Change { get { return 0m; } }

        public override bool TryValidate(out string error)
        {
            if (ReferenceNumber.Length != ReferenceLength || !ReferenceNumber.All(char.IsDigit))
            {
                error = "Enter the " + ReferenceLength + "-digit GCash reference number.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public override string ReceiptLine
        {
            get { return "GCash ref " + ReferenceNumber; }
        }
    }
}
