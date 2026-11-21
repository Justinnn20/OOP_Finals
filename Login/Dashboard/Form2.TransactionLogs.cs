using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Login
{
    partial class Form2
    {
        private Panel? pnlLogs;
        private Panel? pnlLogsHeader;
        private DataGridView? dgvOrders;
        private ComboBox? cboCashierFilter;
        private CheckBox? chkFilterDate;
        private DateTimePicker? dtpLogDate;
        private Label? lblLogsSummary;

        private const string AllCashiers = "All cashiers";

        /// <summary>
        /// Call this in the constructor, after InitializeComponent().
        /// </summary>
        private void BuildTransactionLogsPanel()
        {
            pnlLogs = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MaroonBg,
                Visible = false,
                Padding = new Padding(20, 16, 20, 16)
            };

            dgvOrders = CreateGrid();
            dgvOrders.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                string? orderNo = dgvOrders.Rows[e.RowIndex].Cells["OrderNo"].Value?.ToString();
                ShowOrderDetails(orderNo);
            };

            BuildLogsHeader();

            // Add the grid first, then the header (Dock.Top), so the header stays on top.
            pnlLogs.Controls.Add(dgvOrders);
            pnlLogs.Controls.Add(pnlLogsHeader!);

            pnlMain.Controls.Add(pnlLogs);
            pnlLogs.BringToFront();

            cboCashierFilter!.SelectedIndexChanged += (s, e) => LoadOrders();
            dtpLogDate!.ValueChanged += (s, e) => LoadOrders();
            chkFilterDate!.CheckedChanged += (s, e) =>
            {
                dtpLogDate.Enabled = chkFilterDate.Checked;
                LoadOrders();
            };
        }

        private void BuildLogsHeader()
        {
            pnlLogsHeader = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = PanelDark,
                Padding = new Padding(16, 12, 16, 12)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowCount = 3;

            // Row 1: Title
            var lblTitle = new Label
            {
                Text = "TRANSACTION LOGS",
                Font = new Font("Georgia", 16F, FontStyle.Bold),
                ForeColor = GoldLight,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 0, 0, 10)
            };
            layout.Controls.Add(lblTitle, 0, 0);

            // Row 2: Filters
            var filterRow = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0)
            };

            var lblCashier = new Label
            {
                Text = "Cashier:",
                Font = new Font("Segoe UI", 9F),
                ForeColor = MutedText,
                AutoSize = true,
                Margin = new Padding(0, 7, 6, 0)
            };

            cboCashierFilter = new ComboBox
            {
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = InputBg,
                ForeColor = CreamText,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 3, 18, 0)
            };

            chkFilterDate = new CheckBox
            {
                Text = "Filter by date",
                Font = new Font("Segoe UI", 9F),
                ForeColor = MutedText,
                AutoSize = true,
                Margin = new Padding(0, 6, 8, 0)
            };

            dtpLogDate = new DateTimePicker
            {
                Width = 160,
                Format = DateTimePickerFormat.Short,
                Enabled = false,
                Margin = new Padding(0, 3, 0, 0)
            };

            filterRow.Controls.Add(lblCashier);
            filterRow.Controls.Add(cboCashierFilter);
            filterRow.Controls.Add(chkFilterDate);
            filterRow.Controls.Add(dtpLogDate);
            layout.Controls.Add(filterRow, 0, 1);

            // Row 3: Summary
            lblLogsSummary = new Label
            {
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = CreamText,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };
            layout.Controls.Add(lblLogsSummary, 0, 2);

            pnlLogsHeader.Controls.Add(layout);
        }

        private DataGridView CreateGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = PanelDark,
                GridColor = InputBg,
                EnableHeadersVisualStyles = false,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = GoldAccent;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = DarkText;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.DefaultCellStyle.BackColor = PanelDark;
            grid.DefaultCellStyle.ForeColor = CreamText;
            grid.DefaultCellStyle.SelectionBackColor = GoldAccent;
            grid.DefaultCellStyle.SelectionForeColor = DarkText;

            grid.AlternatingRowsDefaultCellStyle.BackColor = InputBg;

            return grid;
        }

        public void ShowTransactionLogs()
        {
            RefreshCashierFilter();
            LoadOrders();
            pnlLogs!.Visible = true;
            pnlLogs.BringToFront();
        }

        public void HideTransactionLogs()
        {
            if (pnlLogs != null) pnlLogs.Visible = false;
        }

        private void RefreshCashierFilter()
        {
            if (cboCashierFilter == null) return;

            string? previous = cboCashierFilter.SelectedItem?.ToString();

            cboCashierFilter.Items.Clear();
            cboCashierFilter.Items.Add(AllCashiers);
            foreach (var name in TransactionStore.Records
                                                 .Select(r => r.CashierName)
                                                 .Distinct()
                                                 .OrderBy(n => n))
            {
                cboCashierFilter.Items.Add(name);
            }

            cboCashierFilter.SelectedItem = (previous != null && cboCashierFilter.Items.Contains(previous))
                ? previous
                : AllCashiers;
        }

        private void LoadOrders()
        {
            if (cboCashierFilter == null || chkFilterDate == null || dtpLogDate == null ||
                dgvOrders == null || lblLogsSummary == null)
                return;

            IEnumerable<TransactionRecord> query = TransactionStore.Records;

            string? selectedCashier = cboCashierFilter.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedCashier) && selectedCashier != AllCashiers)
                query = query.Where(r => r.CashierName == selectedCashier);

            if (chkFilterDate.Checked)
                query = query.Where(r => r.OrderDate.Date == dtpLogDate.Value.Date);

            var list = query.OrderByDescending(r => r.OrderDate).ToList();

            dgvOrders.DataSource = list.Select(r => new
            {
                OrderNo = r.OrderNo,
                Date = r.OrderDate.ToString("MMM dd, yyyy hh:mm tt"),
                Cashier = r.CashierName,
                Student = string.IsNullOrEmpty(r.StudentId) ? "-" : r.StudentId,
                Discount = r.DiscountType,
                Payment = r.PaymentMethod,
                Total = "\u20B1" + r.Total.ToString("N2")
            }).ToList();

            decimal sum = list.Sum(r => r.Total);
            lblLogsSummary.Text = $"{list.Count} order(s)  ·  Total sales: \u20B1{sum:N2}";
        }

        /// <summary>
        /// Opens a separate window showing the details and items of one order.
        /// </summary>
        private void ShowOrderDetails(string? orderNo)
        {
            var record = TransactionStore.Records.FirstOrDefault(r => r.OrderNo == orderNo);
            if (record == null) return;

            using var win = new Form
            {
                Text = $"Order {record.OrderNo}",
                Size = new Size(560, 460),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                BackColor = MaroonBg,
                ForeColor = CreamText,
                Font = new Font("Segoe UI", 9.5F)
            };

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16),
                BackColor = MaroonBg
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Row 1: Order info
            var info = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 10)
            };
            info.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AddInfoRow(info, "Date", record.OrderDate.ToString("MMM dd, yyyy hh:mm tt"));
            AddInfoRow(info, "Cashier", record.CashierName);
            AddInfoRow(info, "Student ID", string.IsNullOrEmpty(record.StudentId) ? "-" : record.StudentId);
            AddInfoRow(info, "Discount", record.DiscountType);
            AddInfoRow(info, "Payment", record.PaymentMethod);
            AddInfoRow(info, "Subtotal", "\u20B1" + record.Subtotal.ToString("N2"));
            AddInfoRow(info, "Discount amount", "\u20B1" + record.DiscountAmount.ToString("N2"));
            AddInfoRow(info, "Total", "\u20B1" + record.Total.ToString("N2"));
            AddInfoRow(info, "Cash received", "\u20B1" + record.CashReceived.ToString("N2"));
            AddInfoRow(info, "Change", "\u20B1" + record.Change.ToString("N2"));
            root.Controls.Add(info, 0, 0);

            // Row 2: Items
            var items = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = PanelDark,
                GridColor = InputBg,
                EnableHeadersVisualStyles = false
            };
            items.ColumnHeadersDefaultCellStyle.BackColor = GoldAccent;
            items.ColumnHeadersDefaultCellStyle.ForeColor = DarkText;
            items.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            items.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            items.DefaultCellStyle.BackColor = PanelDark;
            items.DefaultCellStyle.ForeColor = CreamText;
            items.DefaultCellStyle.SelectionBackColor = GoldAccent;
            items.DefaultCellStyle.SelectionForeColor = DarkText;
            items.AlternatingRowsDefaultCellStyle.BackColor = InputBg;

            items.DataSource = record.Items.Select(i => new
            {
                Item = i.Name,
                Qty = i.Qty,
                UnitPrice = "\u20B1" + i.UnitPrice.ToString("N2"),
                LineTotal = "\u20B1" + i.LineTotal.ToString("N2")
            }).ToList();
            root.Controls.Add(items, 0, 1);

            // Row 3: Close button
            var close = new Button
            {
                Text = "Close",
                Anchor = AnchorStyles.Right,
                AutoSize = false,
                Size = new Size(110, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = GoldAccent,
                ForeColor = DarkText,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                DialogResult = DialogResult.OK,
                Margin = new Padding(0, 10, 0, 0)
            };
            close.FlatAppearance.BorderSize = 0;
            root.Controls.Add(close, 0, 2);

            win.Controls.Add(root);
            win.AcceptButton = close;
            win.ShowDialog(FindForm());
        }

        private static void AddInfoRow(TableLayoutPanel table, string caption, string value)
        {
            int row = table.RowCount;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.RowCount = row + 1;

            var left = new Label
            {
                Text = caption,
                AutoSize = true,
                ForeColor = MutedText,
                Font = new Font("Segoe UI", 9.5F),
                Margin = new Padding(0, 2, 16, 2)
            };
            var right = new Label
            {
                Text = value,
                AutoSize = true,
                ForeColor = CreamText,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Margin = new Padding(0, 2, 0, 2)
            };

            table.Controls.Add(left, 0, row);
            table.Controls.Add(right, 1, row);
        }

        /// <summary>
        /// Call this from the checkout button after a successful payment.
        /// Uses the receipt order ID so the logs and the receipt match.
        /// </summary>
        public void RecordTransaction(string orderNo, string cashierName, string studentId, string discount,
            string paymentMethod, decimal subtotal, decimal discountAmount, decimal total,
            decimal cashReceived, List<TransactionItem> items)
        {
            var record = new TransactionRecord
            {
                OrderNo = orderNo,
                OrderDate = DateTime.Now,
                CashierName = cashierName,
                StudentId = studentId,
                DiscountType = discount,
                PaymentMethod = paymentMethod,
                Subtotal = subtotal,
                DiscountAmount = discountAmount,
                Total = total,
                CashReceived = cashReceived,
                Change = cashReceived - total,
                Items = items
            };

            TransactionStore.Add(record);

            if (pnlLogs != null && pnlLogs.Visible)
                LoadOrders();
        }
    }
}