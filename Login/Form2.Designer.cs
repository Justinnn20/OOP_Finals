using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Colors
        private static readonly Color MaroonBg = Color.FromArgb(44, 16, 22);
        private static readonly Color TopBarBg = Color.FromArgb(30, 10, 14);
        private static readonly Color PanelDark = Color.FromArgb(36, 12, 18);
        private static readonly Color InputBg = Color.FromArgb(58, 20, 28);
        private static readonly Color GoldAccent = Color.FromArgb(212, 160, 23);
        private static readonly Color GoldLight = Color.FromArgb(255, 213, 79);
        private static readonly Color ParchmentBg = Color.FromArgb(243, 225, 180);
        private static readonly Color ChipBg = Color.FromArgb(250, 240, 210);
        private static readonly Color ChipHover = Color.FromArgb(250, 228, 160);
        private static readonly Color CreamText = Color.FromArgb(245, 230, 200);
        private static readonly Color MutedText = Color.FromArgb(200, 170, 120);
        private static readonly Color DarkText = Color.FromArgb(60, 20, 25);
        private static readonly Color PriceRed = Color.FromArgb(140, 20, 30);

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            topBar = new Panel();
            pnlTopLine = new Panel();
            lblLogo = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            btnCashier = new Button();
            btnTransactions = new Button();
            pnlSearch = new Panel();
            txtSearch = new TextBox();
            lblSalesValue = new Label();
            lblSalesCaption = new Label();
            lblOrdersValue = new Label();
            lblOrdersCaption = new Label();
            lblAvatar = new Label();
            lblUserName = new Label();
            lblUserRole = new Label();
            btnSignOut = new Button();

            pnlSide = new Panel();
            btnTabAll = new Button();
            btnTabBooks = new Button();
            btnTabUniforms = new Button();
            btnTabSupplies = new Button();
            btnStockIn = new Button();
            btnTransactionLogs = new Button();
            btnCashierPos = new Button();

            pnlOrder = new Panel();
            lblOrderTitle = new Label();
            lblBlank = new Label();
            rbNone = new RadioButton();
            rbScholar = new RadioButton();
            rbStaff = new RadioButton();
            rbCash = new RadioButton();
            rbGCash = new RadioButton();
            pnlLine = new Panel();
            lblSubtotalCaption = new Label();
            lblSubtotalValue = new Label();
            lblTotalCaption = new Label();
            lblTotalValue = new Label();
            lblCashCaption = new Label();
            txtCash = new TextBox();
            btnCheckout = new Button();

            pnlMain = new Panel();
            pnlScan = new Panel();
            lblStudentHint = new Label();
            txtStudentId = new TextBox();
            btnScanId = new Button();
            lblTry = new Label();
            btnTry1 = new Button();
            btnTry2 = new Button();
            btnTry3 = new Button();
            pnlBarcode = new Panel();
            lblBarcodeHint = new Label();
            txtBarcode = new TextBox();
            lblShelves = new Label();
            btnAddItem = new Button();
            lblHint = new Label();
            flpProducts = new FlowLayoutPanel();

            SuspendLayout();
            topBar.SuspendLayout();
            pnlSide.SuspendLayout();
            pnlOrder.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlScan.SuspendLayout();
            pnlBarcode.SuspendLayout();

            // 
            // topBar
            // 
            topBar.BackColor = TopBarBg;
            topBar.Controls.Add(pnlTopLine);
            topBar.Controls.Add(lblLogo);
            topBar.Controls.Add(lblTitle);
            topBar.Controls.Add(lblSubtitle);
            topBar.Controls.Add(btnCashier);
            topBar.Controls.Add(btnTransactions);
            topBar.Controls.Add(pnlSearch);
            topBar.Controls.Add(lblSalesValue);
            topBar.Controls.Add(lblSalesCaption);
            topBar.Controls.Add(lblOrdersValue);
            topBar.Controls.Add(lblOrdersCaption);
            topBar.Controls.Add(lblAvatar);
            topBar.Controls.Add(lblUserName);
            topBar.Controls.Add(lblUserRole);
            topBar.Controls.Add(btnSignOut);
            topBar.Dock = DockStyle.Top;
            topBar.Location = new Point(0, 0);
            topBar.Name = "topBar";
            topBar.Size = new Size(1200, 72);
            topBar.TabIndex = 3;
            // 
            // pnlTopLine
            // 
            pnlTopLine.BackColor = GoldAccent;
            pnlTopLine.Dock = DockStyle.Bottom;
            pnlTopLine.Location = new Point(0, 70);
            pnlTopLine.Name = "pnlTopLine";
            pnlTopLine.Size = new Size(1200, 2);
            pnlTopLine.TabIndex = 15;
            // 
            // lblLogo
            // 
            lblLogo.BackColor = GoldAccent;
            lblLogo.Font = new Font("Georgia", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLogo.ForeColor = DarkText;
            lblLogo.Location = new Point(14, 16);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(38, 38);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "U";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Georgia", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = GoldLight;
            lblTitle.Location = new Point(62, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(300, 23);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "University of Makati Coop";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.ForeColor = MutedText;
            lblSubtitle.Location = new Point(64, 36);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(160, 15);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Bookstore · Quick release";
            // 
            // btnCashier
            // 
            btnCashier.BackColor = GoldAccent;
            btnCashier.Cursor = Cursors.Hand;
            btnCashier.FlatAppearance.BorderSize = 0;
            btnCashier.FlatStyle = FlatStyle.Flat;
            btnCashier.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCashier.ForeColor = DarkText;
            btnCashier.Location = new Point(386, 18);
            btnCashier.Name = "btnCashier";
            btnCashier.Size = new Size(104, 34);
            btnCashier.TabIndex = 3;
            btnCashier.Text = "CASHIER";
            btnCashier.UseVisualStyleBackColor = false;
            // 
            // btnTransactions
            // 
            btnTransactions.BackColor = TopBarBg;
            btnTransactions.Cursor = Cursors.Hand;
            btnTransactions.FlatAppearance.BorderSize = 0;
            btnTransactions.FlatStyle = FlatStyle.Flat;
            btnTransactions.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTransactions.ForeColor = CreamText;
            btnTransactions.Location = new Point(500, 18);
            btnTransactions.Name = "btnTransactions";
            btnTransactions.Size = new Size(150, 34);
            btnTransactions.TabIndex = 4;
            btnTransactions.Text = "TRANSACTIONS";
            btnTransactions.UseVisualStyleBackColor = false;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = InputBg;
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Location = new Point(670, 18);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(180, 34);
            pnlSearch.TabIndex = 5;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = InputBg;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = CreamText;
            txtSearch.Location = new Point(10, 8);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(160, 19);
            txtSearch.TabIndex = 0;
            // 
            // lblSalesValue
            // 
            lblSalesValue.Font = new Font("Georgia", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSalesValue.ForeColor = GoldLight;
            lblSalesValue.Location = new Point(866, 6);
            lblSalesValue.Name = "lblSalesValue";
            lblSalesValue.Size = new Size(86, 28);
            lblSalesValue.TabIndex = 6;
            lblSalesValue.Text = "\u20B10";
            lblSalesValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSalesCaption
            // 
            lblSalesCaption.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSalesCaption.ForeColor = CreamText;
            lblSalesCaption.Location = new Point(866, 38);
            lblSalesCaption.Name = "lblSalesCaption";
            lblSalesCaption.Size = new Size(86, 15);
            lblSalesCaption.TabIndex = 7;
            lblSalesCaption.Text = "Today's sales";
            lblSalesCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOrdersValue
            // 
            lblOrdersValue.Font = new Font("Georgia", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrdersValue.ForeColor = GoldLight;
            lblOrdersValue.Location = new Point(958, 6);
            lblOrdersValue.Name = "lblOrdersValue";
            lblOrdersValue.Size = new Size(52, 28);
            lblOrdersValue.TabIndex = 8;
            lblOrdersValue.Text = "0";
            lblOrdersValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOrdersCaption
            // 
            lblOrdersCaption.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblOrdersCaption.ForeColor = CreamText;
            lblOrdersCaption.Location = new Point(958, 38);
            lblOrdersCaption.Name = "lblOrdersCaption";
            lblOrdersCaption.Size = new Size(52, 15);
            lblOrdersCaption.TabIndex = 9;
            lblOrdersCaption.Text = "Orders";
            lblOrdersCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAvatar
            // 
            lblAvatar.BackColor = GoldAccent;
            lblAvatar.Font = new Font("Georgia", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAvatar.ForeColor = DarkText;
            lblAvatar.Location = new Point(1022, 16);
            lblAvatar.Name = "lblAvatar";
            lblAvatar.Size = new Size(38, 38);
            lblAvatar.TabIndex = 10;
            lblAvatar.Text = "AR";
            lblAvatar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserName.ForeColor = CreamText;
            lblUserName.Location = new Point(1068, 16);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(30, 15);
            lblUserName.TabIndex = 11;
            lblUserName.Text = "Ana";
            // 
            // lblUserRole
            // 
            lblUserRole.AutoSize = true;
            lblUserRole.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUserRole.ForeColor = MutedText;
            lblUserRole.Location = new Point(1068, 36);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(48, 15);
            lblUserRole.TabIndex = 12;
            lblUserRole.Text = "Cashier";
            // 
            // btnSignOut
            // 
            btnSignOut.BackColor = TopBarBg;
            btnSignOut.Cursor = Cursors.Hand;
            btnSignOut.FlatAppearance.BorderColor = GoldAccent;
            btnSignOut.FlatStyle = FlatStyle.Flat;
            btnSignOut.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSignOut.ForeColor = CreamText;
            btnSignOut.Location = new Point(1124, 20);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(66, 30);
            btnSignOut.TabIndex = 13;
            btnSignOut.Text = "Sign out";
            btnSignOut.UseVisualStyleBackColor = false;
            // 
            // pnlSide
            // 
            pnlSide.BackColor = TopBarBg;
            pnlSide.Controls.Add(btnTabAll);
            pnlSide.Controls.Add(btnTabBooks);
            pnlSide.Controls.Add(btnTabUniforms);
            pnlSide.Controls.Add(btnTabSupplies);
            pnlSide.Controls.Add(btnStockIn);
            pnlSide.Controls.Add(btnTransactionLogs);
            pnlSide.Controls.Add(btnCashierPos);
            pnlSide.Dock = DockStyle.Left;
            pnlSide.Location = new Point(0, 72);
            pnlSide.Name = "pnlSide";
            pnlSide.Size = new Size(210, 688);
            pnlSide.TabIndex = 2;
            // 
            // btnTabAll
            // 
            SetTabStyle(btnTabAll, 16, GoldLight, DarkText);
            btnTabAll.Name = "btnTabAll";
            btnTabAll.TabIndex = 0;
            btnTabAll.Text = "Dashboard";
            // 
            // btnTabBooks
            // 
            SetTabStyle(btnTabBooks, 66, TopBarBg, CreamText);
            btnTabBooks.Name = "btnTabBooks";
            btnTabBooks.TabIndex = 1;
            btnTabBooks.Text = "Books && Manuals";
            // 
            // btnTabUniforms
            // 
            SetTabStyle(btnTabUniforms, 116, TopBarBg, CreamText);
            btnTabUniforms.Name = "btnTabUniforms";
            btnTabUniforms.TabIndex = 2;
            btnTabUniforms.Text = "Uniforms";
            // 
            // btnTabSupplies
            // 
            SetTabStyle(btnTabSupplies, 166, TopBarBg, CreamText);
            btnTabSupplies.Name = "btnTabSupplies";
            btnTabSupplies.TabIndex = 3;
            btnTabSupplies.Text = "School Supplies";
            // 
            // btnStockIn
            // 
            SetTabStyle(btnStockIn, 216, TopBarBg, CreamText);
            btnStockIn.Name = "btnStockIn";
            btnStockIn.TabIndex = 4;
            btnStockIn.Text = "Stock-In && Release";
            // 
            // btnTransactionLogs
            // 
            SetTabStyle(btnTransactionLogs, 266, TopBarBg, CreamText);
            btnTransactionLogs.Name = "btnTransactionLogs";
            btnTransactionLogs.TabIndex = 5;
            btnTransactionLogs.Text = "Transaction Logs";
            // 
            // btnCashierPos
            // 
            SetTabStyle(btnCashierPos, 316, TopBarBg, CreamText);
            btnCashierPos.Name = "btnCashierPos";
            btnCashierPos.TabIndex = 6;
            btnCashierPos.Text = "Cashier && POS";
            // 
            // pnlOrder
            // 
            pnlOrder.BackColor = ParchmentBg;
            pnlOrder.Controls.Add(lblOrderTitle);
            pnlOrder.Controls.Add(lblBlank);
            pnlOrder.Controls.Add(rbNone);
            pnlOrder.Controls.Add(rbScholar);
            pnlOrder.Controls.Add(rbStaff);
            pnlOrder.Controls.Add(rbCash);
            pnlOrder.Controls.Add(rbGCash);
            pnlOrder.Controls.Add(pnlLine);
            pnlOrder.Controls.Add(lblSubtotalCaption);
            pnlOrder.Controls.Add(lblSubtotalValue);
            pnlOrder.Controls.Add(lblTotalCaption);
            pnlOrder.Controls.Add(lblTotalValue);
            pnlOrder.Controls.Add(lblCashCaption);
            pnlOrder.Controls.Add(txtCash);
            pnlOrder.Controls.Add(btnCheckout);
            pnlOrder.Dock = DockStyle.Right;
            pnlOrder.Location = new Point(860, 72);
            pnlOrder.Name = "pnlOrder";
            pnlOrder.Size = new Size(340, 688);
            pnlOrder.TabIndex = 1;
            // 
            // lblOrderTitle
            // 
            lblOrderTitle.AutoSize = true;
            lblOrderTitle.Font = new Font("Georgia", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrderTitle.ForeColor = DarkText;
            lblOrderTitle.Location = new Point(20, 20);
            lblOrderTitle.Name = "lblOrderTitle";
            lblOrderTitle.Size = new Size(140, 23);
            lblOrderTitle.TabIndex = 0;
            lblOrderTitle.Text = "ORDER SCROLL";
            // 
            // lblBlank
            // 
            lblBlank.Font = new Font("Georgia", 10F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblBlank.ForeColor = Color.FromArgb(120, 90, 80);
            lblBlank.Location = new Point(20, 200);
            lblBlank.Name = "lblBlank";
            lblBlank.Size = new Size(300, 50);
            lblBlank.TabIndex = 1;
            lblBlank.Text = "The scroll is blank.\nChoose a tome to begin.";
            lblBlank.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rbNone, rbScholar, rbStaff (row 1)
            // 
            SetChipStyle(rbNone, 20, 430, 70, true);
            rbNone.Name = "rbNone";
            rbNone.TabIndex = 2;
            rbNone.TabStop = true;
            rbNone.Text = "None";
            SetChipStyle(rbScholar, 96, 430, 100, false);
            rbScholar.Name = "rbScholar";
            rbScholar.TabIndex = 3;
            rbScholar.Text = "Scholar 10%";
            SetChipStyle(rbStaff, 202, 430, 90, false);
            rbStaff.Name = "rbStaff";
            rbStaff.TabIndex = 4;
            rbStaff.Text = "Staff 15%";
            // 
            // rbCash, rbGCash (row 2)
            // 
            SetChipStyle(rbCash, 20, 466, 70, true);
            rbCash.Name = "rbCash";
            rbCash.TabIndex = 5;
            rbCash.TabStop = true;
            rbCash.Text = "Cash";
            SetChipStyle(rbGCash, 96, 466, 80, false);
            rbGCash.Name = "rbGCash";
            rbGCash.TabIndex = 6;
            rbGCash.Text = "GCash";
            // 
            // pnlLine
            // 
            pnlLine.BackColor = GoldAccent;
            pnlLine.Location = new Point(20, 504);
            pnlLine.Name = "pnlLine";
            pnlLine.Size = new Size(300, 1);
            pnlLine.TabIndex = 7;
            // 
            // lblSubtotalCaption
            // 
            lblSubtotalCaption.AutoSize = true;
            lblSubtotalCaption.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtotalCaption.ForeColor = DarkText;
            lblSubtotalCaption.Location = new Point(20, 518);
            lblSubtotalCaption.Name = "lblSubtotalCaption";
            lblSubtotalCaption.Size = new Size(63, 19);
            lblSubtotalCaption.TabIndex = 8;
            lblSubtotalCaption.Text = "Subtotal";
            // 
            // lblSubtotalValue
            // 
            lblSubtotalValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtotalValue.ForeColor = DarkText;
            lblSubtotalValue.Location = new Point(220, 518);
            lblSubtotalValue.Name = "lblSubtotalValue";
            lblSubtotalValue.Size = new Size(100, 19);
            lblSubtotalValue.TabIndex = 9;
            lblSubtotalValue.Text = "\u20B10";
            lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Georgia", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalCaption.ForeColor = DarkText;
            lblTotalCaption.Location = new Point(20, 545);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(60, 23);
            lblTotalCaption.TabIndex = 10;
            lblTotalCaption.Text = "TOTAL";
            // 
            // lblTotalValue
            // 
            lblTotalValue.Font = new Font("Georgia", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.ForeColor = PriceRed;
            lblTotalValue.Location = new Point(160, 538);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(160, 40);
            lblTotalValue.TabIndex = 11;
            lblTotalValue.Text = "\u20B10";
            lblTotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCashCaption
            // 
            lblCashCaption.AutoSize = true;
            lblCashCaption.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCashCaption.ForeColor = DarkText;
            lblCashCaption.Location = new Point(20, 592);
            lblCashCaption.Name = "lblCashCaption";
            lblCashCaption.Size = new Size(102, 19);
            lblCashCaption.TabIndex = 12;
            lblCashCaption.Text = "Cash received";
            // 
            // txtCash
            // 
            txtCash.BackColor = ChipBg;
            txtCash.BorderStyle = BorderStyle.FixedSingle;
            txtCash.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCash.ForeColor = DarkText;
            txtCash.Location = new Point(180, 588);
            txtCash.Name = "txtCash";
            txtCash.Size = new Size(140, 27);
            txtCash.TabIndex = 13;
            txtCash.Text = "0";
            txtCash.TextAlign = HorizontalAlignment.Right;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = GoldAccent;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatAppearance.MouseOverBackColor = GoldLight;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCheckout.ForeColor = DarkText;
            btnCheckout.Location = new Point(20, 630);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(300, 44);
            btnCheckout.TabIndex = 14;
            btnCheckout.Text = "Checkout";
            btnCheckout.UseVisualStyleBackColor = false;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = MaroonBg;
            pnlMain.Controls.Add(pnlScan);
            pnlMain.Controls.Add(pnlBarcode);
            pnlMain.Controls.Add(lblShelves);
            pnlMain.Controls.Add(btnAddItem);
            pnlMain.Controls.Add(lblHint);
            pnlMain.Controls.Add(flpProducts);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(210, 72);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(20, 16, 20, 16);
            pnlMain.Size = new Size(650, 688);
            pnlMain.TabIndex = 0;
            // 
            // pnlScan
            // 
            pnlScan.BackColor = PanelDark;
            pnlScan.Controls.Add(lblStudentHint);
            pnlScan.Controls.Add(txtStudentId);
            pnlScan.Controls.Add(btnScanId);
            pnlScan.Controls.Add(lblTry);
            pnlScan.Controls.Add(btnTry1);
            pnlScan.Controls.Add(btnTry2);
            pnlScan.Controls.Add(btnTry3);
            pnlScan.Location = new Point(20, 16);
            pnlScan.Name = "pnlScan";
            pnlScan.Size = new Size(610, 50);
            pnlScan.TabIndex = 0;
            // 
            // lblStudentHint
            // 
            lblStudentHint.AutoSize = true;
            lblStudentHint.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStudentHint.ForeColor = MutedText;
            lblStudentHint.Location = new Point(12, 17);
            lblStudentHint.Name = "lblStudentHint";
            lblStudentHint.Size = new Size(64, 15);
            lblStudentHint.TabIndex = 3;
            lblStudentHint.Text = "Student ID";
            // 
            // txtStudentId
            // 
            txtStudentId.BackColor = InputBg;
            txtStudentId.BorderStyle = BorderStyle.FixedSingle;
            txtStudentId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStudentId.ForeColor = CreamText;
            txtStudentId.Location = new Point(92, 12);
            txtStudentId.Name = "txtStudentId";
            txtStudentId.Size = new Size(120, 25);
            txtStudentId.TabIndex = 0;
            // 
            // btnScanId
            // 
            btnScanId.BackColor = GoldAccent;
            btnScanId.Cursor = Cursors.Hand;
            btnScanId.FlatAppearance.BorderSize = 0;
            btnScanId.FlatStyle = FlatStyle.Flat;
            btnScanId.Font = new Font("Georgia", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnScanId.ForeColor = DarkText;
            btnScanId.Location = new Point(222, 9);
            btnScanId.Name = "btnScanId";
            btnScanId.Size = new Size(100, 32);
            btnScanId.TabIndex = 1;
            btnScanId.Text = "Scan ID";
            btnScanId.UseVisualStyleBackColor = false;
            btnScanId.Click += btnScanId_Click;
            // 
            // lblTry
            // 
            lblTry.AutoSize = true;
            lblTry.Font = new Font("Georgia", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTry.ForeColor = GoldLight;
            lblTry.Location = new Point(332, 17);
            lblTry.Name = "lblTry";
            lblTry.Size = new Size(32, 17);
            lblTry.TabIndex = 2;
            lblTry.Text = "Try:";
            // 
            // btnTry1, btnTry2, btnTry3
            // 
            SetChipButton(btnTry1, 370, "22-10234");
            btnTry1.Name = "btnTry1";
            btnTry1.TabIndex = 4;
            SetChipButton(btnTry2, 446, "22-20311");
            btnTry2.Name = "btnTry2";
            btnTry2.TabIndex = 5;
            SetChipButton(btnTry3, 522, "21-30788");
            btnTry3.Name = "btnTry3";
            btnTry3.TabIndex = 6;
            // 
            // pnlBarcode
            // 
            pnlBarcode.BackColor = PanelDark;
            pnlBarcode.Controls.Add(lblBarcodeHint);
            pnlBarcode.Controls.Add(txtBarcode);
            pnlBarcode.Location = new Point(20, 76);
            pnlBarcode.Name = "pnlBarcode";
            pnlBarcode.Size = new Size(610, 46);
            pnlBarcode.TabIndex = 1;
            // 
            // lblBarcodeHint
            // 
            lblBarcodeHint.AutoSize = true;
            lblBarcodeHint.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBarcodeHint.ForeColor = MutedText;
            lblBarcodeHint.Location = new Point(12, 15);
            lblBarcodeHint.Name = "lblBarcodeHint";
            lblBarcodeHint.Size = new Size(96, 15);
            lblBarcodeHint.TabIndex = 1;
            lblBarcodeHint.Text = "Barcode / SKU:";
            // 
            // txtBarcode
            // 
            txtBarcode.BackColor = InputBg;
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBarcode.ForeColor = CreamText;
            txtBarcode.Location = new Point(130, 10);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(466, 25);
            txtBarcode.TabIndex = 0;
            // 
            // lblShelves
            // 
            lblShelves.AutoSize = true;
            lblShelves.Font = new Font("Georgia", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblShelves.ForeColor = GoldLight;
            lblShelves.Location = new Point(20, 138);
            lblShelves.Name = "lblShelves";
            lblShelves.Size = new Size(150, 30);
            lblShelves.TabIndex = 2;
            lblShelves.Text = "ALL SHELVES";
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = GoldAccent;
            btnAddItem.Cursor = Cursors.Hand;
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.FlatAppearance.MouseOverBackColor = GoldLight;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Georgia", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddItem.ForeColor = DarkText;
            btnAddItem.Location = new Point(240, 138);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(130, 34);
            btnAddItem.TabIndex = 7;
            btnAddItem.Text = "+ Add Item";
            btnAddItem.UseVisualStyleBackColor = false;
            // 
            // lblHint
            // 
            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblHint.ForeColor = MutedText;
            lblHint.Location = new Point(400, 146);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(230, 15);
            lblHint.TabIndex = 3;
            lblHint.Text = "Tap a card to add it to the scroll";
            // 
            // flpProducts
            // 
            flpProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpProducts.AutoScroll = true;
            flpProducts.BackColor = MaroonBg;
            flpProducts.FlowDirection = FlowDirection.LeftToRight;
            flpProducts.Location = new Point(20, 180);
            flpProducts.Name = "flpProducts";
            flpProducts.Size = new Size(610, 490);
            flpProducts.TabIndex = 4;
            flpProducts.WrapContents = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = MaroonBg;
            ClientSize = new Size(1200, 760);
            Controls.Add(pnlMain);
            Controls.Add(pnlOrder);
            Controls.Add(pnlSide);
            Controls.Add(topBar);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1000, 650);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UMak Coop - Wizarding POS";
            topBar.ResumeLayout(false);
            topBar.PerformLayout();
            pnlSide.ResumeLayout(false);
            pnlOrder.ResumeLayout(false);
            pnlOrder.PerformLayout();
            pnlScan.ResumeLayout(false);
            pnlScan.PerformLayout();
            pnlBarcode.ResumeLayout(false);
            pnlBarcode.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private void SetTabStyle(Button btn, int top, Color back, Color fore)
        {
            btn.BackColor = back;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn.ForeColor = fore;
            btn.Location = new Point(0, top);
            btn.Size = new Size(210, 46);
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.UseVisualStyleBackColor = false;
        }

        private void SetChipStyle(RadioButton rb, int x, int y, int width, bool isChecked)
        {
            rb.Appearance = Appearance.Button;
            rb.Checked = isChecked;
            rb.Cursor = Cursors.Hand;
            rb.FlatStyle = FlatStyle.Flat;
            rb.BackColor = ChipBg;
            rb.ForeColor = DarkText;
            rb.FlatAppearance.BorderColor = GoldAccent;
            rb.FlatAppearance.BorderSize = 1;
            rb.FlatAppearance.CheckedBackColor = GoldAccent;
            rb.FlatAppearance.MouseDownBackColor = GoldLight;
            rb.FlatAppearance.MouseOverBackColor = ChipHover;
            rb.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rb.Location = new Point(x, y);
            rb.Size = new Size(width, 28);
            rb.TextAlign = ContentAlignment.MiddleCenter;
            rb.UseVisualStyleBackColor = false;
        }

        private void SetChipButton(Button btn, int x, string text)
        {
            btn.BackColor = PanelDark;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderColor = GoldAccent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn.ForeColor = GoldLight;
            btn.Location = new Point(x, 9);
            btn.Size = new Size(70, 32);
            btn.Text = text;
            btn.UseVisualStyleBackColor = false;
        }

        private Panel topBar;
        private Panel pnlTopLine;
        private Label lblLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnCashier;
        private Button btnTransactions;
        private Panel pnlSearch;
        private TextBox txtSearch;
        private Label lblSalesValue;
        private Label lblSalesCaption;
        private Label lblOrdersValue;
        private Label lblOrdersCaption;
        private Label lblAvatar;
        private Label lblUserName;
        private Label lblUserRole;
        private Button btnSignOut;

        private Panel pnlSide;
        private Button btnTabAll;
        private Button btnTabBooks;
        private Button btnTabUniforms;
        private Button btnTabSupplies;
        private Button btnStockIn;
        private Button btnTransactionLogs;
        private Button btnCashierPos;

        private Panel pnlOrder;
        private Label lblOrderTitle;
        private Label lblBlank;
        private RadioButton rbNone;
        private RadioButton rbScholar;
        private RadioButton rbStaff;
        private RadioButton rbCash;
        private RadioButton rbGCash;
        private Panel pnlLine;
        private Label lblSubtotalCaption;
        private Label lblSubtotalValue;
        private Label lblTotalCaption;
        private Label lblTotalValue;
        private Label lblCashCaption;
        private TextBox txtCash;
        private Button btnCheckout;

        private Panel pnlMain;
        private Panel pnlScan;
        private Label lblStudentHint;
        private TextBox txtStudentId;
        private Button btnScanId;
        private Label lblTry;
        private Button btnTry1;
        private Button btnTry2;
        private Button btnTry3;
        private Panel pnlBarcode;
        private Label lblBarcodeHint;
        private TextBox txtBarcode;
        private Label lblShelves;
        private Button btnAddItem;
        private Label lblHint;
        private FlowLayoutPanel flpProducts;
    }
}