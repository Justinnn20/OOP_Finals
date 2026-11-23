using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Login.Database;

namespace Login
{
    public partial class AdminInventory : Form
    {
        // Theme Palette (matches AdminDB, AdminTransactions, AdminSettings)
        private readonly Color clrBgDark = Color.FromArgb(42, 10, 20);
        private readonly Color clrNavDark = Color.FromArgb(35, 8, 16);
        private readonly Color clrParchment = Color.FromArgb(244, 227, 178);
        private readonly Color clrGold = Color.FromArgb(229, 181, 58);
        private readonly Color clrGoldBorder = Color.FromArgb(196, 152, 54);
        private readonly Color clrMaroon = Color.FromArgb(88, 23, 38);
        private readonly Color clrTextDark = Color.FromArgb(44, 14, 22);
        private readonly Color clrCrimson = Color.FromArgb(158, 34, 38);
        private readonly Color clrMuted = Color.FromArgb(124, 102, 78);
        private readonly Color clrDivider = Color.FromArgb(216, 198, 152);
        private readonly Color clrActiveGreen = Color.FromArgb(68, 138, 78);
        private readonly Color clrLowOrange = Color.FromArgb(204, 122, 34);
        private readonly Color clrSelectedRow = Color.FromArgb(238, 217, 160);

        private Button activeNavButton;
        private string activeStockFilter = "ALL"; // ALL, IN_STOCK, LOW_STOCK, OUT_OF_STOCK
        private string activeCategoryFilter = "All Categories";
        private bool isSearchPlaceholder = true;

        // Model
        public class InventoryItem
        {
            public string Sku { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public int ReorderAt { get; set; }
        }

        private List<InventoryItem> inventoryItems = new List<InventoryItem>();
        private InventoryItem? selectedItem = null;
        private bool isInitialized = false;

        // Pagination
        private int currentPage = 1;
        private int pageSize = 6;

        public AdminInventory()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            EnableDoubleBuffering(pnlItemList);
            activeNavButton = btnNavInventory;

            InitInventory();
            SetupPaintHandlers();
            SetupEventHandlers();

            if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
            if (cmbFilterCategory.Items.Count > 0) cmbFilterCategory.SelectedIndex = 0;
            if (inventoryItems.Count > 0) LoadItemToEditor(inventoryItems[0]);

            UpdateKpiCards();
            RenderInventoryTable();

            isInitialized = true;
            this.Load += (s, e) => LayoutResponsive();
        }

        private void InitInventory()
        {
            inventoryItems = CoopDatabase.LoadInventory()
                .Select(product => new InventoryItem
                {
                    Sku = product.Sku,
                    Name = product.Name,
                    Category = product.Category,
                    Price = product.Price,
                    Stock = product.Stock,
                    ReorderAt = 10
                }).ToList();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutResponsive();
        }

        private void LayoutResponsive()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            LayoutHeaderNav();

            if (!isInitialized) return;

            int margin = 28;
            int gap = 16;
            int availableWidth = Math.Max(1200, this.ClientSize.Width) - (margin * 2);

            // 4 KPI Cards across top
            int cardWidth = (availableWidth - (gap * 3)) / 4;
            pnlCardTotalProducts.Location = new Point(margin, 144);
            pnlCardTotalProducts.Size = new Size(cardWidth, 88);

            pnlCardTotalUnits.Location = new Point(margin + (cardWidth + gap), 144);
            pnlCardTotalUnits.Size = new Size(cardWidth, 88);

            pnlCardLowStock.Location = new Point(margin + (cardWidth + gap) * 2, 144);
            pnlCardLowStock.Size = new Size(cardWidth, 88);

            pnlCardOutStock.Location = new Point(margin + (cardWidth + gap) * 3, 144);
            pnlCardOutStock.Size = new Size(availableWidth - 3 * (cardWidth + gap), 88);

            // Main Panels
            int topY = 244;
            int bottomMargin = 20;
            int availableHeight = Math.Max(420, this.ClientSize.Height - topY - bottomMargin);

            int catWidth = (int)(availableWidth * 0.66);
            int detWidth = availableWidth - catWidth - gap;

            pnlCatalogCard.Location = new Point(margin, topY);
            pnlCatalogCard.Size = new Size(catWidth, availableHeight);

            pnlDetailCard.Location = new Point(margin + catWidth + gap, topY);
            pnlDetailCard.Size = new Size(detWidth, availableHeight);

            // Adjust children of pnlCatalogCard
            pnlItemList.Width = pnlCatalogCard.Width - 36;
            pnlItemList.Height = Math.Max(200, pnlCatalogCard.Height - 120);

            pageSize = Math.Max(6, Math.Min(12, (pnlItemList.Height - 10) / 48));

            btnAddItemModal.Location = new Point(pnlCatalogCard.Width - btnAddItemModal.Width - 18, 40);
            cmbFilterCategory.Location = new Point(btnAddItemModal.Left - cmbFilterCategory.Width - 10, 42);

            lblCatalogShowing.Location = new Point(24, pnlCatalogCard.Height - 34);
            btnPageNext.Location = new Point(pnlCatalogCard.Width - btnPageNext.Width - 18, pnlCatalogCard.Height - 36);
            lblPageNum.Location = new Point(btnPageNext.Left - lblPageNum.Width - 4, pnlCatalogCard.Height - 34);
            btnPagePrev.Location = new Point(lblPageNum.Left - btnPagePrev.Width - 4, pnlCatalogCard.Height - 36);

            LayoutDetailCard();
            RenderInventoryTable();
            this.Invalidate(true);
        }

