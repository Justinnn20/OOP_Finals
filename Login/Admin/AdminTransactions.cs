using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Login
{
    public partial class AdminTransactions : Form
    {
        // Theme Palette (matches AdminDB & AdminSettings)
        private readonly Color clrBgDark = Color.FromArgb(42, 10, 20);
        private readonly Color clrNavDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrParchment = Color.FromArgb(244, 227, 178);
        private readonly Color clrSelectedRow = Color.FromArgb(236, 208, 134);
        private readonly Color clrInputBg = Color.FromArgb(250, 241, 214);
        private readonly Color clrInputBorder = Color.FromArgb(182, 160, 114);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrTextDark = Color.FromArgb(44, 14, 22);
        private readonly Color clrCrimson = Color.FromArgb(174, 34, 48);
        private readonly Color clrMuted = Color.FromArgb(124, 102, 78);
        private readonly Color clrDivider = Color.FromArgb(216, 198, 152);
        private readonly Color clrPaidGreen = Color.FromArgb(118, 144, 78);

        private Button activeNavButton;
        private string activeMethodFilter = "ALL";
        private bool isSearchPlaceholder = true;
        private int currentPage = 1;
        private const int PageSize = 6;

        private class ReceiptLineItem
        {
            public int Qty { get; set; }
            public string ItemName { get; set; }
            public decimal LineTotal { get; set; }
        }

        private class TransactionRecord
        {
            public string ReceiptNo { get; set; }
            public string DateLabel { get; set; }
            public string TimeLabel { get; set; }
            public string Cashier { get; set; }
            public string Method { get; set; } // "Cash" or "GCash"
            public string RefNo { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Discount { get; set; }
            public decimal Total { get; set; }
            public bool IsVoid { get; set; }
            public List<ReceiptLineItem> Items { get; set; }
        }

        private List<TransactionRecord> transactions;
        private TransactionRecord selectedTxn;

        public AdminTransactions()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            EnableDoubleBuffering(pnlTxnList);
            EnableDoubleBuffering(pnlReceiptBody);

            activeNavButton = btnNavTransactions;
            lblDate.Text = DateTime.Now.ToString("dddd · MMM dd, yyyy").ToUpper();

            InitSampleTransactions();
            if (transactions.Count > 0)
            {
                selectedTxn = transactions[0];
            }

            WireEvents();
            RefreshKpiSummary();
            RenderTransactionTable();
            pnlReceiptBody.Invalidate();
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitSampleTransactions()
        {
            string todayShort = DateTime.Now.ToString("MMM dd, yyyy");

            transactions = new List<TransactionRecord>
            {
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1042",
                    DateLabel = todayShort,
                    TimeLabel = "02:15 PM",
                    Cashier = "Maria Santos",
                    Method = "GCash",
                    RefNo = "9048215",
                    Subtotal = 510.00m,
                    Discount = 30.00m,
                    Total = 480.00m,
                    IsVoid = false,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 1, ItemName = "UMak Polo (L)", LineTotal = 350.00m },
                        new ReceiptLineItem { Qty = 2, ItemName = "Blue Book", LineTotal = 40.00m },
                        new ReceiptLineItem { Qty = 1, ItemName = "CCIS Lab Manual", LineTotal = 120.00m }
                    }
                },
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1041",
                    DateLabel = todayShort,
                    TimeLabel = "01:48 PM",
                    Cashier = "Maria Santos",
                    Method = "Cash",
                    RefNo = "",
                    Subtotal = 150.00m,
                    Discount = 0.00m,
                    Total = 150.00m,
                    IsVoid = false,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 1, ItemName = "CCIS Lab Manual", LineTotal = 120.00m },
                        new ReceiptLineItem { Qty = 1, ItemName = "Yellow Pad", LineTotal = 30.00m }
                    }
                },
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1040",
                    DateLabel = todayShort,
                    TimeLabel = "01:12 PM",
                    Cashier = "Mark Villanueva",
                    Method = "Cash",
                    RefNo = "",
                    Subtotal = 620.00m,
                    Discount = 0.00m,
                    Total = 620.00m,
                    IsVoid = false,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 1, ItemName = "Calculus Textbook", LineTotal = 450.00m },
                        new ReceiptLineItem { Qty = 1, ItemName = "PE Uniform (M)", LineTotal = 170.00m }
                    }
                },
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1039",
                    DateLabel = todayShort,
                    TimeLabel = "12:30 PM",
                    Cashier = "Maria Santos",
                    Method = "GCash",
                    RefNo = "7731902",
                    Subtotal = 240.00m,
                    Discount = 0.00m,
                    Total = 240.00m,
                    IsVoid = true,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 2, ItemName = "CCIS Lab Manual", LineTotal = 240.00m }
                    }
                },
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1038",
                    DateLabel = todayShort,
                    TimeLabel = "11:55 AM",
                    Cashier = "Mark Villanueva",
                    Method = "Cash",
                    RefNo = "",
                    Subtotal = 310.00m,
                    Discount = 0.00m,
                    Total = 310.00m,
                    IsVoid = false,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 1, ItemName = "Blouse (Female)", LineTotal = 280.00m },
                        new ReceiptLineItem { Qty = 1, ItemName = "Yellow Pad", LineTotal = 30.00m }
                    }
                },
                new TransactionRecord
                {
                    ReceiptNo = "TXN-1037",
                    DateLabel = todayShort,
                    TimeLabel = "11:10 AM",
                    Cashier = "Maria Santos",
                    Method = "GCash",
                    RefNo = "6610488",
                    Subtotal = 890.00m,
                    Discount = 0.00m,
                    Total = 890.00m,
                    IsVoid = false,
                    Items = new List<ReceiptLineItem>
                    {
                        new ReceiptLineItem { Qty = 2, ItemName = "UMak Polo (L)", LineTotal = 700.00m },
                        new ReceiptLineItem { Qty = 1, ItemName = "PE Uniform (M)", LineTotal = 190.00m }
                    }
                }
            };
        }

        private void WireEvents()
        {
            // Top Navbar
            pnlTopNav.Paint += (s, e) =>
            {
                using (Pen goldLine = new Pen(Color.FromArgb(140, 108, 42), 1.5f))
                {
                    e.Graphics.DrawLine(goldLine, 0, pnlTopNav.Height - 1, pnlTopNav.Width, pnlTopNav.Height - 1);
                }
            };

            pnlLogoIcon.Paint += PnlLogoIcon_Paint;
            pnlNotif.Paint += PnlNotif_Paint;
            pnlAvatar.Paint += PnlAvatar_Paint;

            pnlNotif.Click += (s, e) =>
            {
                MessageBox.Show("You have 2 unread inventory alerts:\n• 3 items are Out of Stock\n• 12 items are at or below reorder level",
                    "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            Button[] navButtons = { btnNavDashboard, btnNavInventory, btnNavTransactions, btnNavReports, btnNavSettings };
            foreach (Button btn in navButtons)
            {
                btn.Paint += NavButton_Paint;
                btn.Click += NavButton_Click;
            }

            btnSignOut.Paint += BtnSignOut_Paint;
            btnSignOut.Click += BtnSignOut_Click;

            // KPI Cards & Icons
            pnlCardSales.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardSales.ClientRectangle, 16);
            pnlCardCash.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardCash.ClientRectangle, 16);
            pnlCardGcash.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardGcash.ClientRectangle, 16);
            pnlCardVoid.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardVoid.ClientRectangle, 16);

            pnlIconSales.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconSales.ClientRectangle, "peso");
            pnlIconCash.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconCash.ClientRectangle, "cash");
            pnlIconGcash.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconGcash.ClientRectangle, "gcash");
            pnlIconVoid.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconVoid.ClientRectangle, "void");

            // History Card & Receipt Card
            pnlHistoryCard.Paint += PnlHistoryCard_Paint;
            pnlReceiptCard.Paint += (s, e) =>
            {
                DrawParchmentCard(e.Graphics, pnlReceiptCard.ClientRectangle, 16);
                using (Pen divPen = new Pen(clrDivider, 1f))
                {
                    e.Graphics.DrawLine(divPen, 18, 40, pnlReceiptCard.Width - 18, 40);
                }
            };

            pnlReceiptBody.Paint += PnlReceiptBody_Paint;

            // Search Box
            pnlSearchBox.Paint += (s, e) =>
            {
                DrawInputContainer(e.Graphics, pnlSearchBox.ClientRectangle, 10);
                using (Pen iconPen = new Pen(clrMuted, 1.6f))
                {
                    e.Graphics.DrawEllipse(iconPen, pnlSearchBox.Width - 28, 8, 11, 11);
                    e.Graphics.DrawLine(iconPen, pnlSearchBox.Width - 18, 18, pnlSearchBox.Width - 13, 23);
                }
            };

            txtSearchTxn.Enter += (s, e) =>
            {
                if (isSearchPlaceholder)
                {
                    isSearchPlaceholder = false;
                    txtSearchTxn.Text = "";
                    txtSearchTxn.Font = new Font("Georgia", 9F, FontStyle.Regular);
                    txtSearchTxn.ForeColor = clrTextDark;
                }
            };

            txtSearchTxn.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchTxn.Text))
                {
                    isSearchPlaceholder = true;
                    txtSearchTxn.Text = "Search receipt # or cashier...";
                    txtSearchTxn.Font = new Font("Georgia", 9F, FontStyle.Italic);
                    txtSearchTxn.ForeColor = clrMuted;
                }
            };

            txtSearchTxn.TextChanged += (s, e) =>
            {
                if (!isSearchPlaceholder)
                {
                    currentPage = 1;
                    RenderTransactionTable();
                }
            };

            // Filter Pills
            btnFilterAll.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterAll, activeMethodFilter == "ALL");
            btnFilterCash.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterCash, activeMethodFilter == "CASH");
            btnFilterGcash.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterGcash, activeMethodFilter == "GCASH");

            btnFilterAll.Click += (s, e) => SetMethodFilter("ALL");
            btnFilterCash.Click += (s, e) => SetMethodFilter("CASH");
            btnFilterGcash.Click += (s, e) => SetMethodFilter("GCASH");

            // Export Button
            btnExport.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnExport, 14);
            btnExport.Click += BtnExport_Click;

            // Pagination
            btnPagePrev.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RenderTransactionTable();
                }
            };

            btnPageNext.Click += (s, e) =>
            {
                int totalFiltered = GetFilteredTransactions().Count;
                int maxPage = Math.Max(1, (int)Math.Ceiling((double)totalFiltered / PageSize));
                if (currentPage < maxPage)
                {
                    currentPage++;
                    RenderTransactionTable();
                }
            };

            // Receipt Action Buttons
            btnPrintReceipt.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnPrintReceipt, 16);
            btnPrintReceipt.Click += BtnPrintReceipt_Click;

            btnVoidSale.Paint += (s, e) => DrawCrimsonPillButton(e.Graphics, btnVoidSale, 16);
            btnVoidSale.Click += BtnVoidSale_Click;
        }

        // ==========================================================
        // NAVIGATION & ACTIONS
        // ==========================================================

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || clicked == btnNavTransactions) return;

            if (clicked == btnNavDashboard)
            {
                SwitchForm(new AdminDB());
            }
            else if (clicked == btnNavSettings)
            {
                SwitchForm(new AdminSettings());
            }
            else if (clicked == btnNavInventory)
            {
                SwitchForm(new AdminInventory());
            }
            else if (clicked == btnNavReports)
            {
                SwitchForm(new AdminReports());
            }
        }

        private void BtnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?", "Sign Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SwitchForm(new LoginPage());
            }
        }

        private void SwitchForm(Form targetForm)
        {
            targetForm.StartPosition = FormStartPosition.Manual;
            targetForm.Location = this.Location;
            targetForm.Size = this.Size;
            targetForm.Show();
            this.Hide();
            targetForm.FormClosed += (s, args) => this.Close();
        }

        // ==========================================================
        // KPI SUMMARY & TABLE RENDERING
        // ==========================================================

        private void RefreshKpiSummary()
        {
            decimal paidTotal = 0m;
            decimal cashTotal = 0m;
            decimal gcashTotal = 0m;
            decimal voidTotal = 0m;

            int completedCount = 0;
            int cashCount = 0;
            int gcashCount = 0;
            int voidCount = 0;

            foreach (TransactionRecord t in transactions)
            {
                if (t.IsVoid)
                {
                    voidCount++;
                    voidTotal += t.Total;
                }
                else
                {
                    completedCount++;
                    paidTotal += t.Total;

                    if (t.Method.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                    {
                        cashCount++;
                        cashTotal += t.Total;
                    }
                    else if (t.Method.Equals("GCash", StringComparison.OrdinalIgnoreCase))
                    {
                        gcashCount++;
                        gcashTotal += t.Total;
                    }
                }
            }

            lblSalesValue.Text = string.Format("₱ {0:N0}", paidTotal);
            lblSalesSub.Text = string.Format("{0} completed orders", completedCount);

            lblCashValue.Text = string.Format("₱ {0:N0}", cashTotal);
            lblCashSub.Text = string.Format("{0} transactions", cashCount);

            lblGcashValue.Text = string.Format("₱ {0:N0}", gcashTotal);
            lblGcashSub.Text = string.Format("{0} transactions", gcashCount);

            lblVoidValue.Text = string.Format("{0} {1}", voidCount, voidCount == 1 ? "ORDER" : "ORDERS");
            lblVoidSub.Text = string.Format("₱ {0:N0} reversed", voidTotal);
        }

        private void SetMethodFilter(string method)
        {
            activeMethodFilter = method;
            currentPage = 1;
            btnFilterAll.Invalidate();
            btnFilterCash.Invalidate();
            btnFilterGcash.Invalidate();
            RenderTransactionTable();
        }

        private List<TransactionRecord> GetFilteredTransactions()
        {
            string query = isSearchPlaceholder ? "" : txtSearchTxn.Text.Trim().ToLower();
            List<TransactionRecord> result = new List<TransactionRecord>();

            foreach (TransactionRecord t in transactions)
            {
                bool matchesMethod = (activeMethodFilter == "ALL") ||
                    t.Method.Equals(activeMethodFilter, StringComparison.OrdinalIgnoreCase);

                bool matchesSearch = string.IsNullOrEmpty(query) ||
                    t.ReceiptNo.ToLower().Contains(query) ||
                    t.Cashier.ToLower().Contains(query) ||
                    t.Method.ToLower().Contains(query);

                if (matchesMethod && matchesSearch)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        private void RenderTransactionTable()
        {
            pnlTxnList.SuspendLayout();
            pnlTxnList.Controls.Clear();

            List<TransactionRecord> filtered = GetFilteredTransactions();
            int totalFiltered = filtered.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalFiltered / PageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            int startIndex = (currentPage - 1) * PageSize;
            int countToShow = Math.Min(PageSize, totalFiltered - startIndex);

            int rowHeight = 49;

            if (countToShow <= 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "No matching transactions found.",
                    Font = new Font("Georgia", 10F, FontStyle.Italic),
                    ForeColor = clrMuted,
                    AutoSize = false,
                    Size = new Size(pnlTxnList.Width, 60),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                pnlTxnList.Controls.Add(lblEmpty);
            }
            else
            {
                for (int i = 0; i < countToShow; i++)
                {
                    TransactionRecord txn = filtered[startIndex + i];
                    bool isSelected = (selectedTxn != null && selectedTxn.ReceiptNo == txn.ReceiptNo);
                    bool isLast = (i == countToShow - 1);
                    Color rowBg = isSelected ? clrSelectedRow : clrParchment;

                    Panel row = new Panel
                    {
                        Location = new Point(0, i * rowHeight),
                        Size = new Size(pnlTxnList.Width, rowHeight),
                        BackColor = rowBg,
                        Cursor = Cursors.Hand
                    };

                    row.Click += (s, e) =>
                    {
                        selectedTxn = txn;
                        RenderTransactionTable();
                        pnlReceiptBody.Invalidate();
                    };

                    row.Paint += (s, e) =>
                    {
                        Graphics g = e.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // Status Pill Badge
                        Rectangle badgeRect = new Rectangle(702, 13, 76, 22);
                        Color bBg = txn.IsVoid ? clrCrimson : clrPaidGreen;
                        string bText = txn.IsVoid ? "VOID" : "PAID";

                        using (GraphicsPath bPath = GetRoundedRectPath(badgeRect, 11))
                        using (SolidBrush bBrush = new SolidBrush(bBg))
                        {
                            g.FillPath(bBrush, bPath);
                        }

                        TextRenderer.DrawText(g, bText, new Font("Segoe UI", 7.25F, FontStyle.Bold),
                            badgeRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                        if (!isLast)
                        {
                            using (Pen divPen = new Pen(clrDivider, 1f))
                            {
                                g.DrawLine(divPen, 0, rowHeight - 1, row.Width, rowHeight - 1);
                            }
                        }
                    };

                    Label lblReceipt = CreateRowLabel(txn.ReceiptNo, new Point(8, 14), new Size(148, 20), rowBg, true, txn);
                    Label lblTime = CreateRowLabel(txn.TimeLabel, new Point(164, 14), new Size(116, 20), rowBg, false, txn);
                    Label lblCashier = CreateRowLabel(txn.Cashier, new Point(288, 14), new Size(176, 20), rowBg, false, txn);
                    Label lblMethod = CreateRowLabel(txn.Method, new Point(472, 14), new Size(92, 20), rowBg, false, txn);
                    Label lblTotal = CreateRowLabel(string.Format("₱ {0:N2}", txn.Total), new Point(568, 14), new Size(122, 20), rowBg, true, txn);
                    lblTotal.TextAlign = ContentAlignment.MiddleRight;

                    row.Controls.Add(lblReceipt);
                    row.Controls.Add(lblTime);
                    row.Controls.Add(lblCashier);
                    row.Controls.Add(lblMethod);
                    row.Controls.Add(lblTotal);

                    pnlTxnList.Controls.Add(row);
                }
            }

            lblHistoryShowing.Text = string.Format("Showing {0} of {1} transactions", countToShow, totalFiltered);
            lblPageNum.Text = string.Format("{0} / {1}", currentPage, totalPages);
            btnPagePrev.Enabled = (currentPage > 1);
            btnPageNext.Enabled = (currentPage < totalPages);

            pnlTxnList.ResumeLayout();
        }

        private Label CreateRowLabel(string text, Point loc, Size sz, Color bg, bool bold, TransactionRecord txn)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = loc,
                Size = sz,
                BackColor = bg,
                ForeColor = clrTextDark,
                Font = new Font("Georgia", 9.5F, bold ? FontStyle.Bold : FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Cursor = Cursors.Hand
            };

            lbl.Click += (s, e) =>
            {
                selectedTxn = txn;
                RenderTransactionTable();
                pnlReceiptBody.Invalidate();
            };

            return lbl;
        }

        // ==========================================================
        // RECEIPT PREVIEW, PRINT, VOID & EXPORT
        // ==========================================================

        private void PnlReceiptBody_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(clrParchment);

            if (selectedTxn == null)
            {
                using (Font f = new Font("Georgia", 9.5F, FontStyle.Italic))
                using (SolidBrush br = new SolidBrush(clrMuted))
                {
                    g.DrawString("Select a transaction to view receipt details.", f, br, 16, 24);
                }
                return;
            }

            int w = pnlReceiptBody.Width;

            using (Font hdrFont = new Font("Georgia", 13F, FontStyle.Bold))
            using (Font metaFont = new Font("Georgia", 9F, FontStyle.Regular))
            using (Font itemFont = new Font("Georgia", 9.25F, FontStyle.Regular))
            using (Font totalFont = new Font("Georgia", 9.5F, FontStyle.Regular))
            using (Font grandFont = new Font("Georgia", 10.5F, FontStyle.Bold))
            using (SolidBrush darkBrush = new SolidBrush(clrTextDark))
            using (SolidBrush mutedBrush = new SolidBrush(Color.FromArgb(88, 66, 48)))
            using (SolidBrush crimsonBrush = new SolidBrush(clrCrimson))
            using (Pen divPen = new Pen(clrDivider, 1.2f))
            {
                // Receipt # & Status Pill
                g.DrawString(selectedTxn.ReceiptNo, hdrFont, darkBrush, 4, 4);

                Rectangle pillRect = new Rectangle(w - 72, 6, 68, 22);
                Color pBg = selectedTxn.IsVoid ? clrCrimson : clrPaidGreen;
                using (GraphicsPath pPath = GetRoundedRectPath(pillRect, 11))
                using (SolidBrush pBrush = new SolidBrush(pBg))
                {
                    g.FillPath(pBrush, pPath);
                }
                TextRenderer.DrawText(g, selectedTxn.IsVoid ? "VOID" : "PAID",
                    new Font("Segoe UI", 7.25F, FontStyle.Bold), pillRect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // Metadata
                g.DrawString(string.Format("{0} · {1}", selectedTxn.DateLabel, selectedTxn.TimeLabel), metaFont, mutedBrush, 4, 34);
                g.DrawString(string.Format("Cashier: {0}", selectedTxn.Cashier), metaFont, darkBrush, 4, 54);

                string payText = selectedTxn.Method.Equals("GCash", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(selectedTxn.RefNo)
                    ? string.Format("Payment: GCash (Ref: {0})", selectedTxn.RefNo)
                    : string.Format("Payment: {0}", selectedTxn.Method);
                g.DrawString(payText, metaFont, darkBrush, 4, 74);

                // Divider 1
                g.DrawLine(divPen, 4, 102, w - 4, 102);

                // Itemized Lines
                int y = 114;
                foreach (ReceiptLineItem line in selectedTxn.Items)
                {
                    string leftTxt = string.Format("{0}x {1}", line.Qty, line.ItemName);
                    string rightTxt = string.Format("₱ {0:N2}", line.LineTotal);

                    g.DrawString(leftTxt, itemFont, darkBrush, new Rectangle(4, y, w - 96, 20));
                    SizeF rSz = g.MeasureString(rightTxt, itemFont);
                    g.DrawString(rightTxt, itemFont, darkBrush, w - 4 - rSz.Width, y);

                    y += 24;
                }

                // Divider 2
                int div2Y = Math.Max(214, y + 10);
                g.DrawLine(divPen, 4, div2Y, w - 4, div2Y);

                // Totals
                int totY = div2Y + 12;
                g.DrawString("Subtotal", totalFont, mutedBrush, 4, totY);
                string subStr = string.Format("₱ {0:N2}", selectedTxn.Subtotal);
                g.DrawString(subStr, totalFont, mutedBrush, w - 4 - g.MeasureString(subStr, totalFont).Width, totY);

                totY += 22;
                g.DrawString("Discount", totalFont, mutedBrush, 4, totY);
                string discStr = selectedTxn.Discount > 0 ? string.Format("- ₱ {0:N2}", selectedTxn.Discount) : "₱ 0.00";
                g.DrawString(discStr, totalFont, mutedBrush, w - 4 - g.MeasureString(discStr, totalFont).Width, totY);

                totY += 26;
                g.DrawString("TOTAL PAID", grandFont, darkBrush, 4, totY);
                string paidStr = string.Format("₱ {0:N2}", selectedTxn.Total);
                g.DrawString(paidStr, grandFont, crimsonBrush, w - 4 - g.MeasureString(paidStr, grandFont).Width, totY);
            }
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (selectedTxn == null)
            {
                MessageBox.Show("Please select a transaction first.", "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UNIVERSITY OF MAKATI COOP");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine("Receipt #: " + selectedTxn.ReceiptNo);
            sb.AppendLine("Date/Time: " + selectedTxn.DateLabel + " · " + selectedTxn.TimeLabel);
            sb.AppendLine("Cashier:   " + selectedTxn.Cashier);
            sb.AppendLine("Payment:   " + selectedTxn.Method + (string.IsNullOrEmpty(selectedTxn.RefNo) ? "" : " (Ref: " + selectedTxn.RefNo + ")"));
            sb.AppendLine("Status:    " + (selectedTxn.IsVoid ? "VOID" : "PAID"));
            sb.AppendLine("----------------------------------------");
            foreach (ReceiptLineItem item in selectedTxn.Items)
            {
                sb.AppendLine(string.Format("{0}x {1,-22} ₱ {2:N2}", item.Qty, item.ItemName, item.LineTotal));
            }
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(string.Format("Subtotal:   ₱ {0:N2}", selectedTxn.Subtotal));
            sb.AppendLine(string.Format("Discount: - ₱ {0:N2}", selectedTxn.Discount));
            sb.AppendLine(string.Format("TOTAL PAID: ₱ {0:N2}", selectedTxn.Total));

            MessageBox.Show(sb.ToString(), "Receipt Preview - " + selectedTxn.ReceiptNo,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnVoidSale_Click(object sender, EventArgs e)
        {
            if (selectedTxn == null) return;

            if (selectedTxn.IsVoid)
            {
                MessageBox.Show(string.Format("Transaction {0} is already VOID.", selectedTxn.ReceiptNo),
                    "Void Sale", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show(
                string.Format("Void transaction {0} worth ₱ {1:N2}?\nThis will reverse it from today's completed sales.",
                    selectedTxn.ReceiptNo, selectedTxn.Total),
                "Confirm Void Sale",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                selectedTxn.IsVoid = true;
                RefreshKpiSummary();
                RenderTransactionTable();
                pnlReceiptBody.Invalidate();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            List<TransactionRecord> filtered = GetFilteredTransactions();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Export Transactions to CSV";
                sfd.Filter = "CSV File (*.csv)|*.csv";
                sfd.FileName = "UMakCoop_Transactions_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            sw.WriteLine("ReceiptNo,Date,Time,Cashier,Method,RefNo,Subtotal,Discount,Total,Status");
                            foreach (TransactionRecord t in filtered)
                            {
                                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",{6:F2},{7:F2},{8:F2},\"{9}\"",
                                    t.ReceiptNo, t.DateLabel, t.TimeLabel, t.Cashier, t.Method, t.RefNo,
                                    t.Subtotal, t.Discount, t.Total, t.IsVoid ? "VOID" : "PAID"));
                            }
                        }

                        MessageBox.Show("Transactions exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Export failed: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ==========================================================
        // CUSTOM PAINTING HELPERS
        // ==========================================================

        private void PnlHistoryCard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawParchmentCard(g, pnlHistoryCard.ClientRectangle, 16);

            using (Font hdrFont = new Font("Georgia", 8.5F, FontStyle.Bold))
            using (SolidBrush hdrBrush = new SolidBrush(clrTextDark))
            {
                g.DrawString("RECEIPT #", hdrFont, hdrBrush, 26, 90);
                g.DrawString("TIME", hdrFont, hdrBrush, 182, 90);
                g.DrawString("CASHIER", hdrFont, hdrBrush, 306, 90);
                g.DrawString("METHOD", hdrFont, hdrBrush, 490, 90);
                g.DrawString("TOTAL", hdrFont, hdrBrush, 656, 90);
                g.DrawString("STATUS", hdrFont, hdrBrush, 728, 90);
            }

            using (Pen divPen = new Pen(clrDivider, 1.2f))
            {
                g.DrawLine(divPen, 18, 113, pnlHistoryCard.Width - 18, 113);
                g.DrawLine(divPen, 18, 415, pnlHistoryCard.Width - 18, 415);
            }
        }

        private void DrawParchmentCard(Graphics g, Rectangle bounds, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1);

            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush bgBrush = new SolidBrush(clrParchment))
            using (Pen borderPen = new Pen(clrGoldBorder, 1.5f))
            {
                g.FillPath(bgBrush, path);
                g.DrawPath(borderPen, path);
            }
        }

        private void DrawInputContainer(Graphics g, Rectangle bounds, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle rect = new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush bg = new SolidBrush(clrInputBg))
            using (Pen border = new Pen(clrInputBorder, 1.2f))
            {
                g.FillPath(bg, path);
                g.DrawPath(border, path);
            }
        }

        private void DrawFilterPill(Graphics g, Button btn, bool isActive)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, 14))
            {
                if (isActive)
                {
                    using (SolidBrush fill = new SolidBrush(clrBgDark))
                        g.FillPath(fill, path);
                }
                else
                {
                    using (Pen border = new Pen(clrTextDark, 1.2f))
                        g.DrawPath(border, path);
                }
            }

            Color txtColor = isActive ? Color.FromArgb(245, 232, 200) : clrTextDark;
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, txtColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawGoldPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush fill = new SolidBrush(clrGold))
            using (Pen border = new Pen(Color.FromArgb(148, 102, 26), 1.2f))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }

            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawCrimsonPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush fill = new SolidBrush(clrCrimson))
            {
                g.FillPath(fill, path);
            }

            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawKpiCircleIcon(Graphics g, Rectangle bounds, string iconType)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circle = new Rectangle(2, 2, bounds.Width - 5, bounds.Height - 5);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(46, 14, 22)))
            using (Pen goldPen = new Pen(clrGoldBorder, 1.5f))
            {
                g.FillEllipse(darkBrush, circle);
                g.DrawEllipse(goldPen, circle);
            }

            using (Pen iconPen = new Pen(clrGold, 1.8f))
            {
                if (iconType == "peso")
                {
                    g.DrawEllipse(iconPen, 15, 15, 20, 20);
                    TextRenderer.DrawText(g, "₱", new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        new Rectangle(15, 14, 20, 22), clrGold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else if (iconType == "cash")
                {
                    g.DrawRectangle(iconPen, 15, 18, 20, 14);
                    g.DrawEllipse(iconPen, 22, 22, 6, 6);
                }
                else if (iconType == "gcash")
                {
                    g.DrawEllipse(iconPen, 15, 15, 20, 20);
                    TextRenderer.DrawText(g, "G", new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        new Rectangle(15, 14, 20, 22), clrGold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else if (iconType == "void")
                {
                    g.DrawEllipse(iconPen, 15, 15, 20, 20);
                    g.DrawLine(iconPen, 21, 21, 29, 29);
                    g.DrawLine(iconPen, 29, 21, 21, 29);
                }
            }
        }

        private void NavButton_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrNavDark);

            bool isActive = (btn == activeNavButton);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);

            if (isActive)
            {
                using (GraphicsPath path = GetRoundedRectPath(rect, 10))
                using (SolidBrush fill = new SolidBrush(clrGold))
                {
                    g.FillPath(fill, path);
                }
            }

            Color txtColor = isActive ? clrTextDark : Color.FromArgb(238, 224, 195);
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, txtColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void BtnSignOut_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrNavDark);

            Rectangle rect = new Rectangle(1, 1, btnSignOut.Width - 3, btnSignOut.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, 18))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.DrawPath(border, path);
            }

            TextRenderer.DrawText(g, btnSignOut.Text, btnSignOut.Font, rect, clrGold,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlLogoIcon_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point[] shieldPoints =
            {
                new Point(19, 3),
                new Point(34, 8),
                new Point(33, 25),
                new Point(19, 38),
                new Point(5, 25),
                new Point(4, 8)
            };

            using (SolidBrush fill = new SolidBrush(Color.FromArgb(52, 16, 26)))
            using (Pen goldPen = new Pen(clrGold, 1.8f))
            {
                g.FillPolygon(fill, shieldPoints);
                g.DrawPolygon(goldPen, shieldPoints);
            }

            TextRenderer.DrawText(g, "U", new Font("Georgia", 13F, FontStyle.Bold),
                new Rectangle(0, 2, 38, 34), clrGold,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlNotif_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circle = new Rectangle(3, 5, 36, 36);
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(54, 16, 26)))
            using (Pen border = new Pen(Color.FromArgb(135, 102, 46), 1.2f))
            {
                g.FillEllipse(bg, circle);
                g.DrawEllipse(border, circle);
            }

            using (Pen bellPen = new Pen(clrGold, 1.5f))
            {
                g.DrawArc(bellPen, 16, 15, 10, 12, 180, 180);
                g.DrawLine(bellPen, 15, 26, 27, 26);
                g.DrawArc(bellPen, 19, 25, 4, 4, 0, 180);
            }

            Rectangle badge = new Rectangle(26, 2, 17, 17);
            using (SolidBrush redBrush = new SolidBrush(Color.FromArgb(196, 42, 54)))
            {
                g.FillEllipse(redBrush, badge);
            }
            TextRenderer.DrawText(g, "2", new Font("Segoe UI", 7.5F, FontStyle.Bold),
                badge, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(1, 1, 37, 37);
            using (SolidBrush goldBrush = new SolidBrush(clrGold))
            {
                g.FillEllipse(goldBrush, rect);
            }

            TextRenderer.DrawText(g, "AD", new Font("Georgia", 10.5F, FontStyle.Bold),
                rect, clrTextDark, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}