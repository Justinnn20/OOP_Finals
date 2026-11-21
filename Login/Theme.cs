using System.Drawing;
using System.Windows.Forms;

namespace UMakCoop.Views
{
    /// <summary>The wizarding-school colours and button styles, in one place so every screen matches.</summary>
    public static class Theme
    {
        public static readonly Color Wine = ColorTranslator.FromHtml("#2A0F18");
        public static readonly Color WineDark = ColorTranslator.FromHtml("#1C0A10");
        public static readonly Color Gold = ColorTranslator.FromHtml("#D4A017");
        public static readonly Color GoldHover = ColorTranslator.FromHtml("#F0C75E");
        public static readonly Color Parchment = ColorTranslator.FromHtml("#F1E2BD");
        public static readonly Color Paper = ColorTranslator.FromHtml("#FFFAF0");
        public static readonly Color Ink = ColorTranslator.FromHtml("#3B2415");
        public static readonly Color Crimson = ColorTranslator.FromHtml("#8C1C2C");
        public static readonly Color Ok = ColorTranslator.FromHtml("#2F6B3A");

        public static Font Serif(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Georgia", size, style);
        }

        public static Font Sans(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }

        /// <summary>Discount and payment "chips": a flat radio button that fills with gold when selected.</summary>
        public static void Chip(RadioButton rb)
        {
            rb.Appearance = Appearance.Button;
            rb.FlatStyle = FlatStyle.Flat;
            rb.UseVisualStyleBackColor = false;
            rb.AutoSize = true;
            rb.TextAlign = ContentAlignment.MiddleCenter;
            rb.Padding = new Padding(6, 2, 6, 2);
            rb.Margin = new Padding(0, 0, 6, 6);
            rb.BackColor = Color.FromArgb(255, 250, 240);
            rb.ForeColor = Ink;
            rb.Font = Sans(9.5f);
            rb.Cursor = Cursors.Hand;
            rb.FlatAppearance.BorderColor = Gold;
            rb.FlatAppearance.CheckedBackColor = Gold;
            rb.FlatAppearance.MouseOverBackColor = Color.FromArgb(246, 232, 190);
        }

        /// <summary>Big gold action button. Turns grey when disabled.</summary>
        public static void GoldButton(Button b, float fontSize = 11f)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = GoldHover;
            b.Font = Serif(fontSize, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            System.EventHandler paint = (s, e) =>
            {
                b.BackColor = b.Enabled ? Gold : Color.FromArgb(181, 162, 122);
                b.ForeColor = b.Enabled ? Wine : Color.FromArgb(107, 90, 58);
            };
            b.EnabledChanged += paint;
            paint(b, System.EventArgs.Empty);
        }

        /// <summary>Small outlined button (quick cash amounts).</summary>
        public static void SmallButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.AutoSize = true;
            b.Margin = new Padding(0, 0, 6, 6);
            b.Padding = new Padding(4, 0, 4, 0);
            b.BackColor = Color.FromArgb(255, 250, 240);
            b.ForeColor = Ink;
            b.Font = Sans(9f);
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderColor = Gold;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(246, 232, 190);
        }
    }
}
