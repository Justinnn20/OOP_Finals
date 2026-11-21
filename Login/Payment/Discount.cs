using System;

namespace UMakCoop.Models
{
    /// <summary>Abstract base for every discount. Subclasses decide their own name, rate and eligibility.</summary>
    // OOP: Abstraction and inheritance. This parent class defines the shared discount
    // behavior while child classes provide the rules for each discount type.
    public abstract class Discount
    {
        public abstract string Name { get; }

        /// <summary>0.10 means 10%.</summary>
        public abstract decimal Rate { get; }

        /// <summary>True when the discount can only be given against an ID (Senior Citizen / PWD).</summary>
        public virtual bool RequiresIdNumber
        {
            get { return false; }
        }

        /// <summary>Caption for the ID box and the receipt, e.g. "PWD ID no.".</summary>
        public virtual string IdLabel
        {
            get { return "ID no."; }
        }

        public virtual decimal Compute(decimal subtotal)
        {
            return Math.Round(subtotal * Rate, 2, MidpointRounding.AwayFromZero);
        }

        public override string ToString()
        {
            return Rate == 0m ? Name : Name + " " + (Rate * 100m).ToString("0.##") + "%";
        }
    }
}
