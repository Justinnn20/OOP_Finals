using System;
using System.Drawing;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using UMakCoop.Controllers;
using UMakCoop.Models;
using UMakCoop.Services;

namespace UMakCoop.Views
{
    /// <summary>Transaction Logs: list of receipts with search, status filter, view receipt, and (Admin) void.</summary>
    public class TransactionsView : UserControl
    {
        private TransactionController _transactions;
        private string _userName = "";
        private bool _isAdmin;

        private TextBox _search;
        private ComboBox _status;
        private CheckBox _today;
        private Button _viewButton, _voidButton, _summaryButton, _exportButton;
        private Label _todayLabel, _warning;
        private DataGridView _grid;
        private readonly ToolTip _tip = new ToolTip();

        public TransactionsView()
        {
            BuildUi();
        }

        /// <summary>Call once from MainForm after login.</summary>
        public void Attach(TransactionController controller, string userName, bool isAdmin)
        {
            if (_transactions != null)
            {
                _transactions.ReceiptIssued -= OnChanged;
                _transactions.ReceiptVoided -= OnChanged;
            }
            _transactions = controller;
            _transactions.ReceiptIssued += OnChanged;
            _transactions.ReceiptVoided += OnChanged;
            SetUser(userName, isAdmin);
        }

        /// <summary>Call again when the user signs in or out. A Cashier cannot void.</summary>
        public void SetUser(string userName, bool isAdmin)
        {
            _userName = userName;
            _isAdmin = isAdmin;
            _voidButton.Visible = isAdmin;
            ReloadList();
        }

        private void OnChanged(object sender, Receipt receipt)
        {
            ReloadList();
        }

        // ---------------------------------------------------------------- UI

        private void BuildUi()
        {
            BackColor = Theme.Wine;
            Padding = new Padding(14);

            var title = new Label
            {
                Text = "TRANSACTION LOGS",
                AutoSize = true,
                Font = Theme.Serif(18f, FontStyle.Bold),
                ForeColor = Theme.GoldHover
            };
            _todayLabel = new Label { AutoSize = true, ForeColor = Theme.Parchment, Font = Theme.Sans(10f), Anchor = AnchorStyles.Right };

            var header = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(0, 0, 0, 8) };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.Controls.Add(title, 0, 0);
            header.Controls.Add(_todayLabel, 1, 0);

            _search = new TextBox { Width = 240, Font = Theme.Sans(10f), Anchor = AnchorStyles.Left };
            _search.TextChanged += (s, e) => ReloadList();
            _tip.SetToolTip(_search, "Search by receipt no, student, cashier, discount ID no, discount, or GCash reference");

            _status = new ComboBox { Width = 120, DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Sans(10f), Anchor = AnchorStyles.Left };
            _status.Items.AddRange(new object[] { "All status", "Completed", "Voided" });
            _status.SelectedIndex = 0;
            _status.SelectedIndexChanged += (s, e) => ReloadList();

            _today = new CheckBox { Text = "Today only", AutoSize = true, ForeColor = Theme.Parchment, Font = Theme.Sans(10f), Anchor = AnchorStyles.Left };
            _today.CheckedChanged += (s, e) => ReloadList();

            _viewButton = new Button { Text = "View receipt", Width = 120, Height = 32, Anchor = AnchorStyles.Right };
            _voidButton = new Button { Text = "Void", Width = 90, Height = 32, Anchor = AnchorStyles.Right, Visible = false };
            _summaryButton = new Button { Text = "Daily summary", Width = 130, Height = 32, Anchor = AnchorStyles.Right };
            _exportButton = new Button { Text = "Export CSV", Width = 110, Height = 32, Anchor = AnchorStyles.Right };
            Theme.GoldButton(_viewButton, 10f);
            Theme.GoldButton(_voidButton, 10f);
            Theme.GoldButton(_summaryButton, 10f);
            Theme.GoldButton(_exportButton, 10f);
            _viewButton.Click += (s, e) => ShowSelectedReceipt();
            _voidButton.Click += OnVoid;
            _summaryButton.Click += OnSummary;
            _exportButton.Click += OnExport;

            _warning = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 40,
                ForeColor = Color.FromArgb(255, 170, 170),
                Font = Theme.Sans(9.5f),
                Visible = false
            };

