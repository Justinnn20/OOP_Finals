using UMakCoop.Services;

namespace UMakCoop.Models
{
    // OOP: Inheritance and polymorphism. CashPayment is a Payment child class
    // that provides cash-specific implementations of the abstract members.
    public class CashPayment : Payment
    {
        /// <summary>Guards against a typo like 5000000 instead of 500.</summary>
        public const decimal MaxCash = 100000m;

        public CashPayment(decimal amountDue, decimal cashReceived) : base(amountDue)
        {
            CashReceived = cashReceived;
        }

        public decimal CashReceived { get; private set; }

        public override PaymentMethod Method { get { return PaymentMethod.Cash; } }

        public override decimal Change
        {
            get { return CashReceived > AmountDue ? CashReceived - AmountDue : 0m; }
        }

        public override bool TryValidate(out string error)
        {
            if (decimal.Round(CashReceived, 2) != CashReceived)
            {
                error = "Use at most 2 decimal places.";
                return false;
            }
            if (CashReceived > MaxCash)
            {
                error = "That amount looks too large. Please check the cash received.";
                return false;
            }
            if (CashReceived < AmountDue)
            {
                error = "Cash is short by " + Money.Peso(AmountDue - CashReceived) + ".";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public override string ReceiptLine
        {
            get { return "Cash " + Money.Peso(CashReceived); }
        }
    }
}
