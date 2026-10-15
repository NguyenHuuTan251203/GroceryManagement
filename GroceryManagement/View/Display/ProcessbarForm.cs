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
    public partial class ProcessbarForm : Form
    {
        public ProcessbarForm()
        {
            InitializeComponent();
        }

        private void Processbar_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
           
            if (pbProcess.Value < 100)
            {
                await Task.Delay(0);
                pbProcess.Value += 20;
                guna2HtmlLabel2.Text = pbProcess.Value.ToString() + "%";
            }
            else
            {
                timer1.Stop();
                pbProcess.Value = 0;
                this.Close();
            }
        }
    }
}
