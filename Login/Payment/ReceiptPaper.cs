using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using UMakCoop.Models;
using UMakCoop.Services;

namespace UMakCoop.Views
{
    /// <summary>
    /// The receipt drawn like a carbon-copy bill pad: crest and shop name on top, "Sold to / Receipt No." fields,
    /// a ruled table (No. / Description / Pcs. / Rate / Amount), payment details on the left, the totals box on the
    /// right, then terms and signature lines. Colours come from Theme (wine, gold, parchment).
    /// The same Draw method is used on screen (ReceiptForm) and on paper (Print), so they always match.
    /// </summary>
    public class ReceiptPaper : Control
    {
        public const int PaperWidth = 520;

        private const int Edge = 16;            // space above the header / below the signatures
        private const int Pad = 24;             // left and right margin inside the border
        private const int RowH = 24;
        private const int MinRows = 8;          // an empty pad still shows ruled lines
        private const int HeaderH = 96;
        private const int FieldsH = 104;
        private const int TableHeadH = 28;
        private const int FooterH = 118;
        private const int SignatureH = 110;

        private const int ColNo = 36, ColPcs = 50, ColRate = 82, ColAmt = 96;
        private const int ColDesc = PaperWidth - 2 * Pad - ColNo - ColPcs - ColRate - ColAmt;

        private readonly Receipt _receipt;
        private readonly bool _isReprint;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer { Interval = 15 };
        private DateTime _stampStart;
        private float _stampT;

        private readonly Font _fCrest = Theme.Serif(20f, FontStyle.Bold);
        private readonly Font _fTitle = Theme.Serif(26f, FontStyle.Bold);
        private readonly Font _fSub = Theme.Sans(9.5f, FontStyle.Bold);
        private readonly Font _fTag = Theme.Serif(9f, FontStyle.Italic);
        private readonly Font _fBadge = Theme.Serif(13f, FontStyle.Bold);
        private readonly Font _fHead = Theme.Serif(10.5f, FontStyle.Bold);
        private readonly Font _fLabel = Theme.Sans(9f, FontStyle.Bold);
        private readonly Font _fValue = Theme.Sans(10f);
        private readonly Font _fCell = Theme.Sans(9.5f);
        private readonly Font _fSmall = Theme.Sans(8f);
        private readonly Font _fGrand = Theme.Serif(14f, FontStyle.Bold);
        private readonly Font _fGrandLabel = Theme.Serif(9f, FontStyle.Bold);
        private readonly Font _fFor = Theme.Serif(11f, FontStyle.Italic);
        private readonly Font _fMark = Theme.Serif(11f, FontStyle.Bold);
        private readonly Font _fBig = Theme.Serif(60f, FontStyle.Bold);

        private readonly Brush _bPaper = new SolidBrush(Theme.Paper);
        private readonly Brush _bWine = new SolidBrush(Theme.Wine);
        private readonly Brush _bGold = new SolidBrush(Theme.Gold);
        private readonly Brush _bCrimson = new SolidBrush(Theme.Crimson);
        private readonly Brush _bInk = new SolidBrush(Theme.Ink);
        private readonly Brush _bParch = new SolidBrush(Theme.Parchment);

        private readonly Pen _penGold1 = new Pen(Theme.Gold, 1f);
        private readonly Pen _penGold2 = new Pen(Theme.Gold, 2f);
        private readonly Pen _penWine = new Pen(Theme.Wine, 1f);
        private readonly Pen _penDot = new Pen(Color.FromArgb(190, 160, 100), 1f) { DashStyle = DashStyle.Dot };
        private readonly Pen _penFaint = new Pen(Color.FromArgb(225, 205, 160), 1f);

        public ReceiptPaper(Receipt receipt, bool isReprint)
        {
            _receipt = receipt;
            _isReprint = isReprint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Theme.Paper;
            Size = new Size(PaperWidth, TotalHeight());
            _timer.Tick += OnTick;
        }

        private bool IsVoided { get { return _receipt.Status == ReceiptStatus.Voided; } }

