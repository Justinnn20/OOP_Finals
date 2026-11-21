using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Login
{
    public partial class Form2 : Form
    {
        private Button[] tabs;
        private readonly CartController _cartController;
        private string _activeCategory = "ALL";
        private readonly ToolTip _statusTip = new ToolTip();

        // Product Catalog Item representation for Shelves
        private class ShelfProduct
        {
            public string Sku { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Subtitle { get; set; } = string.Empty;
        }

        private readonly List<ShelfProduct> _shelfProducts = new List<ShelfProduct>();

        public Form2()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            EnableDoubleBuffering(pnlCartContainer);
            EnableDoubleBuffering(flpProducts);

            _cartController = new CartController();

            InitShelfCatalog();
            SetupTabs();
            SetupCartEvents();
            SetupOrderActions();
            SetupBarcodeScanner();
            SetupStudentIdQuickSelect();
            SetupDiscountRadios();
            SetupCashInput();
            SetupNavigation();

            RenderShelves();
            RenderCart();
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitShelfCatalog()
        {
            _shelfProducts.Clear();
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-004", Name = "PE Uniform", Category = "Uniforms", Price = 350.00m, Subtitle = "Size M · ₱350 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-008", Name = "ID Holder", Category = "School Supplies", Price = 45.00m, Subtitle = "₱45 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-005", Name = "UMak Polo", Category = "Uniforms", Price = 450.00m, Subtitle = "Size L · ₱450 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-009", Name = "UMak Lanyard", Category = "School Supplies", Price = 60.00m, Subtitle = "₱60 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-010", Name = "PE Jogging Pants", Category = "Uniforms", Price = 400.00m, Subtitle = "Size M · ₱400 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-014", Name = "College Manual", Category = "Books & Manuals", Price = 280.00m, Subtitle = "₱280 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-011", Name = "UMak Spiral Notebook", Category = "School Supplies", Price = 35.00m, Subtitle = "₱35 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-012", Name = "Ballpen Set (3pcs)", Category = "School Supplies", Price = 65.00m, Subtitle = "₱65 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-013", Name = "University Pin", Category = "School Supplies", Price = 50.00m, Subtitle = "₱50 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-015", Name = "Computer Programming", Category = "Books & Manuals", Price = 550.00m, Subtitle = "₱550 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-001", Name = "Calculus Textbook", Category = "Books & Manuals", Price = 450.00m, Subtitle = "₱450 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-002", Name = "Blouse (Female)", Category = "Uniforms", Price = 380.00m, Subtitle = "Size M · ₱380 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-003", Name = "Yellow Pad", Category = "School Supplies", Price = 30.00m, Subtitle = "₱30 each" });
            _shelfProducts.Add(new ShelfProduct { Sku = "UM-007", Name = "Blue Book", Category = "School Supplies", Price = 20.00m, Subtitle = "₱20 each" });

            // Ensure all shelf products are known to the CartController catalog
            foreach (var p in _shelfProducts)
            {
                _cartController.RegisterProduct(p.Sku, p.Name, p.Category, p.Price, p.Subtitle);
            }
        }

        private void SetupTabs()
        {
            tabs = new[]
            {
                btnTabAll, btnTabBooks, btnTabUniforms, btnTabSupplies,
                btnStockIn, btnTransactionLogs, btnCashierPos
            };

            btnTabAll.Click += (s, e) => { _activeCategory = "ALL"; lblShelves.Text = "ALL SHELVES"; SelectTab(btnTabAll); RenderShelves(); };
            btnTabBooks.Click += (s, e) => { _activeCategory = "Books & Manuals"; lblShelves.Text = "BOOKS & MANUALS"; SelectTab(btnTabBooks); RenderShelves(); };
            btnTabUniforms.Click += (s, e) => { _activeCategory = "Uniforms"; lblShelves.Text = "UNIFORMS"; SelectTab(btnTabUniforms); RenderShelves(); };
            btnTabSupplies.Click += (s, e) => { _activeCategory = "School Supplies"; lblShelves.Text = "SCHOOL SUPPLIES"; SelectTab(btnTabSupplies); RenderShelves(); };

            btnStockIn.Click += (s, e) =>
            {
                SelectTab(btnStockIn);
                MessageBox.Show("Stock-In & Release module is available in the Admin Inventory portal.", "Stock-In & Release", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnTransactionLogs.Click += (s, e) =>
            {
                SelectTab(btnTransactionLogs);
                MessageBox.Show("Complete transaction audit logs are available in Admin Transactions.", "Transaction Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnCashierPos.Click += (s, e) =>
            {
                _activeCategory = "ALL";
                lblShelves.Text = "ALL SHELVES";
                SelectTab(btnTabAll);
                RenderShelves();
            };

            SelectTab(btnTabAll);
        }

        private void SelectTab(Button selected)
        {
            foreach (Button tab in tabs)
            {
                bool isSelected = (tab == selected);
                tab.BackColor = isSelected ? Color.FromArgb(255, 213, 79) : Color.FromArgb(30, 10, 14);
                tab.ForeColor = isSelected ? Color.FromArgb(60, 20, 25) : Color.FromArgb(245, 230, 200);
            }
        }

        private void RenderShelves()
        {
            flpProducts.SuspendLayout();
            flpProducts.Controls.Clear();

            var visibleProducts = _activeCategory == "ALL"
                ? _shelfProducts
                : _shelfProducts.Where(p => string.Equals(p.Category, _activeCategory, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var prod in visibleProducts)
            {
                Panel card = new Panel
                {
                    Size = new Size(188, 122),
                    Margin = new Padding(6),
                    BackColor = Color.FromArgb(42, 14, 20),
                    Cursor = Cursors.Hand
                };

                card.Paint += (s, e) =>
                {
                    using (Pen p = new Pen(Color.FromArgb(90, 30, 38), 1f))
                    {
                        e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
                    }
                };

                Label lblName = new Label
                {
                    Text = prod.Name,
                    Font = new Font("Georgia", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(255, 235, 195),
                    Location = new Point(8, 8),
                    Size = new Size(172, 22),
                    AutoEllipsis = true
                };

                Label lblSub = new Label
                {
                    Text = prod.Subtitle,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(180, 150, 130),
                    Location = new Point(8, 30),
                    Size = new Size(172, 18),
                    AutoEllipsis = true
                };

                Label lblSku = new Label
                {
                    Text = $"SKU: {prod.Sku}",
                    Font = new Font("Segoe UI", 7.5F),
                    ForeColor = Color.FromArgb(220, 180, 100),
                    Location = new Point(8, 50),
                    Size = new Size(172, 18)
                };

                Button btnAdd = new Button
                {
                    Text = "+ Add to Cart",
                    Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                    BackColor = Color.FromArgb(229, 181, 58),
                    ForeColor = Color.FromArgb(44, 14, 22),
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(8, 76),
                    Size = new Size(172, 32),
                    Cursor = Cursors.Hand
                };
                btnAdd.FlatAppearance.BorderSize = 0;

                string sku = prod.Sku;
                btnAdd.Click += (s, e) => _cartController.AddToCart(sku);
                card.Click += (s, e) => _cartController.AddToCart(sku);
                lblName.Click += (s, e) => _cartController.AddToCart(sku);

                card.Controls.Add(lblName);
                card.Controls.Add(lblSub);
                card.Controls.Add(lblSku);
                card.Controls.Add(btnAdd);

                flpProducts.Controls.Add(card);
            }

            flpProducts.ResumeLayout();
        }

        private void SetupCartEvents()
        {
            _cartController.CartChanged += RenderCart;
            _cartController.StatusMessage += msg =>
            {
                _statusTip.Show(msg, txtBarcode, 0, txtBarcode.Height + 4, 2500);
            };
        }

        private void RenderCart()
        {
            pnlCartContainer.SuspendLayout();
            pnlCartContainer.Controls.Clear();

            var order = _cartController.ActiveOrder;

            if (order.IsEmpty)
            {
                lblBlank.Visible = true;
            }
            else
            {
                lblBlank.Visible = false;

                int y = 0;
                int itemHeight = 64;

                foreach (var item in order.Items)
                {
                    Panel itemRow = new Panel
                    {
                        Location = new Point(0, y),
                        Size = new Size(pnlCartContainer.Width - 10, itemHeight),
                        BackColor = Color.Transparent
                    };

                    itemRow.Paint += (s, e) =>
                    {
                        using (Pen dottedPen = new Pen(Color.FromArgb(216, 198, 152), 1f))
                        {
                            dottedPen.DashStyle = DashStyle.Dot;
                            e.Graphics.DrawLine(dottedPen, 4, itemHeight - 1, itemRow.Width - 4, itemHeight - 1);
                        }
                    };

                    // Product Name
                    Label lblName = new Label
                    {
                        Text = item.ProductName,
                        Font = new Font("Georgia", 9.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(44, 14, 22),
                        Location = new Point(4, 4),
                        Size = new Size(190, 18),
                        AutoEllipsis = true
                    };

                    // Subtitle / Unit Price
                    string subtitle = string.IsNullOrWhiteSpace(item.Size)
                        ? $"₱{item.UnitPrice:N0} each"
                        : $"{item.Size} · ₱{item.UnitPrice:N0} each";

                    Label lblSub = new Label
                    {
                        Text = subtitle,
                        Font = new Font("Segoe UI", 8F),
                        ForeColor = Color.FromArgb(120, 90, 80),
                        Location = new Point(4, 22),
                        Size = new Size(190, 16),
                        AutoEllipsis = true
                    };

                    // Quantity Stepper: [-] [qty] [+]
                    Panel pnlStepper = new Panel
                    {
                        Location = new Point(4, 38),
                        Size = new Size(95, 22),
                        BackColor = Color.FromArgb(250, 240, 215)
                    };
                    pnlStepper.Paint += (s, e) =>
                    {
                        using (Pen p = new Pen(Color.FromArgb(210, 185, 140), 1f))
                        {
                            e.Graphics.DrawRectangle(p, 0, 0, pnlStepper.Width - 1, pnlStepper.Height - 1);
                        }
                    };

                    Button btnMinus = new Button
                    {
                        Text = "-",
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        Location = new Point(0, 0),
                        Size = new Size(24, 22),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        BackColor = Color.Transparent,
                        ForeColor = Color.FromArgb(44, 14, 22)
                    };
                    btnMinus.FlatAppearance.BorderSize = 0;

                    Label lblQty = new Label
                    {
                        Text = item.Quantity.ToString(),
                        Font = new Font("Georgia", 8.5F, FontStyle.Bold),
                        Location = new Point(24, 2),
                        Size = new Size(47, 18),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.FromArgb(44, 14, 22)
                    };

                    Button btnPlus = new Button
                    {
                        Text = "+",
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        Location = new Point(71, 0),
                        Size = new Size(24, 22),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        BackColor = Color.Transparent,
                        ForeColor = Color.FromArgb(44, 14, 22)
                    };
                    btnPlus.FlatAppearance.BorderSize = 0;

                    string currentSku = item.Sku;
                    btnMinus.Click += (s, e) => _cartController.DecreaseQuantity(currentSku);
                    btnPlus.Click += (s, e) => _cartController.IncreaseQuantity(currentSku);

                    pnlStepper.Controls.Add(btnMinus);
                    pnlStepper.Controls.Add(lblQty);
                    pnlStepper.Controls.Add(btnPlus);

                    // Line Total (Price on right)
                    Label lblTotal = new Label
                    {
                        Text = $"₱{item.LineTotal:N0}",
                        Font = new Font("Georgia", 10.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(44, 14, 22),
                        Location = new Point(190, 8),
                        Size = new Size(105, 22),
                        TextAlign = ContentAlignment.MiddleRight
                    };

                    itemRow.Controls.Add(lblName);
                    itemRow.Controls.Add(lblSub);
                    itemRow.Controls.Add(pnlStepper);
                    itemRow.Controls.Add(lblTotal);

                    pnlCartContainer.Controls.Add(itemRow);
                    y += itemHeight;
                }
            }

            pnlCartContainer.ResumeLayout();

            // Totals and KPI update
            lblSubtotalValue.Text = $"₱{order.Subtotal:N0}";
            lblTotalValue.Text = $"₱{order.Total:N0}";
            lblOrdersValue.Text = order.TotalItemCount.ToString();

            // Hold Order Button State
            if (_cartController.HeldOrderCount > 0)
            {
                btnHoldOrder.Text = $"Hold ({_cartController.HeldOrderCount})";
                btnHoldOrder.BackColor = Color.FromArgb(235, 205, 140);
            }
            else
            {
                btnHoldOrder.Text = "Hold";
                btnHoldOrder.BackColor = Color.FromArgb(244, 227, 178);
            }
        }

        private void SetupOrderActions()
        {
            // Hold Order
            btnHoldOrder.Click += (s, e) =>
            {
                if (!_cartController.ActiveOrder.IsEmpty)
                {
                    _cartController.HoldCurrentOrder();
                }
                else if (_cartController.HeldOrderCount > 0)
                {
                    _cartController.RestoreLatestHeldOrder();
                }
                else
                {
                    MessageBox.Show("The order scroll is empty. Add items before holding.", "Hold Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Clear Cart
            btnClearCart.Click += (s, e) =>
            {
                if (_cartController.ActiveOrder.IsEmpty) return;

                DialogResult dr = MessageBox.Show(
                    "Are you sure you want to clear all items in the Order Scroll?",
                    "Clear Order Scroll",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    _cartController.ClearCart();
                }
            };

            // Checkout
            btnCheckout.Click += BtnCheckout_Click;
        }

        private void SetupBarcodeScanner()
        {
            btnAddBarcode.Click += (s, e) => ExecuteBarcodeInput();
            txtBarcode.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ExecuteBarcodeInput();
                }
            };
        }

        private void ExecuteBarcodeInput()
        {
            string input = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            bool success = _cartController.ProcessBarcodeInput(input);
            if (success)
            {
                txtBarcode.Clear();
            }
            else
            {
                txtBarcode.SelectAll();
            }
        }

        private void SetupStudentIdQuickSelect()
        {
            btnTry1.Click += (s, e) => { txtStudentId.Text = "22-102"; btnScanId.PerformClick(); };
            btnTry2.Click += (s, e) => { txtStudentId.Text = "22-203"; btnScanId.PerformClick(); };
            btnTry3.Click += (s, e) => { txtStudentId.Text = "21-307"; btnScanId.PerformClick(); };

            btnScanId.Click += (s, e) =>
            {
                string id = txtStudentId.Text.Trim();
                if (!string.IsNullOrEmpty(id))
                {
                    _cartController.ActiveOrder.StudentId = id;
                    _statusTip.Show($"Verified Student ID: {id}", txtStudentId, 0, txtStudentId.Height + 4, 2000);
                }
            };
        }

        private void SetupDiscountRadios()
        {
            rbNone.CheckedChanged += (s, e) =>
            {
                if (rbNone.Checked) _cartController.SetDiscount("None", 0m);
            };

            rbScholar.CheckedChanged += (s, e) =>
            {
                if (rbScholar.Checked) _cartController.SetDiscount("Scholar 10%", 0.10m);
            };

            rbStaff.CheckedChanged += (s, e) =>
            {
                if (rbStaff.Checked) _cartController.SetDiscount("Staff 15%", 0.15m);
            };
        }

        private void SetupCashInput()
        {
            string lastCash = string.IsNullOrWhiteSpace(txtCash.Text) ? "0" : txtCash.Text.Trim();

            txtCash.TextChanged += (s, e) =>
            {
                if (!txtCash.Focused && !string.IsNullOrWhiteSpace(txtCash.Text))
                {
                    lastCash = txtCash.Text.Trim();
                }
            };

            // Once clicked / focused: clear current number
            txtCash.Enter += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtCash.Text))
                {
                    lastCash = txtCash.Text.Trim();
                }
                txtCash.Text = "";
            };

            // When clicked outside / lost focus: restore previous number if empty
            txtCash.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCash.Text))
                {
                    txtCash.Text = string.IsNullOrWhiteSpace(lastCash) ? "0" : lastCash;
                }
                else
                {
                    lastCash = txtCash.Text.Trim();
                }
            };

            // KeyPress restriction
            txtCash.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;

                if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && txtCash.Text.Contains('.'))
                {
                    e.Handled = true;
                }
            };

            // Remove focus when clicking on background panels
            pnlOrder.Click += (s, e) => { this.ActiveControl = null; };
            this.Click += (s, e) => { this.ActiveControl = null; };
        }

        private void SetupNavigation()
        {
            btnSignOut.Click += (s, e) =>
            {
                DialogResult dr = MessageBox.Show("Sign out from Cashier session?", "Sign Out",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    LoginPage login = new LoginPage();
                    login.Show();
                    this.Hide();
                    login.FormClosed += (sender, args) => this.Close();
                }
            };
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            var order = _cartController.ActiveOrder;
            if (order.IsEmpty)
            {
                MessageBox.Show("Cannot checkout: The Order Scroll has no items.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = order.Total;
            decimal cash = 0m;

            if (rbCash.Checked)
            {
                if (!decimal.TryParse(txtCash.Text.Trim(), out cash) || cash < total)
                {
                    MessageBox.Show($"Cash payment must be at least ₱{total:N0}.", "Insufficient Cash", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCash.Focus();
                    return;
                }
            }

            decimal change = rbCash.Checked ? (cash - total) : 0m;
            string method = rbCash.Checked ? "Cash" : "GCash";

            string receiptSummary =
                $"==========================================\n" +
                $"   UNIVERSITY OF MAKATI COOP BOOKSTORE    \n" +
                $"==========================================\n" +
                $"Order ID:    {order.OrderId}\n" +
                $"Student ID:  {(string.IsNullOrEmpty(order.StudentId) ? "Walk-in" : order.StudentId)}\n" +
                $"Date:        {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                $"Payment:     {method}\n" +
                $"------------------------------------------\n";

            foreach (var item in order.Items)
            {
                receiptSummary += $"{item.Quantity}x {item.ProductName,-24} ₱{item.LineTotal:N2}\n";
            }

            receiptSummary +=
                $"------------------------------------------\n" +
                $"Subtotal:    ₱{order.Subtotal:N2}\n" +
                $"Discount:   -₱{order.DiscountAmount:N2} ({order.DiscountType})\n" +
                $"TOTAL PAID:  ₱{total:N2}\n";

            if (rbCash.Checked)
            {
                receiptSummary +=
                    $"Cash Tender: ₱{cash:N2}\n" +
                    $"Change:      ₱{change:N2}\n";
            }

            receiptSummary +=
                $"==========================================\n" +
                $"   Thank you for supporting UMak Coop!    \n";

            MessageBox.Show(receiptSummary, "Payment Successful - Receipt Printed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Complete order and clear cart
            order.Complete();
            _cartController.ClearCart();
            txtCash.Text = "0";
            txtStudentId.Clear();
            rbNone.Checked = true;
        }

        private void btnScanId_Click(object? sender, EventArgs e)
        {
            // Designer compatibility
        }
    }
}