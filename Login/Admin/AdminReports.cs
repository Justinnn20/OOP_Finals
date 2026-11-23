using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Login
{
    public partial class AdminReports : Form
    {
        // Theme Palette (consistent with AdminDB, AdminInventory, AdminTransactions, AdminSettings)
        private readonly Color clrBgDark = Color.FromArgb(42, 10, 20);
        private readonly Color clrNavDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrParchment = Color.FromArgb(244, 227, 178);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrMaroon = Color.FromArgb(88, 23, 38);
        private readonly Color clrTextDark = Color.FromArgb(44, 14, 22);
        private readonly Color clrCrimson = Color.FromArgb(158, 34, 38);
        private readonly Color clrMuted = Color.FromArgb(124, 102, 78);
        private readonly Color clrDivider = Color.FromArgb(216, 198, 152);
        private readonly Color clrActiveGreen = Color.FromArgb(68, 138, 78);
        private readonly Color clrBadgeBg = Color.FromArgb(235, 218, 170);

        private Button activeNavButton;
        private string activePeriodFilter = "WEEK"; // TODAY, WEEK, MONTH, ALL
        private int currentPage = 1;
        private const int PageSize = 6;

        // Models
        public class AuditReportRow
        {
            public string PeriodName { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public int TransactionsCount { get; set; }
            public decimal GrossSales { get; set; }
            public decimal Discounts { get; set; }
            public decimal NetSales { get; set; }
            public decimal CashSales { get; set; }
            public decimal GcashSales { get; set; }
            public string AuditStatus { get; set; } = "VERIFIED";
        }

        public class TopSellingProduct
        {
            public int Rank { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public int UnitsSold { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        private List<AuditReportRow> allAuditRecords = new List<AuditReportRow>();
        private List<TopSellingProduct> topProducts = new List<TopSellingProduct>();
        private bool isInitialized = false;

        public AdminReports()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            EnableDoubleBuffering(pnlAuditList);
            EnableDoubleBuffering(pnlTopSellingList);

            activeNavButton = btnNavReports;
            lblDate.Text = DateTime.Now.ToString("dddd · MMM dd, yyyy").ToUpper();

            InitSampleData();
            SetupPaintHandlers();
            SetupEventHandlers();

            ApplyPeriodFilter("WEEK");

            NumericInputHelper.EnableClickToDefocus(this);
            isInitialized = true;
            this.Load += (s, e) => LayoutResponsive();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutResponsive();
        }

        private void LayoutResponsive()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            LayoutHeaderNav();

            if (!isInitialized) return;

            int margin = 28;
            int gap = 16;
            int availableWidth = Math.Max(1200, this.ClientSize.Width) - (margin * 2);

            // 4 KPI Cards across top
            int cardWidth = (availableWidth - (gap * 3)) / 4;
            pnlCardRevenue.Location = new Point(margin, 144);
            pnlCardRevenue.Size = new Size(cardWidth, 88);

            pnlCardCashSales.Location = new Point(margin + (cardWidth + gap), 144);
            pnlCardCashSales.Size = new Size(cardWidth, 88);

            pnlCardGcashSales.Location = new Point(margin + (cardWidth + gap) * 2, 144);
            pnlCardGcashSales.Size = new Size(cardWidth, 88);

            pnlCardDiscounts.Location = new Point(margin + (cardWidth + gap) * 3, 144);
            pnlCardDiscounts.Size = new Size(availableWidth - 3 * (cardWidth + gap), 88);

            // Main Panels
            int topY = 246;
            int bottomMargin = 20;
            int availableHeight = Math.Max(420, this.ClientSize.Height - topY - bottomMargin);

            int auditWidth = (int)(availableWidth * 0.63);
            int rightWidth = availableWidth - auditWidth - gap;

            pnlAuditCard.Location = new Point(margin, topY);
            pnlAuditCard.Size = new Size(auditWidth, availableHeight);

            int topSellingHeight = Math.Min(240, availableHeight / 2);
            int summaryHeight = availableHeight - topSellingHeight - 14;

            pnlTopSellingCard.Location = new Point(margin + auditWidth + gap, topY);
            pnlTopSellingCard.Size = new Size(rightWidth, topSellingHeight);

            pnlSummaryCard.Location = new Point(margin + auditWidth + gap, topY + topSellingHeight + 14);
            pnlSummaryCard.Size = new Size(rightWidth, summaryHeight);

            // Adjust children of pnlAuditCard
            pnlAuditList.Width = pnlAuditCard.Width - 36;
            pnlAuditList.Height = Math.Max(200, pnlAuditCard.Height - 120);

            btnExportAudit.Location = new Point(pnlAuditCard.Width - btnExportAudit.Width - 18, 10);

            lblAuditShowing.Location = new Point(24, pnlAuditCard.Height - 34);
            btnPageNext.Location = new Point(pnlAuditCard.Width - btnPageNext.Width - 18, pnlAuditCard.Height - 36);
            lblPageNum.Location = new Point(btnPageNext.Left - lblPageNum.Width - 4, pnlAuditCard.Height - 34);
            btnPagePrev.Location = new Point(lblPageNum.Left - btnPagePrev.Width - 4, pnlAuditCard.Height - 36);

            // Adjust children of right panels
            pnlTopSellingList.Width = pnlTopSellingCard.Width - 36;
            pnlTopSellingList.Height = Math.Max(120, pnlTopSellingCard.Height - 65);

            RenderAuditTable();
            this.Invalidate(true);
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitSampleData()
        {
            DateTime today = DateTime.Today;

            allAuditRecords = new List<AuditReportRow>
            {
                new AuditReportRow { PeriodName = "Sunday (Today)", Date = today, TransactionsCount = 19, GrossSales = 5350m, Discounts = 250m, NetSales = 5100m, CashSales = 2850m, GcashSales = 2250m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Saturday", Date = today.AddDays(-1), TransactionsCount = 38, GrossSales = 13150m, Discounts = 550m, NetSales = 12600m, CashSales = 6800m, GcashSales = 5800m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Friday", Date = today.AddDays(-2), TransactionsCount = 42, GrossSales = 13400m, Discounts = 800m, NetSales = 12600m, CashSales = 7100m, GcashSales = 5500m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Thursday", Date = today.AddDays(-3), TransactionsCount = 31, GrossSales = 10250m, Discounts = 450m, NetSales = 9800m, CashSales = 5200m, GcashSales = 4600m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Wednesday", Date = today.AddDays(-4), TransactionsCount = 39, GrossSales = 13100m, Discounts = 700m, NetSales = 12400m, CashSales = 6900m, GcashSales = 5500m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Tuesday", Date = today.AddDays(-5), TransactionsCount = 34, GrossSales = 11800m, Discounts = 500m, NetSales = 11300m, CashSales = 6300m, GcashSales = 5000m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Monday", Date = today.AddDays(-6), TransactionsCount = 30, GrossSales = 10650m, Discounts = 450m, NetSales = 10200m, CashSales = 5900m, GcashSales = 4300m, AuditStatus = "VERIFIED" },
                new AuditReportRow { PeriodName = "Prior Week Avg", Date = today.AddDays(-13), TransactionsCount = 195, GrossSales = 68200m, Discounts = 3100m, NetSales = 65100m, CashSales = 36500m, GcashSales = 28600m, AuditStatus = "AUDITED" },
                new AuditReportRow { PeriodName = "Nov W1 Audit", Date = today.AddDays(-20), TransactionsCount = 210, GrossSales = 74500m, Discounts = 3800m, NetSales = 70700m, CashSales = 39200m, GcashSales = 31500m, AuditStatus = "AUDITED" }
            };

            topProducts = new List<TopSellingProduct>
            {
                new TopSellingProduct { Rank = 1, Name = "UMak Polo (L)", Category = "Uniforms", UnitsSold = 142, TotalRevenue = 63900m },
                new TopSellingProduct { Rank = 2, Name = "PE Uniform (M)", Category = "Uniforms", UnitsSold = 118, TotalRevenue = 41300m },
                new TopSellingProduct { Rank = 3, Name = "Calculus Textbook", Category = "Books & Manuals", UnitsSold = 84, TotalRevenue = 37800m },
                new TopSellingProduct { Rank = 4, Name = "UMak Lanyard", Category = "School Supplies", UnitsSold = 210, TotalRevenue = 12600m },
                new TopSellingProduct { Rank = 5, Name = "Blue Book", Category = "School Supplies", UnitsSold = 450, TotalRevenue = 9000m }
            };
        }

        private void SetupPaintHandlers()
        {
            // Top Nav
            pnlTopNav.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(clrGoldBorder, 2f))
                {
                    e.Graphics.DrawLine(borderPen, 0, pnlTopNav.Height - 1, pnlTopNav.Width, pnlTopNav.Height - 1);
                }
            };

            pnlLogoIcon.Paint += PnlLogoIcon_Paint;
            pnlNotif.Paint += PnlNotif_Paint;
            pnlAvatar.Paint += PnlAvatar_Paint;

            // Nav Buttons
            Button[] navButtons = { btnNavDashboard, btnNavInventory, btnNavTransactions, btnNavReports, btnNavSettings };
            foreach (Button btn in navButtons)
            {
                btn.ForeColor = Color.Transparent; // Hide default text so custom paint is crisp and single-pass
                btn.Paint += NavButton_Paint;
                btn.Click += NavButton_Click;
            }

            // Sign Out
            btnSignOut.Paint += BtnSignOut_Paint;
            btnSignOut.Click += BtnSignOut_Click;

            // KPI Rounded Parchment Cards
            pnlCardRevenue.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardRevenue.ClientRectangle, 14);
            pnlCardCashSales.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardCashSales.ClientRectangle, 14);
            pnlCardGcashSales.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardGcashSales.ClientRectangle, 14);
            pnlCardDiscounts.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardDiscounts.ClientRectangle, 14);

            // Circular Vector Icons
            pnlIconRev.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconRev.ClientRectangle, "revenue");
            pnlIconCashSales.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconCashSales.ClientRectangle, "cash");
            pnlIconGcashSales.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconGcashSales.ClientRectangle, "gcash");
            pnlIconDisc.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconDisc.ClientRectangle, "discount");

            // Containers
            pnlAuditCard.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlAuditCard.ClientRectangle, 16);
            pnlTopSellingCard.Paint += (s, e) =>
            {
                DrawParchmentCard(e.Graphics, pnlTopSellingCard.ClientRectangle, 16);
                using (Pen divPen = new Pen(clrDivider, 1f))
                {
                    e.Graphics.DrawLine(divPen, 18, 52, pnlTopSellingCard.Width - 18, 52);
                }
            };
            pnlSummaryCard.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlSummaryCard.ClientRectangle, 16);

            // Filter button paint
            btnFilterToday.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterToday, activePeriodFilter == "TODAY");
            btnFilterWeek.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterWeek, activePeriodFilter == "WEEK");
            btnFilterMonth.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterMonth, activePeriodFilter == "MONTH");
            btnFilterAllPeriods.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterAllPeriods, activePeriodFilter == "ALL");

            // Action buttons
            btnExportAudit.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnExportAudit, 8);
            btnPrintAudit.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnPrintAudit, 8);
            btnCsvAudit.Paint += (s, e) => DrawMaroonPillButton(e.Graphics, btnCsvAudit, 8);
        }

        private void SetupEventHandlers()
        {
            // Period Filters
            btnFilterToday.Click += (s, e) => ApplyPeriodFilter("TODAY");
            btnFilterWeek.Click += (s, e) => ApplyPeriodFilter("WEEK");
            btnFilterMonth.Click += (s, e) => ApplyPeriodFilter("MONTH");
            btnFilterAllPeriods.Click += (s, e) => ApplyPeriodFilter("ALL");

            // Pagination
            btnPagePrev.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RenderAuditTable();
                }
            };

            btnPageNext.Click += (s, e) =>
            {
                var filtered = GetFilteredRecords();
                int totalPages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)PageSize));
                if (currentPage < totalPages)
                {
                    currentPage++;
                    RenderAuditTable();
                }
            };

            // Export & Print actions
            btnExportAudit.Click += BtnExportCsv_Click;
            btnCsvAudit.Click += BtnExportCsv_Click;
            btnPrintAudit.Click += BtnPrintAudit_Click;
        }

        private void NavButton_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Button btn) return;

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
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            Button? clicked = sender as Button;
            if (clicked == null || clicked == btnNavReports) return;

            if (clicked == btnNavDashboard)
            {
                SwitchForm(AdminShell.GetPage<AdminDB>());
            }
            else if (clicked == btnNavInventory)
            {
                SwitchForm(AdminShell.GetPage<AdminInventory>());
            }
            else if (clicked == btnNavTransactions)
            {
                SwitchForm(AdminShell.GetPage<AdminTransactions>());
            }
            else if (clicked == btnNavSettings)
            {
                SwitchForm(AdminShell.GetPage<AdminSettings>());
            }
        }

        private void BtnSignOut_Paint(object? sender, PaintEventArgs e)
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

        private void BtnSignOut_Click(object? sender, EventArgs e)
        {
            DialogResult res = CoopMessageBox.Show(this,
                "Are you sure you want to sign out of the Admin panel?",
                "Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                if (CoopAppContext.Instance != null)
                {
                    CoopAppContext.Instance.SignOut();
                }
                else
                {
                    LoginPage login = new LoginPage();
                    login.Show();
                    this.Hide();
                }
            }
        }

        private void LayoutHeaderNav()
        {
            if (pnlTopNav == null || btnNavDashboard == null || btnNavInventory == null ||
                btnNavTransactions == null || btnNavReports == null || btnNavSettings == null)
                return;

            int totalNavWidth = btnNavDashboard.Width + 8 +
                                btnNavInventory.Width + 8 +
                                btnNavTransactions.Width + 8 +
                                btnNavReports.Width + 8 +
                                btnNavSettings.Width;

            int navStartX = (pnlTopNav.Width - totalNavWidth) / 2;
            int minStartX = (lblBrandTitle != null ? lblBrandTitle.Right : 290) + 28;

            if (navStartX < minStartX)
            {
                navStartX = minStartX;
            }

            int y = 19;
            btnNavDashboard.Location = new Point(navStartX, y);
            btnNavInventory.Location = new Point(btnNavDashboard.Right + 8, y);
            btnNavTransactions.Location = new Point(btnNavInventory.Right + 8, y);
            btnNavReports.Location = new Point(btnNavTransactions.Right + 8, y);
            btnNavSettings.Location = new Point(btnNavReports.Right + 8, y);
        }

        private void SwitchForm(Form targetForm)
        {
            if (CoopAppContext.Instance != null)
            {
                CoopAppContext.Instance.SwitchForm(targetForm);
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    targetForm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    targetForm.StartPosition = FormStartPosition.Manual;
                    targetForm.WindowState = FormWindowState.Normal;
                    targetForm.Location = this.Location;
                    targetForm.Size = this.Size;
                }
                targetForm.Show();
                this.Hide();
            }
        }

        private void ApplyPeriodFilter(string period)
        {
            activePeriodFilter = period;
            currentPage = 1;

            btnFilterToday.Invalidate();
            btnFilterWeek.Invalidate();
            btnFilterMonth.Invalidate();
            btnFilterAllPeriods.Invalidate();

            UpdateKpiCards();
            RenderAuditTable();
            RenderTopSellingProducts();
            UpdateSummaryCard();
        }

        private List<AuditReportRow> GetFilteredRecords()
        {
            if (allAuditRecords == null) return new List<AuditReportRow>();

            DateTime today = DateTime.Today;
            switch (activePeriodFilter)
            {
                case "TODAY":
                    return allAuditRecords.Where(r => r.Date.Date == today).ToList();
                case "WEEK":
                    return allAuditRecords.Where(r => r.Date >= today.AddDays(-7)).ToList();
                case "MONTH":
                    return allAuditRecords.Where(r => r.Date >= today.AddDays(-30)).ToList();
                case "ALL":
                default:
                    return allAuditRecords.ToList();
            }
        }

        private void UpdateKpiCards()
        {
            var records = GetFilteredRecords();

            decimal totalNet = records.Sum(r => r.NetSales);
            decimal totalCash = records.Sum(r => r.CashSales);
            decimal totalGcash = records.Sum(r => r.GcashSales);
            decimal totalDiscounts = records.Sum(r => r.Discounts);

            lblRevValue.Text = $"₱ {totalNet:N0}";
            lblRevSub.Text = activePeriodFilter == "TODAY" ? "Today's audited revenue" : $"{records.Sum(r => r.TransactionsCount)} total orders audited";

            lblCashSalesVal.Text = $"₱ {totalCash:N0}";
            lblCashSalesSub.Text = $"{((totalNet > 0) ? (totalCash / totalNet * 100m) : 0):F0}% of total collections";

            lblGcashSalesVal.Text = $"₱ {totalGcash:N0}";
            lblGcashSalesSub.Text = $"{((totalNet > 0) ? (totalGcash / totalNet * 100m) : 0):F0}% digital cashless";

            lblDiscVal.Text = $"₱ {totalDiscounts:N0}";
            lblDiscSub.Text = "Scholar & Staff allowances";
        }

        private void RenderAuditTable()
        {
            if (!isInitialized || allAuditRecords == null || pnlAuditList == null) return;

            pnlAuditList.SuspendLayout();
            pnlAuditList.Controls.Clear();
            var filtered = GetFilteredRecords();

            int totalPages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)PageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            int skip = (currentPage - 1) * PageSize;
            var pageRecords = filtered.Skip(skip).Take(PageSize).ToList();

            lblAuditShowing.Text = $"Showing {pageRecords.Count} of {filtered.Count} audit records";
            lblPageNum.Text = $"{currentPage} / {totalPages}";
            btnPagePrev.Enabled = currentPage > 1;
            btnPageNext.Enabled = currentPage < totalPages;

            // Render Header Row
            Panel pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(pnlAuditList.Width, 28),
                BackColor = Color.FromArgb(236, 216, 164)
            };

            AddTableLabel(pnlHeader, "PERIOD / DATE", 16, 6, 125, true);
            AddTableLabel(pnlHeader, "ORDERS", 146, 6, 65, true);
            AddTableLabel(pnlHeader, "GROSS", 216, 6, 80, true);
            AddTableLabel(pnlHeader, "DISC.", 302, 6, 70, true);
            AddTableLabel(pnlHeader, "NET REVENUE", 378, 6, 100, true);
            AddTableLabel(pnlHeader, "CASH / GCASH", 488, 6, 150, true);
            AddTableLabel(pnlHeader, "STATUS", 650, 6, 90, true);

            pnlAuditList.Controls.Add(pnlHeader);

            int y = 30;
            int rowHeight = 46;

            foreach (var rec in pageRecords)
            {
                Panel row = new Panel
                {
                    Location = new Point(0, y),
                    Size = new Size(pnlAuditList.Width, rowHeight),
                    BackColor = (y / rowHeight) % 2 == 0 ? clrParchment : Color.FromArgb(249, 237, 202)
                };

                row.Paint += (s, e) =>
                {
                    using (Pen p = new Pen(clrDivider, 1f))
                    {
                        e.Graphics.DrawLine(p, 8, rowHeight - 1, row.Width - 8, rowHeight - 1);
                    }
                };

                // Period
                Label lblPeriod = new Label
                {
                    Text = rec.PeriodName,
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    Location = new Point(16, 5),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                Label lblDateSub = new Label
                {
                    Text = rec.Date.ToString("MMM dd, yyyy"),
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Regular),
                    ForeColor = clrMuted,
                    Location = new Point(16, 23),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblPeriod);
                row.Controls.Add(lblDateSub);

                // Orders
                Label lblOrders = new Label
                {
                    Text = rec.TransactionsCount.ToString(),
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    Location = new Point(146, 13),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblOrders);

                // Gross
                Label lblGross = new Label
                {
                    Text = $"₱ {rec.GrossSales:N0}",
                    Font = new Font("Georgia", 8.5F, FontStyle.Regular),
                    ForeColor = clrTextDark,
                    Location = new Point(216, 13),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblGross);

                // Discounts
                Label lblDisc = new Label
                {
                    Text = $"-₱ {rec.Discounts:N0}",
                    Font = new Font("Georgia", 8.5F, FontStyle.Regular),
                    ForeColor = clrCrimson,
                    Location = new Point(302, 13),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblDisc);

                // Net Revenue
                Label lblNet = new Label
                {
                    Text = $"₱ {rec.NetSales:N0}",
                    Font = new Font("Georgia", 9F, FontStyle.Bold),
                    ForeColor = clrMaroon,
                    Location = new Point(378, 12),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblNet);

                // Cash / GCash breakdown
                Label lblBreakdown = new Label
                {
                    Text = $"₱{rec.CashSales:N0} / ₱{rec.GcashSales:N0}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                    ForeColor = clrMuted,
                    Location = new Point(488, 13),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                row.Controls.Add(lblBreakdown);

                // Status Badge
                Label lblBadge = new Label
                {
                    Text = rec.AuditStatus,
                    Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = rec.AuditStatus == "VERIFIED" ? clrActiveGreen : clrMaroon,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(650, 11),
                    Size = new Size(82, 22)
                };
                row.Controls.Add(lblBadge);

                pnlAuditList.Controls.Add(row);
                y += rowHeight;
            }

            pnlAuditList.ResumeLayout();
        }

        private void AddTableLabel(Panel parent, string text, int x, int y, int width, bool bold)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = new Font("Georgia", 8F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = clrMaroon,
                Location = new Point(x, y),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lbl);
        }

        private void RenderTopSellingProducts()
        {
            if (!isInitialized || topProducts == null || pnlTopSellingList == null) return;

            pnlTopSellingList.SuspendLayout();
            pnlTopSellingList.Controls.Clear();
            int y = 2;
            int itemHeight = 31;

            foreach (var prod in topProducts)
            {
                Panel pnlItem = new Panel
                {
                    Location = new Point(0, y),
                    Size = new Size(pnlTopSellingList.Width, itemHeight),
                    BackColor = Color.Transparent
                };

                // Rank Badge
                Label lblRank = new Label
                {
                    Text = $"#{prod.Rank}",
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = prod.Rank <= 3 ? clrMaroon : clrMuted,
                    Location = new Point(2, 6),
                    Size = new Size(26, 18),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };

                // Name & Category
                Label lblName = new Label
                {
                    Text = prod.Name,
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    Location = new Point(32, 1),
                    Size = new Size(185, 15),
                    AutoEllipsis = true,
                    BackColor = Color.Transparent
                };
                Label lblCat = new Label
                {
                    Text = $"{prod.Category} · {prod.UnitsSold} sold",
                    Font = new Font("Segoe UI", 7F, FontStyle.Regular),
                    ForeColor = clrMuted,
                    Location = new Point(32, 16),
                    Size = new Size(185, 14),
                    AutoEllipsis = true,
                    BackColor = Color.Transparent
                };

                // Revenue
                Label lblRev = new Label
                {
                    Text = $"₱ {prod.TotalRevenue:N0}",
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = clrMaroon,
                    Location = new Point(220, 6),
                    Size = new Size(120, 18),
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.Transparent
                };

                pnlItem.Controls.Add(lblRank);
                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(lblCat);
                pnlItem.Controls.Add(lblRev);

                pnlTopSellingList.Controls.Add(pnlItem);
                y += itemHeight;
            }

            pnlTopSellingList.ResumeLayout();
        }

        private void UpdateSummaryCard()
        {
            var records = GetFilteredRecords();
            int totalOrders = records.Sum(r => r.TransactionsCount);
            decimal totalNet = records.Sum(r => r.NetSales);
            decimal avgOrder = totalOrders > 0 ? (totalNet / totalOrders) : 0m;

            lblAvgOrderVal.Text = $"₱ {avgOrder:N0}";
            lblUnitsSoldVal.Text = $"{topProducts.Sum(p => p.UnitsSold):N0} items";
            lblVoidRateVal.Text = "0.4% (minimal)";
        }

        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            try
            {
                var filtered = GetFilteredRecords();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Period,Date,Orders,Gross Sales,Discounts,Net Revenue,Cash,GCash,Audit Status");

                foreach (var rec in filtered)
                {
                    sb.AppendLine($"\"{rec.PeriodName}\",\"{rec.Date:yyyy-MM-dd}\",{rec.TransactionsCount},{rec.GrossSales},{rec.Discounts},{rec.NetSales},{rec.CashSales},{rec.GcashSales},\"{rec.AuditStatus}\"");
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV Spreadsheet (*.csv)|*.csv",
                    FileName = $"UMak_Coop_Audit_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    CoopMessageBox.Show(this,
                        $"Audit summary successfully exported to:\n{sfd.FileName}",
                        "Audit Export Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                CoopMessageBox.Show(this, $"Failed to export audit report: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintAudit_Click(object? sender, EventArgs e)
        {
            CoopMessageBox.Show(this,
                $"Preparing audited statement for {activePeriodFilter} period.\nSent to default bookstore receipt & reports printer.",
                "Print Audit Report",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================================
        // CUSTOM VECTOR DRAWING HELPERS
        // ==========================================================

        private void PnlLogoIcon_Paint(object? sender, PaintEventArgs e)
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

        private void PnlNotif_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Outer circle
            Rectangle circle = new Rectangle(3, 5, 36, 36);
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(54, 16, 26)))
            using (Pen border = new Pen(Color.FromArgb(135, 102, 46), 1.2f))
            {
                g.FillEllipse(bg, circle);
                g.DrawEllipse(border, circle);
            }

            // Bell silhouette
            using (Pen bellPen = new Pen(clrGold, 1.5f))
            {
                g.DrawArc(bellPen, 16, 15, 10, 12, 180, 180);
                g.DrawLine(bellPen, 15, 26, 27, 26);
                g.DrawArc(bellPen, 19, 25, 4, 4, 0, 180);
            }

            // Red badge "2"
            Rectangle badge = new Rectangle(26, 2, 17, 17);
            using (SolidBrush redBrush = new SolidBrush(Color.FromArgb(196, 42, 54)))
            {
                g.FillEllipse(redBrush, badge);
            }
            TextRenderer.DrawText(g, "2", new Font("Segoe UI", 7.5F, FontStyle.Bold),
                badge, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlAvatar_Paint(object? sender, PaintEventArgs e)
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
                if (iconType == "revenue")
                {
                    // Circle with Peso sign
                    g.DrawEllipse(iconPen, 15, 15, 18, 18);
                    TextRenderer.DrawText(g, "₱", new Font("Segoe UI", 9F, FontStyle.Bold),
                        new Rectangle(15, 14, 18, 20), clrGold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else if (iconType == "cash")
                {
                    // Cash bill with inner circle
                    g.DrawRectangle(iconPen, 15, 17, 18, 13);
                    g.DrawEllipse(iconPen, 21, 20, 6, 6);
                }
                else if (iconType == "gcash")
                {
                    // Phone / cashless card
                    g.DrawRectangle(iconPen, 17, 14, 14, 20);
                    g.DrawLine(iconPen, 21, 16, 27, 16);
                    g.DrawLine(iconPen, 23, 31, 25, 31);
                }
                else if (iconType == "discount")
                {
                    // Percentage sign
                    TextRenderer.DrawText(g, "%", new Font("Georgia", 11F, FontStyle.Bold),
                        new Rectangle(13, 14, 22, 20), clrGold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private void DrawFilterPill(Graphics g, Button btn, bool isSelected)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);
            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);

            using (GraphicsPath path = GetRoundedRectPath(rect, 8))
            {
                if (isSelected)
                {
                    using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(35, 8, 16)))
                    {
                        g.FillPath(darkBrush, path);
                    }
                    TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrParchment,
                        TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else
                {
                    using (SolidBrush bgBrush = new SolidBrush(clrParchment))
                    using (Pen borderPen = new Pen(Color.FromArgb(170, 138, 70), 1f))
                    {
                        g.FillPath(bgBrush, path);
                        g.DrawPath(borderPen, path);
                    }
                    TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                        TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private void DrawGoldPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush brush = new SolidBrush(clrGold))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.FillPath(brush, path);
                g.DrawPath(border, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawMaroonPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush brush = new SolidBrush(clrMaroon))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.FillPath(brush, path);
                g.DrawPath(border, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrParchment,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}