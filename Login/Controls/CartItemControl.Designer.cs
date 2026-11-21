namespace Login.Controls
{
    partial class CartItemControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            lblItemName = new System.Windows.Forms.Label();
            lblItemDetail = new System.Windows.Forms.Label();
            lblItemSubtotal = new System.Windows.Forms.Label();
            btnMinus = new System.Windows.Forms.Button();
            lblItemQty = new System.Windows.Forms.Label();
            btnPlus = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblItemName
            // 
            lblItemName.AutoEllipsis = true;
            lblItemName.Font = new System.Drawing.Font("Georgia", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblItemName.ForeColor = System.Drawing.Color.FromArgb(44, 16, 22);
            lblItemName.Location = new System.Drawing.Point(8, 8);
            lblItemName.Name = "lblItemName";
            lblItemName.Size = new System.Drawing.Size(165, 20);
            lblItemName.TabIndex = 0;
            lblItemName.Text = "Item Name";
            // 
            // lblItemDetail
            // 
            lblItemDetail.AutoEllipsis = true;
            lblItemDetail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblItemDetail.ForeColor = System.Drawing.Color.FromArgb(125, 90, 75);
            lblItemDetail.Location = new System.Drawing.Point(8, 28);
            lblItemDetail.Name = "lblItemDetail";
            lblItemDetail.Size = new System.Drawing.Size(165, 18);
            lblItemDetail.TabIndex = 1;
            lblItemDetail.Text = "Details · ₱0 each";
            // 
            // lblItemSubtotal
            // 
            lblItemSubtotal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblItemSubtotal.Font = new System.Drawing.Font("Georgia", 11.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblItemSubtotal.ForeColor = System.Drawing.Color.FromArgb(44, 16, 22);
            lblItemSubtotal.Location = new System.Drawing.Point(170, 8);
            lblItemSubtotal.Name = "lblItemSubtotal";
            lblItemSubtotal.Size = new System.Drawing.Size(114, 22);
            lblItemSubtotal.TabIndex = 2;
            lblItemSubtotal.Text = "₱0";
            lblItemSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMinus
            // 
            btnMinus.BackColor = System.Drawing.Color.FromArgb(250, 240, 215);
            btnMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            btnMinus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 175, 130);
            btnMinus.FlatAppearance.BorderSize = 1;
            btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMinus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnMinus.ForeColor = System.Drawing.Color.FromArgb(60, 20, 25);
            btnMinus.Location = new System.Drawing.Point(8, 50);
            btnMinus.Name = "btnMinus";
            btnMinus.Size = new System.Drawing.Size(26, 24);
            btnMinus.TabIndex = 3;
            btnMinus.Text = "−";
            btnMinus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            btnMinus.UseVisualStyleBackColor = false;
            // 
            // lblItemQty
            // 
            lblItemQty.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblItemQty.ForeColor = System.Drawing.Color.FromArgb(44, 16, 22);
            lblItemQty.Location = new System.Drawing.Point(36, 50);
            lblItemQty.Name = "lblItemQty";
            lblItemQty.Size = new System.Drawing.Size(24, 24);
            lblItemQty.TabIndex = 4;
            lblItemQty.Text = "1";
            lblItemQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPlus
            // 
            btnPlus.BackColor = System.Drawing.Color.FromArgb(250, 240, 215);
            btnPlus.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPlus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 175, 130);
            btnPlus.FlatAppearance.BorderSize = 1;
            btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPlus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnPlus.ForeColor = System.Drawing.Color.FromArgb(60, 20, 25);
            btnPlus.Location = new System.Drawing.Point(62, 50);
            btnPlus.Name = "btnPlus";
            btnPlus.Size = new System.Drawing.Size(26, 24);
            btnPlus.TabIndex = 5;
            btnPlus.Text = "+";
            btnPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            btnPlus.UseVisualStyleBackColor = false;
            // 
            // CartItemControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(243, 225, 180);
            Controls.Add(btnPlus);
            Controls.Add(lblItemQty);
            Controls.Add(btnMinus);
            Controls.Add(lblItemSubtotal);
            Controls.Add(lblItemDetail);
            Controls.Add(lblItemName);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "CartItemControl";
            Size = new System.Drawing.Size(292, 82);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label lblItemDetail;
        private System.Windows.Forms.Label lblItemSubtotal;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Label lblItemQty;
        private System.Windows.Forms.Button btnPlus;
    }
}
