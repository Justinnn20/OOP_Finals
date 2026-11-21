using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Windows.Forms;
using Login.Models;

namespace Login
{
    public sealed class OrderReceiptForm : Form
    {
        private const int PaperWidth = 306;
        private const int ButtonWidth = 230;
        private const int ButtonHeight = 30;
        private const int ButtonGap = 8;
        private const int BottomPadding = 16;

        // Space reserved under the receipt text for the two buttons (NEW SALE + PRINT RECEIPT).
        private const int ButtonsSpace = ButtonHeight * 2 + ButtonGap + BottomPadding;

        // Space under the receipt text when it is printed (no buttons).
        private const int PrintPadding = 14;

        private static readonly Color Paper = Color.FromArgb(255, 250, 240);
        private static readonly Color Ink = Color.FromArgb(59, 36, 21);
        private static readonly Color Gold = Color.FromArgb(212, 160, 23);
        private static readonly Color Wine = Color.FromArgb(42, 15, 24);
        private static readonly Color Crimson = Color.FromArgb(140, 28, 44);

        private readonly Order _order;
        private readonly string _cashier;
        private readonly Panel _receipt;
        private readonly int _contentEnd;   // y where the receipt text (and stamp) ends

        public OrderReceiptForm(Order order, string cashierName = "Cashier")
        {
            _order = order;
            _cashier = cashierName;
            _contentEnd = MeasureContentEnd();

            int paperHeight = _contentEnd + ButtonsSpace;

            Text = "Receipt " + order.OrderId;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            BackColor = Wine;
            AutoScroll = true;   // very long orders scroll instead of running off the screen

            int maxHeight = (int)(Screen.PrimaryScreen!.WorkingArea.Height * 0.9);
            int wantedHeight = paperHeight + 24;
            int extraWidth = wantedHeight > maxHeight ? SystemInformation.VerticalScrollBarWidth : 0;
            ClientSize = new Size(PaperWidth + 24 + extraWidth, Math.Min(wantedHeight, maxHeight));

            _receipt = new Panel
            {
                Location = new Point(12, 12),
                Size = new Size(PaperWidth, paperHeight),
                BackColor = Paper
            };
            _receipt.Paint += PaintReceipt;
            Controls.Add(_receipt);

            int buttonX = (PaperWidth - ButtonWidth) / 2;

            var newSale = MakeButton("NEW SALE", buttonX, _contentEnd, true);
            newSale.DialogResult = DialogResult.OK;
            newSale.Click += (s, e) => Close();
            _receipt.Controls.Add(newSale);

            var print = MakeButton("PRINT RECEIPT", buttonX, _contentEnd + ButtonHeight + ButtonGap, false);
            print.Click += (s, e) => PrintReceipt();
            _receipt.Controls.Add(print);

            AcceptButton = newSale;
            CancelButton = newSale;
        }

        // ------------------------------------------------------------------ buttons

        private static Button MakeButton(string text, int x, int y, bool primary)
        {
            var button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(ButtonWidth, ButtonHeight),
                BackColor = primary ? Gold : Paper,
                ForeColor = Ink,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Georgia", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = Gold;
            return button;
        }

        // ------------------------------------------------------------------ printing

