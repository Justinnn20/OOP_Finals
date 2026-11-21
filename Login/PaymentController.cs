using System;
using System.Collections.Generic;
using System.Linq;
using UMakCoop.Models;
using UMakCoop.Services;

namespace UMakCoop.Controllers
{
    public class OrderTotals
    {
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Turns what the cashier picked on screen (discount, method, cash, reference)
    /// into numbers and a Payment object. It does not touch stock or the transaction list.
    /// </summary>
    public class PaymentController
    {
        private readonly List<Discount> _discounts = new List<Discount>
        {
            new NoDiscount(), new SeniorDiscount(), new PwdDiscount()
        };

        public PaymentController()
        {
            SelectedDiscount = _discounts[0];
            Method = PaymentMethod.Cash;
        }

        public IList<Discount> Discounts { get { return _discounts.AsReadOnly(); } }
        public Discount SelectedDiscount { get; set; }
        public PaymentMethod Method { get; set; }

        public OrderTotals Compute(IEnumerable<ReceiptLine> lines)
        {
            decimal subtotal = lines.Sum(l => l.LineTotal);
            decimal discount = SelectedDiscount.Compute(subtotal);
            return new OrderTotals { Subtotal = subtotal, DiscountAmount = discount, Total = subtotal - discount };
        }

        /// <summary>Senior Citizen and PWD discounts need the ID number typed in by the cashier.</summary>
        public bool IsDiscountAllowed(string idNumber, out string error)
        {
            if (SelectedDiscount.RequiresIdNumber && string.IsNullOrWhiteSpace(idNumber))
            {
                error = "Enter the " + SelectedDiscount.IdLabel + " to use the " + SelectedDiscount.Name + " discount.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        /// <summary>Exact amount first, then the next 100 / 500 / 1000 bills.</summary>
        public IList<decimal> QuickCashAmounts(decimal total)
        {
            var amounts = new List<decimal> { total };
            foreach (decimal step in new[] { 100m, 500m, 1000m })
            {
                decimal rounded = Math.Ceiling(total / step) * step;
                if (!amounts.Contains(rounded)) amounts.Add(rounded);
            }
            return amounts;
        }

        public bool TryBuildPayment(decimal total, string cashText, string referenceText,
                                    out Payment payment, out string error)
        {
            payment = null;

            if (total <= 0m)
            {
                error = "Add items to the order first.";
                return false;
            }

            if (Method == PaymentMethod.Cash)
            {
                decimal cash;
                if (!Money.TryParse(cashText, out cash))
                {
                    error = "Enter the cash received.";
                    return false;
                }
                payment = new CashPayment(total, cash);
            }
            else
            {
                payment = new GCashPayment(total, referenceText);
            }

            if (payment.TryValidate(out error)) return true;

            payment = null;
            return false;
        }
    }
}
