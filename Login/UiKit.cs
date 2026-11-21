using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// Shared theme + custom controls for the UMak Coop "Wizarding POS" look.
// Reuse Theme / AppFonts / RoundedPanel / RoundedButton in the other forms too,
// so every screen your group builds matches.
namespace Login
{
    public static class CoopTheme
    {
        // Gold
        public static readonly Color GoldBright = ColorTranslator.FromHtml("#F1C84B");
        public static readonly Color Gold = ColorTranslator.FromHtml("#E8B923");
        public static readonly Color GoldDeep = ColorTranslator.FromHtml("#C99A1E");
        public static readonly Color GoldBottom = ColorTranslator.FromHtml("#C48E12");
        public static readonly Color GoldDark = ColorTranslator.FromHtml("#8A6410");
        public static readonly Color GoldEdge = ColorTranslator.FromHtml("#6B4C0C");

        // Maroon backgrounds
        public static readonly Color Maroon700 = ColorTranslator.FromHtml("#3D1526");
        public static readonly Color Maroon900 = ColorTranslator.FromHtml("#170810");
        public static readonly Color HeaderBg = ColorTranslator.FromHtml("#1B0911");
        public static readonly Color Ink = ColorTranslator.FromHtml("#2A0F1A");

        // Parchment + text
        public static readonly Color Parchment100 = ColorTranslator.FromHtml("#F7E9BF");
        public static readonly Color Parchment200 = ColorTranslator.FromHtml("#EBD596");
        public static readonly Color InputFill = ColorTranslator.FromHtml("#FCF3D6");
        public static readonly Color InputBorder = ColorTranslator.FromHtml("#A97C14");
        public static readonly Color ShowBtnFill = ColorTranslator.FromHtml("#F1DFA6");
        public static readonly Color Brown = ColorTranslator.FromHtml("#3A1020");
        public static readonly Color BrownSoft = ColorTranslator.FromHtml("#5E3F33");
        public static readonly Color Cream = ColorTranslator.FromHtml("#F6E6B8");
        public static readonly Color CreamSoft = ColorTranslator.FromHtml("#E9D9AD");
        public static readonly Color FooterText = ColorTranslator.FromHtml("#D9C593");

        // Shelf tab colors
        public static readonly Color ChipBooks = ColorTranslator.FromHtml("#7A5A12");
        public static readonly Color ChipUniforms = ColorTranslator.FromHtml("#1F5A3A");
        public static readonly Color ChipSupplies = ColorTranslator.FromHtml("#5E4A10");
        public static readonly Color ChipMerch = ColorTranslator.FromHtml("#8E1F35");
    }

    // Uses Cinzel / Cormorant Garamond when they are installed, otherwise Georgia.
    public static class AppFonts
    {
        private static readonly HashSet<string> Installed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        static AppFonts()
        {
            foreach (FontFamily f in FontFamily.Families) Installed.Add(f.Name);
        }

        public static string HeadingName
        {
            get { return Installed.Contains("Cinzel") ? "Cinzel" : "Georgia"; }
        }

        public static string BodyName
        {
            get { return Installed.Contains("Cormorant Garamond") ? "Cormorant Garamond" : "Georgia"; }
        }

        public static Font Heading(float pt, FontStyle style = FontStyle.Regular)
        {
            return new Font(HeadingName, pt, style);
        }

        public static Font Body(float pt, FontStyle style = FontStyle.Regular)
        {
            return new Font(BodyName, pt, style);
        }
    }

    internal static class Gfx
    {
        public static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2f, Math.Min(r.Width, r.Height));
            var p = new GraphicsPath();
            if (d <= 0f)
            {
                p.AddRectangle(r);
                return p;
            }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static void FillGradient(Graphics g, GraphicsPath path, int width, int height, Color top, Color bottom)
        {
            if (top.ToArgb() == bottom.ToArgb())
            {
                using (var sb = new SolidBrush(top)) g.FillPath(sb, path);
            }
            else
            {
                var rect = new Rectangle(0, 0, Math.Max(1, width), Math.Max(1, height + 1));
                using (var lb = new LinearGradientBrush(rect, top, bottom, 90f)) g.FillPath(lb, path);
            }
        }
    }

    // Panel with rounded corners, optional gradient fill, border and centered text.
    public class RoundedPanel : Panel
    {
        public RoundedPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int CornerRadius { get; set; } = 12;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor { get; set; } = Color.Transparent;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float BorderWidth { get; set; } = 1.5f;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillTop { get; set; } = Color.Transparent;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillBottom { get; set; } = Color.Transparent;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float half = BorderWidth / 2f;
            var rect = new RectangleF(half, half, Width - BorderWidth - 1f, Height - BorderWidth - 1f);

            using (var path = Gfx.RoundedRect(rect, CornerRadius))
            {
                if (FillTop.A > 0 || FillBottom.A > 0)
                    Gfx.FillGradient(g, path, Width, Height, FillTop, FillBottom);

                if (BorderWidth > 0f && BorderColor.A > 0)
                {
                    using (var pen = new Pen(BorderColor, BorderWidth)) g.DrawPath(pen, path);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }
        }
    }

    // Button with rounded corners, gradient fill, border and hover/pressed feedback.
    public class RoundedButton : Button
    {
        private int _radius = 8;
        private Color _fillTop = Color.Transparent;
        private Color _fillBottom = Color.Transparent;
        private Color _border = Color.Transparent;
        private float _borderWidth = 1.5f;
        private bool _hover;
        private bool _down;

        public RoundedButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int CornerRadius
        {
            get { return _radius; }
            set { _radius = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillTop
        {
            get { return _fillTop; }
            set { _fillTop = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillBottom
        {
            get { return _fillBottom; }
            set { _fillBottom = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get { return _border; }
            set { _border = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            ButtonRenderer.DrawParentBackground(g, ClientRectangle, this);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float half = _borderWidth / 2f;
            var rect = new RectangleF(half, half, Width - _borderWidth - 1f, Height - _borderWidth - 1f);

            using (var path = Gfx.RoundedRect(rect, _radius))
            {
                Color top = _fillTop;
                Color bottom = _fillBottom;

                if (top.A > 0 || bottom.A > 0)
                {
                    if (_down)
                    {
                        top = ControlPaint.Dark(top, 0.05f);
                        bottom = ControlPaint.Dark(bottom, 0.05f);
                    }
                    else if (_hover)
                    {
                        top = ControlPaint.Light(top, 0.35f);
                        bottom = ControlPaint.Light(bottom, 0.35f);
                    }
                    Gfx.FillGradient(g, path, Width, Height, top, bottom);
                }
                else if ((_hover || _down) && _border.A > 0)
                {
                    using (var br = new SolidBrush(Color.FromArgb(_down ? 45 : 24, _border))) g.FillPath(br, path);
                }

                if (_borderWidth > 0f && _border.A > 0)
                {
                    using (var pen = new Pen(_border, _borderWidth)) g.DrawPath(pen, path);
                }
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // Keyboard focus ring (shows when you Tab to the button)
            if (Focused && ShowFocusCues)
            {
                var ring = new RectangleF(3.5f, 3.5f, Width - 8f, Height - 8f);
                using (var rp = Gfx.RoundedRect(ring, Math.Max(2, _radius - 3)))
                using (var pen = new Pen(CoopTheme.Gold, 2f)) g.DrawPath(pen, rp);
            }
        }
    }
}