            var filters = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 8, Padding = new Padding(0, 0, 0, 8) };
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 4; i++) filters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            filters.Controls.Add(_search, 0, 0);
            filters.Controls.Add(_status, 1, 0);
            filters.Controls.Add(_today, 2, 0);
            filters.Controls.Add(_summaryButton, 4, 0);
            filters.Controls.Add(_exportButton, 5, 0);
            filters.Controls.Add(_viewButton, 6, 0);
            filters.Controls.Add(_voidButton, 7, 0);

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Theme.WineDark,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 34,
                GridColor = Color.FromArgb(90, 60, 30),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            _grid.RowTemplate.Height = 30;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Theme.Gold;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Theme.Wine;
            _grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Theme.Gold;
            _grid.ColumnHeadersDefaultCellStyle.Font = Theme.Serif(10f, FontStyle.Bold);
            _grid.DefaultCellStyle.BackColor = Theme.Wine;
            _grid.DefaultCellStyle.ForeColor = Theme.Parchment;
            _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(107, 78, 26);
            _grid.DefaultCellStyle.SelectionForeColor = Color.White;
            _grid.DefaultCellStyle.Font = Theme.Sans(10f);
            _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(36, 13, 22);

            AddColumn("no", "Receipt", 130);
            AddColumn("time", "Time", 110);
            AddColumn("student", "Student", 120);
            AddColumn("cashier", "Cashier", 100);
            AddColumn("items", "Items", 45);
            AddColumn("discount", "Discount", 150);
            AddColumn("pay", "Payment", 75);
            AddColumn("total", "Total", 100);
            AddColumn("status", "Status", 90);
            _grid.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ShowSelectedReceipt(); };

            // Fill first, then edge-docked controls (WinForms docks the last-added first, so Top ones stack in reverse).
            Controls.Add(_grid);
            Controls.Add(_warning);
            Controls.Add(filters);
            Controls.Add(header);
        }

        private void AddColumn(string name, string header, float weight)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = header, FillWeight = weight });
        }

        // ---------------------------------------------------------------- behaviour

        public void ReloadList()
        {
            if (_transactions == null) return;

            Receipt keep = SelectedReceipt();

            _grid.Rows.Clear();
            foreach (Receipt r in CurrentList())
            {
                int index = _grid.Rows.Add(
                    r.ReceiptNo,
                    r.IssuedAt.ToString("MMM d, h:mm tt", CultureInfo.InvariantCulture),
                    r.StudentName ?? "Walk-in",
                    r.CashierName,
                    r.ItemCount,
                    r.DiscountAmount > 0m ? r.Discount.ToString() : "-",
                    r.Payment.Method.ToString(),
                    Money.Peso(r.Total),
                    r.Status.ToString());

                DataGridViewRow row = _grid.Rows[index];
                row.Tag = r;
                if (r.Status == ReceiptStatus.Voided) row.DefaultCellStyle.ForeColor = Color.Gray;
                if (keep != null && r.ReceiptNo == keep.ReceiptNo) row.Selected = true;
            }

            _warning.Text = _transactions.StorageWarning ?? "";
            _warning.Visible = _transactions.StorageWarning != null;

            _todayLabel.Text = "Today: " + Money.Peso(_transactions.TodaySales) + "  ·  " + _transactions.TodayCount + " orders";
        }

        /// <summary>What the list is showing right now (search text, status and Today filters applied).</summary>
        private IList<Receipt> CurrentList()
        {
            ReceiptStatus? status = null;
            if (_status.SelectedIndex == 1) status = ReceiptStatus.Completed;
            else if (_status.SelectedIndex == 2) status = ReceiptStatus.Voided;
            return _transactions.Search(_search.Text, status, _today.Checked);
        }

        private Receipt SelectedReceipt()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            return _grid.SelectedRows[0].Tag as Receipt;
        }

        private void ShowSelectedReceipt()
        {
            Receipt receipt = SelectedReceipt();
            if (receipt == null) return;
            using (var form = new ReceiptForm(receipt, true))   // viewing an old receipt is always a reprint
                form.ShowDialog(FindForm());
        }

        private void OnVoid(object sender, EventArgs e)
        {
            Receipt receipt = SelectedReceipt();
            if (_transactions == null || receipt == null) return;

            if (receipt.Status == ReceiptStatus.Voided)
            {
                MessageBox.Show(receipt.ReceiptNo + " is already voided.");
                return;
            }

            string reason = PromptDialog.Ask(FindForm(), "Void " + receipt.ReceiptNo,
                "Reason for voiding " + Money.Peso(receipt.Total) + " (required). Items return to stock.");
            if (string.IsNullOrWhiteSpace(reason)) return;

            try
            {
                _transactions.Void(receipt.ReceiptNo, _userName, _isAdmin, reason.Trim());
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is InvalidOperationException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSummary(object sender, EventArgs e)
        {
            if (_transactions == null) return;
            using (var form = new SummaryForm(_transactions))
                form.ShowDialog(FindForm());
        }

        /// <summary>Exports exactly the receipts currently listed, so the filters decide what goes in the file.</summary>
        private void OnExport(object sender, EventArgs e)
        {
            if (_transactions == null) return;
            IList<Receipt> rows = CurrentList();
            if (rows.Count == 0)
            {
                MessageBox.Show("There are no transactions in the list to export.");
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Export transactions";
                dialog.Filter = "CSV file (*.csv)|*.csv";
                dialog.FileName = "transactions-" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;

                try
                {
                    CsvExporter.Write(dialog.FileName, rows);
                    MessageBox.Show("Exported " + rows.Count + " transaction(s) to:\n" + dialog.FileName);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    MessageBox.Show("Could not save the file. If it is open in Excel, close it and try again.\n\n" + ex.Message);
                }
            }
        }
    }
}
