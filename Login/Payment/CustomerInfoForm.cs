using System;
using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    public sealed class CustomerInfoForm : Form
    {
        private readonly TextBox _nameBox;
        private readonly TextBox _yearBox;
        private readonly TextBox _collegeBox;

        public string PurchaserName => _nameBox.Text.Trim();
        public string YearLevel => _yearBox.Text.Trim();
        public string College => _collegeBox.Text.Trim();

        public CustomerInfoForm()
        {
            Text = "Purchaser Information";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(390, 230);
            BackColor = Color.FromArgb(255, 250, 240);

            var title = new Label
            {
                Text = "Enter purchaser information",
                Left = 18,
                Top = 14,
                Width = 350,
                Height = 26,
                Font = new Font("Georgia", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 36, 21)
            };

            _nameBox = AddField("Name", 52);
            _yearBox = AddField("Year", 92);
            _collegeBox = AddField("College", 132);

            var confirm = new Button
            {
                Text = "CONTINUE TO RECEIPT",
                Left = 18,
                Top = 180,
                Width = 220,
                Height = 32,
                BackColor = Color.FromArgb(212, 160, 23),
                ForeColor = Color.FromArgb(59, 36, 21),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Georgia", 9f, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            confirm.FlatAppearance.BorderSize = 0;
            confirm.Click += Confirm_Click;

            var cancel = new Button
            {
                Text = "CANCEL",
                Left = 248,
                Top = 180,
                Width = 110,
                Height = 32,
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI", 9f)
            };

            Controls.Add(title);
            Controls.Add(confirm);
            Controls.Add(cancel);
            AcceptButton = confirm;
            CancelButton = cancel;
            ActiveControl = _nameBox;
        }

        private TextBox AddField(string labelText, int top)
        {
            var label = new Label
            {
                Text = labelText,
                Left = 18,
                Top = top + 3,
                Width = 80,
                Height = 24,
                ForeColor = Color.FromArgb(59, 36, 21),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            var box = new TextBox
            {
                Left = 105,
                Top = top,
                Width = 253,
                Height = 25,
                Font = new Font("Segoe UI", 10f)
            };
            Controls.Add(label);
            Controls.Add(box);
            return box;
        }

        private void Confirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PurchaserName) ||
                string.IsNullOrWhiteSpace(YearLevel) ||
                string.IsNullOrWhiteSpace(College))
            {
                MessageBox.Show(this, "Please enter the name, year, and college.",
                    "Missing Purchaser Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
