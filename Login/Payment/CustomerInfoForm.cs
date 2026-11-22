using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Login
{
    public sealed class CustomerInfoForm : Form
    {
        private readonly Color clrWineDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrParchment = Color.FromArgb(246, 233, 196);
        private readonly Color clrInputBg = Color.FromArgb(252, 244, 222);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrInkDark = Color.FromArgb(44, 14, 22);

        private readonly TextBox _nameBox;
        private readonly TextBox _yearBox;
        private readonly TextBox _collegeBox;

        public string PurchaserName => _nameBox.Text.Trim();
        public string YearLevel => _yearBox.Text.Trim();
        public string College => _collegeBox.Text.Trim();

        public CustomerInfoForm()
        {
            this.Text = "Purchaser Information";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.ClientSize = new Size(440, 275);
            this.BackColor = clrParchment;
            this.DoubleBuffered = true;

            // Header Panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = clrWineDark
            };
            pnlHeader.Paint += (s, e) =>
            {
                using (Pen p = new Pen(clrGold, 1.5f))
                {
                    e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = "PURCHASER INFORMATION",
                Font = new Font("Georgia", 10.5F, FontStyle.Bold),
                ForeColor = clrGold,
                Location = new Point(18, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(lblTitle);

            Button btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                ForeColor = clrGold,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 28),
                Location = new Point(pnlHeader.Width - 40, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);

            Label lblSubtitle = new Label
            {
                Text = "Enter student / purchaser details to generate official receipt:",
                Font = new Font("Georgia", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 75, 55),
                Location = new Point(22, 54),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblSubtitle);

            _nameBox = AddField("Student Name:", 80);
            _yearBox = AddField("Year Level:", 122);
            _collegeBox = AddField("College / Dept:", 164);

            // Button container
            Panel pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = Color.FromArgb(238, 222, 178)
            };
            pnlButtons.Paint += (s, e) =>
            {
                using (Pen p = new Pen(clrGoldBorder, 1f))
                {
                    e.Graphics.DrawLine(p, 0, 0, pnlButtons.Width, 0);
                }
            };

            Button btnConfirm = new Button
            {
                Text = "CONTINUE TO RECEIPT",
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                ForeColor = clrWineDark,
                BackColor = clrGold,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(185, 34),
                Location = new Point(22, 10),
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 1;
            btnConfirm.FlatAppearance.BorderColor = clrGoldBorder;
            btnConfirm.Click += Confirm_Click;

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                ForeColor = clrInkDark,
                BackColor = Color.FromArgb(226, 206, 162),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 34),
                Location = new Point(pnlButtons.Width - 122, 10),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatAppearance.BorderColor = clrGoldBorder;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            pnlButtons.Controls.Add(btnConfirm);
            pnlButtons.Controls.Add(btnCancel);
            this.Controls.Add(pnlButtons);

            this.AcceptButton = btnConfirm;
            this.CancelButton = btnCancel;
            this.ActiveControl = _nameBox;

            // Drag by header
            bool isDragging = false;
            Point dragCursor = Point.Empty;
            Point dragForm = Point.Empty;

            pnlHeader.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    dragCursor = Cursor.Position;
                    dragForm = this.Location;
                }
            };
            pnlHeader.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragCursor));
                    this.Location = Point.Add(dragForm, new Size(diff));
                }
            };
            pnlHeader.MouseUp += (s, e) => isDragging = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen p = new Pen(clrGold, 2.5f))
            {
                e.Graphics.DrawRectangle(p, 1, 1, this.Width - 2, this.Height - 2);
            }
        }

        private TextBox AddField(string labelText, int top)
        {
            Label lbl = new Label
            {
                Text = labelText,
                Left = 22,
                Top = top + 5,
                Width = 125,
                Height = 22,
                ForeColor = clrInkDark,
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                BackColor = Color.Transparent
            };

            Panel pnlBox = new Panel
            {
                Left = 150,
                Top = top,
                Width = 265,
                Height = 32,
                BackColor = clrInputBg
            };
            pnlBox.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(clrGoldBorder, 1.2f))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, pnlBox.Width - 1, pnlBox.Height - 1);
                }
            };

            TextBox box = new TextBox
            {
                Left = 8,
                Top = 6,
                Width = pnlBox.Width - 16,
                BorderStyle = BorderStyle.None,
                BackColor = clrInputBg,
                Font = new Font("Georgia", 10F, FontStyle.Regular),
                ForeColor = clrInkDark
            };
            pnlBox.Controls.Add(box);

            this.Controls.Add(lbl);
            this.Controls.Add(pnlBox);
            return box;
        }

        private void Confirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PurchaserName) ||
                string.IsNullOrWhiteSpace(YearLevel) ||
                string.IsNullOrWhiteSpace(College))
            {
                CoopMessageBox.Show(this, "Please enter the purchaser name, year level, and college department.",
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
