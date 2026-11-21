using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace UMakCoop.Views
{
    /// <summary>The wax-seal "PAID" stamp. Call Play() and it thumps down onto the paper.</summary>
    public class StampControl : Control
    {
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer { Interval = 15 };
        private DateTime _start;
        private float _t;

        public StampControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(120, 120);
            StampText = "PAID";
            StampColor = Theme.Crimson;
            _timer.Tick += OnTick;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StampText { get; set; } = string.Empty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color StampColor { get; set; }

        public void Play()
        {
            _start = DateTime.Now.AddMilliseconds(250);   // small pause so it lands after the popup opens
            _t = 0f;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            double ms = (DateTime.Now - _start).TotalMilliseconds;
            _t = ms <= 0 ? 0f : (float)Math.Min(1.0, ms / 350.0);
            Invalidate();
            if (_t >= 1f) _timer.Stop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            DrawStamp(e.Graphics, Width / 2f, Height / 2f, StampText, StampColor, _t, 1f);
        }

        /// <summary>
        /// Draws the stamp centred on (cx, cy). progress goes 0 to 1 (0 = invisible, 1 = fully landed).
        /// size 1 = 96 px across. Shared with ReceiptPaper so the stamp can land right on the receipt.
        /// </summary>
        public static void DrawStamp(Graphics g, float cx, float cy, string text, Color color, float progress, float size)
        {
            if (progress <= 0f) return;

            float ease = 1f - (float)Math.Pow(1f - progress, 3);   // ease-out
            float scale = (1.25f - 0.25f * ease) * size;
            int alpha = (int)(255 * Math.Min(1f, progress * 3f));

            GraphicsState state = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-12f);
            g.ScaleTransform(scale, scale);

            const float r = 48f;
            using (var fill = new SolidBrush(Color.FromArgb(alpha, color)))
                g.FillEllipse(fill, -r, -r, r * 2, r * 2);
            using (var ring = new Pen(Color.FromArgb(alpha, 244, 227, 184), 2f))
                g.DrawEllipse(ring, -r + 6, -r + 6, (r - 6) * 2, (r - 6) * 2);

            using (var font = Theme.Serif(17f, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(alpha, 244, 227, 184)))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(text, font, brush, 0, 0, format);

            g.Restore(state);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
