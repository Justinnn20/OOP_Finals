using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Login.Models;

namespace Login.Controls
{
    public partial class CartItemControl : UserControl
    {
        public OrderItem? CurrentItem { get; private set; }

        public event Action<OrderItem>? OnQuantityIncreased;
        public event Action<OrderItem>? OnQuantityDecreased;

        public CartItemControl()
        {
            InitializeComponent();
            SetupCustomStyles();
        }

        private void SetupCustomStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            btnPlus.Click += (s, e) =>
            {
                if (CurrentItem != null)
                {
                    OnQuantityIncreased?.Invoke(CurrentItem);
                }
            };

            btnMinus.Click += (s, e) =>
            {
                if (CurrentItem != null)
                {
                    OnQuantityDecreased?.Invoke(CurrentItem);
                }
            };

            PaintRoundedButton(btnMinus);
            PaintRoundedButton(btnPlus);
        }

        private static void PaintRoundedButton(Button btn)
        {
            btn.Paint += (s, e) =>
            {
                var b = (Button)s!;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
                using var path = CreateRoundedRectanglePath(rect, 4);
                using var brush = new SolidBrush(b.BackColor);
                using var pen = new Pen(Color.FromArgb(200, 175, 130), 1f);

                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);

                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, rect, b.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Clean delicate dotted line across bottom
            using var pen = new Pen(Color.FromArgb(215, 195, 155), 1f)
            {
                DashStyle = DashStyle.Dot
            };
            e.Graphics.DrawLine(pen, 8, Height - 1, Width - 8, Height - 1);
        }

        public void Bind(OrderItem item)
        {
            CurrentItem = item;
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            if (CurrentItem == null) return;

            lblItemName.Text = CurrentItem.Product.Name;
            lblItemDetail.Text = CurrentItem.Product.DisplayDetails;
            lblItemQty.Text = CurrentItem.Quantity.ToString();
            lblItemSubtotal.Text = $"₱{CurrentItem.TotalPrice:N0}";
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
