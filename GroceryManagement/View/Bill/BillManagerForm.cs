using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement;
using GroceryManagement.DataAccess;
using Guna.UI2.WinForms;

namespace hd
{
    public partial class BillManagerForm : Form
    {
        public BillManagerForm()
        {
            InitializeComponent();

        }
        public void CLearData()
        {
            dtgv.DataSource = null;
            dtgv.Rows.Clear();
            dtgv.Columns.Clear();
        }
        public void Loaddulieu()
        {
            CLearData();
            laySHD();
            string sql = @"
            SELECT 
    Hoadon.SoHD,  -- Số hóa đơn
    Khachhang.TenKH,  -- Tên khách hàng
    Nhanvien.TenNV,  -- Tên nhân viên
    Hoadon.Ngayban,  -- Ngày bán
    SUM(CT_Hoadon.Soluong) AS TongSoluong, 
    SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS TongTien  
FROM 
    Hoadon
INNER JOIN 
    CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD  
INNER JOIN 
    Khachhang ON Hoadon.MaKH = Khachhang.MaKH  
INNER JOIN 
    Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV  
GROUP BY 
    Hoadon.SoHD, 
    Khachhang.TenKH, 
    Nhanvien.TenNV, 
    Hoadon.Ngayban  
ORDER BY 
    Hoadon.SoHD DESC;";
            DataTable dt = DataAcess.getTable(sql);
            dtgv.DataSource = dt;

            CustomizeDataGridView();


            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Chi tiết";
            btn.Text = "Xem chi tiết";
            btn.Name = "ChiTiet";
            btn.UseColumnTextForButtonValue = true;
            dtgv.Columns.Add(btn);
            dtgv.Columns["TongTien"].DefaultCellStyle.Format = "C0";
            dtgv.Columns["TongTien"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Loaddulieu();

            cbSoHD.DropDownStyle = ComboBoxStyle.DropDown;

        }

        private void CustomizeDataGridView()
        {
            dtgv.Columns["SoHD"].HeaderText = "Số HĐ";
            dtgv.Columns["TenKH"].HeaderText = "Tên KH";
            dtgv.Columns["TenNV"].HeaderText = "Tên NV";
            dtgv.Columns["Ngayban"].HeaderText = "Ngày bán";
            dtgv.Columns["TongSoluong"].HeaderText = "Số Lượng";
            dtgv.Columns["TongTien"].HeaderText = "Tổng tiền";
            dtgv.Columns["Ngayban"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dtgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dtgv.Columns["ChiTiet"].Index && e.RowIndex >= 0)
            {
                string soHD = dtgv.Rows[e.RowIndex].Cells["SoHD"].Value.ToString();

                DetailBillForm formChiTiet = new DetailBillForm(soHD);
                formChiTiet.fromBill = this;
                formChiTiet.ShowDialog();
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbSoHD.Text))
            {
                string txtSoHD = cbSoHD.Text.Trim();

                bool matchFound = false;
                foreach (var item in cbSoHD.Items)
                {
                    if (txtSoHD.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    MessageBox.Show("Vui lòng chọn một giá trị từ danh sách.");
                    cbSoHD.Focus();
                    return; 
                }
                string sql = @"
SELECT 
    Hoadon.SoHD,  -- Số hóa đơn
    Khachhang.TenKH,  -- Tên khách hàng
    Nhanvien.TenNV,  -- Tên nhân viên
    Hoadon.Ngayban,  -- Ngày bán
    SUM(CT_Hoadon.Soluong) AS TongSoluong, 
    SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS TongTien  
FROM 
    Hoadon
INNER JOIN 
    CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD  
INNER JOIN 
    Khachhang ON Hoadon.MaKH = Khachhang.MaKH  
INNER JOIN 
    Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV  
WHERE Hoadon.SoHD = '" + txtSoHD + @"'
GROUP BY 
    Hoadon.SoHD, 
    Khachhang.TenKH, 
    Nhanvien.TenNV, 
    Hoadon.Ngayban";
                DataTable dt = DataAcess.getTable(sql);
                dtgv.DataSource = dt;
            }
            else
            {
                string sql = @"
            SELECT 
    Hoadon.SoHD,  -- Số hóa đơn
    Khachhang.TenKH,  -- Tên khách hàng
    Nhanvien.TenNV,  -- Tên nhân viên
    Hoadon.Ngayban,  -- Ngày bán
    SUM(CT_Hoadon.Soluong) AS TongSoluong, 
    SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS TongTien  
FROM 
    Hoadon
INNER JOIN 
    CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD  
INNER JOIN 
    Khachhang ON Hoadon.MaKH = Khachhang.MaKH  
INNER JOIN 
    Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV  
GROUP BY 
    Hoadon.SoHD, 
    Khachhang.TenKH, 
    Nhanvien.TenNV, 
    Hoadon.Ngayban  
ORDER BY 
    Hoadon.SoHD;";
                DataTable dt = DataAcess.getTable(sql);
                dtgv.DataSource = dt;
            }


            // }
        }

        private void laySHD()
        {
            string sql = "SELECT SoHD FROM HoaDon";
            DataTable dt = DataAcess.getTable(sql);
            cbSoHD.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string soHD = row["SoHD"].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(soHD)) 
                {
                    cbSoHD.Items.Add(soHD);
                }
            }
        }

        private void btLoadData_Click(object sender, EventArgs e)
        {
            Loaddulieu();
        }
    }
}