        private int RowCount { get { return Math.Max(MinRows, _receipt.Lines.Count); } }

        private int TotalHeight()
        {
            return Edge + HeaderH + FieldsH + 6 + TableHeadH + RowCount * RowH + 10 + FooterH + SignatureH + Edge;
        }

        /// <summary>The wax-seal stamp thumps down onto the signature area.</summary>
        public void PlayStamp()
        {
            _stampStart = DateTime.Now.AddMilliseconds(250);   // small pause so it lands after the popup opens
            _stampT = 0f;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            double ms = (DateTime.Now - _stampStart).TotalMilliseconds;
            _stampT = ms <= 0 ? 0f : (float)Math.Min(1.0, ms / 350.0);
            Invalidate();
            if (_stampT >= 1f) _timer.Stop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics, false, _stampT);
        }

        // ---------------------------------------------------------------- drawing

        /// <summary>
        /// Draws the whole receipt with its top-left corner at (0,0), PaperWidth wide and Height tall.
        /// forPrint skips the paper-coloured background so the printer does not waste ink.
        /// stampProgress: 0 = no stamp yet, 1 = stamp fully down.
        /// </summary>
        public void Draw(Graphics g, bool forPrint, float stampProgress)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = forPrint ? TextRenderingHint.AntiAlias : TextRenderingHint.ClearTypeGridFit;

            int height = TotalHeight();
            if (!forPrint) g.FillRectangle(_bPaper, 0, 0, PaperWidth, height);
            g.DrawRectangle(_penGold2, 6, 6, PaperWidth - 13, height - 13);
            g.DrawRectangle(_penWine, 11, 11, PaperWidth - 23, height - 23);

            int y = Edge;
            DrawHeader(g, y);
            y += HeaderH;
            DrawFields(g, y);
            y += FieldsH + 6;

            int tableTop = y;
            DrawTable(g, tableTop);
            y += TableHeadH + RowCount * RowH + 10;

            DrawFooter(g, y);
            y += FooterH;
            DrawSignatures(g, y, stampProgress);

