using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    partial class LoginPage
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlCrest = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblBrandSub = new System.Windows.Forms.Label();
            this.pnlStaff = new RoundedPanel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.lblEyebrow = new System.Windows.Forms.Label();
            this.lblHeroTitle = new System.Windows.Forms.Label();
            this.lblHeroText = new System.Windows.Forms.Label();
            this.pnlChipBooks = new RoundedPanel();
            this.pnlChipUniforms = new RoundedPanel();
            this.pnlChipSupplies = new RoundedPanel();
            this.pnlChipMerch = new RoundedPanel();
            this.pnlRod = new RoundedPanel();
            this.pnlScroll = new RoundedPanel();
            this.lblSignIn = new System.Windows.Forms.Label();
            this.lblSignInSub = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.btnCashier = new RoundedButton();
            this.btnAdmin = new RoundedButton();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlUser = new RoundedPanel();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.pnlPass = new RoundedPanel();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnShowPass = new RoundedButton();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.lblForgot = new System.Windows.Forms.Label();
            this.btnSignIn = new RoundedButton();
            this.lblRoleNote = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlPass.SuspendLayout();
            this.SuspendLayout();

            // ---------------- pnlHeader ----------------
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(9)))), ((int)(((byte)(17)))));
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);
            this.pnlHeader.Controls.Add(this.pnlStaff);
            this.pnlHeader.Controls.Add(this.lblBrandSub);
            this.pnlHeader.Controls.Add(this.lblBrand);
            this.pnlHeader.Controls.Add(this.pnlCrest);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 76);
            this.pnlHeader.TabIndex = 20;

            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(154)))), ((int)(((byte)(30)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 74);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(1200, 2);
            this.pnlHeaderLine.TabIndex = 4;

            this.pnlCrest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(9)))), ((int)(((byte)(17)))));
            this.pnlCrest.Location = new System.Drawing.Point(36, 16);
            this.pnlCrest.Name = "pnlCrest";
            this.pnlCrest.Size = new System.Drawing.Size(40, 44);
            this.pnlCrest.TabIndex = 0;
            this.pnlCrest.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCrest_Paint);

            this.lblBrand.AutoSize = true;
            this.lblBrand.BackColor = System.Drawing.Color.Transparent;
            this.lblBrand.Font = new System.Drawing.Font("Cinzel", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.lblBrand.Location = new System.Drawing.Point(90, 13);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.TabIndex = 1;
            this.lblBrand.Text = "UNIVERSITY OF MAKATI COOP";

            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.BackColor = System.Drawing.Color.Transparent;
            this.lblBrandSub.Font = new System.Drawing.Font("Cormorant Garamond", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(217)))), ((int)(((byte)(173)))));
            this.lblBrandSub.Location = new System.Drawing.Point(91, 44);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.TabIndex = 2;
            this.lblBrandSub.Text = "Bookstore · Quick release";

            this.pnlStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStaff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(154)))), ((int)(((byte)(30)))));
            this.pnlStaff.BorderWidth = 1.5F;
            this.pnlStaff.CornerRadius = 16;
            this.pnlStaff.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.pnlStaff.Location = new System.Drawing.Point(1014, 22);
            this.pnlStaff.Name = "pnlStaff";
            this.pnlStaff.Size = new System.Drawing.Size(150, 32);
            this.pnlStaff.TabIndex = 3;
            this.pnlStaff.TabStop = false;
            this.pnlStaff.Text = "STAFF ENTRANCE";

            // ---------------- footer ----------------
            this.lblFooter.BackColor = System.Drawing.Color.Transparent;
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFooter.Font = new System.Drawing.Font("Cormorant Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(197)))), ((int)(((byte)(147)))));
            this.lblFooter.Location = new System.Drawing.Point(0, 680);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(1200, 40);
            this.lblFooter.TabIndex = 19;
            this.lblFooter.Text = "For authorized UMak Coop staff only";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ---------------- hero (left side) ----------------
            this.lblEyebrow.AutoSize = true;
            this.lblEyebrow.BackColor = System.Drawing.Color.Transparent;
            this.lblEyebrow.Font = new System.Drawing.Font("Cinzel", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEyebrow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.lblEyebrow.Location = new System.Drawing.Point(72, 212);
            this.lblEyebrow.Name = "lblEyebrow";
            this.lblEyebrow.TabIndex = 10;
            this.lblEyebrow.Text = "WIZARDING POS";

            this.lblHeroTitle.AutoSize = true;
            this.lblHeroTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblHeroTitle.Font = new System.Drawing.Font("Cinzel", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeroTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.lblHeroTitle.Location = new System.Drawing.Point(68, 244);
            this.lblHeroTitle.Name = "lblHeroTitle";
            this.lblHeroTitle.TabIndex = 11;
            this.lblHeroTitle.Text = "Unlock the\r\nshelves.";

            this.lblHeroText.BackColor = System.Drawing.Color.Transparent;
            this.lblHeroText.Font = new System.Drawing.Font("Cormorant Garamond", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeroText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(217)))), ((int)(((byte)(173)))));
            this.lblHeroText.Location = new System.Drawing.Point(72, 376);
            this.lblHeroText.Name = "lblHeroText";
            this.lblHeroText.Size = new System.Drawing.Size(410, 84);
            this.lblHeroText.TabIndex = 12;
            this.lblHeroText.Text = "Sign in to ring up sales, restock the tomes, and keep the coop\'s ledger in order.";

            this.pnlChipBooks.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.pnlChipBooks.CornerRadius = 8;
            this.pnlChipBooks.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(90)))), ((int)(((byte)(18)))));
            this.pnlChipBooks.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(90)))), ((int)(((byte)(18)))));
            this.pnlChipBooks.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlChipBooks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.pnlChipBooks.Location = new System.Drawing.Point(72, 480);
            this.pnlChipBooks.Name = "pnlChipBooks";
            this.pnlChipBooks.Size = new System.Drawing.Size(84, 34);
            this.pnlChipBooks.TabIndex = 13;
            this.pnlChipBooks.TabStop = false;
            this.pnlChipBooks.Text = "BOOKS";

            this.pnlChipUniforms.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.pnlChipUniforms.CornerRadius = 8;
            this.pnlChipUniforms.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(90)))), ((int)(((byte)(58)))));
            this.pnlChipUniforms.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(90)))), ((int)(((byte)(58)))));
            this.pnlChipUniforms.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlChipUniforms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.pnlChipUniforms.Location = new System.Drawing.Point(166, 480);
            this.pnlChipUniforms.Name = "pnlChipUniforms";
            this.pnlChipUniforms.Size = new System.Drawing.Size(112, 34);
            this.pnlChipUniforms.TabIndex = 14;
            this.pnlChipUniforms.TabStop = false;
            this.pnlChipUniforms.Text = "UNIFORMS";

            this.pnlChipSupplies.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.pnlChipSupplies.CornerRadius = 8;
            this.pnlChipSupplies.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(74)))), ((int)(((byte)(16)))));
            this.pnlChipSupplies.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(74)))), ((int)(((byte)(16)))));
            this.pnlChipSupplies.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlChipSupplies.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.pnlChipSupplies.Location = new System.Drawing.Point(288, 480);
            this.pnlChipSupplies.Name = "pnlChipSupplies";
            this.pnlChipSupplies.Size = new System.Drawing.Size(108, 34);
            this.pnlChipSupplies.TabIndex = 15;
            this.pnlChipSupplies.TabStop = false;
            this.pnlChipSupplies.Text = "SUPPLIES";

            this.pnlChipMerch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(185)))), ((int)(((byte)(35)))));
            this.pnlChipMerch.CornerRadius = 8;
            this.pnlChipMerch.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.pnlChipMerch.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.pnlChipMerch.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlChipMerch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.pnlChipMerch.Location = new System.Drawing.Point(406, 480);
            this.pnlChipMerch.Name = "pnlChipMerch";
            this.pnlChipMerch.Size = new System.Drawing.Size(90, 34);
            this.pnlChipMerch.TabIndex = 16;
            this.pnlChipMerch.TabStop = false;
            this.pnlChipMerch.Text = "MERCH";

            // ---------------- scroll rod (gold bar on top of the card) ----------------
            this.pnlRod.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(76)))), ((int)(((byte)(12)))));
            this.pnlRod.BorderWidth = 1F;
            this.pnlRod.CornerRadius = 8;
            this.pnlRod.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(100)))), ((int)(((byte)(16)))));
            this.pnlRod.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(200)))), ((int)(((byte)(75)))));
            this.pnlRod.Location = new System.Drawing.Point(680, 121);
            this.pnlRod.Name = "pnlRod";
            this.pnlRod.Size = new System.Drawing.Size(456, 16);
            this.pnlRod.TabIndex = 18;
            this.pnlRod.TabStop = false;

            // ---------------- pnlScroll (parchment login card) ----------------
            this.pnlScroll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(154)))), ((int)(((byte)(30)))));
            this.pnlScroll.BorderWidth = 2F;
            this.pnlScroll.CornerRadius = 12;
            this.pnlScroll.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(213)))), ((int)(((byte)(150)))));
            this.pnlScroll.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(233)))), ((int)(((byte)(191)))));
            this.pnlScroll.Controls.Add(this.lblRoleNote);
            this.pnlScroll.Controls.Add(this.btnSignIn);
            this.pnlScroll.Controls.Add(this.lblForgot);
            this.pnlScroll.Controls.Add(this.chkRemember);
            this.pnlScroll.Controls.Add(this.pnlPass);
            this.pnlScroll.Controls.Add(this.lblPass);
            this.pnlScroll.Controls.Add(this.pnlUser);
            this.pnlScroll.Controls.Add(this.lblUser);
            this.pnlScroll.Controls.Add(this.btnAdmin);
            this.pnlScroll.Controls.Add(this.btnCashier);
            this.pnlScroll.Controls.Add(this.lblRole);
            this.pnlScroll.Controls.Add(this.lblSignInSub);
            this.pnlScroll.Controls.Add(this.lblSignIn);
            this.pnlScroll.Location = new System.Drawing.Point(688, 133);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(440, 502);
            this.pnlScroll.TabIndex = 17;
            this.pnlScroll.TabStop = false;

            this.lblSignIn.AutoSize = true;
            this.lblSignIn.BackColor = System.Drawing.Color.Transparent;
            this.lblSignIn.Font = new System.Drawing.Font("Cinzel", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.lblSignIn.Location = new System.Drawing.Point(30, 28);
            this.lblSignIn.Name = "lblSignIn";
            this.lblSignIn.TabIndex = 0;
            this.lblSignIn.Text = "SIGN IN";

            this.lblSignInSub.BackColor = System.Drawing.Color.Transparent;
            this.lblSignInSub.Font = new System.Drawing.Font("Cormorant Garamond", 13F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignInSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblSignInSub.Location = new System.Drawing.Point(32, 68);
            this.lblSignInSub.Name = "lblSignInSub";
            this.lblSignInSub.Size = new System.Drawing.Size(376, 24);
            this.lblSignInSub.TabIndex = 1;
            this.lblSignInSub.Text = "Choose your role, then show your credentials.";

            this.lblRole.AutoSize = true;
            this.lblRole.BackColor = System.Drawing.Color.Transparent;
            this.lblRole.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblRole.Location = new System.Drawing.Point(32, 106);
            this.lblRole.Name = "lblRole";
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "I AM A";

            this.btnCashier.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.btnCashier.CornerRadius = 21;
            this.btnCashier.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(15)))), ((int)(((byte)(26)))));
            this.btnCashier.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(15)))), ((int)(((byte)(26)))));
            this.btnCashier.Font = new System.Drawing.Font("Cinzel", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCashier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(230)))), ((int)(((byte)(184)))));
            this.btnCashier.Location = new System.Drawing.Point(32, 126);
            this.btnCashier.Name = "btnCashier";
            this.btnCashier.Size = new System.Drawing.Size(184, 42);
            this.btnCashier.TabIndex = 3;
            this.btnCashier.Text = "CASHIER";
            this.btnCashier.Click += new System.EventHandler(this.btnCashier_Click);

            this.btnAdmin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.btnAdmin.CornerRadius = 21;
            this.btnAdmin.Font = new System.Drawing.Font("Cinzel", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.btnAdmin.Location = new System.Drawing.Point(224, 126);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(184, 42);
            this.btnAdmin.TabIndex = 4;
            this.btnAdmin.Text = "ADMIN";
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);

            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblUser.Location = new System.Drawing.Point(32, 186);
            this.lblUser.Name = "lblUser";
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "USERNAME";

            this.pnlUser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(124)))), ((int)(((byte)(20)))));
            this.pnlUser.CornerRadius = 8;
            this.pnlUser.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.pnlUser.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.pnlUser.Controls.Add(this.txtUser);
            this.pnlUser.Location = new System.Drawing.Point(32, 206);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(376, 46);
            this.pnlUser.TabIndex = 6;
            this.pnlUser.TabStop = false;

            this.txtUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUser.Font = new System.Drawing.Font("Cormorant Garamond", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.txtUser.Location = new System.Drawing.Point(14, 10);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(348, 26);
            this.txtUser.TabIndex = 0;

            this.lblPass.AutoSize = true;
            this.lblPass.BackColor = System.Drawing.Color.Transparent;
            this.lblPass.Font = new System.Drawing.Font("Cinzel", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblPass.Location = new System.Drawing.Point(32, 268);
            this.lblPass.Name = "lblPass";
            this.lblPass.TabIndex = 7;
            this.lblPass.Text = "PASSWORD";

            this.pnlPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(124)))), ((int)(((byte)(20)))));
            this.pnlPass.CornerRadius = 8;
            this.pnlPass.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.pnlPass.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.pnlPass.Controls.Add(this.btnShowPass);
            this.pnlPass.Controls.Add(this.txtPass);
            this.pnlPass.Location = new System.Drawing.Point(32, 288);
            this.pnlPass.Name = "pnlPass";
            this.pnlPass.Size = new System.Drawing.Size(376, 46);
            this.pnlPass.TabIndex = 8;
            this.pnlPass.TabStop = false;

            this.txtPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(243)))), ((int)(((byte)(214)))));
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPass.Font = new System.Drawing.Font("Cormorant Garamond", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.txtPass.Location = new System.Drawing.Point(14, 10);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(270, 26);
            this.txtPass.TabIndex = 0;
            this.txtPass.UseSystemPasswordChar = true;

            this.btnShowPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(124)))), ((int)(((byte)(20)))));
            this.btnShowPass.BorderWidth = 1F;
            this.btnShowPass.CornerRadius = 6;
            this.btnShowPass.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(223)))), ((int)(((byte)(166)))));
            this.btnShowPass.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(223)))), ((int)(((byte)(166)))));
            this.btnShowPass.Font = new System.Drawing.Font("Cinzel", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.btnShowPass.Location = new System.Drawing.Point(298, 6);
            this.btnShowPass.Name = "btnShowPass";
            this.btnShowPass.Size = new System.Drawing.Size(72, 34);
            this.btnShowPass.TabIndex = 1;
            this.btnShowPass.Text = "SHOW";
            this.btnShowPass.Click += new System.EventHandler(this.btnShowPass_Click);

            this.chkRemember.AutoSize = true;
            this.chkRemember.BackColor = System.Drawing.Color.Transparent;
            this.chkRemember.Font = new System.Drawing.Font("Cormorant Garamond", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(16)))), ((int)(((byte)(32)))));
            this.chkRemember.Location = new System.Drawing.Point(32, 350);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.TabIndex = 9;
            this.chkRemember.Text = "Keep me signed in";
            this.chkRemember.UseVisualStyleBackColor = false;

            this.lblForgot.BackColor = System.Drawing.Color.Transparent;
            this.lblForgot.Font = new System.Drawing.Font("Cormorant Garamond", 12.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForgot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblForgot.Location = new System.Drawing.Point(212, 350);
            this.lblForgot.Name = "lblForgot";
            this.lblForgot.Size = new System.Drawing.Size(196, 24);
            this.lblForgot.TabIndex = 10;
            this.lblForgot.Text = "Forgot it? Ask an admin.";
            this.lblForgot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.btnSignIn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(76)))), ((int)(((byte)(12)))));
            this.btnSignIn.CornerRadius = 8;
            this.btnSignIn.FillBottom = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(142)))), ((int)(((byte)(18)))));
            this.btnSignIn.FillTop = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(200)))), ((int)(((byte)(75)))));
            this.btnSignIn.Font = new System.Drawing.Font("Cinzel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(15)))), ((int)(((byte)(26)))));
            this.btnSignIn.Location = new System.Drawing.Point(32, 390);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(376, 52);
            this.btnSignIn.TabIndex = 11;
            this.btnSignIn.Text = "ENTER AS CASHIER";
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);

            this.lblRoleNote.BackColor = System.Drawing.Color.Transparent;
            this.lblRoleNote.Font = new System.Drawing.Font("Cormorant Garamond", 12.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoleNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(63)))), ((int)(((byte)(51)))));
            this.lblRoleNote.Location = new System.Drawing.Point(32, 458);
            this.lblRoleNote.Name = "lblRoleNote";
            this.lblRoleNote.Size = new System.Drawing.Size(376, 24);
            this.lblRoleNote.TabIndex = 12;
            this.lblRoleNote.Text = "Cashiers ring up sales and check stock.";
            this.lblRoleNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ---------------- LoginPage (the form) ----------------
            this.AcceptButton = this.btnSignIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(8)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.pnlRod);
            this.Controls.Add(this.pnlScroll);
            this.Controls.Add(this.pnlChipMerch);
            this.Controls.Add(this.pnlChipSupplies);
            this.Controls.Add(this.pnlChipUniforms);
            this.Controls.Add(this.pnlChipBooks);
            this.Controls.Add(this.lblHeroText);
            this.Controls.Add(this.lblHeroTitle);
            this.Controls.Add(this.lblEyebrow);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UMak Coop - Login";
            this.Load += new System.EventHandler(this.LoginPage_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            this.pnlScroll.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlPass.ResumeLayout(false);
            this.pnlPass.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlCrest;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblBrandSub;
        private RoundedPanel pnlStaff;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Label lblEyebrow;
        private System.Windows.Forms.Label lblHeroTitle;
        private System.Windows.Forms.Label lblHeroText;
        private RoundedPanel pnlChipBooks;
        private RoundedPanel pnlChipUniforms;
        private RoundedPanel pnlChipSupplies;
        private RoundedPanel pnlChipMerch;
        private RoundedPanel pnlRod;
        private RoundedPanel pnlScroll;
        private System.Windows.Forms.Label lblSignIn;
        private System.Windows.Forms.Label lblSignInSub;
        private System.Windows.Forms.Label lblRole;
        private RoundedButton btnCashier;
        private RoundedButton btnAdmin;
        private System.Windows.Forms.Label lblUser;
        private RoundedPanel pnlUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPass;
        private RoundedPanel pnlPass;
        private System.Windows.Forms.TextBox txtPass;
        private RoundedButton btnShowPass;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.Label lblForgot;
        private RoundedButton btnSignIn;
        private System.Windows.Forms.Label lblRoleNote;
    }
}