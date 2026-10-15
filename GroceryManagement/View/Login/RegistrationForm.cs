using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement.Models;
using GroceryManagement.Controller;
using GroceryManagement.View;
using GroceryManagement.Service;
using System.Threading;
using System.Text.RegularExpressions;
namespace GroceryManagement
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }
        public string SetMA()
        {
            // Lấy mã của bản ghi cuối cùng từ database
            var lastRecord = DBConText.db.Nhanviens
                              .OrderByDescending(nv => nv.MaNV) 
                              .FirstOrDefault()?.MaNV;         

            int newNumber = 1; // Giá trị mặc định nếu chưa có bản ghi nào

            if (!string.IsNullOrEmpty(lastRecord))
            {
                // Tách số từ mã nhân viên cuối cùng
                string numberPart = Regex.Replace(lastRecord, @"\D", ""); 
                newNumber = int.Parse(numberPart) + 1;                   
            }
            string newMa = "NV" + newNumber.ToString("D3");
            return newMa;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public async Task<bool> Check()
        {
            Const.error.Clear();
            if (!Validation.IsNotNull(register_FullName.Text, register_Password.Text,register_FullName.Text,register_Address.Text))
            {
                Const.error.Append(Const.Error_Input_Is_Null);
                return false;
            }
            var progressForm = new ProcessbarForm();
            progressForm.Show();
            return await Task.Run(() => {
                if (Validation.CheckUserNameExists(register_UserName.Text) != null)
                {
                    Const.error.Append(Const.Error_UserName_Exist);
                    return false;
                }
                return true;
            });
        }

        private async void btRegister_Click(object sender, EventArgs e)
        {
            bool check = await Check();
            if (check)
            {
                Add.Account(register_FullName.Text, register_UserName.Text, register_Password.Text, SetMA(),register_Address.Text);
            }
            else
            {
                new MessageCustomForm(Const.error.ToString()).Show();
                TextProcess.ResetText(register_FullName, register_Password,register_UserName,register_Address);
            }
        }
    }
}