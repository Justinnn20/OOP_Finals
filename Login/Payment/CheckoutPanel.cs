using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using UMakCoop.Controllers;
using UMakCoop.Models;
using UMakCoop.Services;
using System.ComponentModel;

namespace UMakCoop.Views
{
    /// <summary>
    /// Bottom half of the Order Scroll: discount chips, Cash/GCash, totals, cash received,
    /// change, and the "Seal the sale" button. Built in code so it can be dropped on any form.
    /// </summary>
    public class CheckoutPanel : UserControl
    {
        private readonly PaymentController _payments = new PaymentController();
        private IList<ReceiptLine> _lines = new List<ReceiptLine>();
        private string _studentName;
        private decimal _quickFor = -1m;

        private FlowLayoutPanel _discountFlow, _methodFlow, _quickFlow;
        private TableLayoutPanel _cashGroup, _gcashGroup, _idGroup;
        private Label _subtotal, _discount, _total, _change, _error, _idLabel;
        private TextBox _cashBox, _refBox, _idBox;
        private Button _seal;

        /// <summary>Set by MainForm. Required before the first sale.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TransactionController Transactions { get; set; }

        /// <summary>Set by MainForm from the logged-in user (Module 2).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CashierName { get; set; }

        /// <summary>Raised after a sale is sealed. The cart should clear itself and the product list should refresh.</summary>
        public event EventHandler<Receipt> SaleCompleted;

        public CheckoutPanel()
        {
            CashierName = "Cashier";
            BuildUi();
            UpdateView();
        }

        /// <summary>Module 5 (cart) calls this every time the cart changes.</summary>
        public void SetOrder(IList<ReceiptLine> lines, string studentName)
        {
            _lines = lines ?? new List<ReceiptLine>();
            _studentName = studentName;
            UpdateView();
        }

        // ---------------------------------------------------------------- UI

