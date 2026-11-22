using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Login
{
    // VIEW (MVC): only draws the screen and raises events.
    // No SQL here. A controller (e.g. AuthController) subscribes to LoginRequested,
    // checks the credentials, then opens the main form or calls ShowError(...).
    public partial class LoginPage : Form
    {
        // Raised when the user presses "Sign In". The controller or view reads the properties below.
        public event EventHandler LoginRequested;

        public string Username { get { return txtUser.Text.Trim(); } }
        public string Password { get { return txtPass.Text; } }
        public bool KeepSignedIn { get { return chkRemember.Checked; } }

        public LoginPage()
        {
            InitializeComponent();
            ApplyFontFallback(this);
        }

        public void ShowError(string message)
        {
            CoopMessageBox.Show(this, message, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPass.Clear();
            txtPass.Focus();
        }

        // ------------------------------------------------------------ form events

        private void LoginPage_Load(object sender, EventArgs e)
        {
            pnlRod.BringToFront();
            CenterVertically(txtUser);
            CenterVertically(txtPass);
            SetCue(txtUser, "Enter your username");
            SetCue(txtPass, "Enter your password");
            RepositionContent();
            ActiveControl = txtUser;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RepositionContent();
        }

        private void RepositionContent()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            int clientW = this.ClientSize.Width;
            int clientH = this.ClientSize.Height;

            bool isLarge = clientW >= 1440;

            // Dimensions:
            // Hero section width (strictly constrained so it never spills over)
            int heroW = isLarge ? 460 : 380;
            // Scroll card width and height
            int scrollW = isLarge ? 460 : 390;
            int scrollH = isLarge ? 490 : 440;

            // Compute gap and horizontal positions
            int remainingW = clientW - (heroW + scrollW);
            int gap = Math.Max(28, Math.Min(isLarge ? 90 : 54, remainingW / 3));

            int totalW = heroW + gap + scrollW;
            int startX = Math.Max(28, (clientW - totalW) / 2);

            // Crucial: scrollX is guaranteed to be strictly to the right of heroW + gap
            // Even if the form is shrunk down, scrollX will never intersect heroW
            int scrollX = Math.Max(startX + heroW + gap, clientW - scrollW - Math.Max(24, startX));

            int availH = clientH - pnlHeader.Height - lblFooter.Height;
            int startY = pnlHeader.Height + Math.Max(16, (availH - scrollH) / 2);

            // Left Hero Positioning - strictly bounded to heroW
            lblEyebrow.Location = new Point(startX, startY + 12);
            lblEyebrow.Size = new Size(heroW, 24);
            lblEyebrow.Font = new Font("Cinzel", isLarge ? 11F : 10F, FontStyle.Bold);

            lblHeroTitle.Location = new Point(startX - 2, startY + 42);
            lblHeroTitle.Size = new Size(heroW, isLarge ? 112 : 96);
            lblHeroTitle.Font = new Font("Cinzel", isLarge ? 38F : 32F, FontStyle.Bold);

            lblHeroText.Location = new Point(startX, lblHeroTitle.Bottom + 12);
            lblHeroText.Size = new Size(heroW, isLarge ? 88 : 74);
            lblHeroText.Font = new Font("Cormorant Garamond", isLarge ? 16F : 14.5F, FontStyle.Regular);

            // 4 Department Badges / Chips
            int chipH = isLarge ? 34 : 30;
            int chipsY = lblHeroText.Bottom + (isLarge ? 24 : 18);
            int chipGap = isLarge ? 10 : 8;

            int chipBooksW = isLarge ? 90 : 76;
            int chipUniformsW = isLarge ? 108 : 92;
            int chipSuppliesW = isLarge ? 102 : 88;
            int chipMerchW = isLarge ? 84 : 72;

            pnlChipBooks.Size = new Size(chipBooksW, chipH);
            pnlChipUniforms.Size = new Size(chipUniformsW, chipH);
            pnlChipSupplies.Size = new Size(chipSuppliesW, chipH);
            pnlChipMerch.Size = new Size(chipMerchW, chipH);

            pnlChipBooks.Location = new Point(startX, chipsY);
            pnlChipUniforms.Location = new Point(pnlChipBooks.Right + chipGap, chipsY);
            pnlChipSupplies.Location = new Point(pnlChipUniforms.Right + chipGap, chipsY);
            pnlChipMerch.Location = new Point(pnlChipSupplies.Right + chipGap, chipsY);

            // Right Scroll Card Positioning
            pnlScroll.Location = new Point(scrollX, startY);
            pnlScroll.Size = new Size(scrollW, scrollH);

            pnlRod.Size = new Size(scrollW + 28, 16);
            pnlRod.Location = new Point(scrollX - 14, startY - 18);

            // Internal controls inside pnlScroll
            int pad = isLarge ? 34 : 26;
            int fieldW = scrollW - (pad * 2);

            lblSignIn.Location = new Point(pad, 22);
            lblSignIn.UseMnemonic = false;
            lblSignIn.Font = new Font("Cinzel", isLarge ? 20F : 18F, FontStyle.Bold);

            lblSignInSub.Location = new Point(pad, 58);
            lblSignInSub.Size = new Size(fieldW, 24);
            lblSignInSub.Font = new Font("Cormorant Garamond", isLarge ? 13F : 12F, FontStyle.Italic);

            lblUser.Location = new Point(pad, 92);
            lblUser.Font = new Font("Cinzel", 8.5F, FontStyle.Bold);

            pnlUser.Location = new Point(pad, 114);
            pnlUser.Size = new Size(fieldW, 44);
            txtUser.Location = new Point(14, 10);
            txtUser.Size = new Size(fieldW - 28, 24);
            CenterVertically(txtUser);

            lblPass.Location = new Point(pad, 168);
            lblPass.Font = new Font("Cinzel", 8.5F, FontStyle.Bold);

            pnlPass.Location = new Point(pad, 190);
            pnlPass.Size = new Size(fieldW, 44);
            btnShowPass.Size = new Size(64, 30);
            btnShowPass.Location = new Point(fieldW - 72, 7);
            txtPass.Location = new Point(14, 10);
            txtPass.Size = new Size(fieldW - 90, 24);
            CenterVertically(txtPass);

            int checkY = 246;
            chkRemember.Location = new Point(pad, checkY);
            chkRemember.Size = new Size(155, 24);
            chkRemember.Font = new Font("Cormorant Garamond", isLarge ? 12.5F : 11.5F, FontStyle.Regular);

            lblForgot.Location = new Point(pad + 160, checkY);
            lblForgot.Size = new Size(fieldW - 160, 24);
            lblForgot.TextAlign = ContentAlignment.MiddleRight;
            lblForgot.Font = new Font("Cormorant Garamond", isLarge ? 12F : 11F, FontStyle.Italic);

            btnSignIn.Location = new Point(pad, 284);
            btnSignIn.Size = new Size(fieldW, isLarge ? 48 : 44);
            btnSignIn.Font = new Font("Cinzel", isLarge ? 12.5F : 11.5F, FontStyle.Bold);

            lblRoleNote.Location = new Point(pad, btnSignIn.Bottom + 12);
            lblRoleNote.Size = new Size(fieldW, 22);
            lblRoleNote.Font = new Font("Cormorant Garamond", isLarge ? 12.5F : 11.5F, FontStyle.Italic);

            this.Invalidate();
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;
            btnShowPass.Text = txtPass.UseSystemPasswordChar ? "SHOW" : "HIDE";
            txtPass.Focus();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (Username.Length == 0 || Password.Length == 0)
            {
                ShowError("Please enter your username and password.");
                return;
            }

            var handler = LoginRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        // ------------------------------------------------------------ painting

        // Enchanted gothic scriptorium background: deep midnight wine gradient,
        // warm candlelit portal glow, astrological celestial rings, and starry constellation runes.
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = ClientRectangle;

            // Deep majestic midnight wine gradient
            using (var br = new LinearGradientBrush(r, Color.FromArgb(16, 4, 9), Color.FromArgb(38, 8, 20), 60f))
            {
                g.FillRectangle(br, r);
            }

            // Warm ambient candlelight / portal glow radiating behind the parchment scroll
            var glow = new Rectangle(pnlScroll.Left - 140, pnlScroll.Top - 100, pnlScroll.Width + 280, pnlScroll.Height + 200);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glow);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.FromArgb(50, 235, 185, 50);
                    pgb.SurroundColors = new[] { Color.FromArgb(0, 235, 185, 50) };
                    g.FillPath(pgb, path);
                }
            }

            DrawCelestialAstrologicalBackground(g, r);
        }

        private void DrawCelestialAstrologicalBackground(Graphics g, Rectangle r)
        {
            // Astrological celestial circles centered in the left hero zone
            PointF center = new PointF(r.Width * 0.28f, r.Height * 0.52f);

            using (Pen ringPen = new Pen(Color.FromArgb(28, 229, 181, 58), 1.2f))
            using (Pen dashPen = new Pen(Color.FromArgb(22, 229, 181, 58), 1f) { DashPattern = new float[] { 4f, 4f } })
            using (Pen dotPen = new Pen(Color.FromArgb(26, 229, 181, 58), 1.5f) { DashPattern = new float[] { 2f, 6f } })
            {
                float[] radii = { 120f, 210f, 310f, 420f, 540f };
                for (int i = 0; i < radii.Length; i++)
                {
                    float rad = radii[i];
                    RectangleF circRect = new RectangleF(center.X - rad, center.Y - rad, rad * 2, rad * 2);
                    if (i % 2 == 0)
                        g.DrawEllipse(ringPen, circRect);
                    else
                        g.DrawEllipse(dashPen, circRect);
                }

                // Cardinal and diagonal celestial axis lines
                g.DrawLine(dotPen, center.X - 540, center.Y, center.X + 540, center.Y);
                g.DrawLine(dotPen, center.X, center.Y - 540, center.X, center.Y + 540);
                g.DrawLine(dotPen, center.X - 380, center.Y - 380, center.X + 380, center.Y + 380);
                g.DrawLine(dotPen, center.X - 380, center.Y + 380, center.X + 380, center.Y - 380);

                // Gothic vault arch lines rising from the bottom corners to the header
                using (Pen archPen = new Pen(Color.FromArgb(24, 216, 175, 55), 1.2f))
                {
                    g.DrawBezier(archPen, new PointF(0, r.Height), new PointF(r.Width * 0.15f, r.Height * 0.4f),
                        new PointF(r.Width * 0.35f, 90), new PointF(r.Width * 0.5f, 76));
                    g.DrawBezier(archPen, new PointF(r.Width, r.Height), new PointF(r.Width * 0.85f, r.Height * 0.4f),
                        new PointF(r.Width * 0.65f, 90), new PointF(r.Width * 0.5f, 76));
                }

                // Scattered constellation star points
                PointF[] stars =
                {
                    new PointF(center.X - 280, center.Y - 180),
                    new PointF(center.X + 220, center.Y - 260),
                    new PointF(center.X - 160, center.Y + 240),
                    new PointF(center.X + 340, center.Y + 160),
                    new PointF(r.Width * 0.12f, r.Height * 0.22f),
                    new PointF(r.Width * 0.88f, r.Height * 0.26f),
                    new PointF(r.Width * 0.92f, r.Height * 0.72f),
                    new PointF(r.Width * 0.08f, r.Height * 0.78f)
                };

                using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(70, 240, 205, 110)))
                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(22, 240, 205, 110)))
                {
                    foreach (var pt in stars)
                    {
                        g.FillEllipse(glowBrush, pt.X - 3.5f, pt.Y - 3.5f, 7f, 7f);
                        g.FillEllipse(starBrush, pt.X - 1.5f, pt.Y - 1.5f, 3f, 3f);
                    }
                }
            }
        }

        // Top Rod with carved golden scroll finials
        private void pnlRod_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlRod.Width;
            int h = pnlRod.Height;

            // Carved golden finials on left and right
            using (SolidBrush finialBrush = new SolidBrush(Color.FromArgb(238, 195, 75)))
            using (Pen finialBorder = new Pen(Color.FromArgb(145, 105, 20), 1.2f))
            {
                Rectangle leftKnob = new Rectangle(0, 1, 16, h - 2);
                Rectangle rightKnob = new Rectangle(w - 17, 1, 16, h - 2);
                g.FillEllipse(finialBrush, leftKnob);
                g.DrawEllipse(finialBorder, leftKnob);
                g.FillEllipse(finialBrush, rightKnob);
                g.DrawEllipse(finialBorder, rightKnob);
            }
        }

        // Shield crest with the "U".
        private void pnlCrest_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.ScaleTransform(pnlCrest.Width / 40f, pnlCrest.Height / 44f);

            using (var shield = new GraphicsPath())
            {
                shield.AddLine(20, 2, 36, 8);
                shield.AddLine(36, 8, 36, 21);
                shield.AddBezier(36, 21, 36, 31, 29, 38, 20, 42);
                shield.AddBezier(20, 42, 11, 38, 4, 31, 4, 21);
                shield.AddLine(4, 21, 4, 8);
                shield.CloseFigure();

                using (var fill = new SolidBrush(CoopTheme.Ink)) g.FillPath(fill, shield);
                using (var pen = new Pen(CoopTheme.Gold, 2.2f) { LineJoin = LineJoin.Round }) g.DrawPath(pen, shield);
            }

            using (var f = new Font(AppFonts.HeadingName, 18f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var br = new SolidBrush(CoopTheme.Gold))
                g.DrawString("U", f, br, new RectangleF(0, 8, 40, 28), sf);
        }

        // Wizardly scroll flourishes: aged parchment border, corner accents,
        // and subtle watermark.
        private void pnlScroll_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlScroll.Width;
            int h = pnlScroll.Height;

            // Inset antique double gold borders
            using (var goldPen = new Pen(Color.FromArgb(110, 201, 154, 30), 1.2f))
            using (var innerPen = new Pen(Color.FromArgb(60, 201, 154, 30), 1f))
            {
                g.DrawRectangle(goldPen, 10, 10, w - 21, h - 21);
                g.DrawRectangle(innerPen, 14, 14, w - 29, h - 29);
            }

            // Ornate corner accents (all 4 corners)
            using (var cornerPen = new Pen(Color.FromArgb(180, 169, 124, 20), 1.6f))
            using (var thinPen = new Pen(Color.FromArgb(120, 169, 124, 20), 1f))
            {
                int cornerSize = 24;

                // Top-Left
                g.DrawLine(cornerPen, 16, 16, 16 + cornerSize, 16);
                g.DrawLine(cornerPen, 16, 16, 16, 16 + cornerSize);
                g.DrawArc(thinPen, 16, 16, 12, 12, 180, 90);

                // Top-Right
                g.DrawLine(cornerPen, w - 17 - cornerSize, 16, w - 17, 16);
                g.DrawLine(cornerPen, w - 17, 16, w - 17, 16 + cornerSize);
                g.DrawArc(thinPen, w - 29, 16, 12, 12, 270, 90);

                // Bottom-Left
                g.DrawLine(cornerPen, 16, h - 17, 16 + cornerSize, h - 17);
                g.DrawLine(cornerPen, 16, h - 17 - cornerSize, 16, h - 17);
                g.DrawArc(thinPen, 16, h - 29, 12, 12, 90, 90);

                // Bottom-Right
                g.DrawLine(cornerPen, w - 17 - cornerSize, h - 17, w - 17, h - 17);
                g.DrawLine(cornerPen, w - 17, h - 17 - cornerSize, w - 17, h - 17);
                g.DrawArc(thinPen, w - 29, h - 29, 12, 12, 0, 90);
            }

            // Subtle UMak Crest Watermark in center of the scroll
            int wmX = (w - 170) / 2;
            int wmY = (h - 190) / 2;
            using (Pen wmPen = new Pen(Color.FromArgb(20, 201, 154, 30), 1.8f))
            {
                g.DrawEllipse(wmPen, wmX, wmY, 170, 170);
                g.DrawEllipse(wmPen, wmX + 8, wmY + 8, 154, 154);
                using (Font wmFont = new Font("Cinzel", 34F, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (SolidBrush wmBrush = new SolidBrush(Color.FromArgb(16, 160, 120, 20)))
                {
                    g.DrawString("U", wmFont, wmBrush, new RectangleF(wmX, wmY, 170, 170), sf);
                }
            }
        }

        // ------------------------------------------------------------ helpers

        // If Cinzel / Cormorant Garamond are not installed, Windows silently swaps in a plain
        // sans-serif font. Replace those with Georgia so the theme still looks close.
        private static void ApplyFontFallback(Control root)
        {
            foreach (Control child in root.Controls) ApplyFontFallback(child);

            if (root.Font.Name == "Microsoft Sans Serif" && !(root is Form))
                root.Font = new Font("Georgia", root.Font.Size, root.Font.Style);
        }

        private static void CenterVertically(Control c)
        {
            c.Top = (c.Parent.ClientSize.Height - c.Height) / 2;
        }

        // Gray placeholder text inside a TextBox (works on .NET Framework and .NET 6+).
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private static void SetCue(TextBox tb, string text)
        {
            if (tb.IsHandleCreated)
                SendMessage(tb.Handle, EM_SETCUEBANNER, 1, text);
            else
                tb.HandleCreated += delegate { SendMessage(tb.Handle, EM_SETCUEBANNER, 1, text); };
        }
    }
}