        private void PrintReceipt()
        {
            int paperHeight = _contentEnd + PrintPadding;

            using var doc = new PrintDocument { DocumentName = "Receipt " + _order.OrderId };
            doc.PrintPage += (s, e) =>
            {
                var g = e.Graphics!;
                Rectangle m = e.MarginBounds;

                // Same receipt as on screen, shrunk only if it is bigger than the page.
                float scale = Math.Min(1f, Math.Min(m.Width / (float)PaperWidth,
                                                    m.Height / (float)paperHeight));
                g.TranslateTransform(m.Left, m.Top);
                g.ScaleTransform(scale, scale);
                DrawReceipt(g, PrintPadding);
                e.HasMorePages = false;
            };

            try
            {
                // Preview first: works for testing, and has its own Print button inside.
                using var preview = new PrintPreviewDialog { Document = doc, Width = 760, Height = 720 };
                preview.ShowDialog(this);
            }
            catch (InvalidPrinterException)
            {
                MessageBox.Show("No printer is installed on this computer.", "Print receipt",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ------------------------------------------------------------------ drawing

        private void PaintReceipt(object? sender, PaintEventArgs e)
        {
            DrawReceipt(e.Graphics, ButtonsSpace);
        }

        /// <summary>Runs the drawing on a throw-away bitmap just to learn how tall the receipt text is.</summary>
        private int MeasureContentEnd()
        {
            using var bitmap = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bitmap);
            return DrawReceipt(g, 0);
        }

        /// <summary>
        /// Draws the whole receipt (used for the screen AND for printing).
        /// bottomSpace is the room left under the text (buttons on screen, a small margin when printing).
        /// Returns the y position where the text and stamp end.
        /// </summary>
        private int DrawReceipt(Graphics g, int bottomSpace)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            using var goldPen = new Pen(Gold, 1.5f);
            using var winePen = new Pen(Wine, 1f);
            using var titleFont = new Font("Georgia", 11f, FontStyle.Bold);
            using var small = new Font("Georgia", 7.5f);
            using var itemFont = new Font("Georgia", 8.5f);
            using var totalFont = new Font("Georgia", 9f, FontStyle.Bold);
            using var brush = new SolidBrush(Ink);
            using var dashPen = new Pen(Color.FromArgb(190, 160, 100), 1f) { DashStyle = DashStyle.Dot };

            // Header
            DrawCentered(g, "UNIVERSITY OF MAKATI COOP", titleFont, brush, 18, 18);
            DrawCentered(g, _order.OrderId + " · " + _order.CreatedAt.ToString("MM/dd/yy, h:mm tt"), small, brush, 34, 16);
            DrawCentered(g, "Cashier · " + _cashier, small, brush, 48, 16);
            g.DrawLine(dashPen, 20, 72, 286, 72);

            // Items
            int y = 82;
            foreach (var item in _order.Items)
            {
                DrawLeft(g, item.Product.Name + " ×" + item.Quantity, itemFont, brush, 20, y, 168, 18);
                DrawRight(g, Peso(item.TotalPrice), itemFont, brush, 188, y, 96, 18);
                y += 20;
            }

            // Totals
            y += 5;
            g.DrawLine(dashPen, 20, y, 286, y);
            y += 8;

            DrawLeft(g, "Subtotal", itemFont, brush, 20, y, 168, 18);
            DrawRight(g, Peso(_order.Subtotal), itemFont, brush, 188, y, 96, 18);
            y += 20;

            if (_order.DiscountAmount > 0m)
            {
                DrawLeft(g, "Discount (" + _order.DiscountName + ")", itemFont, brush, 20, y, 168, 18);
                DrawRight(g, "-" + Peso(_order.DiscountAmount), itemFont, brush, 188, y, 96, 18);
                y += 20;
            }

            y += 2;
            DrawLeft(g, "Total", totalFont, brush, 20, y, 168, 20);
            DrawRight(g, Peso(_order.Total), totalFont, brush, 188, y, 96, 20);
            y += 26;

            // Payment
            if (string.Equals(_order.PaymentMethod, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                DrawLeft(g, "Cash", itemFont, brush, 20, y, 168, 18);
                DrawRight(g, Peso(_order.CashReceived), itemFont, brush, 188, y, 96, 18);
                y += 20;
                DrawLeft(g, "Change", itemFont, brush, 20, y, 168, 18);
                DrawRight(g, Peso(_order.Change), itemFont, brush, 188, y, 96, 18);
                y += 20;
            }
            else
            {
                DrawLeft(g, "Paid via " + _order.PaymentMethod, itemFont, brush, 20, y, 168, 18);
                DrawRight(g, Peso(_order.Total), itemFont, brush, 188, y, 96, 18);
                y += 20;
            }

            // Footer
            y += 4;
            g.DrawLine(dashPen, 20, y, 286, y);
            y += 6;
            DrawCentered(g, "Thank you! Keep this receipt for exchanges.", small, brush, y, 18);
            y += 24;

            // PAID stamp gets its own row, so it can never sit on top of the buttons or the text.
            const int stampSize = 58;
            DrawStamp(g, PaperWidth / 2, y + stampSize / 2, stampSize);
            y += stampSize + 14;

            // Double border around the whole paper
            int paperHeight = y + bottomSpace;
            g.DrawRectangle(goldPen, 5, 5, PaperWidth - 11, paperHeight - 11);
            g.DrawRectangle(winePen, 9, 9, PaperWidth - 19, paperHeight - 19);

            return y;
        }

        private static void DrawStamp(Graphics g, int centerX, int centerY, int size)
        {
            GraphicsState state = g.Save();
            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(-12f);

            var rect = new Rectangle(-size / 2, -size / 2, size, size);
            using var fill = new SolidBrush(Crimson);
            using var border = new Pen(Color.FromArgb(105, 18, 30), 2f);
            using var ring = new Pen(Color.FromArgb(200, 255, 255, 255), 1f);
            using var font = new Font("Georgia", 10f, FontStyle.Bold);
            using var white = new SolidBrush(Color.White);
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            g.FillEllipse(fill, rect);
            g.DrawEllipse(border, rect);
            g.DrawEllipse(ring, Rectangle.Inflate(rect, -4, -4));
            g.DrawString("PAID", font, white, rect, format);

            g.Restore(state);
        }

        private static void DrawCentered(Graphics g, string text, Font font, Brush brush, int y, int height)
        {
            var rect = new Rectangle(18, y, 270, height);
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(text, font, brush, rect, format);
        }

        private static void DrawLeft(Graphics g, string text, Font font, Brush brush, int x, int y, int width, int height)
        {
            using var format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };
            g.DrawString(text, font, brush, new Rectangle(x, y, width, height), format);
        }

        private static void DrawRight(Graphics g, string text, Font font, Brush brush, int x, int y, int width, int height)
        {
            using var format = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
            g.DrawString(text, font, brush, new Rectangle(x, y, width, height), format);
        }

        /// <summary>Whole pesos show as ₱700, amounts with centavos (discounts) show as ₱1,129.50.</summary>
        private static string Peso(decimal amount)
        {
            return "₱" + amount.ToString(amount % 1m == 0m ? "N0" : "N2");
        }
    }
}