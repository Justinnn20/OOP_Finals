using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Login.Controllers;
using Login.Controls;
using Login.Models;

namespace Login
{
    // MVC: View. Form2 displays the dashboard and forwards user actions to the
    // CartController; it is also composed of controls and a CartController object.
    public partial class Form2 : Form
    {
        private Button[] tabs = Array.Empty<Button>();
        private readonly CartController _cartController = new();
        private decimal _totalSalesToday = 0m;
        private int _totalOrdersToday = 0;

        public Form2()
        {
            InitializeComponent();
            SetupTabs();
            SetupCartEvents();
            SetupPillRadioPaint();
            SetupActionButtonsPaint();
            SetupBarcodeAndInputs();
            RenderProductCatalog();
            LoadInitialDemoCart();
        }

        private void SetupTabs()
        {
            tabs = new[]
            {
                btnTabAll, btnTabBooks, btnTabUniforms, btnTabSupplies,
                btnStockIn, btnTransactionLogs, btnCashierPos
            };

            btnTabAll.Click += (s, e) => { SelectTab(btnTabAll); FilterProducts(null); };
            btnTabBooks.Click += (s, e) => { SelectTab(btnTabBooks); FilterProducts("Books & Manuals"); };
            btnTabUniforms.Click += (s, e) => { SelectTab(btnTabUniforms); FilterProducts("Uniforms"); };
            btnTabSupplies.Click += (s, e) => { SelectTab(btnTabSupplies); FilterProducts("School Supplies"); };
            btnStockIn.Click += (s, e) => SelectTab(btnStockIn);
            btnTransactionLogs.Click += (s, e) => SelectTab(btnTransactionLogs);
            btnCashierPos.Click += (s, e) => { SelectTab(btnCashierPos); FilterProducts(null); };

            SelectTab(btnTabAll);
        }

        private void SelectTab(Button selected)
        {
            foreach (Button tab in tabs)
            {
                bool isSelected = tab == selected;
                tab.BackColor = isSelected ? Color.FromArgb(255, 213, 79) : Color.FromArgb(30, 10, 14);
                tab.ForeColor = isSelected ? Color.FromArgb(60, 20, 25) : Color.FromArgb(245, 230, 200);
            }
        }

        private void SetupPillRadioPaint()
        {
            RadioButton[] pills = { rbNone, rbScholar, rbStaff, rbCash, rbGCash };
            foreach (var rb in pills)
            {
                rb.Paint += (s, e) =>
                {
                    var r = (RadioButton)s!;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.Clear(ParchmentBg);

                    bool isChecked = r.Checked;
                    Color bg = isChecked ? Color.FromArgb(36, 12, 18) : Color.FromArgb(250, 240, 215);
                    Color fg = isChecked ? Color.FromArgb(245, 230, 200) : Color.FromArgb(60, 20, 25);
                    Color border = isChecked ? Color.FromArgb(36, 12, 18) : Color.FromArgb(215, 190, 140);

                    var rect = new Rectangle(0, 0, r.Width - 1, r.Height - 1);
                    using var path = CreateRoundedRectanglePath(rect, r.Height / 2);
                    using var brush = new SolidBrush(bg);
                    using var pen = new Pen(border, 1.2f);

                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);

                    TextRenderer.DrawText(e.Graphics, r.Text, r.Font, rect, fg,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };

                rb.CheckedChanged += (s, e) => { rb.Invalidate(); };
            }
        }

        private void SetupActionButtonsPaint()
        {
            Button[] actionBtns = { btnHoldOrder, btnClearOrder };
            foreach (var b in actionBtns)
            {
                b.Paint += (s, e) =>
                {
                    var btn = (Button)s!;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.Clear(ParchmentBg);

                    var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                    using var path = CreateRoundedRectanglePath(rect, btn.Height / 2);
                    using var brush = new SolidBrush(Color.FromArgb(250, 240, 215));
                    using var pen = new Pen(Color.FromArgb(215, 190, 140), 1f);

                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);

                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect, Color.FromArgb(60, 20, 25),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };
            }
        }

