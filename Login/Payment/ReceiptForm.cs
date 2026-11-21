using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using UMakCoop.Models;

namespace UMakCoop.Views
{
    /// <summary>The receipt popup shown after "Seal the sale" and when viewing a receipt from the Transactions list.</summary>
    public class ReceiptForm : Form
    {
        private readonly Receipt _receipt;
        private readonly ReceiptPaper _paper;

        public ReceiptForm(Receipt receipt, bool isReprint)
        {
            _receipt = receipt;
            _paper = new ReceiptPaper(receipt, isReprint);
            BuildUi();
            Shown += (s, e) => _paper.PlayStamp();
        }

        private void BuildUi()
        {
            Text = "Receipt " + _receipt.ReceiptNo;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Theme.Wine;
            Padding = new Padding(14);

            // Tall receipts scroll inside the popup instead of making the window taller than the screen.
            int maxHeight = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.85);
            int wanted = _paper.Height + 56 + 28;
            ClientSize = new Size(ReceiptPaper.PaperWidth + 28 + SystemInformation.VerticalScrollBarWidth,
                                  Math.Min(wanted, maxHeight));

            var scroller = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Wine, AutoScroll = true };
            _paper.Location = new Point(0, 0);
            scroller.Controls.Add(_paper);

            var buttons = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = Theme.Wine };
            var print = new Button { Text = "Print", Width = 110, Height = 36, Top = 14, Left = 0 };
            var done = new Button { Text = "Done", Width = 110, Height = 36, Top = 14, DialogResult = DialogResult.OK };
            done.Left = buttons.Width - done.Width;
            done.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Theme.GoldButton(print, 10f);
            Theme.GoldButton(done, 10f);
            print.Click += OnPrint;
            buttons.Controls.Add(print);
            buttons.Controls.Add(done);

            // Fill goes in first, edge-docked panel after it (WinForms docks the last-added first).
            Controls.Add(scroller);
            Controls.Add(buttons);

            AcceptButton = done;
            CancelButton = done;
        }

        private void OnPrint(object sender, EventArgs e)
        {
            var doc = new PrintDocument();
            doc.DocumentName = "Receipt " + _receipt.ReceiptNo;
            doc.PrintPage += (s, ev) =>
            {
                // Draw the same bill-pad receipt, scaled down only if it is bigger than the page.
                Rectangle m = ev.MarginBounds;
                float scale = Math.Min(1f, Math.Min(m.Width / (float)ReceiptPaper.PaperWidth,
                                                    m.Height / (float)_paper.Height));
                ev.Graphics.TranslateTransform(m.Left, m.Top);
                ev.Graphics.ScaleTransform(scale, scale);
                _paper.Draw(ev.Graphics, true, 1f);
                ev.HasMorePages = false;
            };

            // Preview first: works even with no printer, and has a Print button inside.
            using (var preview = new PrintPreviewDialog { Document = doc, Width = 760, Height = 720 })
                preview.ShowDialog(this);
        }
    }
}
