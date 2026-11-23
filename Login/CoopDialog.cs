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
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    return dlg.ShowDialog(owner);
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
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(460, 205);
            this.BackColor = clrParchment;

            // Header Panel
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = clrWineDark
            };
            pnlHeader.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                // Bottom header divider
                using (Pen p = new Pen(clrGold, 1.5f))
                {
                    g.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
                // Outer gold border continuation on top, left, right
                using (Pen borderPen = new Pen(clrGold, 2.5f))
                {
                    g.DrawLine(borderPen, 1, 1, pnlHeader.Width - 2, 1);
                    g.DrawLine(borderPen, 1, 1, 1, pnlHeader.Height);
                    g.DrawLine(borderPen, pnlHeader.Width - 2, 1, pnlHeader.Width - 2, pnlHeader.Height);
                }
            };

            Label lblTitle = new Label
            {
                Text = title.ToUpperInvariant(),
                Font = new Font("Georgia", 10.5F, FontStyle.Bold),
                ForeColor = clrGold,
                Location = new Point(18, 11),
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
                Location = new Point(this.ClientSize.Width - 38, 7),
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

            // Bottom button container
            Panel pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 58,
                Width = this.ClientSize.Width,
                BackColor = Color.FromArgb(238, 222, 178)
            };
            pnlButtons.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                // Top divider
                using (Pen p = new Pen(clrGold, 1.2f))
                {
                    g.DrawLine(p, 0, 0, pnlButtons.Width, 0);
                }
                // Outer gold border continuation on left, right, bottom
                using (Pen borderPen = new Pen(clrGold, 2.5f))
                {
                    g.DrawLine(borderPen, 1, 0, 1, pnlButtons.Height - 1);
                    g.DrawLine(borderPen, pnlButtons.Width - 2, 0, pnlButtons.Width - 2, pnlButtons.Height - 1);
                    g.DrawLine(borderPen, 1, pnlButtons.Height - 2, pnlButtons.Width - 2, pnlButtons.Height - 2);
                }
            };
            this.Controls.Add(pnlButtons);

            // Button layout
            int btnWidth = 105;
            int btnHeight = 35;
            int btnTop = 11;
            int rightMargin = 20;
            int gap = 12;

            if (buttons == MessageBoxButtons.YesNo)
            {
                Button btnYes = CreateDialogButton("YES", isPrimary: true, width: btnWidth, height: btnHeight);
                btnYes.Location = new Point(this.ClientSize.Width - rightMargin - (btnWidth * 2) - gap, btnTop);
                btnYes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnYes.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };

                Button btnNo = CreateDialogButton("NO", isPrimary: false, width: btnWidth, height: btnHeight);
                btnNo.Location = new Point(this.ClientSize.Width - rightMargin - btnWidth, btnTop);
                btnNo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnNo.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };

                pnlButtons.Controls.Add(btnYes);
                pnlButtons.Controls.Add(btnNo);
                this.AcceptButton = btnYes;
                this.CancelButton = btnNo;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                Button btnOk = CreateDialogButton("OK", isPrimary: true, width: btnWidth, height: btnHeight);
                btnOk.Location = new Point(this.ClientSize.Width - rightMargin - (btnWidth * 2) - gap, btnTop);
                btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

                Button btnCancel = CreateDialogButton("CANCEL", isPrimary: false, width: btnWidth, height: btnHeight);
                btnCancel.Location = new Point(this.ClientSize.Width - rightMargin - btnWidth, btnTop);
                btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                pnlButtons.Controls.Add(btnOk);
                pnlButtons.Controls.Add(btnCancel);
                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }
            else
            {
                Button btnOk = CreateDialogButton("OK", isPrimary: true, width: btnWidth, height: btnHeight);
                btnOk.Location = new Point(this.ClientSize.Width - rightMargin - btnWidth, btnTop);
                btnOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

                pnlButtons.Controls.Add(btnOk);
                this.AcceptButton = btnOk;
            }

            // Icon Panel
            Panel pnlIcon = new Panel
            {
                Location = new Point(24, 58),
                Size = new Size(50, 50),
                BackColor = Color.Transparent
            };
            pnlIcon.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle r = new Rectangle(2, 2, 45, 45);
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
                    g.DrawString(symbol, f, textBrush, new RectangleF(2, 2, 45, 45), sf);
                }
            };
            this.Controls.Add(pnlIcon);

            // Message Label
            Label lblMsg = new Label
            {
                Text = message,
                Font = new Font("Georgia", 11F, FontStyle.Regular),
                ForeColor = clrInkDark,
                Location = new Point(88, 52),
                Size = new Size(350, 68),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblMsg);

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

            // Outer gold border for the middle parchment area
            using (Pen borderPen = new Pen(clrGold, 2.5f))
            {
                g.DrawLine(borderPen, 1, 0, 1, this.Height);
                g.DrawLine(borderPen, this.Width - 2, 0, this.Width - 2, this.Height);
            }

            // Wizardly decorative corner flourishes in parchment area
            DrawCornerFiligree(g, 8, 48);
            DrawCornerFiligree(g, this.Width - 22, 48);
            DrawCornerFiligree(g, 8, this.Height - 66);
            DrawCornerFiligree(g, this.Width - 22, this.Height - 66);
        }

        private void DrawCornerFiligree(Graphics g, int x, int y)
        {
            using (Pen p = new Pen(Color.FromArgb(170, 136, 60), 1f))
            {
                g.DrawLine(p, x, y, x + 14, y);
                g.DrawLine(p, x, y, x, y + 14);
            }
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private Button CreateDialogButton(string text, bool isPrimary, int width = 105, int height = 35)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Georgia", 9.5F, FontStyle.Bold),
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = isPrimary ? Color.FromArgb(229, 181, 58) : Color.FromArgb(238, 222, 178)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Paint += (s, e) =>
            {
                var b = (Button)s!;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var rect = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                using var path = CreateRoundedRectanglePath(rect, 6);

                bool isHover = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));

                if (isPrimary)
                {
                    // Radiant Antique Gold gradient button
                    Color top = isHover ? Color.FromArgb(255, 220, 80) : Color.FromArgb(232, 184, 52);
                    Color bot = isHover ? Color.FromArgb(240, 180, 30) : Color.FromArgb(200, 146, 22);
                    using (var brush = new LinearGradientBrush(rect, top, bot, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (var pen = new Pen(isHover ? Color.FromArgb(255, 245, 160) : Color.FromArgb(245, 205, 90), 1.2f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                    TextRenderer.DrawText(
                        e.Graphics,
                        b.Text,
                        b.Font,
                        rect,
                        Color.FromArgb(38, 10, 18),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else
                {
                    // Refined warm parchment chip button
                    Color bg = isHover ? Color.FromArgb(250, 240, 218) : Color.FromArgb(232, 212, 172);
                    using (var brush = new SolidBrush(bg))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (var pen = new Pen(isHover ? Color.FromArgb(210, 165, 65) : Color.FromArgb(178, 138, 55), 1.2f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                    TextRenderer.DrawText(
                        e.Graphics,
                        b.Text,
                        b.Font,
                        rect,
                        Color.FromArgb(44, 14, 22),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };

            btn.MouseEnter += (s, e) => btn.Invalidate();
            btn.MouseLeave += (s, e) => btn.Invalidate();

            return btn;
        }
    }
}

