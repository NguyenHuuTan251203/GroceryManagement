using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GroceryManagement.Models;

namespace GroceryManagement.View
{
    public partial class AccountManagerForm : Form
    {
        string MaNV = null;
        public AccountManagerForm()
        {
            InitializeComponent();

        }
        // Get data from table NhanVien
        public void ShowData()
        {
            dtgvAccount.DataSource = DBConText.db.Nhanviens
                                     .Select(s => new
                                     {
                                         MaNV = s.MaNV,
                                         TenNV = s.TenNV,
                                         Diachi = s.Diachi,
                                         Matkhau = s.Matkhau,
                                         Quyen = s.Quyen,
                                         Tentaikhoan = s.Tentaikhoan
                                     })
                                     .ToList();
            dtgvAccount.Columns["MaNV"].HeaderText = "Mã Nhân Viên";
            dtgvAccount.Columns["TenNV"].HeaderText = "Tên Nhân Viên";
            dtgvAccount.Columns["Diachi"].HeaderText = "Địa Chỉ";
            dtgvAccount.Columns["Matkhau"].HeaderText = "Mật Khẩu";
            dtgvAccount.Columns["Quyen"].HeaderText = "Quyền";
            dtgvAccount.Columns["Tentaikhoan"].HeaderText = "Tên Tài Khoản";
        }
        private void AccountManager_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        //Check
        public bool CheckInputText()
        {
            if (!Validation.IsNotNull(txtFullName.Text, txtAddress.Text, txtPassword.Text, cbbDistribution.Text))
            {
                new MessageCustomForm(Const.Error_Input_Is_Null).Show();
                return false;
            }
            return true;

        }
        public bool CheckRowSelected()
        {
            if (MaNV == null)
            {
                new MessageCustomForm(Const.Error_No_Selected_Row).Show();
                return false;
            }
            return true;
        }

        //Clear text after control was used
        void ClearText()
        {
            txtFullName.ResetText();
            txtAddress.ResetText();
            txtPassword.ResetText();
            cbbDistribution.ResetText();
        }


        //Get data from text controls
        private void dtgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dtgvAccount.CurrentRow.Index;
            MaNV = dtgvAccount.Rows[i].Cells[0].Value.ToString();
            txtFullName.Text = dtgvAccount.Rows[i].Cells[1].Value.ToString();
            txtAddress.Text = dtgvAccount.Rows[i].Cells[2].Value.ToString();
            txtPassword.Text = dtgvAccount.Rows[i].Cells[3].Value.ToString();
            cbbDistribution.Text = dtgvAccount.Rows[i].Cells[4].Value.ToString();
        }

        //UpDate data
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (CheckRowSelected())
            {
                if (CheckInputText())
                {
                    var upDateNhanVien = DBConText.db.Nhanviens.Find(MaNV);
                    upDateNhanVien.TenNV = txtFullName.Text;
                    upDateNhanVien.Diachi = txtAddress.Text;
                    upDateNhanVien.Matkhau = txtPassword.Text;
                    upDateNhanVien.Quyen = cbbDistribution.Text;
                    DBConText.db.SaveChanges();
                    ClearText();
                    ShowData();
                }
            }
        }

        //delete data
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (CheckRowSelected())
            {
                // Get List NhanVien to delete
                var deleteNhanVien = DBConText.db.Nhanviens.Find(MaNV);

                // Get All HoaDon's NhanVien to delete 
                var delteHoaDon = DBConText.db.Hoadons.Where(x => x.MaNV == MaNV).ToList();

                // Get List SoHD's relate with MaNV
                var soHDList = delteHoaDon.Select(h => h.SoHD).ToList();

                // Get All CT_HoaDons to delete
                var delteCT_HoaDon = DBConText.db.CT_Hoadon
                    .Where(x => soHDList.Contains(x.SoHD))  // So sánh với danh sách SoHD
                    .ToList();

                // Delete CT_HoaDons
                DBConText.db.CT_Hoadon.RemoveRange(delteCT_HoaDon);

                // Delete HoaDons
                DBConText.db.Hoadons.RemoveRange(delteHoaDon);

                // Delte NhanVien
                DBConText.db.Nhanviens.Remove(deleteNhanVien);

                DBConText.db.SaveChanges();
                ClearText();
            }
            ShowData();
        }


    }
}