        private void BuildUi()
        {
            BackColor = Color.Transparent;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Size = new Size(320, 360);

            var root = NewTable(1);
            root.Dock = DockStyle.Top;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _discountFlow = NewFlow();
            foreach (Discount d in _payments.Discounts)
            {
                var chip = new RadioButton { Text = d.ToString(), Tag = d, Checked = d is NoDiscount };
                Theme.Chip(chip);
                chip.CheckedChanged += (s, e) =>
                {
                    if (!chip.Checked) return;
                    _payments.SelectedDiscount = (Discount)chip.Tag;
                    UpdateView();
                };
                _discountFlow.Controls.Add(chip);
            }
            AddRow(root, _discountFlow);

            // Senior Citizen / PWD ID number (only shown when one of those discounts is picked)
            _idGroup = NewTable(1);
            _idGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _idLabel = NewLabel("ID no.", 9.5f, Theme.Ink);
            _idGroup.Controls.Add(_idLabel, 0, 0);
            _idBox = new TextBox { Dock = DockStyle.Fill, Font = Theme.Sans(11f, FontStyle.Bold), MaxLength = 30 };
            _idBox.TextChanged += (s, e) => UpdateView();
            _idBox.KeyDown += EnterToSeal;
            _idGroup.Controls.Add(_idBox, 0, 1);
            _idGroup.Padding = new Padding(0, 0, 0, 6);
            AddRow(root, _idGroup);

            _methodFlow = NewFlow();
            foreach (PaymentMethod m in new[] { PaymentMethod.Cash, PaymentMethod.GCash })
            {
                var chip = new RadioButton { Text = m.ToString(), Tag = m, Checked = m == PaymentMethod.Cash };
                Theme.Chip(chip);
                chip.CheckedChanged += (s, e) =>
                {
                    if (!chip.Checked) return;
                    _payments.Method = (PaymentMethod)chip.Tag;
                    UpdateView();
                };
                _methodFlow.Controls.Add(chip);
            }
            AddRow(root, _methodFlow);

            // Totals
            var totals = NewTable(2);
            totals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            totals.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            totals.Padding = new Padding(0, 6, 0, 0);
            _subtotal = AddTotalRow(totals, "Subtotal", 10f, false, Theme.Ink);
            _discount = AddTotalRow(totals, "Discount", 10f, false, Theme.Ink);
            _total = AddTotalRow(totals, "TOTAL", 18f, true, Theme.Crimson);
            AddRow(root, totals);

            // Cash group
            _cashGroup = NewTable(2);
            _cashGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _cashGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            _cashGroup.Controls.Add(NewLabel("Cash received", 10f, Theme.Ink), 0, 0);
            _cashBox = new TextBox { Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right, Font = Theme.Sans(12f, FontStyle.Bold), MaxLength = 9 };
            _cashBox.KeyPress += DecimalOnly;
            _cashBox.TextChanged += (s, e) => UpdateView();
            _cashBox.KeyDown += EnterToSeal;
            _cashGroup.Controls.Add(_cashBox, 1, 0);
            _quickFlow = NewFlow();
            _cashGroup.Controls.Add(_quickFlow, 0, 1);
            _cashGroup.SetColumnSpan(_quickFlow, 2);
            _cashGroup.Padding = new Padding(0, 6, 0, 0);
            AddRow(root, _cashGroup);

            // GCash group
            _gcashGroup = NewTable(1);
            _gcashGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _gcashGroup.Controls.Add(NewLabel("GCash reference number (13 digits)", 9.5f, Theme.Ink), 0, 0);
            _refBox = new TextBox { Dock = DockStyle.Fill, Font = Theme.Sans(12f, FontStyle.Bold), MaxLength = GCashPayment.ReferenceLength };
            _refBox.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            _refBox.TextChanged += (s, e) => UpdateView();
            _refBox.KeyDown += EnterToSeal;
            _gcashGroup.Controls.Add(_refBox, 0, 1);
            _gcashGroup.Padding = new Padding(0, 6, 0, 0);
            AddRow(root, _gcashGroup);

            // Change
            var changeRow = NewTable(2);
            changeRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            changeRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _change = AddTotalRow(changeRow, "Change", 12f, true, Theme.Ok);
            AddRow(root, changeRow);

            _error = new Label
            {
                Dock = DockStyle.Fill,
                Height = 36,
                ForeColor = Theme.Crimson,
                Font = Theme.Sans(9f),
                Margin = new Padding(0, 2, 0, 2)
            };
            AddRow(root, _error);

            _seal = new Button { Text = "Seal the sale", Dock = DockStyle.Fill, Height = 46, Enabled = false };
            Theme.GoldButton(_seal, 13f);
            _seal.Click += OnSeal;
            AddRow(root, _seal);

            Controls.Add(root);
        }

        private static TableLayoutPanel NewTable(int columns)
        {
            return new TableLayoutPanel
            {
                ColumnCount = columns,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
        }

        private static FlowLayoutPanel NewFlow()
        {
            return new FlowLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                WrapContents = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
        }

        private static void AddRow(TableLayoutPanel table, Control control)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.RowCount = table.RowStyles.Count;
            control.Dock = DockStyle.Fill;
            table.Controls.Add(control, 0, table.RowCount - 1);
        }

        private static Label NewLabel(string text, float size, Color color)
        {
            return new Label { Text = text, AutoSize = true, ForeColor = color, Font = Theme.Sans(size), Anchor = AnchorStyles.Left };
        }

        private static Label AddTotalRow(TableLayoutPanel table, string caption, float size, bool bold, Color color)
        {
            int row = table.RowCount;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.RowCount = row + 1;
            var left = new Label { Text = caption, AutoSize = true, ForeColor = Theme.Ink, Font = Theme.Sans(size > 12 ? 11f : size, bold ? FontStyle.Bold : FontStyle.Regular), Anchor = AnchorStyles.Left };
            var right = new Label { Text = Money.Peso(0m), AutoSize = true, ForeColor = color, Font = Theme.Sans(size, bold ? FontStyle.Bold : FontStyle.Regular), Anchor = AnchorStyles.Right };
            table.Controls.Add(left, 0, row);
            table.Controls.Add(right, 1, row);
            return right;
        }

        // ---------------------------------------------------------------- behaviour

        private void DecimalOnly(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            bool isDot = e.KeyChar == '.';
            if (!char.IsDigit(e.KeyChar) && !isDot) e.Handled = true;
            if (isDot && _cashBox.Text.Contains(".")) e.Handled = true;
        }

