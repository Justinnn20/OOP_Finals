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
                if (loginPage.Username != "admin" || loginPage.Password != "admin123")
                {
                    loginPage.ShowError("Invalid username or password.");
                    return;
                }

                loginPage.Hide();
                using (var mainForm = new Form2())
                {
                    mainForm.ShowDialog(loginPage);
                }

                loginPage.Close();
            };

            Application.Run(loginPage);

        }
    }
}