        private void LayoutDetailCard()
        {
            if (pnlDetailCard == null) return;

            int cardW = pnlDetailCard.Width;
            int cardH = pnlDetailCard.Height;
            int padX = 22;
            int innerW = Math.Max(260, cardW - (padX * 2));

            lblDetailTitle.Font = new Font("Georgia", 11.5F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(padX, 15);

            Font labelFont = new Font("Georgia", 8.5F, FontStyle.Bold);
            Font inputFont = new Font("Georgia", 9.5F);

            lblSku.Font = labelFont;
            lblName.Font = labelFont;
            lblCategory.Font = labelFont;
            lblPrice.Font = labelFont;
            lblStock.Font = labelFont;
            lblReorder.Font = labelFont;

            txtSku.Font = inputFont;
            txtName.Font = inputFont;
            cmbCategory.Font = inputFont;
            txtPrice.Font = inputFont;
            txtStock.Font = inputFont;
            txtReorder.Font = inputFont;

            txtSku.Width = innerW;
            txtName.Width = innerW;
            cmbCategory.Width = innerW;

            txtSku.Height = 24;
            txtName.Height = 24;
            txtPrice.Height = 24;
            txtStock.Height = 24;
            txtReorder.Height = 24;

            int colGap = 10;
            int colW = (innerW - (colGap * 2)) / 3;
            lblPrice.Left = padX;
            txtPrice.Left = padX;
            txtPrice.Width = colW;

            lblStock.Left = padX + colW + colGap;
            txtStock.Left = padX + colW + colGap;
            txtStock.Width = colW;

            lblReorder.Left = padX + (colW + colGap) * 2;
            txtReorder.Left = padX + (colW + colGap) * 2;
            txtReorder.Width = innerW - (colW + colGap) * 2;

            btnSaveItem.Width = innerW;
            btnQuickRestock.Width = innerW;
            btnSaveItem.Left = padX;
            btnQuickRestock.Left = padX;

            int btnGap = 10;
            int halfBtnW = (innerW - btnGap) / 2;
            btnDeleteItem.Left = padX;
            btnDeleteItem.Width = halfBtnW;
            btnClearFields.Left = padX + halfBtnW + btnGap;
            btnClearFields.Width = innerW - halfBtnW - btnGap;

            btnSaveItem.Height = 38;
            btnQuickRestock.Height = 36;
            btnDeleteItem.Height = 34;
            btnClearFields.Height = 34;

            btnSaveItem.Font = new Font("Georgia", 9F, FontStyle.Bold);
            btnQuickRestock.Font = new Font("Georgia", 8.5F, FontStyle.Bold);
            btnDeleteItem.Font = new Font("Georgia", 8F, FontStyle.Bold);
            btnClearFields.Font = new Font("Georgia", 8F, FontStyle.Bold);

            // Dynamic vertical distribution down the card to fit available height gracefully
            int vGap = Math.Clamp((cardH - 370) / 7, 6, 22);

            int y = 54;
            lblSku.Location = new Point(padX, y); y += 18;
            txtSku.Location = new Point(padX, y); y += txtSku.Height + vGap;

            lblName.Location = new Point(padX, y); y += 18;
            txtName.Location = new Point(padX, y); y += txtName.Height + vGap;

            lblCategory.Location = new Point(padX, y); y += 18;
            cmbCategory.Location = new Point(padX, y); y += cmbCategory.Height + vGap;

            lblPrice.Top = y;
            lblStock.Top = y;
            lblReorder.Top = y;
            y += 18;
            txtPrice.Top = y;
            txtStock.Top = y;
            txtReorder.Top = y;
            y += txtPrice.Height + vGap + 4;

            btnSaveItem.Location = new Point(padX, y); y += btnSaveItem.Height + vGap;
            btnQuickRestock.Location = new Point(padX, y); y += btnQuickRestock.Height + vGap;
            btnDeleteItem.Top = y;
            btnClearFields.Top = y;
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitSampleInventory()
        {
            inventoryItems = new List<InventoryItem>
            {
                new InventoryItem { Sku = "UM-001", Name = "Calculus Textbook", Category = "Books & Manuals", Price = 450.00m, Stock = 0, ReorderAt = 10 },
                new InventoryItem { Sku = "UM-002", Name = "Blouse (Female)", Category = "Uniforms", Price = 380.00m, Stock = 0, ReorderAt = 10 },
                new InventoryItem { Sku = "UM-003", Name = "Yellow Pad", Category = "School Supplies", Price = 30.00m, Stock = 0, ReorderAt = 20 },
                new InventoryItem { Sku = "UM-004", Name = "PE Uniform (M)", Category = "Uniforms", Price = 350.00m, Stock = 3, ReorderAt = 10 },
                new InventoryItem { Sku = "UM-005", Name = "UMak Polo (L)", Category = "Uniforms", Price = 450.00m, Stock = 4, ReorderAt = 10 },
                new InventoryItem { Sku = "UM-006", Name = "CCIS Lab Manual", Category = "Books & Manuals", Price = 120.00m, Stock = 5, ReorderAt = 10 },
                new InventoryItem { Sku = "UM-007", Name = "Blue Book", Category = "School Supplies", Price = 20.00m, Stock = 6, ReorderAt = 20 },
                new InventoryItem { Sku = "UM-008", Name = "ID Holder", Category = "School Supplies", Price = 45.00m, Stock = 85, ReorderAt = 15 },
                new InventoryItem { Sku = "UM-009", Name = "UMak Lanyard", Category = "School Supplies", Price = 60.00m, Stock = 140, ReorderAt = 20 },
                new InventoryItem { Sku = "UM-010", Name = "Spiral Notebook", Category = "School Supplies", Price = 35.00m, Stock = 180, ReorderAt = 25 },
                new InventoryItem { Sku = "UM-011", Name = "Ballpen Set (3pcs)", Category = "School Supplies", Price = 65.00m, Stock = 95, ReorderAt = 20 },
                new InventoryItem { Sku = "UM-012", Name = "University Pin", Category = "School Supplies", Price = 50.00m, Stock = 110, ReorderAt = 15 }
            };
        }

        private void SetupPaintHandlers()
        {
            // Top Nav
            pnlTopNav.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(clrGoldBorder, 2f))
                {
                    e.Graphics.DrawLine(borderPen, 0, pnlTopNav.Height - 1, pnlTopNav.Width, pnlTopNav.Height - 1);
                }
            };

            pnlLogoIcon.Paint += PnlLogoIcon_Paint;
            pnlNotif.Paint += PnlNotif_Paint;
            pnlAvatar.Paint += PnlAvatar_Paint;

            // Nav Buttons
            Button[] navButtons = { btnNavDashboard, btnNavInventory, btnNavTransactions, btnNavReports, btnNavSettings };
            foreach (Button btn in navButtons)
            {
                btn.ForeColor = Color.Transparent; // Hide default text to eliminate ghosting
                btn.Paint += NavButton_Paint;
                btn.Click += NavButton_Click;
            }

            // Sign Out
            btnSignOut.Paint += BtnSignOut_Paint;
            btnSignOut.Click += BtnSignOut_Click;

            // KPI Rounded Parchment Cards
            pnlCardTotalProducts.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardTotalProducts.ClientRectangle, 14);
            pnlCardTotalUnits.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardTotalUnits.ClientRectangle, 14);
            pnlCardLowStock.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardLowStock.ClientRectangle, 14);
            pnlCardOutStock.Paint += (s, e) => DrawParchmentCard(e.Graphics, pnlCardOutStock.ClientRectangle, 14);

            // KPI Circular Vector Icons
            pnlIconTotalProd.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconTotalProd.ClientRectangle, "products");
            pnlIconTotalUnits.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconTotalUnits.ClientRectangle, "stock");
            pnlIconLowStock.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconLowStock.ClientRectangle, "warning");
            pnlIconOutStock.Paint += (s, e) => DrawKpiCircleIcon(e.Graphics, pnlIconOutStock.ClientRectangle, "out");

            // Main cards
            pnlCatalogCard.Paint += PnlCatalogCard_Paint;
            pnlDetailCard.Paint += (s, e) =>
            {
                DrawParchmentCard(e.Graphics, pnlDetailCard.ClientRectangle, 16);
                using (Pen divPen = new Pen(clrDivider, 1f))
                {
                    e.Graphics.DrawLine(divPen, 18, 46, pnlDetailCard.Width - 18, 46);
                }
            };

            // Search box inner border
            pnlSearchBox.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(clrGoldBorder, 1.2f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlSearchBox.Width - 1, pnlSearchBox.Height - 1);
                }
            };

            // Stock Filter Pills Painting
            btnFilterAll.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterAll, activeStockFilter == "ALL");
            btnFilterInStock.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterInStock, activeStockFilter == "IN_STOCK");
            btnFilterLowStock.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterLowStock, activeStockFilter == "LOW_STOCK");
            btnFilterOutStock.Paint += (s, e) => DrawFilterPill(e.Graphics, btnFilterOutStock, activeStockFilter == "OUT_OF_STOCK");

            // Button borders
            btnAddItemModal.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnAddItemModal, 8);
            btnQuickRestock.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnQuickRestock, 8);
            btnSaveItem.Paint += (s, e) => DrawMaroonPillButton(e.Graphics, btnSaveItem, 8);
            btnDeleteItem.Paint += (s, e) => DrawCrimsonPillButton(e.Graphics, btnDeleteItem, 8);
            btnClearFields.Paint += (s, e) => DrawParchmentPillButton(e.Graphics, btnClearFields, 8);
        }

        private void SetupEventHandlers()
        {
            // Stock Filter button clicks
            btnFilterAll.Click += (s, e) => SelectStockFilter("ALL");
            btnFilterInStock.Click += (s, e) => SelectStockFilter("IN_STOCK");
            btnFilterLowStock.Click += (s, e) => SelectStockFilter("LOW_STOCK");
            btnFilterOutStock.Click += (s, e) => SelectStockFilter("OUT_OF_STOCK");

            // Quick Filter when clicking the KPI cards!
            pnlCardTotalProducts.Click += (s, e) => SelectStockFilter("ALL");
            lblTotalProdVal.Click += (s, e) => SelectStockFilter("ALL");

            pnlCardTotalUnits.Click += (s, e) => SelectStockFilter("IN_STOCK");
            lblTotalUnitsVal.Click += (s, e) => SelectStockFilter("IN_STOCK");

            pnlCardLowStock.Click += (s, e) => SelectStockFilter("LOW_STOCK");
            lblLowStockVal.Click += (s, e) => SelectStockFilter("LOW_STOCK");

            pnlCardOutStock.Click += (s, e) => SelectStockFilter("OUT_OF_STOCK");
            lblOutStockVal.Click += (s, e) => SelectStockFilter("OUT_OF_STOCK");

            // Category filter dropdown
            cmbFilterCategory.SelectedIndexChanged += (s, e) =>
            {
                activeCategoryFilter = cmbFilterCategory.SelectedItem?.ToString() ?? "All Categories";
                currentPage = 1;
                RenderInventoryTable();
            };

            // Search box placeholder & text change
            txtSearchItem.GotFocus += (s, e) =>
            {
                if (isSearchPlaceholder)
                {
                    txtSearchItem.Text = "";
                    txtSearchItem.Font = new Font("Georgia", 9F, FontStyle.Regular);
                    txtSearchItem.ForeColor = clrTextDark;
                    isSearchPlaceholder = false;
                }
            };

            txtSearchItem.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchItem.Text))
                {
                    isSearchPlaceholder = true;
                    txtSearchItem.Text = "Search SKU or name...";
                    txtSearchItem.Font = new Font("Georgia", 8.5F, FontStyle.Italic);
                    txtSearchItem.ForeColor = clrMuted;
                }
            };

            txtSearchItem.TextChanged += (s, e) =>
            {
                currentPage = 1;
                RenderInventoryTable();
            };

            // Pagination
            btnPagePrev.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    RenderInventoryTable();
                }
            };

            btnPageNext.Click += (s, e) =>
            {
                var filtered = GetFilteredItems();
                int totalPages = Math.Max(1, (int)Math.Ceiling((double)filtered.Count / pageSize));
                if (currentPage < totalPages)
                {
                    currentPage++;
                    RenderInventoryTable();
                }
            };

            // Editor Actions
            btnSaveItem.Click += BtnSaveItem_Click;
            btnQuickRestock.Click += BtnQuickRestock_Click;
            btnDeleteItem.Click += BtnDeleteItem_Click;
            btnClearFields.Click += (s, e) => ClearEditorForm();

            btnAddItemModal.Click += (s, e) =>
            {
                ClearEditorForm();
                txtSku.Focus();
            };

            // Numeric Input Boxes: Clear on focus, restore previous on blur if empty
            NumericInputHelper.Attach(txtPrice, "0.00", isDecimal: true);
            NumericInputHelper.Attach(txtStock, "0", isDecimal: false);
            NumericInputHelper.Attach(txtReorder, "10", isDecimal: false);

            // Clicking outside removes focus so values restore immediately if left blank
            NumericInputHelper.EnableClickToDefocus(this);
            NumericInputHelper.EnableClickToDefocus(pnlDetailCard);
            NumericInputHelper.EnableClickToDefocus(pnlCatalogCard);
        }

        private void SelectStockFilter(string filter)
        {
            activeStockFilter = filter;
            currentPage = 1;

            btnFilterAll.Invalidate();
            btnFilterInStock.Invalidate();
            btnFilterLowStock.Invalidate();
            btnFilterOutStock.Invalidate();

            RenderInventoryTable();
        }

        private List<InventoryItem> GetFilteredItems()
        {
            if (inventoryItems == null) return new List<InventoryItem>();

            string search = (isSearchPlaceholder || string.IsNullOrWhiteSpace(txtSearchItem.Text))
                ? string.Empty
                : txtSearchItem.Text.Trim().ToLowerInvariant();

            List<InventoryItem> result = new List<InventoryItem>();

            foreach (var item in inventoryItems)
            {
                // Category Filter
                if (activeCategoryFilter != "All Categories")
                {
                    if (!string.Equals(item.Category, activeCategoryFilter, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // Stock Status Filter
                if (activeStockFilter == "IN_STOCK")
                {
                    if (item.Stock <= item.ReorderAt) continue;
                }
                else if (activeStockFilter == "LOW_STOCK")
                {
                    if (item.Stock <= 0 || item.Stock > item.ReorderAt) continue;
                }
                else if (activeStockFilter == "OUT_OF_STOCK")
                {
                    if (item.Stock > 0) continue;
                }

                // Search Filter
                if (!string.IsNullOrEmpty(search))
                {
                    bool matchSku = item.Sku.ToLowerInvariant().Contains(search);
                    bool matchName = item.Name.ToLowerInvariant().Contains(search);
                    bool matchCat = item.Category.ToLowerInvariant().Contains(search);
                    if (!matchSku && !matchName && !matchCat) continue;
                }

                result.Add(item);
            }

            return result;
        }

        private void UpdateKpiCards()
        {
            int totalProds = inventoryItems.Count;
            int totalUnits = 0;
            int lowCount = 0;
            int outCount = 0;

            foreach (var itm in inventoryItems)
            {
                totalUnits += itm.Stock;
                if (itm.Stock <= 0) outCount++;
                else if (itm.Stock <= itm.ReorderAt) lowCount++;
            }

            lblTotalProdVal.Text = $"{totalProds} ITEMS";
            lblTotalUnitsVal.Text = $"{totalUnits:N0} UNITS";
            lblLowStockVal.Text = $"{lowCount} ITEMS";
            lblOutStockVal.Text = $"{outCount} ITEMS";

            // Update button texts with live counts
            btnFilterLowStock.Text = $"LOW ({lowCount})";
            btnFilterOutStock.Text = $"OUT ({outCount})";
        }

        private void RenderInventoryTable()
        {
            if (!isInitialized || inventoryItems == null || pnlItemList == null) return;

            pnlItemList.SuspendLayout();
            pnlItemList.Controls.Clear();

            List<InventoryItem> filtered = GetFilteredItems();
            int totalFiltered = filtered.Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalFiltered / pageSize));
            if (currentPage > totalPages) currentPage = totalPages;

            int startIndex = (currentPage - 1) * pageSize;
            int countToShow = Math.Min(pageSize, totalFiltered - startIndex);

            lblCatalogShowing.Text = $"Showing {countToShow} of {totalFiltered} items";
            lblPageNum.Text = $"{currentPage} / {totalPages}";
            btnPagePrev.Enabled = currentPage > 1;
            btnPageNext.Enabled = currentPage < totalPages;

            int rowHeight = 48;

            if (countToShow <= 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "No matching items found for the selected filters.",
                    Font = new Font("Georgia", 10F, FontStyle.Italic),
                    ForeColor = clrMuted,
                    AutoSize = false,
                    Size = new Size(pnlItemList.Width, 60),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                pnlItemList.Controls.Add(lblEmpty);
            }
            else
            {
                for (int i = 0; i < countToShow; i++)
                {
                    InventoryItem itm = filtered[startIndex + i];
                    bool isSelected = (selectedItem != null && selectedItem.Sku == itm.Sku);
                    bool isLast = (i == countToShow - 1);
                    Color rowBg = isSelected ? clrSelectedRow : clrParchment;

                    Panel row = new Panel
                    {
                        Location = new Point(0, i * rowHeight),
                        Size = new Size(pnlItemList.Width, rowHeight),
                        BackColor = rowBg,
                        Cursor = Cursors.Hand
                    };

                    row.Click += (s, e) =>
                    {
                        LoadItemToEditor(itm);
                        RenderInventoryTable();
                    };

                    int rowW = pnlItemList.Width;
                    int rowActW = 150;
                    int rowActX = rowW - rowActW;
                    int rowStatW = 85;
                    int rowStatX = rowActX - rowStatW - 10;
                    int rowStockW = 50;
                    int rowStockX = rowStatX - rowStockW - 10;
                    int rowPriceW = 75;
                    int rowPriceX = rowStockX - rowPriceW - 10;
                    int rowCatW = 135;
                    int rowCatX = rowPriceX - rowCatW - 10;
                    int rowNameX = 88;
                    int rowNameW = Math.Max(100, rowCatX - rowNameX - 10);

                    // Action buttons on the row
                    Button btnRowRestock = new Button
                    {
                        Text = "+ RESTOCK",
                        Size = new Size(86, 26),
                        Location = new Point(rowActX, 11),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        BackColor = Color.Transparent,
                        Anchor = AnchorStyles.Top | AnchorStyles.Right
                    };
                    btnRowRestock.FlatAppearance.BorderSize = 0;
                    btnRowRestock.Paint += (s, e) => DrawGoldPillButton(e.Graphics, btnRowRestock, 8);
                    btnRowRestock.Click += (s, e) =>
                    {
                        itm.Stock += 20;
                        CoopDatabase.SetStock(itm.Sku, itm.Stock);
                        UpdateKpiCards();
                        RenderInventoryTable();
                        if (selectedItem?.Sku == itm.Sku) LoadItemToEditor(itm);
                    };

                    Button btnRowDelete = new Button
                    {
                        Text = "DEL",
                        Size = new Size(50, 26),
                        Location = new Point(rowActX + 92, 11),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        BackColor = Color.Transparent,
                        Anchor = AnchorStyles.Top | AnchorStyles.Right
                    };
                    btnRowDelete.FlatAppearance.BorderSize = 0;
                    btnRowDelete.Paint += (s, e) => DrawCrimsonPillButton(e.Graphics, btnRowDelete, 8);
                    btnRowDelete.Click += (s, e) =>
                    {
                        DialogResult dr = CoopMessageBox.Show(this, $"Delete product '{itm.Name}' ({itm.Sku}) from inventory?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            inventoryItems.Remove(itm);
                            UpdateKpiCards();
                            RenderInventoryTable();
                        }
                    };

                    row.Controls.Add(btnRowRestock);
                    row.Controls.Add(btnRowDelete);

                    row.Paint += (s, e) =>
                    {
                        Graphics g = e.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        // SKU
                        TextRenderer.DrawText(g, itm.Sku, new Font("Georgia", 8.5F, FontStyle.Bold),
                            new Rectangle(8, 0, 75, rowHeight), clrMaroon,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                        // Name
                        TextRenderer.DrawText(g, itm.Name, new Font("Georgia", 9F, FontStyle.Bold),
                            new Rectangle(rowNameX, 0, rowNameW, rowHeight), clrTextDark,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                        // Category
                        TextRenderer.DrawText(g, itm.Category, new Font("Georgia", 8.5F, FontStyle.Italic),
                            new Rectangle(rowCatX, 0, rowCatW, rowHeight), clrMuted,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

                        // Price
                        TextRenderer.DrawText(g, $"₱{itm.Price:N2}", new Font("Georgia", 8.5F, FontStyle.Bold),
                            new Rectangle(rowPriceX, 0, rowPriceW, rowHeight), clrTextDark,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                        // Stock
                        TextRenderer.DrawText(g, $"{itm.Stock}", new Font("Georgia", 9F, FontStyle.Bold),
                            new Rectangle(rowStockX, 0, rowStockW, rowHeight), itm.Stock <= 0 ? clrCrimson : clrTextDark,
                            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                        // Status Badge
                        Rectangle badgeRect = new Rectangle(rowStatX, 12, rowStatW, 24);
                        Color bBg = itm.Stock <= 0 ? clrCrimson : (itm.Stock <= itm.ReorderAt ? clrLowOrange : clrActiveGreen);
                        string bText = itm.Stock <= 0 ? "OUT" : (itm.Stock <= itm.ReorderAt ? "LOW" : "IN STOCK");

                        using (GraphicsPath bPath = GetRoundedRectPath(badgeRect, 10))
                        using (SolidBrush bBrush = new SolidBrush(bBg))
                        {
                            g.FillPath(bBrush, bPath);
                        }

                        TextRenderer.DrawText(g, bText, new Font("Segoe UI", 7F, FontStyle.Bold),
                            badgeRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                        // Bottom border divider
                        if (!isLast)
                        {
                            using (Pen p = new Pen(clrDivider, 1f))
                            {
                                g.DrawLine(p, 8, rowHeight - 1, row.Width - 8, rowHeight - 1);
                            }
                        }
                    };

                    pnlItemList.Controls.Add(row);
                }
            }

            pnlItemList.ResumeLayout();
        }

        private void LoadItemToEditor(InventoryItem item)
        {
            selectedItem = item;
            txtSku.Text = item.Sku;
            txtName.Text = item.Name;
            cmbCategory.SelectedItem = item.Category;
            txtPrice.Text = item.Price.ToString("0.00");
            txtStock.Text = item.Stock.ToString();
            txtReorder.Text = item.ReorderAt.ToString();

            btnDeleteItem.Enabled = true;
            btnQuickRestock.Enabled = true;
        }

        private void ClearEditorForm()
        {
            selectedItem = null;
            txtSku.Text = "";
            txtName.Text = "";
            if (cmbCategory.Items.Count > 0) cmbCategory.SelectedIndex = 0;
            txtPrice.Text = "0.00";
            txtStock.Text = "0";
            txtReorder.Text = "10";

            btnDeleteItem.Enabled = false;
            btnQuickRestock.Enabled = false;

            RenderInventoryTable();
        }

        private void BtnSaveItem_Click(object? sender, EventArgs e)
        {
            string sku = txtSku.Text.Trim().ToUpperInvariant();
            string name = txtName.Text.Trim();
            string category = cmbCategory.SelectedItem?.ToString() ?? "School Supplies";

            if (string.IsNullOrWhiteSpace(sku) || string.IsNullOrWhiteSpace(name))
            {
                CoopMessageBox.Show(this, "Please provide both SKU and Item Name.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal.TryParse(txtPrice.Text.Trim(), out decimal price);
            int.TryParse(txtStock.Text.Trim(), out int stock);
            int.TryParse(txtReorder.Text.Trim(), out int reorder);

            if (selectedItem != null)
            {
                selectedItem.Sku = sku;
                selectedItem.Name = name;
                selectedItem.Category = category;
                selectedItem.Price = Math.Max(0, price);
                selectedItem.Stock = Math.Max(0, stock);
                selectedItem.ReorderAt = Math.Max(0, reorder);

                CoopMessageBox.Show(this, $"Item '{name}' updated successfully!", "Inventory Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Add New
                InventoryItem newItem = new InventoryItem
                {
                    Sku = sku,
                    Name = name,
                    Category = category,
                    Price = Math.Max(0, price),
                    Stock = Math.Max(0, stock),
                    ReorderAt = Math.Max(0, reorder)
                };
                inventoryItems.Insert(0, newItem);
                selectedItem = newItem;
                CoopMessageBox.Show(this, $"New item '{name}' added to inventory!", "Item Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateKpiCards();
            RenderInventoryTable();
        }

        private void BtnQuickRestock_Click(object? sender, EventArgs e)
        {
            if (selectedItem == null) return;
            selectedItem.Stock += 20;
                CoopDatabase.SetStock(selectedItem.Sku, selectedItem.Stock);
            txtStock.Text = selectedItem.Stock.ToString();
            UpdateKpiCards();
            RenderInventoryTable();
            CoopMessageBox.Show(this, $"Restocked +20 units for {selectedItem.Name}. Total stock: {selectedItem.Stock}", "Restock Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteItem_Click(object? sender, EventArgs e)
        {
            if (selectedItem == null) return;

            DialogResult dr = CoopMessageBox.Show(this, $"Are you sure you want to delete '{selectedItem.Name}' ({selectedItem.Sku})?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                inventoryItems.Remove(selectedItem);
                ClearEditorForm();
                UpdateKpiCards();
                RenderInventoryTable();
            }
        }

        // ==========================================================
        // CUSTOM DRAWING HELPERS
        // ==========================================================

        private void PnlCatalogCard_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawParchmentCard(g, pnlCatalogCard.ClientRectangle, 16);

            int cardW = pnlCatalogCard.Width;
            int cardH = pnlCatalogCard.Height;

            // Table Header Bar
            Rectangle headerBar = new Rectangle(18, 80, cardW - 36, 28);
            using (SolidBrush hBrush = new SolidBrush(Color.FromArgb(236, 216, 164)))
            {
                g.FillRectangle(hBrush, headerBar);
            }

            using (Pen borderPen = new Pen(clrDivider, 1.2f))
            {
                g.DrawLine(borderPen, 18, 108, cardW - 18, 108);
                g.DrawLine(borderPen, 18, cardH - 48, cardW - 18, cardH - 48);
            }

            // Header Texts matching row layout
            int actW = 150;
            int actX = cardW - 18 - actW;
            int statW = 85;
            int statX = actX - statW - 10;
            int stockW = 50;
            int stockX = statX - stockW - 10;
            int priceW = 75;
            int priceX = stockX - priceW - 10;
            int catW = 135;
            int catX = priceX - catW - 10;
            int nameX = 106;
            int nameW = Math.Max(100, catX - nameX - 10);

            TextRenderer.DrawText(g, "SKU", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(26, 80, 75, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "ITEM NAME", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(nameX, 80, nameW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "CATEGORY", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(catX, 80, catW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "PRICE", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(priceX, 80, priceW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "STOCK", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(stockX, 80, stockW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "STATUS", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(statX, 80, statW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(g, "ACTIONS", new Font("Georgia", 7.5F, FontStyle.Bold),
                new Rectangle(actX, 80, actW, 28), clrMaroon, TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void NavButton_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Button btn) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrNavDark);

            bool isActive = (btn == activeNavButton);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);

            if (isActive)
            {
                using (GraphicsPath path = GetRoundedRectPath(rect, 10))
                using (SolidBrush fill = new SolidBrush(clrGold))
                {
                    g.FillPath(fill, path);
                }
            }

            Color txtColor = isActive ? clrTextDark : Color.FromArgb(238, 224, 195);
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, txtColor,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void NavButton_Click(object? sender, EventArgs e)
        {
            Button? clicked = sender as Button;
            if (clicked == null || clicked == btnNavInventory) return;

            if (clicked == btnNavDashboard)
            {
                SwitchForm(new AdminDB());
            }
            else if (clicked == btnNavTransactions)
            {
                SwitchForm(new AdminTransactions());
            }
            else if (clicked == btnNavReports)
            {
                SwitchForm(new AdminReports());
            }
            else if (clicked == btnNavSettings)
            {
                SwitchForm(new AdminSettings());
            }
        }

        private void BtnSignOut_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrNavDark);

            Rectangle rect = new Rectangle(1, 1, btnSignOut.Width - 3, btnSignOut.Height - 3);
            using (GraphicsPath path = GetRoundedRectPath(rect, 18))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.DrawPath(border, path);
            }

            TextRenderer.DrawText(g, btnSignOut.Text, btnSignOut.Font, rect, clrGold,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void BtnSignOut_Click(object? sender, EventArgs e)
        {
            DialogResult res = CoopMessageBox.Show(this, "Are you sure you want to sign out?", "Sign Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                if (CoopAppContext.Instance != null)
                {
                    CoopAppContext.Instance.SignOut();
                }
                else
                {
                    LoginPage login = new LoginPage();
                    login.Show();
                    this.Hide();
                }
            }
        }

        private void LayoutHeaderNav()
        {
            if (pnlTopNav == null || btnNavDashboard == null || btnNavInventory == null ||
                btnNavTransactions == null || btnNavReports == null || btnNavSettings == null)
                return;

            int totalNavWidth = btnNavDashboard.Width + 8 +
                                btnNavInventory.Width + 8 +
                                btnNavTransactions.Width + 8 +
                                btnNavReports.Width + 8 +
                                btnNavSettings.Width;

            int navStartX = (pnlTopNav.Width - totalNavWidth) / 2;
            int minStartX = (lblBrandTitle != null ? lblBrandTitle.Right : 290) + 28;

            if (navStartX < minStartX)
            {
                navStartX = minStartX;
            }

            int y = 19;
            btnNavDashboard.Location = new Point(navStartX, y);
            btnNavInventory.Location = new Point(btnNavDashboard.Right + 8, y);
            btnNavTransactions.Location = new Point(btnNavInventory.Right + 8, y);
            btnNavReports.Location = new Point(btnNavTransactions.Right + 8, y);
            btnNavSettings.Location = new Point(btnNavReports.Right + 8, y);
        }

        private void SwitchForm(Form targetForm)
        {
            if (CoopAppContext.Instance != null)
            {
                CoopAppContext.Instance.SwitchForm(targetForm);
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    targetForm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    targetForm.StartPosition = FormStartPosition.Manual;
                    targetForm.WindowState = FormWindowState.Normal;
                    targetForm.Location = this.Location;
                    targetForm.Size = this.Size;
                }
                targetForm.Show();
                this.Hide();
            }
        }

        private void PnlLogoIcon_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point[] shieldPoints =
            {
                new Point(19, 3),
                new Point(34, 8),
                new Point(33, 25),
                new Point(19, 38),
                new Point(5, 25),
                new Point(4, 8)
            };

            using (SolidBrush fill = new SolidBrush(Color.FromArgb(52, 16, 26)))
            using (Pen goldPen = new Pen(clrGold, 1.8f))
            {
                g.FillPolygon(fill, shieldPoints);
                g.DrawPolygon(goldPen, shieldPoints);
            }

            TextRenderer.DrawText(g, "U", new Font("Georgia", 13F, FontStyle.Bold),
                new Rectangle(0, 2, 38, 34), clrGold,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlNotif_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Outer circle
            Rectangle circle = new Rectangle(3, 5, 36, 36);
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(54, 16, 26)))
            using (Pen border = new Pen(Color.FromArgb(135, 102, 46), 1.2f))
            {
                g.FillEllipse(bg, circle);
                g.DrawEllipse(border, circle);
            }

            // Bell silhouette
            using (Pen bellPen = new Pen(clrGold, 1.5f))
            {
                g.DrawArc(bellPen, 16, 15, 10, 12, 180, 180);
                g.DrawLine(bellPen, 15, 26, 27, 26);
                g.DrawArc(bellPen, 19, 25, 4, 4, 0, 180);
            }

            // Red badge "2"
            Rectangle badge = new Rectangle(26, 2, 17, 17);
            using (SolidBrush redBrush = new SolidBrush(Color.FromArgb(196, 42, 54)))
            {
                g.FillEllipse(redBrush, badge);
            }
            TextRenderer.DrawText(g, "2", new Font("Segoe UI", 7.5F, FontStyle.Bold),
                badge, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlAvatar_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(1, 1, 37, 37);
            using (SolidBrush goldBrush = new SolidBrush(clrGold))
            {
                g.FillEllipse(goldBrush, rect);
            }

            TextRenderer.DrawText(g, "AD", new Font("Georgia", 10.5F, FontStyle.Bold),
                rect, clrTextDark, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawParchmentCard(Graphics g, Rectangle bounds, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, bounds.Width - 1, bounds.Height - 1);

            // Illuminated Parchment Gradient
            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                rect,
                Color.FromArgb(252, 244, 222),
                Color.FromArgb(238, 220, 180),
                LinearGradientMode.Vertical))
            {
                using (GraphicsPath path = GetRoundedRectPath(rect, radius))
                {
                    g.FillPath(bgBrush, path);
                    using (Pen borderPen = new Pen(clrGoldBorder, 1.8f))
                    {
                        g.DrawPath(borderPen, path);
                    }
                }
            }

            // Inset Fine Gold Filigree Border & Corner Diamonds
            if (bounds.Width > 50 && bounds.Height > 50)
            {
                Rectangle innerRect = new Rectangle(4, 4, bounds.Width - 9, bounds.Height - 9);
                using (GraphicsPath innerPath = GetRoundedRectPath(innerRect, Math.Max(2, radius - 3)))
                using (Pen innerPen = new Pen(Color.FromArgb(215, 180, 100), 1f))
                {
                    g.DrawPath(innerPen, innerPath);
                }

                DrawCornerDiamond(g, 9, 9);
                DrawCornerDiamond(g, bounds.Width - 10, 9);
                DrawCornerDiamond(g, 9, bounds.Height - 10);
                DrawCornerDiamond(g, bounds.Width - 10, bounds.Height - 10);
            }
        }

        private void DrawCornerDiamond(Graphics g, int cx, int cy)
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(196, 152, 54)))
            {
                Point[] pts = {
                    new Point(cx, cy - 3),
                    new Point(cx + 3, cy),
                    new Point(cx, cy + 3),
                    new Point(cx - 3, cy)
                };
                g.FillPolygon(b, pts);
            }
        }

        private void DrawKpiCircleIcon(Graphics g, Rectangle bounds, string iconType)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circle = new Rectangle(2, 2, bounds.Width - 5, bounds.Height - 5);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(46, 14, 22)))
            using (Pen goldPen = new Pen(clrGoldBorder, 1.5f))
            {
                g.FillEllipse(darkBrush, circle);
                g.DrawEllipse(goldPen, circle);
            }

            using (Pen iconPen = new Pen(clrGold, 1.8f))
            {
                if (iconType == "products")
                {
                    // Box icon
                    g.DrawRectangle(iconPen, 16, 17, 16, 15);
                    g.DrawLine(iconPen, 16, 22, 32, 22);
                    g.DrawLine(iconPen, 24, 22, 24, 32);
                }
                else if (iconType == "stock")
                {
                    // Stacked layers
                    g.DrawLine(iconPen, 16, 18, 32, 18);
                    g.DrawLine(iconPen, 16, 24, 32, 24);
                    g.DrawLine(iconPen, 16, 30, 32, 30);
                }
                else if (iconType == "warning")
                {
                    // Triangle with !
                    Point[] pts = { new Point(24, 15), new Point(15, 33), new Point(33, 33) };
                    g.DrawPolygon(iconPen, pts);
                    TextRenderer.DrawText(g, "!", new Font("Segoe UI", 8F, FontStyle.Bold),
                        new Rectangle(15, 18, 18, 14), clrGold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else if (iconType == "out")
                {
                    // Circle with X
                    g.DrawEllipse(iconPen, 15, 15, 18, 18);
                    g.DrawLine(iconPen, 20, 20, 28, 28);
                    g.DrawLine(iconPen, 28, 20, 20, 28);
                }
            }
        }

        private void DrawFilterPill(Graphics g, Button btn, bool isSelected)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(clrParchment);
            Rectangle rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);

            using (GraphicsPath path = GetRoundedRectPath(rect, 10))
            {
                if (isSelected)
                {
                    using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(35, 8, 16)))
                    {
                        g.FillPath(darkBrush, path);
                    }
                    TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrParchment,
                        TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                else
                {
                    using (SolidBrush bgBrush = new SolidBrush(clrParchment))
                    using (Pen borderPen = new Pen(Color.FromArgb(170, 138, 70), 1f))
                    {
                        g.FillPath(bgBrush, path);
                        g.DrawPath(borderPen, path);
                    }
                    TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                        TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private void DrawGoldPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush brush = new SolidBrush(clrGold))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.FillPath(brush, path);
                g.DrawPath(border, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawMaroonPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush brush = new SolidBrush(clrMaroon))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.FillPath(brush, path);
                g.DrawPath(border, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrParchment,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawCrimsonPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush brush = new SolidBrush(clrCrimson))
            {
                g.FillPath(brush, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, Color.White,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawParchmentPillButton(Graphics g, Button btn, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(btn.Parent?.BackColor ?? clrParchment);
            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            using (GraphicsPath path = GetRoundedRectPath(rect, radius))
            using (SolidBrush bgBrush = new SolidBrush(clrParchment))
            using (Pen border = new Pen(clrGoldBorder, 1.2f))
            {
                g.FillPath(bgBrush, path);
                g.DrawPath(border, path);
            }
            TextRenderer.DrawText(g, btn.Text, btn.Font, rect, clrTextDark,
                TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
