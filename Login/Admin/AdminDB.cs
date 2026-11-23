using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Login.Database;

namespace Login
{
    public partial class AdminDB : Form
    {
        // Theme Palette
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

        private Button activeNavButton;
        private bool isDailyView = true;

        // Chart Data
        private readonly string[] dailyLabels = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
        private readonly float[] dailyValues = { 10200f, 11300f, 12400f, 9800f, 12600f, 5100f, 0f };
        private readonly int dailyHighlightIndex = 5; // SAT

        private readonly string[] weeklyLabels = { "WK 1", "WK 2", "WK 3", "WK 4" };
        private readonly float[] weeklyValues = { 54200f, 62800f, 58900f, 61500f };
        private readonly int weeklyHighlightIndex = 3; // WK 4

        // Restock Model & Sample Data
        private class RestockItem
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int ReorderAt { get; set; }
            public int CurrentStock { get; set; }
        }

        private class ActivityLogRecord
        {
            public string Timestamp { get; set; } = "";
            public string User { get; set; } = "";
            public string Role { get; set; } = ""; // "Staff" or "Admin"
            public string Module { get; set; } = ""; // "POS", "Inventory", "Accounts", "Reports", "Settings"
            public string Action { get; set; } = "";
            public string Status { get; set; } = ""; // "SUCCESS", "UPDATED", "RESTOCKED", "CREATED", "VOIDED"
        }

        private List<RestockItem> restockItems = new List<RestockItem>();
        private List<ActivityLogRecord> activityLogs = new List<ActivityLogRecord>();
        private string activeLogFilter = "ALL";
        private bool isInitialized = false;

        private WizardScrollBar scrollRestock = null!;
        private WizardScrollBar scrollActivity = null!;
        private Panel pnlRestockInner = null!;
        private Panel pnlActivityInner = null!;

        public AdminDB()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            EnableDoubleBuffering(pnlChartCanvas);
            EnableDoubleBuffering(pnlRestockList);
            EnableDoubleBuffering(pnlActivityList);

            activeNavButton = btnNavDashboard;
            lblDate.Text = DateTime.Now.ToString("dddd · MMM dd, yyyy").ToUpper();

<<<<<<< HEAD
            LoadDatabaseRestockData();
            activityLogs = new List<ActivityLogRecord>();
=======
            pnlRestockInner = new Panel
            {
                Location = new Point(0, 0),
                Width = Math.Max(100, pnlRestockList.Width - 14),
                BackColor = Color.Transparent
            };
            pnlRestockList.Controls.Add(pnlRestockInner);

            scrollRestock = new WizardScrollBar();
            pnlRestock.Controls.Add(scrollRestock);

            pnlActivityInner = new Panel
            {
                Location = new Point(0, 0),
                Width = Math.Max(100, pnlActivityList.Width - 14),
                BackColor = Color.Transparent
            };
            pnlActivityList.Controls.Add(pnlActivityInner);

            scrollActivity = new WizardScrollBar();
            pnlActivityLog.Controls.Add(scrollActivity);

            InitSampleRestockData();
            InitSampleActivityLogs();
>>>>>>> origin/FinalBranch
            WireEvents();
            PopulateRestockList();
            RenderActivityLogTable();
            UpdateKpiCounts();
            AlignSalesSubtitle();

