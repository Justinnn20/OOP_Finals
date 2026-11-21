using System;

namespace UMakCoop.Models
{
    /// <summary>Abstract base for every way a customer can pay.</summary>
    // OOP: Abstraction and inheritance. This parent defines the common payment contract
    // that child payment classes must implement.
    public abstract class Payment
    {
        protected Payment(decimal amountDue)
        {
            if (amountDue <= 0m) throw new ArgumentOutOfRangeException("amountDue", "Amount due must be above zero.");
            AmountDue = amountDue;
        }

        public decimal AmountDue { get; private set; }

        public abstract PaymentMethod Method { get; }
        public abstract decimal Change { get; }

        /// <summary>Checks the payment details. Returns false and a readable message when invalid.</summary>
        public abstract bool TryValidate(out string error);

        /// <summary>The line printed on the receipt for this payment.</summary>
        public abstract string ReceiptLine { get; }
    }
}
