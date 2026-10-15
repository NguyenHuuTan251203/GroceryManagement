using hd;
using QuanLyTapHoa;
using QuanLyTapHoa.View.GoodsManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GroceryManagement.Models;
using GroceryManagement.View;
using Guna.UI2.WinForms;

namespace GroceryManagement
{
    public partial class AdminForm : Form
    {
     
        public AdminForm()
        {
            InitializeComponent();
        
        }
        private Form currentFormChild;
        private void OpenchildForm(Form childform)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            panel_body.Controls.Add(childform);
            panel_body.Tag = childform;
            childform.BringToFront();
            childform.Show();

        }
        private void Admin_Load(object sender, EventArgs e)
        {
       
        }

        private void menuQLSP_Click(object sender, EventArgs e)
        {
            OpenchildForm(new GoodsManagementForm());

            changeColor(menuTK, false);
            changeColor(menuQLHD, false);
            changeColor(menuQLKH, false);
            changeColor(menuQLSP, true);
            changeColor(menuQLTK, false);
        }

        private void menuQLTK_Click(object sender, EventArgs e)
        {
            OpenchildForm(new AccountManagerForm());

            changeColor(menuTK, false);
            changeColor(menuQLHD, false);
            changeColor(menuQLKH, false);
            changeColor(menuQLSP, false);
            changeColor(menuQLTK, true);
        }

        private void menuQLHD_Click(object sender, EventArgs e)
        {
            OpenchildForm(new BillManagerForm());

            changeColor(menuTK, false);
            changeColor(menuQLHD, true);
            changeColor(menuQLKH, false);
            changeColor(menuQLSP, false);
            changeColor(menuQLTK, false);
        }

        private void menuQLKH_Click(object sender, EventArgs e)
        {
            OpenchildForm(new CustomerManagerForm());

            changeColor(menuTK, false);
            changeColor(menuQLHD, false);
            changeColor(menuQLKH, true);
            changeColor(menuQLSP, false);
            changeColor(menuQLTK, false);
        }

        private void menuTK_Click(object sender, EventArgs e)
        {
            OpenchildForm(new ThongKeForm());

            changeColor(menuTK, true);
            changeColor(menuQLHD, false);
            changeColor(menuQLKH, false);
            changeColor(menuQLSP, false);
            changeColor(menuQLTK, false);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm f = new LoginForm();
            f.Show();
        }

        void changeColor(Guna2Button btn, bool truefalse)
        {
            if (truefalse)
                btn.FillColor = Color.MintCream;
            else
                btn.FillColor = Color.FromArgb(213, 218, 223);
        }
    }
}
