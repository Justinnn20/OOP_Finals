using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UMakCoop.Controllers;
using UMakCoop.Models;
using UMakCoop.Services;

namespace UMakCoop.Views
{
    /// <summary>
    /// For testing Module 6 alone, before the cart and login from the other modules are ready.
    /// To run it: in Program.cs use Application.Run(new DemoForm());
    /// Delete this file (and InMemoryStockService) before the final merge.
    /// </summary>
    public class DemoForm : Form
    {
        private const string FakeStudent = "Juan Dela Cruz";

        private readonly TransactionController _transactions;
        private readonly List<ReceiptLine> _cart = new List<ReceiptLine>();
        private readonly CheckoutPanel _checkout = new CheckoutPanel();
        private readonly TransactionsView _logs = new TransactionsView();
        private readonly Label _cartLabel = new Label();
        private readonly CheckBox _admin = new CheckBox();
        private readonly CheckBox _student = new CheckBox();

        public DemoForm()
        {
            Text = "Module 6 demo (testing only)";
            ClientSize = new Size(1200, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.Wine;

            var store = new ReceiptStore(ReceiptStore.DefaultPath);
            _transactions = new TransactionController(new InMemoryStockService(), store);
            Text = "Module 6 demo (testing only) - saved to " + store.FilePath;
            _checkout.Transactions = _transactions;
            _checkout.CashierName = "Ana Reyes";
            _checkout.SaleCompleted += (s, e) => { _cart.Clear(); Push(); };
            _logs.Attach(_transactions, "Ana Reyes", false);

            var right = new Panel { Dock = DockStyle.Right, Width = 400, BackColor = Theme.Parchment, Padding = new Padding(16) };

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
            AddProduct(buttons, new ReceiptLine(1, "PE Uniform", "M", 350m, 1));
            AddProduct(buttons, new ReceiptLine(2, "Blue Book", null, 15m, 1));
            AddProduct(buttons, new ReceiptLine(3, "UMak Lanyard", null, 60m, 1));
            AddProduct(buttons, new ReceiptLine(4, "CCIS Lab Manual", null, 120m, 1));

            _cartLabel.Dock = DockStyle.Top;
            _cartLabel.Height = 100;
            _cartLabel.ForeColor = Theme.Ink;
            _cartLabel.Font = Theme.Sans(10f);

            _admin.Text = "Admin mode (can void)";
            _admin.Dock = DockStyle.Top;
            _admin.CheckedChanged += (s, e) =>
            {
                string name = _admin.Checked ? "Marco Villanueva" : "Ana Reyes";
                _checkout.CashierName = name;
                _logs.SetUser(name, _admin.Checked);
            };

            _student.Text = "Student attached: " + FakeStudent + " (shows on the receipt)";
            _student.Dock = DockStyle.Top;
            _student.CheckedChanged += (s, e) => Push();

            var title = new Label { Text = "Fake cart (testing only)", Dock = DockStyle.Top, Height = 28, Font = Theme.Serif(12f, FontStyle.Bold), ForeColor = Theme.Ink };

            _checkout.Dock = DockStyle.Bottom;

            // Docked top to bottom = added bottom to top.
            right.Controls.Add(_checkout);
            right.Controls.Add(_student);
            right.Controls.Add(_admin);
            right.Controls.Add(_cartLabel);
            right.Controls.Add(buttons);
            right.Controls.Add(title);

            _logs.Dock = DockStyle.Fill;
            Controls.Add(_logs);
            Controls.Add(right);

            Push();
        }

        private void AddProduct(FlowLayoutPanel host, ReceiptLine template)
        {
            var button = new Button { Text = "+ " + template.ProductName, AutoSize = true };
            button.Click += (s, e) =>
            {
                int i = _cart.FindIndex(l => l.ProductId == template.ProductId && l.Size == template.Size);
                if (i >= 0)
                    _cart[i] = new ReceiptLine(template.ProductId, template.ProductName, template.Size, template.UnitPrice, _cart[i].Quantity + 1);
                else
                    _cart.Add(template);
                Push();
            };
            host.Controls.Add(button);
        }

        private void Push()
        {
            _cartLabel.Text = _cart.Count == 0
                ? "Cart is empty. Add something above."
                : string.Join(Environment.NewLine, _cart.Select(l => l.Quantity + " x " + l.ProductName + "   " + Money.Peso(l.LineTotal)));
            _checkout.SetOrder(_cart.ToList(), _student.Checked ? FakeStudent : null);
        }
    }
}
