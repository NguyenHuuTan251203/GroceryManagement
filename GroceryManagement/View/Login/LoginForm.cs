using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement.Models;
using GroceryManagement.View;
using GroceryManagement.Service;
using GroceryManagement.View.User;

namespace GroceryManagement
{
    public partial class LoginForm : Form
    {
        string userName, password, distribution;
        public LoginForm()
        {
            InitializeComponent();
        }
        private void GetInput()
        {
            userName = login_TxtUserName.Text;
            password = login_TxtPassword.Text;
            distribution = Login_CbbDistribution.Text;
        }
        private async Task<bool> Check()
        {
            if (!Validation.IsNotNull(login_TxtPassword.Text, login_TxtUserName.Text))
            {
                Const.error.Append(Const.Error_Input_Is_Null);
                return false;
            }
            GetInput();
            var progressForm = new ProcessbarForm();
            progressForm.Show();
            return await Task.Run(() =>
            {
                if (Validation.CheckAccount(userName, password, distribution) == null)
                {
                    Const.error.Append(Const.Error_Incorrect_Account);
                    return false;
                }
                return true;
            });
        }
        private async void Login_BtLogin_Click(object sender, EventArgs e)
        {
            bool check = await Check();
            if (check)
            {
                //var frm = distribution.Equals("Admin") ? (Form)new AdminForm() : new EmployeeForm();
                Form frm;
                if (distribution.Equals("Admin"))
                {
                    DataAcess.ht = 1;
                    frm = new AdminForm();
                   
                }
                else
                {
                    DataAcess.ht = 0;
                    frm = new EmployeeForm();
                    
                }
                //frm.ShowDialog();
                GetName();
                frm.Show();
                this.Hide();
            }
            else
            {
                new MessageCustomForm(Const.error.ToString()).Show();
                TextProcess.ResetText(login_TxtPassword, login_TxtUserName);
                Const.error.Clear();
            }
        }
        private void GetName()
        {
            var epl = DBConText.db.Nhanviens.FirstOrDefault(x => x.Tentaikhoan.Equals(login_TxtUserName.Text));
            Program.NameUser = epl.TenNV;
            Program.maNV = epl.MaNV;
        }
        private void ctrlExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Login_BtRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm frm = new RegistrationForm();
            frm.FormClosed += (s, args) => this.Show();
            frm.ShowDialog();
        }
    }
}