        private void EnterToSeal(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || !_seal.Enabled) return;
            e.SuppressKeyPress = true;
            _seal.PerformClick();
        }

        private void UpdateView()
        {
            if (_seal == null) return;

            OrderTotals totals = _payments.Compute(_lines);
            _subtotal.Text = Money.Peso(totals.Subtotal);
            _discount.Text = totals.DiscountAmount > 0m ? "-" + Money.Peso(totals.DiscountAmount) : Money.Peso(0m);
            _total.Text = Money.Peso(totals.Total);

            bool needsId = _payments.SelectedDiscount.RequiresIdNumber;
            _idGroup.Visible = needsId;
            if (needsId) _idLabel.Text = _payments.SelectedDiscount.IdLabel;
            else if (_idBox.Text.Length > 0) _idBox.Clear();

            bool isCash = _payments.Method == PaymentMethod.Cash;
            _cashGroup.Visible = isCash;
            _gcashGroup.Visible = !isCash;
            RebuildQuickButtons(totals.Total);

            Payment payment;
            string error;
            bool paymentOk = _payments.TryBuildPayment(totals.Total, _cashBox.Text, _refBox.Text, out payment, out error);
            string discountError;
            bool discountOk = _payments.IsDiscountAllowed(_idBox.Text, out discountError);
            bool hasInput = isCash ? _cashBox.Text.Length > 0 : _refBox.Text.Length > 0;

            decimal cash;
            _change.Text = isCash && totals.Total > 0m && Money.TryParse(_cashBox.Text, out cash) && cash >= totals.Total
                ? Money.Peso(cash - totals.Total)
                : Money.Peso(0m);

            if (!discountOk && totals.Total > 0m) _error.Text = discountError;
            else if (!paymentOk && hasInput && totals.Total > 0m) _error.Text = error;
            else _error.Text = string.Empty;

            _seal.Enabled = paymentOk && discountOk;
        }

        private void RebuildQuickButtons(decimal total)
        {
            if (_quickFor == total) return;
            _quickFor = total;

            foreach (Control old in _quickFlow.Controls.Cast<Control>().ToList()) old.Dispose();
            _quickFlow.Controls.Clear();
            if (total <= 0m) return;

            foreach (decimal amount in _payments.QuickCashAmounts(total))
            {
                decimal value = amount;
                var button = new Button { Text = value == total ? "Exact" : Money.Peso(value).Replace(".00", "") };
                Theme.SmallButton(button);
                button.Click += (s, e) => _cashBox.Text = value.ToString("0.##", CultureInfo.InvariantCulture);
                _quickFlow.Controls.Add(button);
            }
        }

        private void OnSeal(object sender, EventArgs e)
        {
            if (Transactions == null)
            {
                MessageBox.Show("Checkout is not connected to a TransactionController yet.");
                return;
            }

            OrderTotals totals = _payments.Compute(_lines);
            Payment payment;
            string error;
            if (!_payments.TryBuildPayment(totals.Total, _cashBox.Text, _refBox.Text, out payment, out error))
            {
                _error.Text = error;
                return;
            }

            _seal.Enabled = false;   // blocks a double-click from selling twice
            try
            {
                Receipt receipt = Transactions.Checkout(_lines, _payments.SelectedDiscount, _idBox.Text, payment, CashierName, _studentName);

                using (var form = new ReceiptForm(receipt, false))
                    form.ShowDialog(FindForm());

                if (Transactions.StorageWarning != null)
                    MessageBox.Show("The sale was completed, but it could not be saved to disk.\n\n" + Transactions.StorageWarning,
                                    "Not saved", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                ResetInputs();
                var handler = SaleCompleted;
                if (handler != null) handler(this, receipt);
            }
            catch (InvalidOperationException ex)
            {
                _error.Text = ex.Message;
                UpdateView();
            }
        }

        private void ResetInputs()
        {
            _cashBox.Clear();
            _refBox.Clear();
            _idBox.Clear();
            _lines = new List<ReceiptLine>();
            _studentName = null;
            ((RadioButton)_discountFlow.Controls[0]).Checked = true;   // back to "None"
            UpdateView();
        }
    }
}
