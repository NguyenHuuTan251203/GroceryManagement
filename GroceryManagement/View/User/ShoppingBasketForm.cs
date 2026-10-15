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
using QuanLyTapHoa;

namespace GroceryManagement.View.User
{
    public partial class ShoppingBasketForm : Form
    {
        internal BindingList<ShoppingBasket> list = new BindingList<ShoppingBasket>();

        public ShoppingBasketForm()
        {
            InitializeComponent();
        }

        void loadData()
        {
            dtgvSP.DataSource = list;

            dtgvSP.Columns["DonGia"].DefaultCellStyle.Format = "C0";
            dtgvSP.Columns["DonGia"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");

            dtgvSP.Columns["ThanhTien"].DefaultCellStyle.Format = "C0";
            dtgvSP.Columns["ThanhTien"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");

            lblTotal.Text = tinhTongTien().ToString("N0") + " đ";
        }

        decimal tinhTongTien()
        {
            decimal tongtien = 0;
            foreach (var item in list)
            {
                tongtien += item.ThanhTien;
            }
            return tongtien;
        }

        private void ShoppingBasketForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void dtgvSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgvSP.Rows[e.RowIndex];
                ShoppingBasket sp = row.DataBoundItem as ShoppingBasket;
                if (e.ColumnIndex == dtgvSP.Columns["up"].Index)
                {
                    if (sp.SoLuong < sp.SLTonKho)
                    {
                        sp.SoLuong++;
                        var tongtien = sp.DonGia * sp.SoLuong;
                        row.Cells["ThanhTien"].Value = tongtien;
                    }
                    else
                        new MessageCustomForm("Xin lỗi quý khách sản phẩm này trong kho không đủ số lượng mua cho quý khách!!").Show();
                }
                else if (e.ColumnIndex == dtgvSP.Columns["down"].Index)
                {
                    if (sp.SoLuong > 1)
                    {
                        sp.SoLuong--;
                        var tongtien = sp.DonGia * sp.SoLuong;
                        row.Cells["ThanhTien"].Value = tongtien;
                    }
                }
                else if (e.ColumnIndex == dtgvSP.Columns["Removee"].Index)
                {
                    list.Remove(sp);
                }

                dtgvSP.Refresh();
                //loadData();
                lblTotal.Text = tinhTongTien().ToString("N0") + " đ";
            }
        }

        //void getDataInDTGV()
        //{
        //    foreach (DataGridViewRow row in dtgvSP.Rows)
        //    {
        //        string maSP = row.Cells["Mahang"].Value.ToString();
        //        string tenSP = row.Cells["Tenhang"].Value.ToString();
        //        int soLuong = Convert.ToInt32(row.Cells["SLMua"].Value);
        //        int soLuongTonKho = Convert.ToInt32(row.Cells["SoLuong"].Value);
        //        string donViTinh = row.Cells["DVT"].Value.ToString();
        //        decimal donGia = Convert.ToDecimal(row.Cells["Dongia"].Value);
        //        decimal thanhtien = Convert.ToDecimal(row.Cells["ThanhTien"].Value);
        //    }
        //}

        private void ShoppingBasketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
