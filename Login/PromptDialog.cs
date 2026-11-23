using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UMakCoop.Views
{
    internal class PromptForm : Form
    {
        public PromptForm()
        {
            this.DoubleBuffered = true;
        }
    }

    /// <summary>A themed "type something and press Confirm" dialog for UMak Coop.</summary>
    public static class PromptDialog
    {
        /// <summary>Returns the text, or null if the user cancelled.</summary>
        public static string? Ask(IWin32Window? owner, string title, string message)
        {
            using (var form = new PromptForm())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.None;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new Size(420, 180);
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.ShowInTaskbar = false;
                form.BackColor = Theme.Parchment;

                // Header
                var pnlHeader = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 40,
                    BackColor = Theme.WineDark
                };
                pnlHeader.Paint += (s, e) =>
                {
                    using var p = new Pen(Theme.Gold, 1.5f);
                    e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                };

                var lblTitle = new Label
                {
                    Text = title.ToUpperInvariant(),
                    Font = Theme.Serif(10f, FontStyle.Bold),
                    ForeColor = Theme.Gold,
                    Location = new Point(16, 11),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                pnlHeader.Controls.Add(lblTitle);

                var btnClose = new Button
                {
                    Text = "✕",
                    Font = Theme.Serif(9F, FontStyle.Bold),
                    ForeColor = Theme.Gold,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(32, 28),
                    Location = new Point(pnlHeader.Width - 38, 6),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Cursor = Cursors.Hand
                };
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 20, 32);
                btnClose.Click += (s, e) => { form.DialogResult = DialogResult.Cancel; form.Close(); };
                pnlHeader.Controls.Add(btnClose);

                var label = new Label
                {
                    Text = message,
                    Left = 18,
                    Top = 52,
                    Width = 384,
                    ForeColor = Theme.Ink,
                    Font = Theme.Sans(9.5f)
                };

                var box = new TextBox
                {
                    Left = 18,
                    Top = 82,
                    Width = 384,
                    Font = Theme.Sans(10.5f),
                    BackColor = Color.FromArgb(255, 250, 240)
                };

                var ok = new Button
                {
                    Text = "Confirm",
                    Left = 210,
                    Top = 130,
                    Width = 92,
                    Height = 34,
                    DialogResult = DialogResult.OK
                };
                var cancel = new Button
                {
                    Text = "Cancel",
                    Left = 310,
                    Top = 130,
                    Width = 92,
                    Height = 34,
                    DialogResult = DialogResult.Cancel,
                    BackColor = Color.FromArgb(236, 222, 190),
                    ForeColor = Theme.Wine,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                cancel.FlatAppearance.BorderColor = Color.FromArgb(196, 170, 120);
                cancel.FlatAppearance.BorderSize = 1;
                cancel.Font = Theme.Sans(9.5f, FontStyle.Bold);

                Theme.GoldButton(ok, 9.5f);

                form.Paint += (s, e) =>
                {
                    using var p = new Pen(Theme.Gold, 2f);
                    e.Graphics.DrawRectangle(p, 0, 0, form.Width - 1, form.Height - 1);
                };

                form.Controls.Add(pnlHeader);
                form.Controls.Add(label);
                form.Controls.Add(box);
                form.Controls.Add(ok);
                form.Controls.Add(cancel);
                form.AcceptButton = ok;
                form.CancelButton = cancel;

                return form.ShowDialog(owner) == DialogResult.OK ? box.Text : null;
            }
        }
    }
}
