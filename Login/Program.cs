using System;
using System.Windows.Forms;

namespace Login
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show($"Application Error:\n{e.Exception.Message}\n\nStack:\n{e.Exception.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    MessageBox.Show($"Fatal Error:\n{ex.Message}\n\nStack:\n{ex.StackTrace}",
                        "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Application.Run(new CoopAppContext());
        }
    }

    public class CoopAppContext : ApplicationContext
    {
        public static CoopAppContext? Instance { get; private set; }
        public Form? CurrentForm { get; private set; }

        public CoopAppContext()
        {
            Instance = this;
            ShowLoginPage();
        }

        public void ShowLoginPage()
        {
            var loginPage = new LoginPage();
            loginPage.LoginRequested += (sender, e) =>
            {
                bool isAdmin = string.Equals(loginPage.Username, "admin", StringComparison.OrdinalIgnoreCase)
                    && loginPage.Password == "admin123";
                bool isStaff = string.Equals(loginPage.Username, "staff", StringComparison.OrdinalIgnoreCase)
                    && loginPage.Password == "staff123";

                if (!isAdmin && !isStaff)
                {
                    loginPage.ShowError("Invalid username or password.");
                    return;
                }

                Form mainForm = isAdmin ? (Form)new AdminDB() : new Form2();
                SwitchForm(mainForm);
            };

            SwitchForm(loginPage);
        }

        public void SwitchForm(Form nextForm)
        {
            Form? oldForm = CurrentForm;

            if (oldForm != null)
            {
                if (oldForm.WindowState == FormWindowState.Maximized)
                {
                    nextForm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    nextForm.StartPosition = FormStartPosition.Manual;
                    nextForm.WindowState = FormWindowState.Normal;
                    nextForm.Location = oldForm.Location;
                    nextForm.Size = oldForm.Size;
                }
            }

            CurrentForm = nextForm;
            nextForm.FormClosed += (s, e) =>
            {
                if (CurrentForm == nextForm)
                {
                    ExitThread();
                }
            };

            nextForm.Show();

            if (oldForm != null)
            {
                oldForm.Hide();
                nextForm.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        oldForm.Close();
                        oldForm.Dispose();
                    }
                    catch
                    {
                    }
                }));
            }
        }

        public void SignOut()
        {
            ShowLoginPage();
        }
    }
}