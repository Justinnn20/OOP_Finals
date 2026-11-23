using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Login
{
    /// <summary>
    /// Custom Wizardly Themed Dialog and MessageBox for UMak Coop Bookstore.
    /// Replaces standard plain Windows MessageBox with deep maroon, parchment, and gold aesthetic.
    /// </summary>
    public static class CoopMessageBox
    {
        public static DialogResult Show(string message, string title = "UMak Coop",
            MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            return Show(null, message, title, buttons, icon);
        }

        public static DialogResult Show(IWin32Window? owner, string message, string title = "UMak Coop",
            MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            using (var dlg = new CoopDialogForm(title, message, buttons, icon))
            {
                if (owner != null && owner is Form f && f.Visible)
                {
                    // Pages embedded in AdminShell aren't top-level, so use the real window as owner.
                    IWin32Window realOwner = f.TopLevelControl ?? f;
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    return dlg.ShowDialog(realOwner);
                }
                else
                {
                    dlg.StartPosition = FormStartPosition.CenterScreen;
                    return dlg.ShowDialog();
                }
            }
        }
    }

    internal class CoopDialogForm : Form
    {
        private readonly Color clrWineDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrWineHeader = Color.FromArgb(48, 12, 24);
        private readonly Color clrParchment = Color.FromArgb(246, 233, 196);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrInkDark = Color.FromArgb(44, 14, 22);
        private readonly Color clrMuted = Color.FromArgb(112, 88, 64);
        private readonly Color clrCrimson = Color.FromArgb(158, 34, 38);

        public CoopDialogForm(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
            this.ClientSize = new Size(460, 230);
            this.Padding = new Padding(3); // keeps the gold border visible around the docked panels
            int contentWidth = this.ClientSize.Width - this.Padding.Horizontal;
            this.BackColor = clrParchment;

            // Header Panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = clrWineDark
            };
            pnlHeader.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                using (Pen p = new Pen(clrGold, 1.5f))
                {
                    g.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = title.ToUpperInvariant(),
                Font = new Font("Georgia", 10.5F, FontStyle.Bold),
                ForeColor = clrGold,
                Location = new Point(18, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(lblTitle);

            // Close 'X' button
            Button btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                ForeColor = clrGold,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(32, 28),
                Location = new Point(contentWidth - 40, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 20, 32);
            btnClose.Click += (s, e) =>
            {
                this.DialogResult = buttons == MessageBoxButtons.YesNo ? DialogResult.No : DialogResult.Cancel;
                this.Close();
            };
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);

            // Icon Panel
            Panel pnlIcon = new Panel
            {
                Location = new Point(24, 64),
                Size = new Size(54, 54),
                BackColor = Color.Transparent
            };
            pnlIcon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle r = new Rectangle(2, 2, 48, 48);
                using (SolidBrush b = new SolidBrush(icon == MessageBoxIcon.Question ? Color.FromArgb(44, 14, 22) :
                                                    (icon == MessageBoxIcon.Warning || icon == MessageBoxIcon.Error) ? clrCrimson :
                                                    Color.FromArgb(174, 134, 40)))
                using (Pen p = new Pen(clrGold, 2f))
                {
                    g.FillEllipse(b, r);
                    g.DrawEllipse(p, r);
                }

                string symbol = "?";
                if (icon == MessageBoxIcon.Warning || icon == MessageBoxIcon.Error) symbol = "!";
                else if (icon == MessageBoxIcon.Information) symbol = "ℹ";

                using (Font f = new Font("Georgia", 16F, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(clrParchment))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(symbol, f, textBrush, new RectangleF(2, 2, 48, 48), sf);
                }
            };
            this.Controls.Add(pnlIcon);

            // Message Label
            Label lblMsg = new Label
            {
                Text = message,
                Font = new Font("Georgia", 10.5F, FontStyle.Regular),
                ForeColor = clrInkDark,
                Location = new Point(90, 64),
                Size = new Size(346, 85),
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblMsg);

            // Button container
            Panel pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(238, 222, 178)
            };
            pnlButtons.Paint += (s, e) =>
            {
                using (Pen p = new Pen(clrGoldBorder, 1f))
                {
                    e.Graphics.DrawLine(p, 0, 0, pnlButtons.Width, 0);
                }
            };

            if (buttons == MessageBoxButtons.YesNo)
            {
                Button btnYes = CreateDialogButton("YES", clrWineDark, clrParchment, 110);
                btnYes.Location = new Point(contentWidth - 240, 14);
                btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };

                Button btnNo = CreateDialogButton("NO", Color.FromArgb(226, 206, 162), clrInkDark, 100);
                btnNo.Location = new Point(contentWidth - 120, 14);
                btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };

                pnlButtons.Controls.Add(btnYes);
                pnlButtons.Controls.Add(btnNo);
                this.AcceptButton = btnYes;
                this.CancelButton = btnNo;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                Button btnOk = CreateDialogButton("OK", clrWineDark, clrParchment, 110);
                btnOk.Location = new Point(contentWidth - 240, 14);
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

                Button btnCancel = CreateDialogButton("CANCEL", Color.FromArgb(226, 206, 162), clrInkDark, 100);
                btnCancel.Location = new Point(contentWidth - 120, 14);
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                pnlButtons.Controls.Add(btnOk);
                pnlButtons.Controls.Add(btnCancel);
                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }
            else
            {
                Button btnOk = CreateDialogButton("OK", clrWineDark, clrParchment, 110);
                btnOk.Location = new Point(contentWidth - 130, 14);
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

                pnlButtons.Controls.Add(btnOk);
                this.AcceptButton = btnOk;
            }

            this.Controls.Add(pnlButtons);

            // Drag by header
            bool isDragging = false;
            Point dragCursorPoint = Point.Empty;
            Point dragFormPoint = Point.Empty;

            pnlHeader.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    dragCursorPoint = Cursor.Position;
                    dragFormPoint = this.Location;
                }
            };
            pnlHeader.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                    this.Location = Point.Add(dragFormPoint, new Size(diff));
                }
            };
            pnlHeader.MouseUp += (s, e) => isDragging = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Outer gold border
            using (Pen borderPen = new Pen(clrGold, 2.5f))
            {
                g.DrawRectangle(borderPen, 1, 1, this.ClientSize.Width - 3, this.ClientSize.Height - 3);
            }

            // Wizardly decorative corner flourishes
            DrawCornerFiligree(g, 6, 50);
            DrawCornerFiligree(g, this.Width - 20, 50);
        }

        private void DrawCornerFiligree(Graphics g, int x, int y)
        {
            using (Pen p = new Pen(Color.FromArgb(170, 136, 60), 1f))
            {
                g.DrawLine(p, x, y, x + 14, y);
                g.DrawLine(p, x, y, x, y + 14);
            }
        }

        private Button CreateDialogButton(string text, Color bg, Color fg, int width)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Georgia", 9F, FontStyle.Bold),
                ForeColor = fg,
                BackColor = bg,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(width, 32),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = clrGoldBorder;
            return btn;
        }
    }
}