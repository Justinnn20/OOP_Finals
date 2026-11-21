using System.Drawing;
using System.Windows.Forms;

namespace UMakCoop.Views
{
    /// <summary>A tiny "type something and press Confirm" dialog (WinForms has no built-in InputBox in C#).</summary>
    public static class PromptDialog
    {
        /// <summary>Returns the text, or null if the user cancelled.</summary>
        public static string Ask(IWin32Window owner, string title, string message)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new Size(380, 140);
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.ShowInTaskbar = false;
                form.BackColor = Theme.Parchment;

                var label = new Label { Text = message, Left = 14, Top = 14, Width = 352, ForeColor = Theme.Ink, Font = Theme.Sans(9.5f) };
                var box = new TextBox { Left = 14, Top = 42, Width = 352, Font = Theme.Sans(10f) };
                var ok = new Button { Text = "Confirm", Left = 186, Top = 90, Width = 86, Height = 32, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Cancel", Left = 280, Top = 90, Width = 86, Height = 32, DialogResult = DialogResult.Cancel };
                Theme.GoldButton(ok, 9.5f);
                cancel.Font = Theme.Sans(9.5f);

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
