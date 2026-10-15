using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroceryManagement.View
{
    public partial class MessageCustomForm : Form
    {
        public MessageCustomForm(string msg)
        {
            InitializeComponent();
            guna2HtmlLabel1.Text = msg;
        }
        private void guna2GradientTileButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
