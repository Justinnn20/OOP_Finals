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

                loginPage.Hide();
                using (Form mainForm = isAdmin ? (Form)new AdminDB() : new Form2())
                {
                    mainForm.ShowDialog(loginPage);
                }

                loginPage.Close();
            };

            Application.Run(loginPage);

        }
    }
}