            isInitialized = true;
            this.AutoScroll = false;
            this.Load += (s, e) => { AlignSalesSubtitle(); LayoutResponsive(); };
        }

        private void LoadDatabaseRestockData()
        {
            restockItems = CoopDatabase.LoadInventory().Select(product => new RestockItem
            {
                Name = product.Name,
                Category = product.Category,
                ReorderAt = 10,
                CurrentStock = product.Stock
            }).Where(item => item.CurrentStock <= item.ReorderAt).ToList();
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
            pnlCardSales.Location = new Point(margin, 162);
            pnlCardSales.Size = new Size(cardWidth, 96);

            pnlCardOrders.Location = new Point(margin + (cardWidth + gap), 162);
            pnlCardOrders.Size = new Size(cardWidth, 96);

            pnlCardLowStock.Location = new Point(margin + (cardWidth + gap) * 2, 162);
            pnlCardLowStock.Size = new Size(cardWidth, 96);

            pnlCardOutStock.Location = new Point(margin + (cardWidth + gap) * 3, 162);
            pnlCardOutStock.Size = new Size(availableWidth - 3 * (cardWidth + gap), 96);

            // 2 Middle Panels (Sales Overview & Restock)
            int topY = 276;
            int bottomMargin = 24;
            int availableHeight = Math.Max(580, this.ClientSize.Height - topY - bottomMargin);
            int midHeight = Math.Max(280, (int)(availableHeight * 0.48));

            int salesWidth = (int)(availableWidth * 0.59);
            int restockWidth = availableWidth - salesWidth - gap;

            pnlSalesOverview.Location = new Point(margin, topY);
            pnlSalesOverview.Size = new Size(salesWidth, midHeight);

            pnlRestock.Location = new Point(margin + salesWidth + gap, topY);
            pnlRestock.Size = new Size(restockWidth, midHeight);

            // Adjust children of pnlRestock
            pnlRestockList.Location = new Point(18, 48);
            pnlRestockList.Width = pnlRestock.Width - 36 - 14;
            pnlRestockList.Height = Math.Max(160, pnlRestock.Height - 85);
            if (scrollRestock != null)
            {
                scrollRestock.Location = new Point(pnlRestock.Width - 22, 48);
                scrollRestock.Height = pnlRestockList.Height;
                scrollRestock.Width = 10;
            }
            if (pnlRestockInner != null)
            {
                pnlRestockInner.Width = pnlRestockList.Width;
            }
            lblViewAll.Location = new Point(pnlRestock.Width - lblViewAll.Width - 18, pnlRestock.Height - 34);

            // Bottom Panel: System Activity Log Table spanning full available width
            int logY = topY + midHeight + 14;
            int logHeight = Math.Max(260, availableHeight - midHeight - 14);

            pnlActivityLog.Location = new Point(margin, logY);
            pnlActivityLog.Size = new Size(availableWidth, logHeight);

            pnlActivityList.Location = new Point(18, 76);
            pnlActivityList.Width = pnlActivityLog.Width - 36 - 14;
            pnlActivityList.Height = Math.Max(140, pnlActivityLog.Height - 92);
            if (scrollActivity != null)
            {
                scrollActivity.Location = new Point(pnlActivityLog.Width - 22, 76);
                scrollActivity.Height = pnlActivityList.Height;
                scrollActivity.Width = 10;
            }
            if (pnlActivityInner != null)
            {
                pnlActivityInner.Width = pnlActivityList.Width;
            }

            btnLogAdmin.Location = new Point(pnlActivityLog.Width - btnLogAdmin.Width - 18, 12);
            btnLogStaff.Location = new Point(btnLogAdmin.Left - btnLogStaff.Width - 8, 12);
            btnLogAll.Location = new Point(btnLogStaff.Left - btnLogAll.Width - 8, 12);

            lblActivityTitle.AutoSize = true;
            lblActivitySub.Left = lblActivityTitle.Right + 12;

            PopulateRestockList();
            RenderActivityLogTable();
            AlignSalesSubtitle();
            this.Invalidate(true);
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

        private void AlignSalesSubtitle()
        {
            lblWeeklyAvg.Left = lblWeeklyTotal.Right + 10;
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitSampleRestockData()
        {
            restockItems = new List<RestockItem>
            {
                new RestockItem { Name = "Calculus Textbook", Category = "Books", ReorderAt = 10, CurrentStock = 0 },
                new RestockItem { Name = "Blouse (Female)", Category = "Uniforms", ReorderAt = 10, CurrentStock = 0 },
                new RestockItem { Name = "Yellow Pad", Category = "Supplies", ReorderAt = 20, CurrentStock = 0 },
                new RestockItem { Name = "PE Uniform (M)", Category = "Uniforms", ReorderAt = 10, CurrentStock = 3 },
                new RestockItem { Name = "UMak Polo (L)", Category = "Uniforms", ReorderAt = 10, CurrentStock = 4 },
                new RestockItem { Name = "CCIS Lab Manual", Category = "Books", ReorderAt = 10, CurrentStock = 5 },
                new RestockItem { Name = "Blue Book", Category = "Supplies", ReorderAt = 20, CurrentStock = 6 }
            };
        }

        private void InitSampleActivityLogs()
        {
            activityLogs = new List<ActivityLogRecord>
            {
                new ActivityLogRecord { Timestamp = "11:42 AM", User = "Ana", Role = "Staff", Module = "POS", Action = "Completed Order #1004 (₱ 1,255.00 · 4 items)", Status = "SUCCESS" },
                new ActivityLogRecord { Timestamp = "11:15 AM", User = "Admin", Role = "Admin", Module = "Inventory", Action = "Restocked +20 units for 'Calculus Textbook' (New Stock: 20)", Status = "RESTOCKED" },
                new ActivityLogRecord { Timestamp = "10:50 AM", User = "Admin", Role = "Admin", Module = "Inventory", Action = "Updated unit price for 'PE Uniform (M)' to ₱ 350.00", Status = "UPDATED" },
                new ActivityLogRecord { Timestamp = "10:32 AM", User = "Ana", Role = "Staff", Module = "POS", Action = "Completed Order #1003 (₱ 450.00 · Senior 20% applied)", Status = "SUCCESS" },
                new ActivityLogRecord { Timestamp = "10:14 AM", User = "Ana", Role = "Staff", Module = "POS", Action = "Voided sale #1002 (Reason: Wrong uniform size chosen)", Status = "VOIDED" },
                new ActivityLogRecord { Timestamp = "09:45 AM", User = "Admin", Role = "Admin", Module = "Accounts", Action = "Created new staff login account for 'Carlos Mendoza'", Status = "CREATED" },
                new ActivityLogRecord { Timestamp = "08:50 AM", User = "Admin", Role = "Admin", Module = "Settings", Action = "Exported weekly transactions database backup archive", Status = "BACKUP" },
                new ActivityLogRecord { Timestamp = "08:00 AM", User = "Ana", Role = "Staff", Module = "POS", Action = "Shift sign-in at Cashier Terminal 1", Status = "SUCCESS" }
            };
        }

        private void WireEvents()
        {
            // Top Bar Paint & Icons
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
                CoopMessageBox.Show(this, "You have 2 unread inventory alerts:\n• 3 items are Out of Stock\n• 12 items are at or below reorder level",
                    "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Nav Buttons
            Button[] navButtons = { btnNavDashboard, btnNavInventory, btnNavTransactions, btnNavReports, btnNavSettings };
            foreach (Button btn in navButtons)
            {
                btn.ForeColor = Color.Transparent; // Hide default text so custom paint is crisp and single-pass
                btn.Paint += NavButton_Paint;
                btn.Click += NavButton_Click;
            }

            // Sign Out Button
            btnSignOut.Paint += BtnSignOut_Paint;
            btnSignOut.Click += BtnSignOut_Click;

            // Rounded Parchment Cards
            pnlCardSales.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardSales.ClientRectangle, 16);
            pnlCardOrders.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardOrders.ClientRectangle, 16);
            pnlCardLowStock.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardLowStock.ClientRectangle, 16);
            pnlCardOutStock.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardOutStock.ClientRectangle, 16);
            pnlSalesOverview.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlSalesOverview.ClientRectangle, 16);
            pnlRestock.Paint += (s, e) =>
            {
                DrawParchmentCard(e.Graphics, pnlRestock.ClientRectangle, 16);
                using (Pen divPen = new Pen(clrDivider, 1f))
                {
                    e.Graphics.DrawLine(divPen, 18, 44, pnlRestock.Width - 18, 44);
                    e.Graphics.DrawLine(divPen, 18, 385, pnlRestock.Width - 18, 385);
                }
            };

            // KPI Circular Icons
            pnlIconSales.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconSales.ClientRectangle, "peso");
            pnlIconOrders.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconOrders.ClientRectangle, "receipt");
            pnlIconLowStock.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconLowStock.ClientRectangle, "warning");
            pnlIconOutStock.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconOutStock.ClientRectangle, "out");

            // Daily / Weekly Toggle Buttons
            btnDaily.Paint += (s, e) => DrawTogglePill(e.Graphics, btnDaily, isDailyView);
            btnWeekly.Paint += (s, e) => DrawTogglePill(e.Graphics, btnWeekly, !isDailyView);

            btnDaily.Click += (s, e) =>
            {
                isDailyView = true;
                lblWeeklyTotal.Text = "₱ 61,500";
                lblWeeklyAvg.Text = "this week · avg ₱ 10,250 per day";
                AlignSalesSubtitle();
                btnDaily.Invalidate();
                btnWeekly.Invalidate();
                pnlChartCanvas.Invalidate();
            };

            btnWeekly.Click += (s, e) =>
            {
                isDailyView = false;
                lblWeeklyTotal.Text = "₱ 237,400";
                lblWeeklyAvg.Text = "this month · avg ₱ 59,350 per week";
                AlignSalesSubtitle();
                btnDaily.Invalidate();
                btnWeekly.Invalidate();
                pnlChartCanvas.Invalidate();
            };

            // Bar Chart Paint
            pnlChartCanvas.Paint += PnlChartCanvas_Paint;

            // View All Link
            lblViewAll.Click += (s, e) =>
            {
                SwitchForm(new AdminInventory());
            };

            // Activity Log Card & Filter Buttons
            pnlActivityLog.Paint += PnlActivityLog_Paint;

            btnLogAll.Paint += (s, e) => DrawTogglePill(e.Graphics, btnLogAll, activeLogFilter == "ALL");
            btnLogStaff.Paint += (s, e) => DrawTogglePill(e.Graphics, btnLogStaff, activeLogFilter == "STAFF");
            btnLogAdmin.Paint += (s, e) => DrawTogglePill(e.Graphics, btnLogAdmin, activeLogFilter == "ADMIN");

            btnLogAll.Click += (s, e) =>
            {
                activeLogFilter = "ALL";
                RenderActivityLogTable();
                btnLogAll.Invalidate();
                btnLogStaff.Invalidate();
                btnLogAdmin.Invalidate();
            };

            btnLogStaff.Click += (s, e) =>
            {
                activeLogFilter = "STAFF";
                RenderActivityLogTable();
                btnLogAll.Invalidate();
                btnLogStaff.Invalidate();
                btnLogAdmin.Invalidate();
            };

            btnLogAdmin.Click += (s, e) =>
            {
                activeLogFilter = "ADMIN";
                RenderActivityLogTable();
                btnLogAll.Invalidate();
                btnLogStaff.Invalidate();
                btnLogAdmin.Invalidate();
            };
        }

        // ==========================================================
        // NAVIGATION & ACTIONS
        // ==========================================================

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || clicked == btnNavDashboard) return;

            activeNavButton = clicked;
            btnNavDashboard.Invalidate();
            btnNavInventory.Invalidate();
            btnNavTransactions.Invalidate();
            btnNavReports.Invalidate();
            btnNavSettings.Invalidate();

            if (clicked == btnNavInventory)
            {
                SwitchForm(new AdminInventory());
            }
            else if (clicked == btnNavTransactions)
            {
                SwitchForm(new AdminTransactions());
            }
            else if (clicked == btnNavReports)
            {
                SwitchForm(new AdminReports());
            }
            else if (clicked == btnNavSettings)
            {
                SwitchForm(new AdminSettings());
            }
        }

        private void BtnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = CoopMessageBox.Show(this, "Are you sure you want to sign out?", "Sign Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (CoopAppContext.Instance != null)
                {
                    CoopAppContext.Instance.SignOut();
                }
                else
                {
                    SwitchForm(new LoginPage());
                }
            }
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

        // ==========================================================
        // ITEMS TO RESTOCK LIST
        // ==========================================================

        private void PopulateRestockList()
        {
            if (restockItems == null || pnlRestockList == null || pnlRestockInner == null) return;

            pnlRestockInner.SuspendLayout();
            pnlRestockInner.Controls.Clear();

            int yOffset = 0;
            int rowHeight = 47;

            for (int i = 0; i < restockItems.Count; i++)
            {
                RestockItem item = restockItems[i];
                bool isLast = (i == restockItems.Count - 1);

                Panel row = new Panel
                {
                    Location = new Point(0, yOffset),
                    Size = new Size(pnlRestockInner.Width, rowHeight),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = Color.Transparent
                };

                row.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Progress Bar Track
                    Rectangle trackRect = new Rectangle(2, 36, 218, 4);
                    using (SolidBrush trackBrush = new SolidBrush(Color.FromArgb(221, 203, 158)))
                    {
                        g.FillRectangle(trackBrush, trackRect);
                    }

                    // Progress Bar Fill
                    float ratio = item.ReorderAt > 0 ? Math.Min(1f, (float)item.CurrentStock / item.ReorderAt) : 0f;
                    int fillWidth = (int)(trackRect.Width * ratio);
                    if (fillWidth > 0)
                    {
                        Color fillColor = item.CurrentStock >= item.ReorderAt
                            ? Color.FromArgb(62, 130, 72)
                            : Color.FromArgb(196, 146, 42);
                        using (SolidBrush fillBrush = new SolidBrush(fillColor))
                        {
                            g.FillRectangle(fillBrush, trackRect.X, trackRect.Y, fillWidth, trackRect.Height);
                        }
                    }

                    // Status Pill (OUT / LOW / OK)
                    bool isOut = item.CurrentStock == 0;
                    bool isLow = item.CurrentStock > 0 && item.CurrentStock <= item.ReorderAt;

                    Color badgeBg = isOut ? clrCrimson : (isLow ? Color.FromArgb(234, 194, 88) : Color.FromArgb(62, 130, 72));
                    Color badgeFg = isLow ? clrTextDark : Color.White;
                    string badgeText = isOut ? "OUT" : (isLow ? "LOW" : "OK");

                    Rectangle badgeRect = new Rectangle(row.Width - 148, 11, 40, 20);
                    using (GraphicsPath badgePath = GetRoundedRectPath(badgeRect, 10))
                    using (SolidBrush bBrush = new SolidBrush(badgeBg))
                    {
                        g.FillPath(bBrush, badgePath);
                    }
                    TextRenderer.DrawText(g, badgeText, new Font("Segoe UI", 7.5F, FontStyle.Bold),
                        badgeRect, badgeFg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    // Divider line
                    if (!isLast)
                    {
                        using (Pen divPen = new Pen(clrDivider, 1f))
                        {
                            g.DrawLine(divPen, 0, rowHeight - 1, row.Width, rowHeight - 1);
                        }
                    }
                };

                Label lblName = new Label
                {
                    Text = item.Name,
                    Font = new Font("Georgia", 9.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    AutoSize = true,
                    Location = new Point(0, 3),
                    BackColor = Color.Transparent
                };

                Label lblMeta = new Label
                {
                    Text = string.Format("{0} · reorder at {1}", item.Category, item.ReorderAt),
                    Font = new Font("Georgia", 7.5F, FontStyle.Italic),
                    ForeColor = clrMuted,
                    AutoSize = true,
                    Location = new Point(1, 20),
                    BackColor = Color.Transparent
                };

                Label lblLeft = new Label
                {
                    Text = string.Format("{0} LEFT", item.CurrentStock),
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = item.CurrentStock == 0 ? clrCrimson : Color.FromArgb(138, 86, 18),
                    Location = new Point(row.Width - 214, 13),
                    Size = new Size(60, 16),
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

                Button btnRestockItem = new Button
                {
                    Text = "RESTOCK",
                    Font = new Font("Georgia", 7.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Location = new Point(row.Width - 100, 9),
                    Size = new Size(84, 24),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnRestockItem.FlatAppearance.BorderSize = 0;

                btnRestockItem.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    Rectangle rect = new Rectangle(0, 0, btnRestockItem.Width - 1, btnRestockItem.Height - 1);
                    using (GraphicsPath path = GetRoundedRectPath(rect, 6))
                    {
                        using (var brush = new LinearGradientBrush(rect, Color.FromArgb(238, 192, 60), Color.FromArgb(208, 150, 25), LinearGradientMode.Vertical))
                        {
                            g.FillPath(brush, path);
                        }
                        using (Pen border = new Pen(Color.FromArgb(250, 220, 110), 1f))
                        {
                            g.DrawPath(border, path);
                        }
                    }
                    TextRenderer.DrawText(g, btnRestockItem.Text, btnRestockItem.Font,
                        rect, Color.FromArgb(38, 10, 18), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };

                btnRestockItem.Click += (s, e) =>
                {
                    int addQty = item.ReorderAt;
                    DialogResult dr = CoopMessageBox.Show(this,
                        string.Format("Restock \"{0}\" by +{1} units?", item.Name, addQty),
                        "Quick Restock", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        item.CurrentStock += addQty;
                        PopulateRestockList();
                        UpdateKpiCounts();
                    }
                };

                row.Controls.Add(lblName);
                row.Controls.Add(lblMeta);
                row.Controls.Add(lblLeft);
                row.Controls.Add(btnRestockItem);

                pnlRestockInner.Controls.Add(row);
                yOffset += rowHeight;
            }

            pnlRestockInner.Height = yOffset;
            pnlRestockInner.ResumeLayout();

            if (scrollRestock != null)
            {
                scrollRestock.BindTo(pnlRestockList, pnlRestockInner);
            }
        }

        private void PnlActivityLog_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawParchmentCard(g, pnlActivityLog.ClientRectangle, 16);

            int cardW = pnlActivityLog.Width;

            using (Pen divPen = new Pen(clrDivider, 1f))
            {
                g.DrawLine(divPen, 18, 46, cardW - 18, 46);
            }

            // Table header bar
            Rectangle headerBar = new Rectangle(18, 47, cardW - 36, 26);
            using (SolidBrush hBrush = new SolidBrush(Color.FromArgb(232, 213, 163)))
            {
                g.FillRectangle(hBrush, headerBar);
            }

            using (Pen borderPen = new Pen(clrDivider, 1.2f))
            {
                g.DrawLine(borderPen, 18, 73, cardW - 18, 73);
            }

            // Headers
            int statW = 100;
            int statX = cardW - 18 - statW;
            int moduleW = 110;
            int roleW = 80;
            int userW = 100;
            int timeW = 90;
            int timeX = 26;
            int userX = timeX + timeW + 10;
            int roleX = userX + userW + 10;
            int moduleX = roleX + roleW + 10;
            int actionX = moduleX + moduleW + 10;
            int actionW = Math.Max(120, statX - actionX - 10);

            using (Font hFont = new Font("Georgia", 7.5F, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, "TIMESTAMP", hFont, new Rectangle(timeX, 47, timeW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "USER", hFont, new Rectangle(userX, 47, userW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "ROLE", hFont, new Rectangle(roleX, 47, roleW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "MODULE", hFont, new Rectangle(moduleX, 47, moduleW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "ACTION & SYSTEM UPDATE", hFont, new Rectangle(actionX, 47, actionW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "STATUS", hFont, new Rectangle(statX, 47, statW, 26),
                    clrTextDark, TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void RenderActivityLogTable()
        {
            if (activityLogs == null || pnlActivityList == null || pnlActivityInner == null) return;

            pnlActivityInner.SuspendLayout();
            pnlActivityInner.Controls.Clear();

            var filtered = activityLogs.Where(a =>
            {
                if (activeLogFilter == "STAFF") return a.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase);
                if (activeLogFilter == "ADMIN") return a.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                return true;
            }).ToList();

            int rowHeight = 36;
            int yOffset = 0;

            for (int i = 0; i < filtered.Count; i++)
            {
                var log = filtered[i];
                Panel row = new Panel
                {
                    Location = new Point(0, yOffset),
                    Size = new Size(pnlActivityInner.Width, rowHeight),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = (i % 2 == 0) ? Color.FromArgb(24, 180, 140, 60) : Color.Transparent
                };

                row.Paint += (s, e) =>
                {
                    using (Pen divPen = new Pen(clrDivider, 0.8f))
                    {
                        e.Graphics.DrawLine(divPen, 0, rowHeight - 1, row.Width, rowHeight - 1);
                    }
                };

                int rowW = row.Width;
                int statW = 100;
                int statX = rowW - statW;
                int moduleW = 110;
                int roleW = 80;
                int userW = 100;
                int timeW = 90;
                int timeX = 8;
                int userX = timeX + timeW + 10;
                int roleX = userX + userW + 10;
                int moduleX = roleX + roleW + 10;
                int actionX = moduleX + moduleW + 10;
                int actionW = Math.Max(120, statX - actionX - 10);

                Label lblTime = new Label
                {
                    Text = log.Timestamp,
                    Font = new Font("Georgia", 8.5F, FontStyle.Regular),
                    ForeColor = clrMuted,
                    Location = new Point(timeX, 8),
                    Size = new Size(timeW, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Label lblUser = new Label
                {
                    Text = log.User,
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    ForeColor = clrTextDark,
                    Location = new Point(userX, 8),
                    Size = new Size(userW, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Label lblRole = new Label
                {
                    Text = log.Role,
                    Font = new Font("Georgia", 8F, FontStyle.Italic),
                    ForeColor = log.Role == "Admin" ? clrCrimson : Color.FromArgb(30, 90, 58),
                    Location = new Point(roleX, 8),
                    Size = new Size(roleW, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Label lblModule = new Label
                {
                    Text = log.Module,
                    Font = new Font("Georgia", 8.5F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(88, 60, 44),
                    Location = new Point(moduleX, 8),
                    Size = new Size(moduleW, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };

                Label lblAction = new Label
                {
                    Text = log.Action,
                    Font = new Font("Georgia", 8.5F, FontStyle.Regular),
                    ForeColor = clrTextDark,
                    Location = new Point(actionX, 8),
                    Size = new Size(actionW, 20),
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };

                // Status Badge Panel
                Panel pnlStatusBadge = new Panel
                {
                    Location = new Point(statX + 10, 7),
                    Size = new Size(80, 22),
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

                pnlStatusBadge.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    Color bg;
                    Color fg = Color.White;
                    switch (log.Status)
                    {
                        case "SUCCESS":
                            bg = Color.FromArgb(30, 110, 60);
                            break;
                        case "RESTOCKED":
                            bg = Color.FromArgb(212, 160, 23);
                            fg = clrTextDark;
                            break;
                        case "UPDATED":
                            bg = Color.FromArgb(184, 115, 24);
                            break;
                        case "CREATED":
                            bg = Color.FromArgb(43, 88, 118);
                            break;
                        case "VOIDED":
                            bg = clrCrimson;
                            break;
                        default:
                            bg = Color.FromArgb(90, 24, 40);
                            break;
                    }

                    Rectangle rect = new Rectangle(0, 0, pnlStatusBadge.Width - 1, pnlStatusBadge.Height - 1);
                    using (GraphicsPath path = GetRoundedRectPath(rect, 8))
                    using (SolidBrush brush = new SolidBrush(bg))
                    {
                        g.FillPath(brush, path);
                    }

                    TextRenderer.DrawText(g, log.Status, new Font("Georgia", 7F, FontStyle.Bold),
                        rect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };

                row.Controls.Add(lblTime);
                row.Controls.Add(lblUser);
                row.Controls.Add(lblRole);
                row.Controls.Add(lblModule);
                row.Controls.Add(lblAction);
                row.Controls.Add(pnlStatusBadge);

                pnlActivityInner.Controls.Add(row);
                yOffset += rowHeight;
            }

            pnlActivityInner.Height = yOffset;
            pnlActivityInner.ResumeLayout();

            if (scrollActivity != null)
            {
                scrollActivity.BindTo(pnlActivityList, pnlActivityInner);
            }
        }

        private void UpdateKpiCounts()
        {
            int outCount = 0;
            int lowCount = 0;

            foreach (RestockItem item in restockItems)
            {
                if (item.CurrentStock == 0) outCount++;
                else if (item.CurrentStock <= item.ReorderAt) lowCount++;
            }

            // Plus 8 additional simulated low-stock items to match "12 ITEMS / 15 need attention"
            int totalLow = lowCount + 8;
            int totalAttention = outCount + totalLow;

            lblOutStockValue.Text = outCount + " ITEMS";
            lblLowStockValue.Text = totalLow + " ITEMS";
            lblNeedAttention.Text = totalAttention + " need attention";
            lblShowingCount.Text = string.Format("Showing {0} of {1} items", restockItems.Count, totalAttention);
        }

        // ==========================================================
        // CUSTOM BAR CHART (SALES OVERVIEW)
        // ==========================================================

        private void PnlChartCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            using (var bgBrush = new LinearGradientBrush(pnlChartCanvas.ClientRectangle,
                Color.FromArgb(249, 237, 204),
                Color.FromArgb(241, 222, 178),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(bgBrush, pnlChartCanvas.ClientRectangle);
            }

            string[] labels = isDailyView ? dailyLabels : weeklyLabels;
            float[] values = isDailyView ? dailyValues : weeklyValues;
            int highlightIdx = isDailyView ? dailyHighlightIndex : weeklyHighlightIndex;
            float maxVal = isDailyView ? 14000f : 70000f;

            int leftMargin = 46;
            int rightMargin = 16;
            int topMargin = 26;
            int bottomMargin = 32;

            int chartWidth = pnlChartCanvas.Width - leftMargin - rightMargin;
            int chartHeight = pnlChartCanvas.Height - topMargin - bottomMargin;
            int bottomY = topMargin + chartHeight;

            // Gridlines & Y-Axis Labels (0, 50%, 100%)
            float[] gridSteps = { 0f, maxVal / 2f, maxVal };
            using (Pen dashPen = new Pen(Color.FromArgb(204, 186, 142), 1f))
            using (Font axisFont = new Font("Georgia", 8F, FontStyle.Regular))
            using (SolidBrush axisBrush = new SolidBrush(clrMuted))
            {
                dashPen.DashStyle = DashStyle.Dot;

                foreach (float step in gridSteps)
                {
                    int y = bottomY - (int)((step / maxVal) * chartHeight);
                    if (step > 0)
                    {
                        g.DrawLine(dashPen, leftMargin, y, leftMargin + chartWidth, y);
                    }
                    else
                    {
                        using (Pen basePen = new Pen(Color.FromArgb(196, 178, 134), 1f))
                            g.DrawLine(basePen, leftMargin, y, leftMargin + chartWidth, y);
                    }

                    string yText = step == 0 ? "₱0" : string.Format("₱{0:0}k", step / 1000f);
                    SizeF txtSize = g.MeasureString(yText, axisFont);
                    g.DrawString(yText, axisFont, axisBrush, leftMargin - txtSize.Width - 6, y - txtSize.Height / 2f);
                }
            }

            // Bars
            int count = labels.Length;
            float slotWidth = (float)chartWidth / count;
            int barWidth = isDailyView ? Math.Min(74, (int)(slotWidth - 16)) : Math.Min(110, (int)(slotWidth - 36));

            using (Font valFont = new Font("Georgia", 8.5F, FontStyle.Bold))
            using (Font dayFont = new Font("Georgia", 8F, FontStyle.Regular))
            using (SolidBrush textBrush = new SolidBrush(clrTextDark))
            using (SolidBrush mutedBrush = new SolidBrush(clrMuted))
            {
                for (int i = 0; i < count; i++)
                {
                    float centerX = leftMargin + slotWidth * i + slotWidth / 2f;
                    int barX = (int)(centerX - barWidth / 2f);
                    int barH = (int)((values[i] / maxVal) * chartHeight);

                    if (values[i] <= 0)
                    {
                        // Flat line for zero sales (e.g., SUN)
                        using (SolidBrush flatBrush = new SolidBrush(clrMaroon))
                        {
                            g.FillRectangle(flatBrush, barX, bottomY - 3, barWidth, 3);
                        }
                        SizeF dashSize = g.MeasureString("—", valFont);
                        g.DrawString("—", valFont, mutedBrush, centerX - dashSize.Width / 2f, bottomY - 22);
                    }
                    else
                    {
                        int barY = bottomY - barH;
                        Rectangle barRect = new Rectangle(barX, barY, barWidth, barH);
                        bool isHighlighted = (i == highlightIdx);

                        using (GraphicsPath barPath = GetTopRoundedRectPath(barRect, 6))
                        using (SolidBrush barBrush = new SolidBrush(isHighlighted ? Color.FromArgb(221, 165, 32) : clrMaroon))
                        {
                            g.FillPath(barBrush, barPath);
                            if (isHighlighted)
                            {
                                using (Pen hiBorder = new Pen(Color.FromArgb(180, 128, 18), 1f))
                                    g.DrawPath(hiBorder, barPath);
                            }
                        }

                        string valLabel = string.Format("₱{0:0.#}k", values[i] / 1000f);
                        SizeF valSize = g.MeasureString(valLabel, valFont);
                        g.DrawString(valLabel, valFont, textBrush, centerX - valSize.Width / 2f, barY - valSize.Height - 4);
                    }

                    // X-Axis Label
                    SizeF daySize = g.MeasureString(labels[i], dayFont);
                    g.DrawString(labels[i], dayFont, mutedBrush, centerX - daySize.Width / 2f, bottomY + 8);
                }
            }
        }

        // ==========================================================
        // CUSTOM PAINT HELPERS (CARDS, BUTTONS, ICONS)
        // ==========================================================

        private void DrawParchmentCard(Graphics g, Rectangle bounds, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            {
                // 1. Rich illuminated parchment gradient
                using (var bgBrush = new LinearGradientBrush(rect,
                    Color.FromArgb(249, 237, 204),
                    Color.FromArgb(241, 222, 178),
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(bgBrush, path);
                }

                // 2. Inner concentric gold hairline frame (inset 4px)
                if (bounds.Width > 20 && bounds.Height > 20)
                {
                    Rectangle innerRect = new Rectangle(4, 4, bounds.Width - 9, bounds.Height - 9);
                    using (GraphicsPath innerPath = GetRoundedRectPath(innerRect, Math.Max(2, radius - 3)))
                    using (Pen innerPen = new Pen(Color.FromArgb(90, 196, 152, 54), 1f))
                    {
                        g.DrawPath(innerPen, innerPath);
                    }
                }

                // 3. Antique Gold outer border
                using (Pen borderPen = new Pen(Color.FromArgb(212, 162, 48), 1.6f))
                {
                    g.DrawPath(borderPen, path);
                }

                // 4. Subtle corner diamond filigree accents
                if (bounds.Width >= 120 && bounds.Height >= 60)
                {
                    using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(185, 142, 52)))
                    {
                        // Top-left
                        g.FillPolygon(dotBrush, new[] { new Point(8, 5), new Point(10, 7), new Point(8, 9), new Point(6, 7) });
                        // Top-right
                        g.FillPolygon(dotBrush, new[] { new Point(bounds.Width - 9, 5), new Point(bounds.Width - 7, 7), new Point(bounds.Width - 9, 9), new Point(bounds.Width - 11, 7) });
                        // Bottom-left
                        g.FillPolygon(dotBrush, new[] { new Point(8, bounds.Height - 10), new Point(10, bounds.Height - 8), new Point(8, bounds.Height - 6), new Point(6, bounds.Height - 8) });
                        // Bottom-right
                        g.FillPolygon(dotBrush, new[] { new Point(bounds.Width - 9, bounds.Height - 10), new Point(bounds.Width - 7, bounds.Height - 8), new Point(bounds.Width - 9, bounds.Height - 6), new Point(bounds.Width - 11, bounds.Height - 8) });
                    }
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
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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

        private void DrawTogglePill(Graphics g, Button btn, bool isActive)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, 14))
            {
                if (isActive)
                {
                    using (var fill = new LinearGradientBrush(rect, Color.FromArgb(52, 14, 24), Color.FromArgb(32, 8, 16), LinearGradientMode.Vertical))
                        g.FillPath(fill, path);
                    using (Pen border = new Pen(Color.FromArgb(212, 162, 48), 1.4f))
                        g.DrawPath(border, path);
                }
                else
                {
                    using (var fill = new SolidBrush(Color.FromArgb(235, 216, 175)))
                        g.FillPath(fill, path);
                    using (Pen border = new Pen(Color.FromArgb(195, 160, 110), 1.2f))
                        g.DrawPath(border, path);
                }
            }

            Color txtColor = isActive ? Color.FromArgb(255, 225, 120) : clrTextDark;
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, txtColor,
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

        private void DrawKpiCircleIcon(Graphics g, Rectangle bounds, string iconType)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circle = new Rectangle(2, 2, bounds.Width - 5, bounds.Height - 5);

            Color topColor = iconType == "out" ? Color.FromArgb(140, 24, 34) :
                             iconType == "warning" ? Color.FromArgb(74, 26, 20) :
                             Color.FromArgb(56, 16, 26);
            Color botColor = iconType == "out" ? Color.FromArgb(88, 16, 22) :
                             iconType == "warning" ? Color.FromArgb(48, 14, 18) :
                             Color.FromArgb(34, 10, 18);

            using (LinearGradientBrush darkBrush = new LinearGradientBrush(circle, topColor, botColor, LinearGradientMode.Vertical))
            using (Pen goldPen = new Pen(clrGold, 1.8f))
            {
                g.FillEllipse(darkBrush, circle);
                g.DrawEllipse(goldPen, circle);
            }

            // Inset thin gold ring
            Rectangle innerCircle = new Rectangle(4, 4, bounds.Width - 9, bounds.Height - 9);
            using (Pen innerPen = new Pen(Color.FromArgb(80, clrGold), 1f))
            {
                g.DrawEllipse(innerPen, innerCircle);
            }

            Color iconColor = Color.FromArgb(255, 225, 110);
            using (Pen iconPen = new Pen(iconColor, 2f))
            {
                if (iconType == "peso")
                {
                    g.DrawEllipse(iconPen, 16, 16, 20, 20);
                    TextRenderer.DrawText(g, "₱", new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        new Rectangle(16, 15, 20, 22), iconColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else if (iconType == "receipt")
                {
                    g.DrawRectangle(iconPen, 19, 16, 14, 19);
                    g.DrawLine(iconPen, 22, 21, 30, 21);
                    g.DrawLine(iconPen, 22, 25, 30, 25);
                    g.DrawLine(iconPen, 22, 29, 27, 29);
                }
                else if (iconType == "warning")
                {
                    Point[] tri = { new Point(26, 15), new Point(36, 34), new Point(16, 34) };
                    g.DrawPolygon(iconPen, tri);
                    g.DrawLine(iconPen, 26, 22, 26, 28);
                    using (SolidBrush dot = new SolidBrush(iconColor))
                        g.FillEllipse(dot, 25, 30, 2.5f, 2.5f);
                }
                else if (iconType == "out")
                {
                    g.DrawEllipse(iconPen, 16, 16, 20, 20);
                    g.DrawLine(iconPen, 22, 22, 30, 30);
                    g.DrawLine(iconPen, 30, 22, 22, 30);
                }
            }
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

        private GraphicsPath GetTopRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.CloseFigure();
            return path;
        }
    }
}