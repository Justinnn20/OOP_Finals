using System;
using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    public partial class Form2 : Form
    {
        private Button[] tabs;

        public Form2()
        {
            InitializeComponent();
            SetupTabs();
        }

        private void SetupTabs()
        {
            // Lahat ng sidebar button ay kasama sa listahan
            tabs = new[]
            {
                btnTabAll, btnTabBooks, btnTabUniforms, btnTabSupplies,
                btnStockIn, btnTransactionLogs, btnCashierPos
            };

            btnTabAll.Click += (s, e) => SelectTab(btnTabAll);
            btnTabBooks.Click += (s, e) => SelectTab(btnTabBooks);
            btnTabUniforms.Click += (s, e) => SelectTab(btnTabUniforms);
            btnTabSupplies.Click += (s, e) => SelectTab(btnTabSupplies);
            btnStockIn.Click += (s, e) => SelectTab(btnStockIn);
            btnTransactionLogs.Click += (s, e) => SelectTab(btnTransactionLogs);
            btnCashierPos.Click += (s, e) => SelectTab(btnCashierPos);

            SelectTab(btnTabAll);
        }

        // Highlight lang ng napiling tab
        private void SelectTab(Button selected)
        {
            foreach (Button tab in tabs)
            {
                bool isSelected = tab == selected;
                tab.BackColor = isSelected ? Color.FromArgb(255, 213, 79) : Color.FromArgb(30, 10, 14);
                tab.ForeColor = isSelected ? Color.FromArgb(60, 20, 25) : Color.FromArgb(245, 230, 200);
            }
        }

        // Kailangan ito ng designer kahit walang laman pa
        private void btnScanId_Click(object sender, EventArgs e)
        {
        }
    }
}