            if (IsVoided) DrawVoidWatermark(g, tableTop);
        }

        private void DrawHeader(Graphics g, int top)
        {
            // Crest
            const int d = 66;
            var crest = new Rectangle(Pad, top + 10, d, d);
            g.FillEllipse(_bWine, crest);
            using (var ring = new Pen(Theme.Gold, 2.5f))
                g.DrawEllipse(ring, crest.X + 4, crest.Y + 4, d - 8, d - 8);
            DrawText(g, "UC", _fCrest, _bGold, crest, StringAlignment.Center, StringAlignment.Center);

            // Shop name
            float tx = Pad + d + 14;
            DrawText(g, "UMak Coop", _fTitle, _bCrimson, new RectangleF(tx, top + 6, 260, 40), StringAlignment.Near, StringAlignment.Center);
            DrawText(g, "UNIVERSITY OF MAKATI BOOKSTORE", _fSub, _bInk, new RectangleF(tx, top + 46, 280, 18), StringAlignment.Near, StringAlignment.Center);
            DrawText(g, "Official Cooperative Receipt", _fTag, _bInk, new RectangleF(tx, top + 63, 260, 16), StringAlignment.Near, StringAlignment.Center);

            // RECEIPT badge and status marks
            var badge = new RectangleF(PaperWidth - Pad - 130, top + 12, 130, 30);
            g.FillRectangle(_bWine, badge);
            g.DrawRectangle(_penGold1, badge.X + 2, badge.Y + 2, badge.Width - 4, badge.Height - 4);
            DrawText(g, "RECEIPT", _fBadge, _bGold, badge, StringAlignment.Center, StringAlignment.Center);

            float markY = top + 46;
            if (_isReprint)
            {
                DrawText(g, "*** REPRINT ***", _fMark, _bCrimson, new RectangleF(PaperWidth - Pad - 160, markY, 160, 18), StringAlignment.Far, StringAlignment.Center);
                markY += 18;
            }
            if (IsVoided)
                DrawText(g, "*** VOIDED ***", _fMark, _bCrimson, new RectangleF(PaperWidth - Pad - 160, markY, 160, 18), StringAlignment.Far, StringAlignment.Center);

            g.DrawLine(_penGold2, Pad, top + HeaderH - 6, PaperWidth - Pad, top + HeaderH - 6);
        }

        private void DrawFields(Graphics g, int top)
        {
            float leftX = Pad, leftW = 250;
            float rightX = Pad + 270, rightW = PaperWidth - Pad - rightX;
            float y = top + 4;

            Field(g, "Sold to:", _receipt.StudentName ?? "Walk-in", leftX, y, leftW, 66);
            Field(g, "Cashier:", _receipt.CashierName, leftX, y + RowH, leftW, 66);
            Field(g, "Discount:", _receipt.Discount.ToString(), leftX, y + 2 * RowH, leftW, 66);
            if (_receipt.DiscountIdNumber != null)
                Field(g, "ID no.:", _receipt.DiscountIdNumber, leftX, y + 3 * RowH, leftW, 66);

            Field(g, "Receipt No.:", _receipt.ReceiptNo, rightX, y, rightW, 82);
            Field(g, "Date:", _receipt.IssuedAt.ToString("MMM d, yyyy", CultureInfo.InvariantCulture), rightX, y + RowH, rightW, 82);
            Field(g, "Time:", _receipt.IssuedAt.ToString("h:mm tt", CultureInfo.InvariantCulture), rightX, y + 2 * RowH, rightW, 82);
            Field(g, "Items:", _receipt.ItemCount.ToString(CultureInfo.InvariantCulture), rightX, y + 3 * RowH, rightW, 82);
        }

        private void DrawTable(Graphics g, int top)
        {
            int rows = RowCount;
            int bodyTop = top + TableHeadH;
            int bodyHeight = rows * RowH;

            // Column header bar: wine / gold / crimson / crimson / wine, like the coloured headers on a bill pad.
            int x = Pad;
            HeadCell(g, "No.", x, top, ColNo, _bWine, _bGold, StringAlignment.Center); x += ColNo;
            HeadCell(g, "Description", x, top, ColDesc, _bGold, _bWine, StringAlignment.Near); x += ColDesc;
            HeadCell(g, "Pcs.", x, top, ColPcs, _bCrimson, _bParch, StringAlignment.Center); x += ColPcs;
            HeadCell(g, "Rate", x, top, ColRate, _bCrimson, _bParch, StringAlignment.Far); x += ColRate;
            HeadCell(g, "Amount", x, top, ColAmt, _bWine, _bGold, StringAlignment.Far);

            // Faint column lines
            int[] edges = { Pad + ColNo, Pad + ColNo + ColDesc, Pad + ColNo + ColDesc + ColPcs, Pad + ColNo + ColDesc + ColPcs + ColRate };
            foreach (int ex in edges) g.DrawLine(_penFaint, ex, bodyTop, ex, bodyTop + bodyHeight);

            for (int i = 0; i < rows; i++)
            {
                int ry = bodyTop + i * RowH;
                g.DrawLine(_penDot, Pad, ry + RowH, PaperWidth - Pad, ry + RowH);

                if (i >= _receipt.Lines.Count) continue;
                ReceiptLine line = _receipt.Lines[i];

                string name = line.Size != null ? line.ProductName + " (" + line.Size + ")" : line.ProductName;
                int cx = Pad;
                Cell(g, (i + 1).ToString(CultureInfo.InvariantCulture), cx, ry, ColNo, StringAlignment.Center); cx += ColNo;
                Cell(g, name, cx, ry, ColDesc, StringAlignment.Near); cx += ColDesc;
                Cell(g, line.Quantity.ToString(CultureInfo.InvariantCulture), cx, ry, ColPcs, StringAlignment.Center); cx += ColPcs;
                Cell(g, Money.Peso(line.UnitPrice), cx, ry, ColRate, StringAlignment.Far); cx += ColRate;
                Cell(g, Money.Peso(line.LineTotal), cx, ry, ColAmt, StringAlignment.Far);
            }

            g.DrawRectangle(_penWine, Pad, top, PaperWidth - 2 * Pad, TableHeadH + bodyHeight);
        }

        private void DrawFooter(Graphics g, int top)
        {
            // Payment details (left)
            float x = Pad, w = 226, labelW = 94, y = top;
            Field(g, "Payment:", _receipt.Payment.Method.ToString(), x, y, w, labelW);

            var cash = _receipt.Payment as CashPayment;
            var gcash = _receipt.Payment as GCashPayment;
            if (cash != null)
            {
                Field(g, "Cash received:", Money.Peso(cash.CashReceived), x, y + RowH, w, labelW);
                Field(g, "Change:", Money.Peso(cash.Change), x, y + 2 * RowH, w, labelW);
            }
            else if (gcash != null)
            {
                Field(g, "GCash ref no.:", gcash.ReferenceNumber, x, y + RowH, w, labelW);
                Field(g, "Amount paid:", Money.Peso(_receipt.Total), x, y + 2 * RowH, w, labelW);
            }

            if (IsVoided)
            {
                string when = _receipt.VoidedAt.HasValue
                    ? _receipt.VoidedAt.Value.ToString("MMM d, h:mm tt", CultureInfo.InvariantCulture)
                    : "";
                string note = "Voided by " + _receipt.VoidedBy + " (" + when + ")\nReason: " + _receipt.VoidReason;
                g.DrawString(note, _fSmall, _bCrimson, new RectangleF(x, y + 3 * RowH + 6, w, 40));
            }

            // Totals box (right)
            float bx = PaperWidth - Pad - 230, bw = 230;
            g.FillRectangle(_bWine, bx, top, bw, 56);

            DrawText(g, "Subtotal", _fCell, _bParch, new RectangleF(bx + 10, top, 100, 28), StringAlignment.Near, StringAlignment.Center);
            DrawText(g, Money.Peso(_receipt.Subtotal), _fCell, _bParch, new RectangleF(bx + 100, top, bw - 110, 28), StringAlignment.Far, StringAlignment.Center);

            string discountLabel = _receipt.DiscountAmount > 0m ? "Less: " + _receipt.Discount : "Discount";
            string discountValue = _receipt.DiscountAmount > 0m ? "-" + Money.Peso(_receipt.DiscountAmount) : Money.Peso(0m);
            DrawText(g, discountLabel, _fSmall, _bParch, new RectangleF(bx + 10, top + 28, 130, 28), StringAlignment.Near, StringAlignment.Center);
            DrawText(g, discountValue, _fCell, _bParch, new RectangleF(bx + 100, top + 28, bw - 110, 28), StringAlignment.Far, StringAlignment.Center);

            g.FillRectangle(_bGold, bx, top + 56, bw, 40);
            DrawText(g, "GRAND TOTAL", _fGrandLabel, _bWine, new RectangleF(bx + 10, top + 56, 120, 40), StringAlignment.Near, StringAlignment.Center);
            DrawText(g, Money.Peso(_receipt.Total), _fGrand, _bWine, new RectangleF(bx + 120, top + 56, bw - 130, 40), StringAlignment.Far, StringAlignment.Center);
            g.DrawRectangle(_penGold2, bx, top, bw, 96);
        }

        private void DrawSignatures(Graphics g, int top, float stampProgress)
        {
            // Terms strip (left)
            var strip = new RectangleF(Pad, top + 8, 230, 46);
            g.FillRectangle(_bCrimson, strip);
            g.DrawString("1) Keep this receipt for exchanges.\n2) A voided receipt is no longer valid.",
                _fSmall, _bParch, new RectangleF(strip.X + 8, strip.Y + 6, strip.Width - 16, strip.Height - 8));

            // Receiver's sign (left)
            g.DrawLine(_penWine, Pad, top + 88, Pad + 170, top + 88);
            DrawText(g, "Receiver's Sign.", _fSmall, _bInk, new RectangleF(Pad, top + 90, 170, 16), StringAlignment.Center, StringAlignment.Near);

            // For: UMak Coop / cashier (right)
            float rx = PaperWidth - Pad - 170;
            DrawText(g, "For: UMak Coop", _fFor, _bCrimson, new RectangleF(rx, top + 6, 170, 22), StringAlignment.Far, StringAlignment.Center);
            DrawText(g, _receipt.CashierName, _fValue, _bInk, new RectangleF(rx, top + 62, 170, 24), StringAlignment.Center, StringAlignment.Far);
            g.DrawLine(_penWine, rx, top + 88, rx + 170, top + 88);
            DrawText(g, "Cashier's Sign.", _fSmall, _bInk, new RectangleF(rx, top + 90, 170, 16), StringAlignment.Center, StringAlignment.Near);

            // Wax-seal stamp between the two sides
            float cx = (Pad + 230 + rx) / 2f;
            StampControl.DrawStamp(g, cx, top + 52, IsVoided ? "VOID" : "PAID",
                IsVoided ? Color.DimGray : Theme.Crimson, stampProgress, 0.85f);
        }

        private void DrawVoidWatermark(Graphics g, int tableTop)
        {
            GraphicsState state = g.Save();
            g.TranslateTransform(PaperWidth / 2f, tableTop + (TableHeadH + RowCount * RowH) / 2f);
            g.RotateTransform(-18f);
            using (var brush = new SolidBrush(Color.FromArgb(55, Theme.Crimson)))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString("VOIDED", _fBig, brush, 0, 0, format);
            g.Restore(state);
        }

        // ---------------------------------------------------------------- helpers

        /// <summary>A bold label, then the value sitting on a gold underline: the "M/S ____" style of a bill pad.</summary>
        private void Field(Graphics g, string label, string value, float x, float y, float width, float labelWidth)
        {
            DrawText(g, label, _fLabel, _bCrimson, new RectangleF(x, y, labelWidth, RowH - 2), StringAlignment.Near, StringAlignment.Far);
            DrawText(g, value, _fValue, _bInk, new RectangleF(x + labelWidth, y, width - labelWidth, RowH - 2), StringAlignment.Near, StringAlignment.Far);
            g.DrawLine(_penGold1, x + labelWidth, y + RowH - 2, x + width, y + RowH - 2);
        }

        private void HeadCell(Graphics g, string text, int x, int y, int width, Brush fill, Brush ink, StringAlignment align)
        {
            g.FillRectangle(fill, x, y, width, TableHeadH);
            DrawText(g, text, _fHead, ink, new RectangleF(x + 6, y, width - 12, TableHeadH), align, StringAlignment.Center);
        }

        private void Cell(Graphics g, string text, int x, int y, int width, StringAlignment align)
        {
            DrawText(g, text, _fCell, _bInk, new RectangleF(x + 6, y, width - 12, RowH), align, StringAlignment.Center);
        }

        private static void DrawText(Graphics g, string text, Font font, Brush brush, RectangleF area,
                                 StringAlignment horizontal, StringAlignment vertical)
        {
            using (var format = new StringFormat(StringFormatFlags.NoWrap))
            {
                format.Alignment = horizontal;
                format.LineAlignment = vertical;
                format.Trimming = StringTrimming.EllipsisCharacter;
                g.DrawString(text, font, brush, area, format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
                foreach (Font f in new[] { _fCrest, _fTitle, _fSub, _fTag, _fBadge, _fHead, _fLabel, _fValue, _fCell, _fSmall, _fGrand, _fGrandLabel, _fFor, _fMark, _fBig })
                    f.Dispose();
                foreach (Brush b in new[] { _bPaper, _bWine, _bGold, _bCrimson, _bInk, _bParch })
                    b.Dispose();
                foreach (Pen p in new[] { _penGold1, _penGold2, _penWine, _penDot, _penFaint })
                    p.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
