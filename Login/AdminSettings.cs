using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Login
{
    public partial class AdminSettings : Form
    {
        // Theme Palette (matches AdminDB)
        private readonly Color clrBgDark = Color.FromArgb(42, 10, 20);
        private readonly Color clrNavDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrParchment = Color.FromArgb(244, 227, 178);
        private readonly Color clrInputBg = Color.FromArgb(250, 241, 214);
        private readonly Color clrInputBorder = Color.FromArgb(182, 160, 114);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrMaroon = Color.FromArgb(88, 23, 38);
        private readonly Color clrTextDark = Color.FromArgb(44, 14, 22);
        private readonly Color clrCrimson = Color.FromArgb(174, 34, 48);
        private readonly Color clrMuted = Color.FromArgb(124, 102, 78);
        private readonly Color clrDivider = Color.FromArgb(216, 198, 152);
        private readonly Color clrActiveGreen = Color.FromArgb(124, 148, 82);
        private readonly Color clrInactiveGray = Color.FromArgb(166, 162, 156);

        private Button activeNavButton;
        private string activeRoleFilter = "ALL";
        private bool isSearchPlaceholder = true;
        private int currentPage = 1;
        private const int PageSize = 5;

        // Toggle States
        private bool acceptGcash = true;
        private bool autoPrintReceipt = true;
        private bool staffActivityLogs = true;

        // Staff Model
        private class StaffAccount
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
        }

        private List<StaffAccount> staffAccounts;
        private int nextStaffId = 6;

        public AdminSettings()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            EnableDoubleBuffering(pnlStaffList);

            activeNavButton = btnNavSettings;
            lblDate.Text = DateTime.Now.ToString("dddd · MMM dd, yyyy").ToUpper();

            InitSampleStaffData();
            WireEvents();
            RenderStaffTable();
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitSampleStaffData()
        {
            staffAccounts = new List<StaffAccount>
            {
                new StaffAccount { Id = 1, FullName = "Maria Santos", Username = "m.santos", Password = "password123", Role = "Cashier", IsActive = true },
                new StaffAccount { Id = 2, FullName = "Juan Dela Cruz", Username = "j.delacruz", Password = "password123", Role = "Staff", IsActive = true },
                new StaffAccount { Id = 3, FullName = "Ana Reyes", Username = "a.reyes", Password = "password123", Role = "Staff", IsActive = true },
                new StaffAccount { Id = 4, FullName = "Mark Villanueva", Username = "m.villanueva", Password = "password123", Role = "Cashier", IsActive = false },
                new StaffAccount { Id = 5, FullName = "Carlo Mendoza", Username = "c.mendoza", Password = "password123", Role = "Staff", IsActive = true }
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

            // Main Cards Paint
            pnlStaffCard.Paint += PnlStaffCard_Paint;
            pnlConfigCard.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlConfigCard.ClientRectangle, 16);
            pnlSecurityCard.Paint += (s, e) =>
            {
                DrawParchmentCard(e.Graphics, pnlSecurityCard.ClientRectangle, 16);
                using (Pen divPen = new Pen(clrDivider, 1f))
                {
                    e.Graphics.DrawLine(divPen, 20, 90, pnlSecurityCard.Width - 20, 90);
                    e.Graphics.DrawLine(divPen, 20, 138, pnlSecurityCard.Width - 20, 138);
                }
            };

            // Search Box Paint & Placeholder Logic
            pnlSearchBox.Paint += (s, e) =>
            {
                DrawInputContainer(e.Graphics, pnlSearchBox.ClientRectangle, 10);
                using (Pen iconPen = new Pen(clrMuted, 1.6f))
                {
                    e.Graphics.DrawEllipse(iconPen, pnlSearchBox.Width - 27, 9, 11, 11);
                    e.Graphics.DrawLine(iconPen, pnlSearchBox.Width - 17, 19, pnlSearchBox.Width - 12, 24);
                }
            };

            txtSearchStaff.Enter += (s, e) =>
            {
                if (isSearchPlaceholder)
                {
                    isSearchPlaceholder = false;
                    txtSearchStaff.Text = "";
                    txtSearchStaff.Font = new Font("Georgia", 9.5F, FontStyle.Regular);
                    txtSearchStaff.ForeColor = clrTextDark;
                }
            };

            txtSearchStaff.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchStaff.Text))
                {
                    isSearchPlaceholder = true;
                    txtSearchStaff.Text = "Search staff...";
                    txtSearchStaff.Font = new Font("Georgia", 9.5F, FontStyle.Italic);
                    txtSearchStaff.ForeColor = clrMuted;
                }
            };

            txtSearchStaff.TextChanged += (s, e) =>
            {
                if (!isSearchPlaceholder)
                {
                    currentPage = 1;
                    RenderStaffTable();
                }
            };

            // Role Filter Pills (ALL, CASHIER, STAFF)
            btnFilterAll.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterAll, activeRoleFilter == "ALL");
            btnFilterCashier.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterCashier, activeRoleFilter == "CASHIER");
            btnFilterStaff.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterStaff, activeRoleFilter == "STAFF");

            btnFilterAll.Click += (s, e) => SetRoleFilter("ALL");
            btnFilterCashier.Click += (s, e) => SetRoleFilter("CASHIER");
            btnFilterStaff.Click += (s, e) => SetRoleFilter("STAFF");

            // Add Staff Button
            btnAddStaff.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnAddStaff, 14);
            btnAddStaff.Click += (s, e) => OpenStaffModal(null);

            // Pagination
            btnPagePrev.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RenderStaffTable();
                }
            };

            btnPageNext.Click += (s, e) =>
            {
                int totalFiltered = GetFilteredStaff().Count;
                int maxPage = Math.Max(1, (int)Math.Ceiling((double)totalFiltered / PageSize));
                if (currentPage < maxPage)
                {
                    currentPage++;
                    RenderStaffTable();
                }
            };

            // Config Card Input Containers
            pnlStoreNameWrap.Paint += (s, e) => DrawInputContainer(e.Graphics, pnlStoreNameWrap.ClientRectangle, 8);
            pnlReceiptFooterWrap.Paint += (s, e) => DrawInputContainer(e.Graphics, pnlReceiptFooterWrap.ClientRectangle, 8);
            pnlLowStockWrap.Paint += (s, e) => DrawInputContainer(e.Graphics, pnlLowStockWrap.ClientRectangle, 8);
            pnlTaxRateWrap.Paint += (s, e) => DrawInputContainer(e.Graphics, pnlTaxRateWrap.ClientRectangle, 8);

            // Toggle Switches
            pnlToggleGcash.Paint += (s, e) => DrawToggleSwitch(e.Graphics, pnlToggleGcash.ClientRectangle, acceptGcash);
            pnlToggleGcash.Click += (s, e) => { acceptGcash = !acceptGcash; pnlToggleGcash.Invalidate(); };

            pnlToggleAutoPrint.Paint += (s, e) => DrawToggleSwitch(e.Graphics, pnlToggleAutoPrint.ClientRectangle, autoPrintReceipt);
            pnlToggleAutoPrint.Click += (s, e) => { autoPrintReceipt = !autoPrintReceipt; pnlToggleAutoPrint.Invalidate(); };

            pnlToggleLogs.Paint += (s, e) => DrawToggleSwitch(e.Graphics, pnlToggleLogs.ClientRectangle, staffActivityLogs);
            pnlToggleLogs.Click += (s, e) => { staffActivityLogs = !staffActivityLogs; pnlToggleLogs.Invalidate(); };

            // Action Buttons (Right Column)
            btnSaveSettings.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnSaveSettings, 14);
            btnSaveSettings.Click += BtnSaveSettings_Click;

            btnChangePass.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnChangePass, 14);
            btnChangePass.Click += (s, e) => OpenChangePasswordModal();

            btnBackupNow.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnBackupNow, 14);
            btnBackupNow.Click += BtnBackupNow_Click;
        }

        // ==========================================================
        // NAVIGATION
        // ==========================================================

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || clicked == btnNavSettings) return;

            if (clicked == btnNavDashboard)
            {
                AdminDB dash = new AdminDB();
                dash.Show();
                this.Hide();
                dash.FormClosed += (s, args) => this.Close();
            }
            else if (clicked == btnNavInventory)
            {
                MessageBox.Show("Navigate to Inventory Form", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (clicked == btnNavTransactions)
            {
                SwitchForm(new AdminTransactions());
            }
            else if (clicked == btnNavReports)
            {
                MessageBox.Show("Navigate to Reports Form", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            targetForm.StartPosition = FormStartPosition.CenterScreen;
            targetForm.Show();
            this.Hide();
            targetForm.FormClosed += (s, args) => this.Close();
        }

        // ==========================================================
        // STAFF MANAGEMENT (CRUD + FILTER + PAGINATION)
        // ==========================================================

        private void SetRoleFilter(string role)
        {
            activeRoleFilter = role;
            currentPage = 1;
            btnFilterAll.Invalidate();
            btnFilterCashier.Invalidate();
            btnFilterStaff.Invalidate();
            RenderStaffTable();
        }

        private List<StaffAccount> GetFilteredStaff()
        {
            string query = isSearchPlaceholder ? "" : txtSearchStaff.Text.Trim().ToLower();
            List<StaffAccount> result = new List<StaffAccount>();

            foreach (StaffAccount staff in staffAccounts)
            {
                bool matchesRole = (activeRoleFilter == "ALL") ||
                    staff.Role.Equals(activeRoleFilter, StringComparison.OrdinalIgnoreCase);

                bool matchesSearch = string.IsNullOrEmpty(query) ||
                    staff.FullName.ToLower().Contains(query) ||
                    staff.Username.ToLower().Contains(query) ||
                    staff.Role.ToLower().Contains(query);

                if (matchesRole && matchesSearch)
                {
                    result.Add(staff);
                }
            }

            return result;
        }

        private void RenderStaffTable()
        {
            pnlStaffList.SuspendLayout();
            pnlStaffList.Controls.Clear();

            List<StaffAccount> filtered = GetFilteredStaff();
            int totalFiltered = filtered.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalFiltered / PageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            int startIndex = (currentPage - 1) * PageSize;
            int countToShow = Math.Min(PageSize, totalFiltered - startIndex);

            int rowHeight = 68;

            if (countToShow <= 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "No matching staff accounts found.",
                    Font = new Font("Georgia", 10F, FontStyle.Italic),
                    ForeColor = clrMuted,
                    AutoSize = false,
                    Size = new Size(pnlStaffList.Width, 60),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                pnlStaffList.Controls.Add(lblEmpty);
            }
            else
            {
                for (int i = 0; i < countToShow; i++)
                {
                    StaffAccount staff = filtered[startIndex + i];
                    bool isLast = (i == countToShow - 1);

                    Panel row = new Panel
                    {
                        Location = new Point(0, i * rowHeight),
                        Size = new Size(pnlStaffList.Width, rowHeight),
                        BackColor = clrParchment
                    };

                    row.Paint += (s, e) =>
                    {
                        Graphics g = e.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // Status Pill Badge
                        Rectangle badgeRect = new Rectangle(376, 21, 86, 25);
                        Color bg = staff.IsActive ? clrActiveGreen : clrInactiveGray;
                        Color fg = staff.IsActive ? Color.White : clrTextDark;
                        string statusTxt = staff.IsActive ? "ACTIVE" : "INACTIVE";

                        using (GraphicsPath badgePath = GetRoundedRectPath(badgeRect, 12))
                        using (SolidBrush bBrush = new SolidBrush(bg))
                        {
                            g.FillPath(bBrush, badgePath);
                        }

                        TextRenderer.DrawText(g, statusTxt, new Font("Segoe UI", 7.5F, FontStyle.Bold),
                            badgeRect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                        // Row divider
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
                        Text = staff.FullName,
                        Font = new Font("Georgia", 10F, FontStyle.Regular),
                        ForeColor = clrTextDark,
                        Location = new Point(2, 23),
                        Size = new Size(164, 22),
                        AutoEllipsis = true,
                        BackColor = Color.Transparent
                    };

                    Label lblUser = new Label
                    {
                        Text = staff.Username,
                        Font = new Font("Georgia", 9.5F, FontStyle.Regular),
                        ForeColor = Color.FromArgb(68, 42, 34),
                        Location = new Point(172, 23),
                        Size = new Size(116, 22),
                        AutoEllipsis = true,
                        BackColor = Color.Transparent
                    };

                    Label lblRole = new Label
                    {
                        Text = staff.Role,
                        Font = new Font("Georgia", 9.5F, FontStyle.Regular),
                        ForeColor = clrTextDark,
                        Location = new Point(292, 23),
                        Size = new Size(80, 22),
                        BackColor = Color.Transparent
                    };

                    Button btnEdit = new Button
                    {
                        Text = "EDIT",
                        Font = new Font("Georgia", 8F, FontStyle.Bold),
                        ForeColor = clrTextDark,
                        BackColor = clrParchment,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Location = new Point(496, 19),
                        Size = new Size(86, 29)
                    };
                    btnEdit.FlatAppearance.BorderSize = 0;
                    btnEdit.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnEdit, 14);
                    btnEdit.Click += (s, e) => OpenStaffModal(staff);

                    Button btnDelete = new Button
                    {
                        Text = "DELETE",
                        Font = new Font("Georgia", 8F, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = clrParchment,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Location = new Point(592, 19),
                        Size = new Size(104, 29)
                    };
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnDelete.Paint += (s, e) => DrawCrimsonPillButton(e.Graphics, btnDelete, 14);
                    btnDelete.Click += (s, e) => DeleteStaff(staff);

                    row.Controls.Add(lblName);
                    row.Controls.Add(lblUser);
                    row.Controls.Add(lblRole);
                    row.Controls.Add(btnEdit);
                    row.Controls.Add(btnDelete);

                    pnlStaffList.Controls.Add(row);
                }
            }

            lblStaffShowing.Text = string.Format("Showing {0} of {1} staff accounts", countToShow, totalFiltered);
            lblPageNum.Text = currentPage.ToString();
            btnPagePrev.Enabled = (currentPage > 1);
            btnPageNext.Enabled = (currentPage < totalPages);

            pnlStaffList.ResumeLayout();
        }

        private void DeleteStaff(StaffAccount staff)
        {
            DialogResult dr = MessageBox.Show(
                string.Format("Are you sure you want to delete staff account \"{0}\" ({1})?", staff.FullName, staff.Username),
                "Delete Staff Account",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                staffAccounts.Remove(staff);
                RenderStaffTable();
            }
        }

        // ==========================================================
        // ADD / EDIT STAFF MODAL (THEMED)
        // ==========================================================

        private void OpenStaffModal(StaffAccount existing)
        {
            bool isEdit = (existing != null);

            Form modal = new Form
            {
                Text = isEdit ? "Edit Staff Account" : "Add New Staff Account",
                Size = new Size(410, 405),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = clrBgDark
            };

            Panel card = new Panel
            {
                Location = new Point(16, 16),
                Size = new Size(362, 336),
                BackColor = clrBgDark
            };
            card.Paint += (s, e) => DrawParchmentCard(e.Graphics, card.ClientRectangle, 14);
            modal.Controls.Add(card);

            Label lblHeader = new Label
            {
                Text = isEdit ? "EDIT STAFF ACCOUNT" : "ADD NEW STAFF",
                Font = new Font("Georgia", 11F, FontStyle.Bold),
                ForeColor = clrTextDark,
                AutoSize = true,
                Location = new Point(20, 16),
                BackColor = Color.Transparent
            };

            TextBox txtName = CreateModalInput(card, "FULL NAME", isEdit ? existing.FullName : "", 48);
            TextBox txtUser = CreateModalInput(card, "USERNAME", isEdit ? existing.Username : "", 102);
            TextBox txtPass = CreateModalInput(card, "PASSWORD", isEdit ? existing.Password : "", 156);
            txtPass.UseSystemPasswordChar = true;

            Label lblRole = new Label
            {
                Text = "ROLE",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(72, 42, 34),
                AutoSize = true,
                Location = new Point(20, 210),
                BackColor = Color.Transparent
            };

            ComboBox cmbRole = new ComboBox
            {
                Location = new Point(20, 228),
                Size = new Size(155, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Georgia", 9.5F),
                BackColor = clrInputBg,
                ForeColor = clrTextDark
            };
            cmbRole.Items.AddRange(new object[] { "Cashier", "Staff" });
            cmbRole.SelectedItem = (isEdit && cmbRole.Items.Contains(existing.Role)) ? existing.Role : "Cashier";

            Label lblStatus = new Label
            {
                Text = "STATUS",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(72, 42, 34),
                AutoSize = true,
                Location = new Point(190, 210),
                BackColor = Color.Transparent
            };

            ComboBox cmbStatus = new ComboBox
            {
                Location = new Point(190, 228),
                Size = new Size(152, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Georgia", 9.5F),
                BackColor = clrInputBg,
                ForeColor = clrTextDark
            };
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cmbStatus.SelectedIndex = (isEdit && !existing.IsActive) ? 1 : 0;

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                ForeColor = clrTextDark,
                BackColor = clrParchment,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(122, 280),
                Size = new Size(102, 34)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Paint += (s, e) => DrawFilterPill(e.Graphics, btnCancel, false);
            btnCancel.Click += (s, e) => modal.Close();

            Button btnSave = new Button
            {
                Text = isEdit ? "UPDATE" : "CREATE",
                Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                ForeColor = clrTextDark,
                BackColor = clrParchment,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(234, 280),
                Size = new Size(108, 34)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnSave, 14);

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
                {
                    MessageBox.Show("Please fill in Full Name, Username, and Password.", "Missing Fields",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (isEdit)
                {
                    existing.FullName = txtName.Text.Trim();
                    existing.Username = txtUser.Text.Trim();
                    existing.Password = txtPass.Text;
                    existing.Role = cmbRole.SelectedItem.ToString();
                    existing.IsActive = (cmbStatus.SelectedIndex == 0);
                }
                else
                {
                    staffAccounts.Add(new StaffAccount
                    {
                        Id = nextStaffId++,
                        FullName = txtName.Text.Trim(),
                        Username = txtUser.Text.Trim(),
                        Password = txtPass.Text,
                        Role = cmbRole.SelectedItem.ToString(),
                        IsActive = (cmbStatus.SelectedIndex == 0)
                    });
                }

                RenderStaffTable();
                modal.Close();
            };

            card.Controls.Add(lblHeader);
            card.Controls.Add(lblRole);
            card.Controls.Add(cmbRole);
            card.Controls.Add(lblStatus);
            card.Controls.Add(cmbStatus);
            card.Controls.Add(btnCancel);
            card.Controls.Add(btnSave);

            modal.ShowDialog(this);
        }

        private TextBox CreateModalInput(Panel parent, string labelText, string initialValue, int yPos)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(72, 42, 34),
                AutoSize = true,
                Location = new Point(20, yPos),
                BackColor = Color.Transparent
            };

            Panel wrap = new Panel
            {
                Location = new Point(20, yPos + 17),
                Size = new Size(322, 29),
                BackColor = clrParchment
            };
            wrap.Paint += (s, e) => DrawInputContainer(e.Graphics, wrap.ClientRectangle, 8);

            TextBox tb = new TextBox
            {
                Text = initialValue,
                BorderStyle = BorderStyle.None,
                BackColor = clrInputBg,
                ForeColor = clrTextDark,
                Font = new Font("Georgia", 9.5F),
                Location = new Point(10, 6),
                Size = new Size(302, 16)
            };

            wrap.Controls.Add(tb);
            parent.Controls.Add(lbl);
            parent.Controls.Add(wrap);
            return tb;
        }

        // ==========================================================
        // COOP CONFIG & SECURITY ACTIONS
        // ==========================================================

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            int threshold;
            if (!int.TryParse(txtLowStock.Text.Trim(), out threshold) || threshold < 0)
            {
                MessageBox.Show("Low Stock Threshold must be a valid non-negative number.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                string.Format("Settings saved!\n\nStore: {0}\nReceipt Footer: {1}\nLow Stock Threshold: {2}\nTax Rate: {3}\nGCash: {4}\nAuto-Print: {5}",
                    txtStoreName.Text.Trim(),
                    txtReceiptFooter.Text.Trim(),
                    threshold,
                    txtTaxRate.Text.Trim(),
                    acceptGcash ? "ON" : "OFF",
                    autoPrintReceipt ? "ON" : "OFF"),
                "Configuration Updated",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OpenChangePasswordModal()
        {
            Form modal = new Form
            {
                Text = "Change Admin Password",
                Size = new Size(390, 315),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = clrBgDark
            };

            Panel card = new Panel
            {
                Location = new Point(16, 16),
                Size = new Size(342, 246),
                BackColor = clrBgDark
            };
            card.Paint += (s, e) => DrawParchmentCard(e.Graphics, card.ClientRectangle, 14);
            modal.Controls.Add(card);

            Label lblHeader = new Label
            {
                Text = "CHANGE ADMIN PASSWORD",
                Font = new Font("Georgia", 10.5F, FontStyle.Bold),
                ForeColor = clrTextDark,
                AutoSize = true,
                Location = new Point(20, 16),
                BackColor = Color.Transparent
            };

            TextBox txtCurrent = CreateModalInput(card, "CURRENT PASSWORD", "", 48);
            txtCurrent.UseSystemPasswordChar = true;
            txtCurrent.Width = 282;
            txtCurrent.Parent.Width = 302;

            TextBox txtNew = CreateModalInput(card, "NEW PASSWORD", "", 102);
            txtNew.UseSystemPasswordChar = true;
            txtNew.Width = 282;
            txtNew.Parent.Width = 302;

            TextBox txtConfirm = CreateModalInput(card, "CONFIRM NEW PASSWORD", "", 156);
            txtConfirm.UseSystemPasswordChar = true;
            txtConfirm.Width = 282;
            txtConfirm.Parent.Width = 302;

            Button btnSave = new Button
            {
                Text = "UPDATE",
                Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                ForeColor = clrTextDark,
                BackColor = clrParchment,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(214, 202),
                Size = new Size(108, 32)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnSave, 14);

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCurrent.Text) || string.IsNullOrWhiteSpace(txtNew.Text))
                {
                    MessageBox.Show("Please fill in all password fields.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtNew.Text != txtConfirm.Text)
                {
                    MessageBox.Show("New password and confirmation do not match.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Admin password has been updated successfully.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Information);
                modal.Close();
            };

            card.Controls.Add(lblHeader);
            card.Controls.Add(btnSave);
            modal.ShowDialog(this);
        }

        private void BtnBackupNow_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save Coop Database Backup";
                sfd.Filter = "Backup File (*.txt)|*.txt|SQL Script (*.sql)|*.sql";
                sfd.FileName = "UMakCoop_Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            sw.WriteLine("-- University of Makati Coop POS Backup");
                            sw.WriteLine("-- Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sw.WriteLine("StoreName=" + txtStoreName.Text.Trim());
                            sw.WriteLine("ReceiptFooter=" + txtReceiptFooter.Text.Trim());
                            sw.WriteLine("LowStockThreshold=" + txtLowStock.Text.Trim());
                            sw.WriteLine("TaxRate=" + txtTaxRate.Text.Trim());
                            sw.WriteLine("StaffCount=" + staffAccounts.Count);
                            foreach (StaffAccount staff in staffAccounts)
                            {
                                sw.WriteLine(string.Format("STAFF|{0}|{1}|{2}|{3}|{4}",
                                    staff.Id, staff.FullName, staff.Username, staff.Role, staff.IsActive ? "ACTIVE" : "INACTIVE"));
                            }
                        }

                        MessageBox.Show("Database backup saved successfully!", "Backup Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to save backup: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ==========================================================
        // CUSTOM PAINTING HELPERS
        // ==========================================================

        private void PnlStaffCard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawParchmentCard(g, pnlStaffCard.ClientRectangle, 16);

            // Table Column Headers
            using (Font hdrFont = new Font("Georgia", 8.5F, FontStyle.Bold))
            using (SolidBrush hdrBrush = new SolidBrush(clrTextDark))
            {
                g.DrawString("STAFF NAME", hdrFont, hdrBrush, 22, 106);
                g.DrawString("USERNAME", hdrFont, hdrBrush, 192, 106);
                g.DrawString("ROLE", hdrFont, hdrBrush, 312, 106);
                g.DrawString("STATUS", hdrFont, hdrBrush, 404, 106);
                g.DrawString("ACTIONS", hdrFont, hdrBrush, 566, 106);
            }

            // Header & Footer Dividers
            using (Pen divPen = new Pen(clrDivider, 1.2f))
            {
                g.DrawLine(divPen, 20, 129, pnlStaffCard.Width - 20, 129);
                g.DrawLine(divPen, 20, 484, pnlStaffCard.Width - 20, 484);
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

        private void DrawToggleSwitch(Graphics g, Rectangle bounds, bool isOn)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);

            Rectangle track = new Rectangle(1, 1, bounds.Width - 3, bounds.Height - 3);
            Color trackColor = isOn ? clrMaroon : Color.FromArgb(166, 152, 132);

            using (GraphicsPath path = GetRoundedRectPath(track, 12))
            using (SolidBrush bg = new SolidBrush(trackColor))
            {
                g.FillPath(bg, path);
            }

            int knobSize = bounds.Height - 8;
            int knobX = isOn ? (bounds.Width - knobSize - 5) : 5;
            Rectangle knob = new Rectangle(knobX, 4, knobSize, knobSize);

            using (SolidBrush knobBrush = new SolidBrush(Color.FromArgb(248, 236, 200)))
            {
                g.FillEllipse(knobBrush, knob);
            }

            Rectangle textRect = isOn
                ? new Rectangle(5, 1, bounds.Width - knobSize - 10, bounds.Height - 2)
                : new Rectangle(knobSize + 6, 1, bounds.Width - knobSize - 10, bounds.Height - 2);

            TextRenderer.DrawText(g, isOn ? "ON" : "OFF", new Font("Segoe UI", 7.25F, FontStyle.Bold),
                textRect, Color.FromArgb(245, 232, 200),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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