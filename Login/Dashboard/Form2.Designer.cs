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
            lblCashierTag = new Label();
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

            pnlOrder = new Panel();
            lblOrderTitle = new Label();
            btnHoldOrder = new Button();
            btnClearOrder = new Button();
            flpCartItems = new FlowLayoutPanel();
            lblBlank = new Label();
            pnlDiscounts = new Panel();
            rbNone = new RadioButton();
            rbSenior = new RadioButton();
            rbPwd = new RadioButton();
            pnlPayments = new Panel();
            rbCash = new RadioButton();
            rbGCash = new RadioButton();
            pnlLine = new Panel();
            lblSubtotalCaption = new Label();
            lblSubtotalValue = new Label();
            lblTotalCaption = new Label();
            lblTotalValue = new Label();
            lblCashCaption = new Label();
            pnlCashInput = new Panel();
            txtCash = new TextBox();
            btnCheckout = new Button();

            pnlMain = new Panel();
            pnlScan = new Panel();
            lblStudentHint = new Label();
            txtStudentId = new TextBox();
            btnScanId = new Button();
            lblSearchCaption = new Label();
            pnlSearch = new Panel();
            txtSearch = new TextBox();
            pnlBarcode = new Panel();
            lblBarcodeHint = new Label();
            txtBarcode = new TextBox();
            btnScanBarcode = new Button();
            lblShelves = new Label();
            flpProducts = new FlowLayoutPanel();

            SuspendLayout();
            topBar.SuspendLayout();
            pnlSide.SuspendLayout();
            pnlOrder.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlScan.SuspendLayout();
            pnlSearch.SuspendLayout();
            pnlBarcode.SuspendLayout();

            // 
            // topBar
            // 
            topBar.BackColor = TopBarBg;
            topBar.Controls.Add(pnlTopLine);
            topBar.Controls.Add(lblLogo);
            topBar.Controls.Add(lblTitle);
            topBar.Controls.Add(lblSubtitle);
            topBar.Controls.Add(lblCashierTag);
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
            // lblCashierTag
            // 
            lblCashierTag.AutoSize = false;
            lblCashierTag.BackColor = GoldAccent;
            lblCashierTag.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCashierTag.ForeColor = DarkText;
            lblCashierTag.Location = new Point(386, 18);
            lblCashierTag.Name = "lblCashierTag";
            lblCashierTag.Size = new Size(104, 34);
            lblCashierTag.TabIndex = 3;
            lblCashierTag.Text = "CASHIER";
            lblCashierTag.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSalesValue
            // 
            lblSalesValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSalesValue.Font = new Font("Georgia", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSalesValue.ForeColor = GoldLight;
            lblSalesValue.Location = new Point(780, 6);
            lblSalesValue.Name = "lblSalesValue";
            lblSalesValue.Size = new Size(100, 28);
            lblSalesValue.TabIndex = 6;
            lblSalesValue.Text = "\u20B10";
            lblSalesValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSalesCaption
            // 
            lblSalesCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSalesCaption.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSalesCaption.ForeColor = CreamText;
            lblSalesCaption.Location = new Point(780, 38);
            lblSalesCaption.Name = "lblSalesCaption";
            lblSalesCaption.Size = new Size(100, 15);
            lblSalesCaption.TabIndex = 7;
            lblSalesCaption.Text = "Today's sales";
            lblSalesCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOrdersValue
            // 
            lblOrdersValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrdersValue.Font = new Font("Georgia", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrdersValue.ForeColor = GoldLight;
            lblOrdersValue.Location = new Point(890, 6);
            lblOrdersValue.Name = "lblOrdersValue";
            lblOrdersValue.Size = new Size(60, 28);
            lblOrdersValue.TabIndex = 8;
            lblOrdersValue.Text = "0";
            lblOrdersValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOrdersCaption
            // 
            lblOrdersCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrdersCaption.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblOrdersCaption.ForeColor = CreamText;
            lblOrdersCaption.Location = new Point(890, 38);
            lblOrdersCaption.Name = "lblOrdersCaption";
            lblOrdersCaption.Size = new Size(60, 15);
            lblOrdersCaption.TabIndex = 9;
            lblOrdersCaption.Text = "Orders";
            lblOrdersCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAvatar
            // 
            lblAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAvatar.BackColor = GoldAccent;
            lblAvatar.Font = new Font("Georgia", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAvatar.ForeColor = DarkText;
            lblAvatar.Location = new Point(964, 16);
            lblAvatar.Name = "lblAvatar";
            lblAvatar.Size = new Size(38, 38);
            lblAvatar.TabIndex = 10;
            lblAvatar.Text = "AR";
            lblAvatar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUserName
            // 
            lblUserName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserName.ForeColor = CreamText;
            lblUserName.Location = new Point(1012, 16);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(30, 15);
            lblUserName.TabIndex = 11;
            lblUserName.Text = "Ana";
            // 
            // lblUserRole
            // 
            lblUserRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserRole.AutoSize = true;
            lblUserRole.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUserRole.ForeColor = MutedText;
            lblUserRole.Location = new Point(1012, 36);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(48, 15);
            lblUserRole.TabIndex = 12;
            lblUserRole.Text = "Cashier";
            // 
            // btnSignOut
            // 
            btnSignOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSignOut.BackColor = TopBarBg;
            btnSignOut.Cursor = Cursors.Hand;
            btnSignOut.FlatAppearance.BorderSize = 0;
            btnSignOut.FlatStyle = FlatStyle.Flat;
            btnSignOut.Font = new Font("Georgia", 8.25F, FontStyle.Bold);
            btnSignOut.ForeColor = Color.Transparent;
            btnSignOut.Location = new Point(1080, 18);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(108, 36);
            btnSignOut.TabIndex = 13;
            btnSignOut.Text = "SIGN OUT";
            btnSignOut.UseVisualStyleBackColor = false;
            // 
            // pnlSide
            // 
            pnlSide.BackColor = Color.FromArgb(20, 6, 12);
            pnlSide.Controls.Add(btnTabAll);
            pnlSide.Controls.Add(btnTabBooks);
            pnlSide.Controls.Add(btnTabUniforms);
            pnlSide.Controls.Add(btnTabSupplies);
            pnlSide.Controls.Add(btnStockIn);
            pnlSide.Controls.Add(btnTransactionLogs);
            pnlSide.Dock = DockStyle.Left;
            pnlSide.Location = new Point(0, 72);
            pnlSide.Name = "pnlSide";
            pnlSide.Size = new Size(66, 688);
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
            // pnlOrder
            // 
            pnlOrder.BackColor = ParchmentBg;
            pnlOrder.Controls.Add(lblOrderTitle);
            pnlOrder.Controls.Add(btnHoldOrder);
            pnlOrder.Controls.Add(btnClearOrder);
            pnlOrder.Controls.Add(flpCartItems);
            pnlOrder.Controls.Add(lblBlank);
            pnlDiscounts.Controls.Add(rbNone);
            pnlDiscounts.Controls.Add(rbSenior);
            pnlDiscounts.Controls.Add(rbPwd);
            pnlPayments.Controls.Add(rbCash);
            pnlPayments.Controls.Add(rbGCash);
            pnlOrder.Controls.Add(pnlDiscounts);
            pnlOrder.Controls.Add(pnlPayments);
            pnlOrder.Controls.Add(pnlLine);
            pnlOrder.Controls.Add(lblSubtotalCaption);
            pnlOrder.Controls.Add(lblSubtotalValue);
            pnlOrder.Controls.Add(lblTotalCaption);
            pnlOrder.Controls.Add(lblTotalValue);
            pnlOrder.Controls.Add(lblCashCaption);
            pnlCashInput.Controls.Add(txtCash);
            pnlOrder.Controls.Add(pnlCashInput);
            pnlOrder.Controls.Add(btnCheckout);
            pnlOrder.Dock = DockStyle.Right;
            pnlOrder.Location = new Point(860, 72);
            pnlOrder.Name = "pnlOrder";
            pnlOrder.Size = new Size(340, 688);
            pnlOrder.TabIndex = 1;
            // 
            // lblOrderTitle
            // 
            lblOrderTitle.AutoSize = false;
            lblOrderTitle.Font = new Font("Georgia", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrderTitle.ForeColor = DarkText;
            lblOrderTitle.Location = new Point(16, 14);
            lblOrderTitle.Name = "lblOrderTitle";
            lblOrderTitle.Size = new Size(160, 26);
            lblOrderTitle.TabIndex = 0;
            lblOrderTitle.Text = "ORDER SCROLL";
            lblOrderTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnHoldOrder
            // 
            btnHoldOrder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHoldOrder.BackColor = ChipBg;
            btnHoldOrder.Cursor = Cursors.Hand;
            btnHoldOrder.FlatAppearance.BorderColor = Color.FromArgb(215, 190, 140);
            btnHoldOrder.FlatAppearance.BorderSize = 1;
            btnHoldOrder.FlatStyle = FlatStyle.Flat;
            btnHoldOrder.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnHoldOrder.ForeColor = DarkText;
            btnHoldOrder.Location = new Point(190, 14);
            btnHoldOrder.Name = "btnHoldOrder";
            btnHoldOrder.Size = new Size(66, 26);
            btnHoldOrder.TabIndex = 15;
            btnHoldOrder.Text = "Hold";
            btnHoldOrder.UseVisualStyleBackColor = false;
            // 
            // btnClearOrder
            // 
            btnClearOrder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearOrder.BackColor = ChipBg;
            btnClearOrder.Cursor = Cursors.Hand;
            btnClearOrder.FlatAppearance.BorderColor = Color.FromArgb(215, 190, 140);
            btnClearOrder.FlatAppearance.BorderSize = 1;
            btnClearOrder.FlatStyle = FlatStyle.Flat;
            btnClearOrder.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnClearOrder.ForeColor = DarkText;
            btnClearOrder.Location = new Point(260, 14);
            btnClearOrder.Name = "btnClearOrder";
            btnClearOrder.Size = new Size(64, 26);
            btnClearOrder.TabIndex = 16;
            btnClearOrder.Text = "Clear";
            btnClearOrder.UseVisualStyleBackColor = false;
            // 
            // flpCartItems
            // 
            flpCartItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpCartItems.AutoScroll = true;
            flpCartItems.BackColor = ParchmentBg;
            flpCartItems.FlowDirection = FlowDirection.TopDown;
            flpCartItems.Location = new Point(14, 48);
            flpCartItems.Name = "flpCartItems";
            flpCartItems.Size = new Size(312, 370);
            flpCartItems.TabIndex = 18;
            flpCartItems.WrapContents = false;
            // 
            // lblBlank
            // 
            lblBlank.Font = new Font("Georgia", 10F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblBlank.ForeColor = Color.FromArgb(120, 90, 80);
            lblBlank.Location = new Point(16, 180);
            lblBlank.Name = "lblBlank";
            lblBlank.Size = new Size(304, 50);
            lblBlank.TabIndex = 1;
            lblBlank.Text = "The scroll is blank.\nChoose a tome to begin.";
            lblBlank.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlDiscounts
            // 
            pnlDiscounts.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDiscounts.BackColor = ParchmentBg;
            pnlDiscounts.Location = new Point(20, 426);
            pnlDiscounts.Name = "pnlDiscounts";
            pnlDiscounts.Size = new Size(300, 32);
            pnlDiscounts.TabIndex = 2;
            // 
            // rbNone, rbSenior, rbPwd (row 1 - Discounts)
            // 
            SetChipStyle(rbNone, 0, 2, 74, true);
            rbNone.Name = "rbNone";
            rbNone.TabIndex = 0;
            rbNone.TabStop = true;
            rbNone.Text = "None";
            SetChipStyle(rbSenior, 82, 2, 110, false);
            rbSenior.Name = "rbSenior";
            rbSenior.TabIndex = 1;
            rbSenior.Text = "Senior 20%";
            SetChipStyle(rbPwd, 200, 2, 100, false);
            rbPwd.Name = "rbPwd";
            rbPwd.TabIndex = 2;
            rbPwd.Text = "PWD 20%";
            // 
            // pnlPayments
            // 
            pnlPayments.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlPayments.BackColor = ParchmentBg;
            pnlPayments.Location = new Point(20, 462);
            pnlPayments.Name = "pnlPayments";
            pnlPayments.Size = new Size(300, 32);
            pnlPayments.TabIndex = 3;
            // 
            // rbCash, rbGCash (row 2 - Payment Methods)
            // 
            SetChipStyle(rbCash, 0, 2, 145, true);
            rbCash.Name = "rbCash";
            rbCash.TabIndex = 0;
            rbCash.TabStop = true;
            rbCash.Text = "Cash";
            SetChipStyle(rbGCash, 155, 2, 145, false);
            rbGCash.Name = "rbGCash";
            rbGCash.TabIndex = 1;
            rbGCash.Text = "GCash";
            // 
            // pnlLine
            // 
            pnlLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlLine.BackColor = GoldAccent;
            pnlLine.Location = new Point(20, 502);
            pnlLine.Name = "pnlLine";
            pnlLine.Size = new Size(300, 1);
            pnlLine.TabIndex = 7;
            // 
            // lblSubtotalCaption
            // 
            lblSubtotalCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSubtotalCaption.AutoSize = true;
            lblSubtotalCaption.Font = new Font("Georgia", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtotalCaption.ForeColor = DarkText;
            lblSubtotalCaption.Location = new Point(20, 514);
            lblSubtotalCaption.Name = "lblSubtotalCaption";
            lblSubtotalCaption.Size = new Size(65, 18);
            lblSubtotalCaption.TabIndex = 8;
            lblSubtotalCaption.Text = "Subtotal";
            // 
            // lblSubtotalValue
            // 
            lblSubtotalValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblSubtotalValue.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtotalValue.ForeColor = DarkText;
            lblSubtotalValue.Location = new Point(130, 512);
            lblSubtotalValue.Name = "lblSubtotalValue";
            lblSubtotalValue.Size = new Size(190, 22);
            lblSubtotalValue.TabIndex = 9;
            lblSubtotalValue.Text = "₱  0";
            lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Georgia", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCaption.ForeColor = DarkText;
            lblTotalCaption.Location = new Point(20, 546);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(76, 21);
            lblTotalCaption.TabIndex = 10;
            lblTotalCaption.Text = "TOTAL";
            // 
            // lblTotalValue
            // 
            lblTotalValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTotalValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.ForeColor = DarkText;
            lblTotalValue.Location = new Point(100, 538);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(220, 36);
            lblTotalValue.TabIndex = 11;
            lblTotalValue.Text = "₱  0";
            lblTotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCashCaption
            // 
            lblCashCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCashCaption.AutoSize = true;
            lblCashCaption.Font = new Font("Georgia", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCashCaption.ForeColor = DarkText;
            lblCashCaption.Location = new Point(20, 590);
            lblCashCaption.Name = "lblCashCaption";
            lblCashCaption.Size = new Size(105, 18);
            lblCashCaption.TabIndex = 12;
            lblCashCaption.Text = "Cash received";
            // 
            // pnlCashInput
            // 
            pnlCashInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pnlCashInput.BackColor = ChipBg;
            pnlCashInput.Location = new Point(160, 584);
            pnlCashInput.Name = "pnlCashInput";
            pnlCashInput.Size = new Size(160, 30);
            pnlCashInput.TabIndex = 13;
            // 
            // txtCash
            // 
            txtCash.BackColor = ChipBg;
            txtCash.BorderStyle = BorderStyle.None;
            txtCash.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCash.ForeColor = DarkText;
            txtCash.Location = new Point(8, 5);
            txtCash.Name = "txtCash";
            txtCash.Size = new Size(140, 20);
            txtCash.TabIndex = 0;
            txtCash.Text = "0";
            txtCash.TextAlign = HorizontalAlignment.Right;
            // 
            // btnCheckout
            // 
            btnCheckout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCheckout.BackColor = GoldAccent;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatAppearance.MouseOverBackColor = GoldLight;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCheckout.ForeColor = DarkText;
            btnCheckout.Location = new Point(20, 628);
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
            pnlScan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlScan.BackColor = PanelDark;
            pnlScan.Controls.Add(lblStudentHint);
            pnlScan.Controls.Add(txtStudentId);
            pnlScan.Controls.Add(btnScanId);
            pnlScan.Controls.Add(lblSearchCaption);
            pnlScan.Controls.Add(pnlSearch);
            pnlScan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlScan.Location = new Point(20, 16);
            pnlScan.Name = "pnlScan";
            pnlScan.Size = new Size(610, 50);
            pnlScan.TabIndex = 0;
            // 
            // lblStudentHint
            // 
            lblStudentHint.AutoSize = false;
            lblStudentHint.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStudentHint.ForeColor = MutedText;
            lblStudentHint.Location = new Point(10, 12);
            lblStudentHint.Name = "lblStudentHint";
            lblStudentHint.Size = new Size(80, 25);
            lblStudentHint.TabIndex = 3;
            lblStudentHint.Text = "Student ID";
            lblStudentHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtStudentId
            // 
            txtStudentId.BackColor = InputBg;
            txtStudentId.BorderStyle = BorderStyle.FixedSingle;
            txtStudentId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStudentId.ForeColor = CreamText;
            txtStudentId.Location = new Point(92, 12);
            txtStudentId.Name = "txtStudentId";
            txtStudentId.Size = new Size(100, 25);
            txtStudentId.TabIndex = 0;
            // 
            // btnScanId
            // 
            btnScanId.BackColor = GoldAccent;
            btnScanId.Cursor = Cursors.Hand;
            btnScanId.FlatAppearance.BorderSize = 0;
            btnScanId.FlatStyle = FlatStyle.Flat;
            btnScanId.Font = new Font("Georgia", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnScanId.ForeColor = DarkText;
            btnScanId.Location = new Point(200, 9);
            btnScanId.Name = "btnScanId";
            btnScanId.Size = new Size(76, 30);
            btnScanId.TabIndex = 1;
            btnScanId.Text = "Scan ID";
            btnScanId.UseVisualStyleBackColor = false;
            btnScanId.Click += btnScanId_Click;
            // 
            // lblSearchCaption
            // 
            lblSearchCaption.AutoSize = false;
            lblSearchCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSearchCaption.ForeColor = MutedText;
            lblSearchCaption.Location = new Point(292, 12);
            lblSearchCaption.Name = "lblSearchCaption";
            lblSearchCaption.Size = new Size(56, 25);
            lblSearchCaption.TabIndex = 14;
            lblSearchCaption.Text = "Search:";
            lblSearchCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSearch
            // 
            pnlSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearch.BackColor = InputBg;
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Location = new Point(352, 8);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(246, 34);
            pnlSearch.TabIndex = 5;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BackColor = InputBg;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = CreamText;
            txtSearch.Location = new Point(10, 8);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(226, 19);
            txtSearch.TabIndex = 0;
            // 
            // pnlBarcode
            // 
            pnlBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlBarcode.BackColor = PanelDark;
            pnlBarcode.Controls.Add(lblBarcodeHint);
            pnlBarcode.Controls.Add(txtBarcode);
            pnlBarcode.Controls.Add(btnScanBarcode);
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
            txtBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBarcode.BackColor = InputBg;
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBarcode.ForeColor = CreamText;
            txtBarcode.Location = new Point(130, 10);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(366, 25);
            txtBarcode.TabIndex = 0;
            // 
            // btnScanBarcode
            // 
            btnScanBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScanBarcode.BackColor = GoldAccent;
            btnScanBarcode.Cursor = Cursors.Hand;
            btnScanBarcode.FlatAppearance.BorderSize = 0;
            btnScanBarcode.FlatStyle = FlatStyle.Flat;
            btnScanBarcode.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnScanBarcode.ForeColor = DarkText;
            btnScanBarcode.Location = new Point(504, 8);
            btnScanBarcode.Name = "btnScanBarcode";
            btnScanBarcode.Size = new Size(96, 29);
            btnScanBarcode.TabIndex = 2;
            btnScanBarcode.Text = "Add / Scan";
            btnScanBarcode.UseVisualStyleBackColor = false;
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
            MinimumSize = new Size(1200, 650);
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
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
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

        private Panel topBar;
        private Panel pnlTopLine;
        private Label lblLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblCashierTag;
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

        private Panel pnlOrder;
        private Label lblOrderTitle;
        private Button btnHoldOrder;
        private Button btnClearOrder;
        private FlowLayoutPanel flpCartItems;
        private Label lblBlank;
        private Panel pnlDiscounts;
        private RadioButton rbNone;
        private RadioButton rbSenior;
        private RadioButton rbPwd;
        private Panel pnlPayments;
        private RadioButton rbCash;
        private RadioButton rbGCash;
        private Panel pnlLine;
        private Label lblSubtotalCaption;
        private Label lblSubtotalValue;
        private Label lblTotalCaption;
        private Label lblTotalValue;
        private Label lblCashCaption;
        private Panel pnlCashInput;
        private TextBox txtCash;
        private Button btnCheckout;

        private Panel pnlMain;
        private Panel pnlScan;
        private Label lblStudentHint;
        private TextBox txtStudentId;
        private Button btnScanId;
        private Label lblSearchCaption;
        private Panel pnlSearch;
        private TextBox txtSearch;
        private Panel pnlBarcode;
        private Label lblBarcodeHint;
        private TextBox txtBarcode;
        private Button btnScanBarcode;
        private Label lblShelves;
        private FlowLayoutPanel flpProducts;
    }
}