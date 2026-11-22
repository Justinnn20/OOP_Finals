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
            MessageBox.Show(this, message, "UMak Coop", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            ActiveControl = txtUser;
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
                return;
            }

            // Direct fallback authentication
            if (Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && Password == "admin123")
            {
                this.Hide();
                var adminForm = new AdminDB();
                adminForm.StartPosition = FormStartPosition.CenterScreen;
                adminForm.Show();
                adminForm.FormClosed += (s, args) =>
                {
                    txtPass.Clear();
                    this.Show();
                };
            }
            else if (Username.Equals("staff", StringComparison.OrdinalIgnoreCase) && Password == "staff123")
            {
                this.Hide();
                var staffForm = new Form2();
                staffForm.StartPosition = FormStartPosition.CenterScreen;
                staffForm.Show();
                staffForm.FormClosed += (s, args) =>
                {
                    txtPass.Clear();
                    this.Show();
                };
            }
            else
            {
                ShowError("Invalid username or password. Please try again.");
            }
        }

        // ------------------------------------------------------------ painting

        // Maroon gradient background + soft gold glow behind the login card.
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = ClientRectangle;

            using (var br = new LinearGradientBrush(r, CoopTheme.Maroon700, CoopTheme.Maroon900, 50f))
                g.FillRectangle(br, r);

            var glow = new Rectangle(pnlScroll.Left - 110, pnlScroll.Top - 80, pnlScroll.Width + 220, pnlScroll.Height + 160);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glow);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.FromArgb(40, CoopTheme.Gold);
                    pgb.SurroundColors = new[] { Color.FromArgb(0, CoopTheme.Gold) };
                    g.FillPath(pgb, path);
                }
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