        private void SetupCartEvents()
        {
            _cartController.OrderUpdated += OnOrderUpdated;
            _cartController.OrderHeld += OnOrderHeld;
            _cartController.OrderRecalled += OnOrderRecalled;
            _cartController.OrderCleared += OnOrderCleared;

            btnHoldOrder.Click += (s, e) =>
            {
                if (_cartController.CurrentOrder.Items.Count > 0)
                {
                    if (_cartController.HoldOrder(out string msg))
                    {
                        btnHoldOrder.Text = $"Hold ({_cartController.HeldOrders.Count})";
                        btnHoldOrder.Invalidate();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Hold Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (_cartController.HeldOrders.Count > 0)
                {
                    ShowHeldOrdersMenu();
                }
                else
                {
                    MessageBox.Show("The scroll is blank. Add items to hold an order.", "Hold Order",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnClearOrder.Click += (s, e) =>
            {
                if (_cartController.CurrentOrder.Items.Count > 0)
                {
                    var result = MessageBox.Show("Clear all items from the current scroll?", "Clear Order",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        _cartController.ClearCart();
                    }
                }
            };

            // Discounts
            rbNone.CheckedChanged += (s, e) =>
            {
                if (rbNone.Checked) _cartController.ApplyDiscount(0m, "None");
            };
            rbScholar.CheckedChanged += (s, e) =>
            {
                if (rbScholar.Checked) _cartController.ApplyDiscount(0.10m, "Scholar 10%");
            };
            rbStaff.CheckedChanged += (s, e) =>
            {
                if (rbStaff.Checked) _cartController.ApplyDiscount(0.15m, "Staff 15%");
            };

            // Payment Methods
            rbCash.CheckedChanged += (s, e) =>
            {
                if (rbCash.Checked) _cartController.SetPaymentMethod("Cash");
            };
            rbGCash.CheckedChanged += (s, e) =>
            {
                if (rbGCash.Checked) _cartController.SetPaymentMethod("GCash");
            };

            // Cash input
            txtCash.TextChanged += (s, e) =>
            {
                if (decimal.TryParse(txtCash.Text.Trim(), out decimal val))
                {
                    _cartController.SetCashReceived(val);
                }
                else
                {
                    _cartController.SetCashReceived(0m);
                }
            };

            // Checkout
            btnCheckout.Click += (s, e) =>
            {
                using var customerInfo = new CustomerInfoForm();
                if (customerInfo.ShowDialog(this) != DialogResult.OK)
                    return;

                decimal orderAmount = _cartController.CurrentOrder.Total;
                if (_cartController.Checkout(out string msg))
                {
                    _totalSalesToday += orderAmount;
                    _totalOrdersToday++;
                    lblSalesValue.Text = $"₱{_totalSalesToday:N0}";
                    lblOrdersValue.Text = _totalOrdersToday.ToString();

                    if (_cartController.LastCompletedOrder != null)
                    {
                        using var receipt = new OrderReceiptForm(
                            _cartController.LastCompletedOrder,
                            "Admin",
                            customerInfo.PurchaserName,
                            customerInfo.YearLevel,
                            customerInfo.College);
                        receipt.ShowDialog(this);
                    }

                    txtCash.Text = "0";
                    rbNone.Checked = true;
                    rbCash.Checked = true;
                }
                else
                {
                    MessageBox.Show(msg, "Checkout Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        private void SetupBarcodeAndInputs()
        {
            txtBarcode.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ProcessBarcodeInput();
                }
            };

            btnScanBarcode.Click += (s, e) => ProcessBarcodeInput();

            btnTry1.Click += (s, e) => ApplyStudentId(btnTry1.Text);
            btnTry2.Click += (s, e) => ApplyStudentId(btnTry2.Text);
            btnTry3.Click += (s, e) => ApplyStudentId(btnTry3.Text);
            btnScanId.Click += (s, e) => ApplyStudentId(txtStudentId.Text.Trim());

            txtSearch.TextChanged += (s, e) =>
            {
                string query = txtSearch.Text.Trim();
                FilterProducts(null, query);
            };
        }

        private void ProcessBarcodeInput()
        {
            string input = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            if (_cartController.AddItemByBarcode(input, out string msg))
            {
                txtBarcode.Clear();
            }
            else
            {
                MessageBox.Show(msg, "Barcode Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarcode.SelectAll();
            }
        }

        private void ApplyStudentId(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId)) return;
            txtStudentId.Text = studentId;
            rbScholar.Checked = true;
        }

        private void OnOrderUpdated(Order order)
        {
            bool hasItems = order.Items.Count > 0;
            lblBlank.Visible = !hasItems;
            flpCartItems.Visible = hasItems;

            flpCartItems.SuspendLayout();
            flpCartItems.Controls.Clear();

            foreach (var item in order.Items)
            {
                var row = new CartItemControl();
                row.Bind(item);
                row.OnQuantityIncreased += itm => _cartController.IncrementItem(itm);
                row.OnQuantityDecreased += itm => _cartController.DecrementItem(itm);
                flpCartItems.Controls.Add(row);
            }

            flpCartItems.ResumeLayout();

            lblSubtotalValue.Text = $"₱{order.Subtotal:N0}";
            lblTotalValue.Text = $"₱{order.Total:N0}";
            btnHoldOrder.Text = _cartController.HeldOrders.Count > 0
                ? $"Hold ({_cartController.HeldOrders.Count})"
                : "Hold";
            btnHoldOrder.Invalidate();
            btnClearOrder.Invalidate();
        }

        private void OnOrderHeld(Order heldOrder)
        {
            btnHoldOrder.Text = $"Hold ({_cartController.HeldOrders.Count})";
            btnHoldOrder.Invalidate();
        }

        private void OnOrderRecalled(Order order)
        {
            btnHoldOrder.Text = _cartController.HeldOrders.Count > 0
                ? $"Hold ({_cartController.HeldOrders.Count})"
                : "Hold";
            btnHoldOrder.Invalidate();
        }

        private void OnOrderCleared()
        {
            txtCash.Text = "0";
            rbNone.Checked = true;
            rbCash.Checked = true;
        }

        private void ShowHeldOrdersMenu()
        {
            if (_cartController.HeldOrders.Count == 0)
            {
                MessageBox.Show("No orders currently on hold.", "Held Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var menu = new ContextMenuStrip();
            foreach (var order in _cartController.HeldOrders)
            {
                string text = $"{order.OrderId} · ₱ {order.Total:N0} ({order.TotalItemsCount} items)";
                var menuItem = new ToolStripMenuItem(text);
                var targetOrder = order;
                menuItem.Click += (s, e) =>
                {
                    _cartController.RecallOrder(targetOrder, out string msg);
                };
                menu.Items.Add(menuItem);
            }

            menu.Show(btnHoldOrder, new Point(0, btnHoldOrder.Height));
        }

        private void RenderProductCatalog()
        {
            FilterProducts(null);
        }

        private void FilterProducts(string? category, string? searchQuery = null)
        {
            flpProducts.SuspendLayout();
            flpProducts.Controls.Clear();

            var items = _cartController.Catalog.AsEnumerable();

            if (!string.IsNullOrEmpty(category))
            {
                items = items.Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                items = items.Where(p =>
                    p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    p.Sku.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var product in items)
            {
                var card = CreateProductCard(product);
                flpProducts.Controls.Add(card);
            }

            flpProducts.ResumeLayout();
        }

        private Panel CreateProductCard(Product product)
        {
            var pnl = new Panel
            {
                Size = new Size(190, 114),
                BackColor = Color.FromArgb(38, 14, 20),
                Margin = new Padding(4),
                Cursor = Cursors.Hand
            };

            var lblName = new Label
            {
                Text = product.Name,
                Font = new Font("Georgia", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 213, 79),
                Location = new Point(8, 8),
                Size = new Size(174, 34),
                AutoEllipsis = true
            };

            var lblDetail = new Label
            {
                Text = product.DisplayDetails,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 170, 120),
                Location = new Point(8, 44),
                Size = new Size(174, 18),
                AutoEllipsis = true
            };

            var lblSku = new Label
            {
                Text = product.Sku,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.FromArgb(160, 130, 100),
                Location = new Point(8, 78),
                Size = new Size(80, 18)
            };

            var btnAdd = new Button
            {
                Text = "+ Add",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(212, 160, 23),
                ForeColor = Color.FromArgb(60, 20, 25),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(100, 72),
                Size = new Size(80, 28),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            Action addAction = () => _cartController.AddItem(product, 1);
            pnl.Click += (s, e) => addAction();
            lblName.Click += (s, e) => addAction();
            lblDetail.Click += (s, e) => addAction();
            lblSku.Click += (s, e) => addAction();
            btnAdd.Click += (s, e) => addAction();

            pnl.Controls.Add(lblName);
            pnl.Controls.Add(lblDetail);
            pnl.Controls.Add(lblSku);
            pnl.Controls.Add(btnAdd);

            return pnl;
        }

        private void LoadInitialDemoCart()
        {
            var p1 = _cartController.FindProduct("UM-001"); // PE Uniform (Size M, ₱350)
            var p2 = _cartController.FindProduct("UM-002"); // ID Holder (₱45)
            var p3 = _cartController.FindProduct("UM-003"); // UMak Polo (Size L, ₱450)
            var p4 = _cartController.FindProduct("UM-004"); // UMak Lanyard (₱60)

            if (p1 != null) _cartController.AddItem(p1, 2); // 700
            if (p2 != null) _cartController.AddItem(p2, 1); // 45
            if (p3 != null) _cartController.AddItem(p3, 1); // 450
            if (p4 != null) _cartController.AddItem(p4, 1); // 60
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

        private void btnScanId_Click(object sender, EventArgs e)
        {
            ApplyStudentId(txtStudentId.Text.Trim());
        }
    }
}