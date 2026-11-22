namespace Login
{
    partial class AdminInventory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTopNav = new System.Windows.Forms.Panel();
            this.btnSignOut = new System.Windows.Forms.Button();
            this.lblAdminRole = new System.Windows.Forms.Label();
            this.lblAdminName = new System.Windows.Forms.Label();
            this.pnlAvatar = new System.Windows.Forms.Panel();
            this.pnlNotif = new System.Windows.Forms.Panel();
            this.btnNavSettings = new System.Windows.Forms.Button();
            this.btnNavReports = new System.Windows.Forms.Button();
            this.btnNavTransactions = new System.Windows.Forms.Button();
            this.btnNavInventory = new System.Windows.Forms.Button();
            this.btnNavDashboard = new System.Windows.Forms.Button();
            this.lblBrandSub = new System.Windows.Forms.Label();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.pnlLogoIcon = new System.Windows.Forms.Panel();

            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblPageSub = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();

            this.pnlCardTotalProducts = new System.Windows.Forms.Panel();
            this.lblTotalProdSub = new System.Windows.Forms.Label();
            this.lblTotalProdVal = new System.Windows.Forms.Label();
            this.lblTotalProdTitle = new System.Windows.Forms.Label();
            this.pnlIconTotalProd = new System.Windows.Forms.Panel();

            this.pnlCardTotalUnits = new System.Windows.Forms.Panel();
            this.lblTotalUnitsSub = new System.Windows.Forms.Label();
            this.lblTotalUnitsVal = new System.Windows.Forms.Label();
            this.lblTotalUnitsTitle = new System.Windows.Forms.Label();
            this.pnlIconTotalUnits = new System.Windows.Forms.Panel();

            this.pnlCardLowStock = new System.Windows.Forms.Panel();
            this.lblLowStockSub = new System.Windows.Forms.Label();
            this.lblLowStockVal = new System.Windows.Forms.Label();
            this.lblLowStockTitle = new System.Windows.Forms.Label();
            this.pnlIconLowStock = new System.Windows.Forms.Panel();

            this.pnlCardOutStock = new System.Windows.Forms.Panel();
            this.lblOutStockSub = new System.Windows.Forms.Label();
            this.lblOutStockVal = new System.Windows.Forms.Label();
            this.lblOutStockTitle = new System.Windows.Forms.Label();
            this.pnlIconOutStock = new System.Windows.Forms.Panel();

            this.pnlCatalogCard = new System.Windows.Forms.Panel();
            this.lblCatalogTitle = new System.Windows.Forms.Label();
            this.pnlSearchBox = new System.Windows.Forms.Panel();
            this.txtSearchItem = new System.Windows.Forms.TextBox();
            this.btnFilterAll = new System.Windows.Forms.Button();
            this.btnFilterInStock = new System.Windows.Forms.Button();
            this.btnFilterLowStock = new System.Windows.Forms.Button();
            this.btnFilterOutStock = new System.Windows.Forms.Button();
            this.cmbFilterCategory = new System.Windows.Forms.ComboBox();
            this.btnAddItemModal = new System.Windows.Forms.Button();
            this.pnlItemList = new System.Windows.Forms.Panel();
            this.lblCatalogShowing = new System.Windows.Forms.Label();
            this.btnPagePrev = new System.Windows.Forms.Button();
            this.lblPageNum = new System.Windows.Forms.Label();
            this.btnPageNext = new System.Windows.Forms.Button();

            this.pnlDetailCard = new System.Windows.Forms.Panel();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.lblSku = new System.Windows.Forms.Label();
            this.txtSku = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblReorder = new System.Windows.Forms.Label();
            this.txtReorder = new System.Windows.Forms.TextBox();
            this.btnSaveItem = new System.Windows.Forms.Button();
            this.btnQuickRestock = new System.Windows.Forms.Button();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnClearFields = new System.Windows.Forms.Button();

            this.pnlTopNav.SuspendLayout();
            this.pnlCardTotalProducts.SuspendLayout();
            this.pnlCardTotalUnits.SuspendLayout();
            this.pnlCardLowStock.SuspendLayout();
            this.pnlCardOutStock.SuspendLayout();
            this.pnlCatalogCard.SuspendLayout();
            this.pnlSearchBox.SuspendLayout();
            this.pnlDetailCard.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTopNav
            // 
            this.pnlTopNav.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.pnlTopNav.Controls.Add(this.btnSignOut);
            this.pnlTopNav.Controls.Add(this.lblAdminRole);
            this.pnlTopNav.Controls.Add(this.lblAdminName);
            this.pnlTopNav.Controls.Add(this.pnlAvatar);
            this.pnlTopNav.Controls.Add(this.pnlNotif);
            this.pnlTopNav.Controls.Add(this.btnNavSettings);
            this.pnlTopNav.Controls.Add(this.btnNavReports);
            this.pnlTopNav.Controls.Add(this.btnNavTransactions);
            this.pnlTopNav.Controls.Add(this.btnNavInventory);
            this.pnlTopNav.Controls.Add(this.btnNavDashboard);
            this.pnlTopNav.Controls.Add(this.lblBrandSub);
            this.pnlTopNav.Controls.Add(this.lblBrandTitle);
            this.pnlTopNav.Controls.Add(this.pnlLogoIcon);
            this.pnlTopNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopNav.Location = new System.Drawing.Point(0, 0);
            this.pnlTopNav.Name = "pnlTopNav";
            this.pnlTopNav.Size = new System.Drawing.Size(1280, 74);
            this.pnlTopNav.TabIndex = 0;

            // pnlLogoIcon
            this.pnlLogoIcon.BackColor = System.Drawing.Color.Transparent;
            this.pnlLogoIcon.Location = new System.Drawing.Point(20, 16);
            this.pnlLogoIcon.Name = "pnlLogoIcon";
            this.pnlLogoIcon.Size = new System.Drawing.Size(38, 42);
            this.pnlLogoIcon.TabIndex = 0;

            // lblBrandTitle
            this.lblBrandTitle.AutoSize = true;
            this.lblBrandTitle.Font = new System.Drawing.Font("Georgia", 9.25F, System.Drawing.FontStyle.Bold);
            this.lblBrandTitle.ForeColor = System.Drawing.Color.FromArgb(230, 184, 64);
            this.lblBrandTitle.Location = new System.Drawing.Point(64, 19);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(226, 16);
            this.lblBrandTitle.TabIndex = 1;
            this.lblBrandTitle.Text = "UNIVERSITY OF MAKATI COOP";

            // lblBrandSub
            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(216, 199, 168);
            this.lblBrandSub.Location = new System.Drawing.Point(65, 39);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.Size = new System.Drawing.Size(140, 14);
            this.lblBrandSub.TabIndex = 2;
            this.lblBrandSub.Text = "Bookstore · Admin office";

            // btnNavDashboard
            this.btnNavDashboard.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnNavDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavDashboard.FlatAppearance.BorderSize = 0;
            this.btnNavDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavDashboard.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNavDashboard.ForeColor = System.Drawing.Color.FromArgb(238, 224, 195);
            this.btnNavDashboard.Location = new System.Drawing.Point(330, 19);
            this.btnNavDashboard.Name = "btnNavDashboard";
            this.btnNavDashboard.Padding = System.Windows.Forms.Padding.Empty;
            this.btnNavDashboard.Size = new System.Drawing.Size(120, 36);
            this.btnNavDashboard.TabIndex = 3;
            this.btnNavDashboard.Text = "DASHBOARD";
            this.btnNavDashboard.UseMnemonic = false;
            this.btnNavDashboard.UseVisualStyleBackColor = false;

            // btnNavInventory
            this.btnNavInventory.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnNavInventory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavInventory.FlatAppearance.BorderSize = 0;
            this.btnNavInventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavInventory.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNavInventory.ForeColor = System.Drawing.Color.FromArgb(42, 10, 20);
            this.btnNavInventory.Location = new System.Drawing.Point(454, 19);
            this.btnNavInventory.Name = "btnNavInventory";
            this.btnNavInventory.Padding = System.Windows.Forms.Padding.Empty;
            this.btnNavInventory.Size = new System.Drawing.Size(110, 36);
            this.btnNavInventory.TabIndex = 4;
            this.btnNavInventory.Text = "INVENTORY";
            this.btnNavInventory.UseMnemonic = false;
            this.btnNavInventory.UseVisualStyleBackColor = false;

            // btnNavTransactions
            this.btnNavTransactions.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnNavTransactions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavTransactions.FlatAppearance.BorderSize = 0;
            this.btnNavTransactions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavTransactions.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNavTransactions.ForeColor = System.Drawing.Color.FromArgb(238, 224, 195);
            this.btnNavTransactions.Location = new System.Drawing.Point(568, 19);
            this.btnNavTransactions.Name = "btnNavTransactions";
            this.btnNavTransactions.Padding = System.Windows.Forms.Padding.Empty;
            this.btnNavTransactions.Size = new System.Drawing.Size(140, 36);
            this.btnNavTransactions.TabIndex = 5;
            this.btnNavTransactions.Text = "TRANSACTIONS";
            this.btnNavTransactions.UseMnemonic = false;
            this.btnNavTransactions.UseVisualStyleBackColor = false;

            // btnNavReports
            this.btnNavReports.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnNavReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavReports.FlatAppearance.BorderSize = 0;
            this.btnNavReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavReports.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNavReports.ForeColor = System.Drawing.Color.FromArgb(238, 224, 195);
            this.btnNavReports.Location = new System.Drawing.Point(712, 19);
            this.btnNavReports.Name = "btnNavReports";
            this.btnNavReports.Padding = System.Windows.Forms.Padding.Empty;
            this.btnNavReports.Size = new System.Drawing.Size(96, 36);
            this.btnNavReports.TabIndex = 6;
            this.btnNavReports.Text = "REPORTS";
            this.btnNavReports.UseMnemonic = false;
            this.btnNavReports.UseVisualStyleBackColor = false;

            // btnNavSettings
            this.btnNavSettings.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnNavSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSettings.FlatAppearance.BorderSize = 0;
            this.btnNavSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSettings.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnNavSettings.ForeColor = System.Drawing.Color.FromArgb(238, 224, 195);
            this.btnNavSettings.Location = new System.Drawing.Point(812, 19);
            this.btnNavSettings.Name = "btnNavSettings";
            this.btnNavSettings.Padding = System.Windows.Forms.Padding.Empty;
            this.btnNavSettings.Size = new System.Drawing.Size(96, 36);
            this.btnNavSettings.TabIndex = 7;
            this.btnNavSettings.Text = "SETTINGS";
            this.btnNavSettings.UseMnemonic = false;
            this.btnNavSettings.UseVisualStyleBackColor = false;

            // pnlNotif
            this.pnlNotif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNotif.BackColor = System.Drawing.Color.Transparent;
            this.pnlNotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlNotif.Location = new System.Drawing.Point(922, 14);
            this.pnlNotif.Name = "pnlNotif";
            this.pnlNotif.Size = new System.Drawing.Size(46, 46);
            this.pnlNotif.TabIndex = 8;

            // pnlAvatar
            this.pnlAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAvatar.BackColor = System.Drawing.Color.Transparent;
            this.pnlAvatar.Location = new System.Drawing.Point(976, 17);
            this.pnlAvatar.Name = "pnlAvatar";
            this.pnlAvatar.Size = new System.Drawing.Size(40, 40);
            this.pnlAvatar.TabIndex = 9;

            // lblAdminName
            this.lblAdminName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdminName.AutoSize = true;
            this.lblAdminName.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Bold);
            this.lblAdminName.ForeColor = System.Drawing.Color.FromArgb(245, 232, 200);
            this.lblAdminName.Location = new System.Drawing.Point(1024, 20);
            this.lblAdminName.Name = "lblAdminName";
            this.lblAdminName.Size = new System.Drawing.Size(55, 15);
            this.lblAdminName.TabIndex = 10;
            this.lblAdminName.Text = "ADMIN";

            // lblAdminRole
            this.lblAdminRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdminRole.AutoSize = true;
            this.lblAdminRole.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Italic);
            this.lblAdminRole.ForeColor = System.Drawing.Color.FromArgb(201, 180, 143);
            this.lblAdminRole.Location = new System.Drawing.Point(1024, 38);
            this.lblAdminRole.Name = "lblAdminRole";
            this.lblAdminRole.Size = new System.Drawing.Size(82, 14);
            this.lblAdminRole.TabIndex = 11;
            this.lblAdminRole.Text = "Administrator";

            // btnSignOut
            this.btnSignOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignOut.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnSignOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSignOut.FlatAppearance.BorderSize = 0;
            this.btnSignOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignOut.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.btnSignOut.ForeColor = System.Drawing.Color.FromArgb(230, 184, 64);
            this.btnSignOut.Location = new System.Drawing.Point(1148, 19);
            this.btnSignOut.Name = "btnSignOut";
            this.btnSignOut.Size = new System.Drawing.Size(108, 36);
            this.btnSignOut.TabIndex = 12;
            this.btnSignOut.Text = "SIGN OUT";
            this.btnSignOut.UseVisualStyleBackColor = false;

            // 
            // Header Titles
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPageTitle.Font = new System.Drawing.Font("Georgia", 17F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.lblPageTitle.Location = new System.Drawing.Point(26, 88);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(342, 27);
            this.lblPageTitle.TabIndex = 1;
            this.lblPageTitle.Text = "INVENTORY MANAGEMENT";

            this.lblPageSub.AutoSize = true;
            this.lblPageSub.BackColor = System.Drawing.Color.Transparent;
            this.lblPageSub.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Italic);
            this.lblPageSub.ForeColor = System.Drawing.Color.FromArgb(200, 180, 155);
            this.lblPageSub.Location = new System.Drawing.Point(28, 117);
            this.lblPageSub.Name = "lblPageSub";
            this.lblPageSub.Size = new System.Drawing.Size(434, 15);
            this.lblPageSub.TabIndex = 2;
            this.lblPageSub.Text = "Track stock levels, add new bookstore items, update prices, and restock shelves.";

            this.lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(229, 181, 58);
            this.lblDate.Location = new System.Drawing.Point(955, 93);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(301, 22);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "SUNDAY · NOV 22, 2026";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // 
            // pnlCardTotalProducts
            // 
            this.pnlCardTotalProducts.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlCardTotalProducts.Controls.Add(this.lblTotalProdSub);
            this.pnlCardTotalProducts.Controls.Add(this.lblTotalProdVal);
            this.pnlCardTotalProducts.Controls.Add(this.lblTotalProdTitle);
            this.pnlCardTotalProducts.Controls.Add(this.pnlIconTotalProd);
            this.pnlCardTotalProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardTotalProducts.Location = new System.Drawing.Point(28, 144);
            this.pnlCardTotalProducts.Name = "pnlCardTotalProducts";
            this.pnlCardTotalProducts.Size = new System.Drawing.Size(290, 88);
            this.pnlCardTotalProducts.TabIndex = 4;

            this.pnlIconTotalProd.BackColor = System.Drawing.Color.Transparent;
            this.pnlIconTotalProd.Location = new System.Drawing.Point(16, 18);
            this.pnlIconTotalProd.Name = "pnlIconTotalProd";
            this.pnlIconTotalProd.Size = new System.Drawing.Size(50, 50);
            this.pnlIconTotalProd.TabIndex = 0;

            this.lblTotalProdTitle.AutoSize = true;
            this.lblTotalProdTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalProdTitle.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotalProdTitle.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblTotalProdTitle.Location = new System.Drawing.Point(72, 16);
            this.lblTotalProdTitle.Name = "lblTotalProdTitle";
            this.lblTotalProdTitle.Size = new System.Drawing.Size(126, 14);
            this.lblTotalProdTitle.TabIndex = 1;
            this.lblTotalProdTitle.Text = "TOTAL PRODUCTS";

            this.lblTotalProdVal.AutoSize = true;
            this.lblTotalProdVal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalProdVal.Font = new System.Drawing.Font("Georgia", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalProdVal.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.lblTotalProdVal.Location = new System.Drawing.Point(70, 34);
            this.lblTotalProdVal.Name = "lblTotalProdVal";
            this.lblTotalProdVal.Size = new System.Drawing.Size(117, 27);
            this.lblTotalProdVal.TabIndex = 2;
            this.lblTotalProdVal.Text = "12 ITEMS";

            this.lblTotalProdSub.AutoSize = true;
            this.lblTotalProdSub.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalProdSub.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblTotalProdSub.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblTotalProdSub.Location = new System.Drawing.Point(72, 63);
            this.lblTotalProdSub.Name = "lblTotalProdSub";
            this.lblTotalProdSub.Size = new System.Drawing.Size(107, 14);
            this.lblTotalProdSub.TabIndex = 3;
            this.lblTotalProdSub.Text = "Active coop catalog";

            // 
            // pnlCardTotalUnits
            // 
            this.pnlCardTotalUnits.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlCardTotalUnits.Controls.Add(this.lblTotalUnitsSub);
            this.pnlCardTotalUnits.Controls.Add(this.lblTotalUnitsVal);
            this.pnlCardTotalUnits.Controls.Add(this.lblTotalUnitsTitle);
            this.pnlCardTotalUnits.Controls.Add(this.pnlIconTotalUnits);
            this.pnlCardTotalUnits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardTotalUnits.Location = new System.Drawing.Point(338, 144);
            this.pnlCardTotalUnits.Name = "pnlCardTotalUnits";
            this.pnlCardTotalUnits.Size = new System.Drawing.Size(290, 88);
            this.pnlCardTotalUnits.TabIndex = 5;

            this.pnlIconTotalUnits.BackColor = System.Drawing.Color.Transparent;
            this.pnlIconTotalUnits.Location = new System.Drawing.Point(16, 18);
            this.pnlIconTotalUnits.Name = "pnlIconTotalUnits";
            this.pnlIconTotalUnits.Size = new System.Drawing.Size(50, 50);
            this.pnlIconTotalUnits.TabIndex = 0;

            this.lblTotalUnitsTitle.AutoSize = true;
            this.lblTotalUnitsTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalUnitsTitle.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotalUnitsTitle.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblTotalUnitsTitle.Location = new System.Drawing.Point(72, 16);
            this.lblTotalUnitsTitle.Name = "lblTotalUnitsTitle";
            this.lblTotalUnitsTitle.Size = new System.Drawing.Size(142, 14);
            this.lblTotalUnitsTitle.TabIndex = 1;
            this.lblTotalUnitsTitle.Text = "TOTAL STOCK UNITS";

            this.lblTotalUnitsVal.AutoSize = true;
            this.lblTotalUnitsVal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalUnitsVal.Font = new System.Drawing.Font("Georgia", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotalUnitsVal.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.lblTotalUnitsVal.Location = new System.Drawing.Point(70, 34);
            this.lblTotalUnitsVal.Name = "lblTotalUnitsVal";
            this.lblTotalUnitsVal.Size = new System.Drawing.Size(155, 27);
            this.lblTotalUnitsVal.TabIndex = 2;
            this.lblTotalUnitsVal.Text = "1,420 UNITS";

            this.lblTotalUnitsSub.AutoSize = true;
            this.lblTotalUnitsSub.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalUnitsSub.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblTotalUnitsSub.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblTotalUnitsSub.Location = new System.Drawing.Point(72, 63);
            this.lblTotalUnitsSub.Name = "lblTotalUnitsSub";
            this.lblTotalUnitsSub.Size = new System.Drawing.Size(141, 14);
            this.lblTotalUnitsSub.TabIndex = 3;
            this.lblTotalUnitsSub.Text = "Count across bookstore";

            // 
            // pnlCardLowStock
            // 
            this.pnlCardLowStock.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlCardLowStock.Controls.Add(this.lblLowStockSub);
            this.pnlCardLowStock.Controls.Add(this.lblLowStockVal);
            this.pnlCardLowStock.Controls.Add(this.lblLowStockTitle);
            this.pnlCardLowStock.Controls.Add(this.pnlIconLowStock);
            this.pnlCardLowStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardLowStock.Location = new System.Drawing.Point(648, 144);
            this.pnlCardLowStock.Name = "pnlCardLowStock";
            this.pnlCardLowStock.Size = new System.Drawing.Size(290, 88);
            this.pnlCardLowStock.TabIndex = 6;

            this.pnlIconLowStock.BackColor = System.Drawing.Color.Transparent;
            this.pnlIconLowStock.Location = new System.Drawing.Point(16, 18);
            this.pnlIconLowStock.Name = "pnlIconLowStock";
            this.pnlIconLowStock.Size = new System.Drawing.Size(50, 50);
            this.pnlIconLowStock.TabIndex = 0;

            this.lblLowStockTitle.AutoSize = true;
            this.lblLowStockTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLowStockTitle.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblLowStockTitle.Location = new System.Drawing.Point(72, 16);
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            this.lblLowStockTitle.Size = new System.Drawing.Size(130, 14);
            this.lblLowStockTitle.TabIndex = 1;
            this.lblLowStockTitle.Text = "LOW STOCK ITEMS";

            this.lblLowStockVal.AutoSize = true;
            this.lblLowStockVal.BackColor = System.Drawing.Color.Transparent;
            this.lblLowStockVal.Font = new System.Drawing.Font("Georgia", 16F, System.Drawing.FontStyle.Bold);
            this.lblLowStockVal.ForeColor = System.Drawing.Color.FromArgb(158, 34, 38);
            this.lblLowStockVal.Location = new System.Drawing.Point(70, 34);
            this.lblLowStockVal.Name = "lblLowStockVal";
            this.lblLowStockVal.Size = new System.Drawing.Size(107, 27);
            this.lblLowStockVal.TabIndex = 2;
            this.lblLowStockVal.Text = "4 ITEMS";

            this.lblLowStockSub.AutoSize = true;
            this.lblLowStockSub.BackColor = System.Drawing.Color.Transparent;
            this.lblLowStockSub.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblLowStockSub.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblLowStockSub.Location = new System.Drawing.Point(72, 63);
            this.lblLowStockSub.Name = "lblLowStockSub";
            this.lblLowStockSub.Size = new System.Drawing.Size(134, 14);
            this.lblLowStockSub.TabIndex = 3;
            this.lblLowStockSub.Text = "At or below reorder level";

            // 
            // pnlCardOutStock
            // 
            this.pnlCardOutStock.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlCardOutStock.Controls.Add(this.lblOutStockSub);
            this.pnlCardOutStock.Controls.Add(this.lblOutStockVal);
            this.pnlCardOutStock.Controls.Add(this.lblOutStockTitle);
            this.pnlCardOutStock.Controls.Add(this.pnlIconOutStock);
            this.pnlCardOutStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardOutStock.Location = new System.Drawing.Point(958, 144);
            this.pnlCardOutStock.Name = "pnlCardOutStock";
            this.pnlCardOutStock.Size = new System.Drawing.Size(298, 88);
            this.pnlCardOutStock.TabIndex = 7;

            this.pnlIconOutStock.BackColor = System.Drawing.Color.Transparent;
            this.pnlIconOutStock.Location = new System.Drawing.Point(16, 18);
            this.pnlIconOutStock.Name = "pnlIconOutStock";
            this.pnlIconOutStock.Size = new System.Drawing.Size(50, 50);
            this.pnlIconOutStock.TabIndex = 0;

            this.lblOutStockTitle.AutoSize = true;
            this.lblOutStockTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblOutStockTitle.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblOutStockTitle.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblOutStockTitle.Location = new System.Drawing.Point(72, 16);
            this.lblOutStockTitle.Name = "lblOutStockTitle";
            this.lblOutStockTitle.Size = new System.Drawing.Size(107, 14);
            this.lblOutStockTitle.TabIndex = 1;
            this.lblOutStockTitle.Text = "OUT OF STOCK";

            this.lblOutStockVal.AutoSize = true;
            this.lblOutStockVal.BackColor = System.Drawing.Color.Transparent;
            this.lblOutStockVal.Font = new System.Drawing.Font("Georgia", 16F, System.Drawing.FontStyle.Bold);
            this.lblOutStockVal.ForeColor = System.Drawing.Color.FromArgb(158, 34, 38);
            this.lblOutStockVal.Location = new System.Drawing.Point(70, 34);
            this.lblOutStockVal.Name = "lblOutStockVal";
            this.lblOutStockVal.Size = new System.Drawing.Size(107, 27);
            this.lblOutStockVal.TabIndex = 2;
            this.lblOutStockVal.Text = "3 ITEMS";

            this.lblOutStockSub.AutoSize = true;
            this.lblOutStockSub.BackColor = System.Drawing.Color.Transparent;
            this.lblOutStockSub.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblOutStockSub.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblOutStockSub.Location = new System.Drawing.Point(72, 63);
            this.lblOutStockSub.Name = "lblOutStockSub";
            this.lblOutStockSub.Size = new System.Drawing.Size(147, 14);
            this.lblOutStockSub.TabIndex = 3;
            this.lblOutStockSub.Text = "Cannot be sold right now";

            // 
            // pnlCatalogCard (Left Table)
            // 
            this.pnlCatalogCard.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlCatalogCard.Controls.Add(this.btnPageNext);
            this.pnlCatalogCard.Controls.Add(this.lblPageNum);
            this.pnlCatalogCard.Controls.Add(this.btnPagePrev);
            this.pnlCatalogCard.Controls.Add(this.lblCatalogShowing);
            this.pnlCatalogCard.Controls.Add(this.pnlItemList);
            this.pnlCatalogCard.Controls.Add(this.btnAddItemModal);
            this.pnlCatalogCard.Controls.Add(this.cmbFilterCategory);
            this.pnlCatalogCard.Controls.Add(this.btnFilterOutStock);
            this.pnlCatalogCard.Controls.Add(this.btnFilterLowStock);
            this.pnlCatalogCard.Controls.Add(this.btnFilterInStock);
            this.pnlCatalogCard.Controls.Add(this.btnFilterAll);
            this.pnlCatalogCard.Controls.Add(this.pnlSearchBox);
            this.pnlCatalogCard.Controls.Add(this.lblCatalogTitle);
            this.pnlCatalogCard.Location = new System.Drawing.Point(28, 246);
            this.pnlCatalogCard.Name = "pnlCatalogCard";
            this.pnlCatalogCard.Size = new System.Drawing.Size(828, 452);
            this.pnlCatalogCard.TabIndex = 8;

            // lblCatalogTitle
            this.lblCatalogTitle.AutoSize = true;
            this.lblCatalogTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblCatalogTitle.Font = new System.Drawing.Font("Georgia", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblCatalogTitle.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.lblCatalogTitle.Location = new System.Drawing.Point(18, 14);
            this.lblCatalogTitle.Name = "lblCatalogTitle";
            this.lblCatalogTitle.Size = new System.Drawing.Size(189, 18);
            this.lblCatalogTitle.TabIndex = 0;
            this.lblCatalogTitle.Text = "INVENTORY CATALOG";

            // pnlSearchBox
            this.pnlSearchBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearchBox.Controls.Add(this.txtSearchItem);
            this.pnlSearchBox.Location = new System.Drawing.Point(18, 40);
            this.pnlSearchBox.Name = "pnlSearchBox";
            this.pnlSearchBox.Size = new System.Drawing.Size(175, 30);
            this.pnlSearchBox.TabIndex = 1;

            this.txtSearchItem.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtSearchItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearchItem.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.txtSearchItem.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.txtSearchItem.Location = new System.Drawing.Point(8, 7);
            this.txtSearchItem.Name = "txtSearchItem";
            this.txtSearchItem.Size = new System.Drawing.Size(155, 14);
            this.txtSearchItem.TabIndex = 0;
            this.txtSearchItem.Text = "Search SKU or name...";

            // Stock Status Filter Buttons
            this.btnFilterAll.BackColor = System.Drawing.Color.FromArgb(35, 8, 16);
            this.btnFilterAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterAll.FlatAppearance.BorderSize = 0;
            this.btnFilterAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterAll.Font = new System.Drawing.Font("Georgia", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnFilterAll.ForeColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.btnFilterAll.Location = new System.Drawing.Point(190, 40);
            this.btnFilterAll.Name = "btnFilterAll";
            this.btnFilterAll.Size = new System.Drawing.Size(48, 30);
            this.btnFilterAll.TabIndex = 2;
            this.btnFilterAll.Text = "ALL";
            this.btnFilterAll.UseVisualStyleBackColor = false;

            this.btnFilterInStock.BackColor = System.Drawing.Color.Transparent;
            this.btnFilterInStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterInStock.FlatAppearance.BorderSize = 0;
            this.btnFilterInStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterInStock.Font = new System.Drawing.Font("Georgia", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnFilterInStock.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnFilterInStock.Location = new System.Drawing.Point(242, 40);
            this.btnFilterInStock.Name = "btnFilterInStock";
            this.btnFilterInStock.Size = new System.Drawing.Size(80, 30);
            this.btnFilterInStock.TabIndex = 3;
            this.btnFilterInStock.Text = "IN STOCK";
            this.btnFilterInStock.UseVisualStyleBackColor = false;

            this.btnFilterLowStock.BackColor = System.Drawing.Color.Transparent;
            this.btnFilterLowStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterLowStock.FlatAppearance.BorderSize = 0;
            this.btnFilterLowStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterLowStock.Font = new System.Drawing.Font("Georgia", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnFilterLowStock.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnFilterLowStock.Location = new System.Drawing.Point(326, 40);
            this.btnFilterLowStock.Name = "btnFilterLowStock";
            this.btnFilterLowStock.Size = new System.Drawing.Size(84, 30);
            this.btnFilterLowStock.TabIndex = 4;
            this.btnFilterLowStock.Text = "LOW STOCK";
            this.btnFilterLowStock.UseVisualStyleBackColor = false;

            this.btnFilterOutStock.BackColor = System.Drawing.Color.Transparent;
            this.btnFilterOutStock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterOutStock.FlatAppearance.BorderSize = 0;
            this.btnFilterOutStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterOutStock.Font = new System.Drawing.Font("Georgia", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnFilterOutStock.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnFilterOutStock.Location = new System.Drawing.Point(414, 40);
            this.btnFilterOutStock.Name = "btnFilterOutStock";
            this.btnFilterOutStock.Size = new System.Drawing.Size(96, 30);
            this.btnFilterOutStock.TabIndex = 5;
            this.btnFilterOutStock.Text = "OUT OF STOCK";
            this.btnFilterOutStock.UseVisualStyleBackColor = false;

            // Category Filter Dropdown
            this.cmbFilterCategory.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.cmbFilterCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFilterCategory.Font = new System.Drawing.Font("Georgia", 8.25F);
            this.cmbFilterCategory.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.cmbFilterCategory.Items.AddRange(new object[] {
                "All Categories",
                "Books & Manuals",
                "Uniforms",
                "School Supplies"
            });
            this.cmbFilterCategory.Location = new System.Drawing.Point(516, 43);
            this.cmbFilterCategory.Name = "cmbFilterCategory";
            this.cmbFilterCategory.Size = new System.Drawing.Size(172, 22);
            this.cmbFilterCategory.TabIndex = 6;

            // btnAddItemModal
            this.btnAddItemModal.BackColor = System.Drawing.Color.FromArgb(229, 181, 58);
            this.btnAddItemModal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddItemModal.FlatAppearance.BorderSize = 0;
            this.btnAddItemModal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItemModal.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddItemModal.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnAddItemModal.Location = new System.Drawing.Point(694, 40);
            this.btnAddItemModal.Name = "btnAddItemModal";
            this.btnAddItemModal.Size = new System.Drawing.Size(116, 30);
            this.btnAddItemModal.TabIndex = 7;
            this.btnAddItemModal.Text = "+ ADD ITEM";
            this.btnAddItemModal.UseVisualStyleBackColor = false;

            // pnlItemList
            this.pnlItemList.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlItemList.Location = new System.Drawing.Point(18, 114);
            this.pnlItemList.Name = "pnlItemList";
            this.pnlItemList.Size = new System.Drawing.Size(792, 292);
            this.pnlItemList.TabIndex = 8;

            // Pagination
            this.lblCatalogShowing.AutoSize = true;
            this.lblCatalogShowing.BackColor = System.Drawing.Color.Transparent;
            this.lblCatalogShowing.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblCatalogShowing.ForeColor = System.Drawing.Color.FromArgb(124, 102, 78);
            this.lblCatalogShowing.Location = new System.Drawing.Point(18, 416);
            this.lblCatalogShowing.Name = "lblCatalogShowing";
            this.lblCatalogShowing.Size = new System.Drawing.Size(148, 14);
            this.lblCatalogShowing.TabIndex = 9;
            this.lblCatalogShowing.Text = "Showing 6 of 12 items";

            this.btnPagePrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPagePrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPagePrev.FlatAppearance.BorderSize = 0;
            this.btnPagePrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagePrev.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPagePrev.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnPagePrev.Location = new System.Drawing.Point(720, 412);
            this.btnPagePrev.Name = "btnPagePrev";
            this.btnPagePrev.Size = new System.Drawing.Size(24, 23);
            this.btnPagePrev.TabIndex = 10;
            this.btnPagePrev.Text = "<";
            this.btnPagePrev.UseVisualStyleBackColor = false;

            this.lblPageNum.BackColor = System.Drawing.Color.Transparent;
            this.lblPageNum.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPageNum.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.lblPageNum.Location = new System.Drawing.Point(744, 412);
            this.lblPageNum.Name = "lblPageNum";
            this.lblPageNum.Size = new System.Drawing.Size(38, 23);
            this.lblPageNum.TabIndex = 11;
            this.lblPageNum.Text = "1 / 2";
            this.lblPageNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnPageNext.BackColor = System.Drawing.Color.Transparent;
            this.btnPageNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPageNext.FlatAppearance.BorderSize = 0;
            this.btnPageNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPageNext.Font = new System.Drawing.Font("Georgia", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPageNext.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnPageNext.Location = new System.Drawing.Point(786, 412);
            this.btnPageNext.Name = "btnPageNext";
            this.btnPageNext.Size = new System.Drawing.Size(24, 23);
            this.btnPageNext.TabIndex = 12;
            this.btnPageNext.Text = ">";
            this.btnPageNext.UseVisualStyleBackColor = false;

            // 
            // pnlDetailCard (Right Details / Restock Form)
            // 
            this.pnlDetailCard.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.pnlDetailCard.Controls.Add(this.lblDetailTitle);
            this.pnlDetailCard.Controls.Add(this.lblSku);
            this.pnlDetailCard.Controls.Add(this.txtSku);
            this.pnlDetailCard.Controls.Add(this.lblName);
            this.pnlDetailCard.Controls.Add(this.txtName);
            this.pnlDetailCard.Controls.Add(this.lblCategory);
            this.pnlDetailCard.Controls.Add(this.cmbCategory);
            this.pnlDetailCard.Controls.Add(this.lblPrice);
            this.pnlDetailCard.Controls.Add(this.txtPrice);
            this.pnlDetailCard.Controls.Add(this.lblStock);
            this.pnlDetailCard.Controls.Add(this.txtStock);
            this.pnlDetailCard.Controls.Add(this.lblReorder);
            this.pnlDetailCard.Controls.Add(this.txtReorder);
            this.pnlDetailCard.Controls.Add(this.btnSaveItem);
            this.pnlDetailCard.Controls.Add(this.btnQuickRestock);
            this.pnlDetailCard.Controls.Add(this.btnDeleteItem);
            this.pnlDetailCard.Controls.Add(this.btnClearFields);
            this.pnlDetailCard.Location = new System.Drawing.Point(872, 246);
            this.pnlDetailCard.Name = "pnlDetailCard";
            this.pnlDetailCard.Size = new System.Drawing.Size(384, 452);
            this.pnlDetailCard.TabIndex = 9;

            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblDetailTitle.Font = new System.Drawing.Font("Georgia", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.lblDetailTitle.Location = new System.Drawing.Point(18, 14);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(217, 18);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "ITEM DETAILS & RESTOCK";

            // Sku
            this.lblSku.AutoSize = true;
            this.lblSku.BackColor = System.Drawing.Color.Transparent;
            this.lblSku.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblSku.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblSku.Location = new System.Drawing.Point(18, 48);
            this.lblSku.Name = "lblSku";
            this.lblSku.Size = new System.Drawing.Size(107, 14);
            this.lblSku.TabIndex = 1;
            this.lblSku.Text = "SKU / BARCODE";

            this.txtSku.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtSku.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSku.Font = new System.Drawing.Font("Georgia", 9F);
            this.txtSku.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.txtSku.Location = new System.Drawing.Point(18, 66);
            this.txtSku.Name = "txtSku";
            this.txtSku.Size = new System.Drawing.Size(348, 21);
            this.txtSku.TabIndex = 2;

            // Name
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblName.Location = new System.Drawing.Point(18, 96);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(99, 14);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "PRODUCT NAME";

            this.txtName.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Georgia", 9F);
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.txtName.Location = new System.Drawing.Point(18, 114);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(348, 21);
            this.txtName.TabIndex = 4;

            // Category
            this.lblCategory.AutoSize = true;
            this.lblCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblCategory.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblCategory.Location = new System.Drawing.Point(18, 144);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(73, 14);
            this.lblCategory.TabIndex = 5;
            this.lblCategory.Text = "CATEGORY";

            this.cmbCategory.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategory.Font = new System.Drawing.Font("Georgia", 9F);
            this.cmbCategory.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.cmbCategory.Items.AddRange(new object[] {
                "Books & Manuals",
                "Uniforms",
                "School Supplies"
            });
            this.cmbCategory.Location = new System.Drawing.Point(18, 162);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(348, 23);
            this.cmbCategory.TabIndex = 6;

            // Price, Stock, Reorder Row
            this.lblPrice.AutoSize = true;
            this.lblPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblPrice.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblPrice.Location = new System.Drawing.Point(18, 196);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(65, 14);
            this.lblPrice.TabIndex = 7;
            this.lblPrice.Text = "PRICE (₱)";

            this.txtPrice.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrice.Font = new System.Drawing.Font("Georgia", 9F);
            this.txtPrice.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.txtPrice.Location = new System.Drawing.Point(18, 214);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 21);
            this.txtPrice.TabIndex = 8;

            this.lblStock.AutoSize = true;
            this.lblStock.BackColor = System.Drawing.Color.Transparent;
            this.lblStock.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblStock.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblStock.Location = new System.Drawing.Point(142, 196);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(46, 14);
            this.lblStock.TabIndex = 9;
            this.lblStock.Text = "STOCK";

            this.txtStock.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStock.Font = new System.Drawing.Font("Georgia", 9F);
            this.txtStock.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.txtStock.Location = new System.Drawing.Point(142, 214);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(100, 21);
            this.txtStock.TabIndex = 10;

            this.lblReorder.AutoSize = true;
            this.lblReorder.BackColor = System.Drawing.Color.Transparent;
            this.lblReorder.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.lblReorder.ForeColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.lblReorder.Location = new System.Drawing.Point(266, 196);
            this.lblReorder.Name = "lblReorder";
            this.lblReorder.Size = new System.Drawing.Size(65, 14);
            this.lblReorder.TabIndex = 11;
            this.lblReorder.Text = "REORDER";

            this.txtReorder.BackColor = System.Drawing.Color.FromArgb(249, 241, 218);
            this.txtReorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReorder.Font = new System.Drawing.Font("Georgia", 9F);
            this.txtReorder.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.txtReorder.Location = new System.Drawing.Point(266, 214);
            this.txtReorder.Name = "txtReorder";
            this.txtReorder.Size = new System.Drawing.Size(100, 21);
            this.txtReorder.TabIndex = 12;

            // Buttons
            this.btnSaveItem.BackColor = System.Drawing.Color.FromArgb(88, 23, 38);
            this.btnSaveItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveItem.FlatAppearance.BorderSize = 0;
            this.btnSaveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveItem.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSaveItem.ForeColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.btnSaveItem.Location = new System.Drawing.Point(18, 256);
            this.btnSaveItem.Name = "btnSaveItem";
            this.btnSaveItem.Size = new System.Drawing.Size(348, 36);
            this.btnSaveItem.TabIndex = 13;
            this.btnSaveItem.Text = "SAVE / UPDATE ITEM";
            this.btnSaveItem.UseVisualStyleBackColor = false;

            this.btnQuickRestock.BackColor = System.Drawing.Color.FromArgb(229, 181, 58);
            this.btnQuickRestock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickRestock.FlatAppearance.BorderSize = 0;
            this.btnQuickRestock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickRestock.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnQuickRestock.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnQuickRestock.Location = new System.Drawing.Point(18, 302);
            this.btnQuickRestock.Name = "btnQuickRestock";
            this.btnQuickRestock.Size = new System.Drawing.Size(348, 34);
            this.btnQuickRestock.TabIndex = 14;
            this.btnQuickRestock.Text = "RESTOCK (+20 UNITS)";
            this.btnQuickRestock.UseVisualStyleBackColor = false;

            this.btnDeleteItem.BackColor = System.Drawing.Color.FromArgb(158, 34, 38);
            this.btnDeleteItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteItem.FlatAppearance.BorderSize = 0;
            this.btnDeleteItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteItem.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.btnDeleteItem.ForeColor = System.Drawing.Color.White;
            this.btnDeleteItem.Location = new System.Drawing.Point(18, 350);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(168, 32);
            this.btnDeleteItem.TabIndex = 15;
            this.btnDeleteItem.Text = "DELETE ITEM";
            this.btnDeleteItem.UseVisualStyleBackColor = false;

            this.btnClearFields.BackColor = System.Drawing.Color.FromArgb(244, 227, 178);
            this.btnClearFields.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearFields.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(196, 152, 54);
            this.btnClearFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFields.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Bold);
            this.btnClearFields.ForeColor = System.Drawing.Color.FromArgb(44, 14, 22);
            this.btnClearFields.Location = new System.Drawing.Point(198, 350);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(168, 32);
            this.btnClearFields.TabIndex = 16;
            this.btnClearFields.Text = "CLEAR FORM";
            this.btnClearFields.UseVisualStyleBackColor = false;

            // -------------------------------------------------------------
            // AdminInventory (Form)
            // -------------------------------------------------------------
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(42, 10, 20);
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlDetailCard);
            this.Controls.Add(this.pnlCatalogCard);
            this.Controls.Add(this.pnlCardOutStock);
            this.Controls.Add(this.pnlCardLowStock);
            this.Controls.Add(this.pnlCardTotalUnits);
            this.Controls.Add(this.pnlCardTotalProducts);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblPageSub);
            this.Controls.Add(this.lblPageTitle);
            this.Controls.Add(this.pnlTopNav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "AdminInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "University of Makati Coop - Admin Inventory";

            this.pnlTopNav.ResumeLayout(false);
            this.pnlTopNav.PerformLayout();
            this.pnlCardTotalProducts.ResumeLayout(false);
            this.pnlCardTotalProducts.PerformLayout();
            this.pnlCardTotalUnits.ResumeLayout(false);
            this.pnlCardTotalUnits.PerformLayout();
            this.pnlCardLowStock.ResumeLayout(false);
            this.pnlCardLowStock.PerformLayout();
            this.pnlCardOutStock.ResumeLayout(false);
            this.pnlCardOutStock.PerformLayout();
            this.pnlCatalogCard.ResumeLayout(false);
            this.pnlCatalogCard.PerformLayout();
            this.pnlSearchBox.ResumeLayout(false);
            this.pnlSearchBox.PerformLayout();
            this.pnlDetailCard.ResumeLayout(false);
            this.pnlDetailCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopNav;
        private System.Windows.Forms.Panel pnlLogoIcon;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblBrandSub;
        private System.Windows.Forms.Button btnNavDashboard;
        private System.Windows.Forms.Button btnNavInventory;
        private System.Windows.Forms.Button btnNavTransactions;
        private System.Windows.Forms.Button btnNavReports;
        private System.Windows.Forms.Button btnNavSettings;
        private System.Windows.Forms.Panel pnlNotif;
        private System.Windows.Forms.Panel pnlAvatar;
        private System.Windows.Forms.Label lblAdminName;
        private System.Windows.Forms.Label lblAdminRole;
        private System.Windows.Forms.Button btnSignOut;

        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageSub;
        private System.Windows.Forms.Label lblDate;

        private System.Windows.Forms.Panel pnlCardTotalProducts;
        private System.Windows.Forms.Panel pnlIconTotalProd;
        private System.Windows.Forms.Label lblTotalProdTitle;
        private System.Windows.Forms.Label lblTotalProdVal;
        private System.Windows.Forms.Label lblTotalProdSub;

        private System.Windows.Forms.Panel pnlCardTotalUnits;
        private System.Windows.Forms.Panel pnlIconTotalUnits;
        private System.Windows.Forms.Label lblTotalUnitsTitle;
        private System.Windows.Forms.Label lblTotalUnitsVal;
        private System.Windows.Forms.Label lblTotalUnitsSub;

        private System.Windows.Forms.Panel pnlCardLowStock;
        private System.Windows.Forms.Panel pnlIconLowStock;
        private System.Windows.Forms.Label lblLowStockTitle;
        private System.Windows.Forms.Label lblLowStockVal;
        private System.Windows.Forms.Label lblLowStockSub;

        private System.Windows.Forms.Panel pnlCardOutStock;
        private System.Windows.Forms.Panel pnlIconOutStock;
        private System.Windows.Forms.Label lblOutStockTitle;
        private System.Windows.Forms.Label lblOutStockVal;
        private System.Windows.Forms.Label lblOutStockSub;

        private System.Windows.Forms.Panel pnlCatalogCard;
        private System.Windows.Forms.Label lblCatalogTitle;
        private System.Windows.Forms.Panel pnlSearchBox;
        private System.Windows.Forms.TextBox txtSearchItem;
        private System.Windows.Forms.Button btnFilterAll;
        private System.Windows.Forms.Button btnFilterInStock;
        private System.Windows.Forms.Button btnFilterLowStock;
        private System.Windows.Forms.Button btnFilterOutStock;
        private System.Windows.Forms.ComboBox cmbFilterCategory;
        private System.Windows.Forms.Button btnAddItemModal;
        private System.Windows.Forms.Panel pnlItemList;
        private System.Windows.Forms.Label lblCatalogShowing;
        private System.Windows.Forms.Button btnPagePrev;
        private System.Windows.Forms.Label lblPageNum;
        private System.Windows.Forms.Button btnPageNext;

        private System.Windows.Forms.Panel pnlDetailCard;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblSku;
        private System.Windows.Forms.TextBox txtSku;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblReorder;
        private System.Windows.Forms.TextBox txtReorder;
        private System.Windows.Forms.Button btnSaveItem;
        private System.Windows.Forms.Button btnQuickRestock;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnClearFields;
    }
}
