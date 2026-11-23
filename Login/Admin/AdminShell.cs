using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    /// <summary>
    /// OOP: Child-of-Form host window for every Admin page (AdminDB, AdminInventory,
    /// AdminTransactions, AdminReports, AdminSettings). The admin stays in ONE window;
    /// clicking a nav tab only swaps the embedded page instead of opening a new window.
    /// Pages are built once and reused, so switching tabs is instant (no rebuild, no lag).
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")] // code-only form: stops Visual Studio from creating a .resx for it
    public class AdminShell : Form
    {
        private Form? currentPage;
        private readonly Dictionary<Type, Form> pageCache = new Dictionary<Type, Form>();

        public AdminShell()
        {
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1280, 720);
            MinimumSize = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(42, 10, 20);
        }

        public static bool IsAdminPage(Form form) =>
            form is AdminDB || form is AdminInventory || form is AdminTransactions ||
            form is AdminReports || form is AdminSettings;

        /// <summary>
        /// Returns the already-built page of type T if the admin window has one,
        /// otherwise creates a new one. Used by the nav tabs.
        /// </summary>
        public static T GetPage<T>() where T : Form, new()
        {
            if (CoopAppContext.Instance?.CurrentForm is AdminShell shell &&
                shell.pageCache.TryGetValue(typeof(T), out Form? cached) &&
                !cached.IsDisposed)
            {
                return (T)cached;
            }

            return new T();
        }

        /// <summary>Shows <paramref name="page"/> in this window and hides the previous page.</summary>
        public void ShowPage(Form page)
        {
            if (ReferenceEquals(page, currentPage)) return;

            Form? oldPage = currentPage;
            currentPage = page;
            pageCache[page.GetType()] = page;

            SuspendLayout();

            if (oldPage != null)
            {
                oldPage.Visible = false;
                Controls.Remove(oldPage); // kept in the cache, not disposed
            }

            if (page.TopLevel)
            {
                page.TopLevel = false;
                page.FormBorderStyle = FormBorderStyle.None;
                page.MinimumSize = Size.Empty;
                page.StartPosition = FormStartPosition.Manual;
            }

            page.SetBounds(0, 0, ClientSize.Width, ClientSize.Height);
            page.Dock = DockStyle.Fill;
            Controls.Add(page);
            Text = page.Text;
            page.Show();

            ResumeLayout(true);
            page.Invalidate(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Form page in pageCache.Values)
                {
                    try { if (!page.IsDisposed) page.Dispose(); } catch { }
                }
                pageCache.Clear();
            }
            base.Dispose(disposing);
        }
    }
}