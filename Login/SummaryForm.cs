using System;
using System.Drawing;
using System.Windows.Forms;
using UMakCoop.Controllers;

namespace UMakCoop.Views
{
    /// <summary>Daily sales summary popup: pick a date and read the totals (good for end of shift).</summary>
    public class SummaryForm : Form
    {
        private readonly TransactionController _transactions;
        private readonly DateTimePicker _date = new DateTimePicker();
        private readonly Label _text = new Label();

        public SummaryForm(TransactionController transactions)
        {
            _transactions = transactions;
            BuildUi();
            Refresh_();
        }

        private void BuildUi()
        {
            Text = "Daily sales summary";
            ClientSize = new Size(420, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Theme.Wine;
            Padding = new Padding(14);

            var pick = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Theme.Wine };
            var caption = new Label { Text = "Date:", AutoSize = true, Left = 0, Top = 10, ForeColor = Theme.Parchment, Font = Theme.Sans(10f) };
            _date.Format = DateTimePickerFormat.Long;
            _date.Left = 56;
            _date.Top = 6;
            _date.Width = 250;
            _date.Font = Theme.Sans(10f);
            _date.MaxDate = DateTime.Today;
            _date.Value = DateTime.Today;
            _date.ValueChanged += (s, e) => Refresh_();
            pick.Controls.Add(caption);
            pick.Controls.Add(_date);

            var paper = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Paper, AutoScroll = true };
            _text.AutoSize = true;
            _text.Dock = DockStyle.Top;
            _text.Font = new Font("Consolas", 10f);
            _text.ForeColor = Theme.Ink;
            _text.Padding = new Padding(16);
            paper.Controls.Add(_text);

            var buttons = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = Theme.Wine };
            var close = new Button { Text = "Close", Width = 110, Height = 36, Top = 14, DialogResult = DialogResult.OK };
            close.Left = buttons.Width - close.Width;
            close.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Theme.GoldButton(close, 10f);
            buttons.Controls.Add(close);

            Controls.Add(paper);
            Controls.Add(pick);
            Controls.Add(buttons);
            AcceptButton = close;
            CancelButton = close;
        }

        private void Refresh_()
        {
            _text.Text = _transactions.Summarize(_date.Value).ToText();
        }
    }
}
