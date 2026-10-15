using GroceryManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement.DataAccess;
using GroceryManagement;
namespace hd
{
    public partial class FormLY : Form
    {
        public DetailBillForm formDetail;
        private string soHD = "HD001";
        public FormLY(string soHD)
        {
            InitializeComponent();
            this.soHD = soHD;

            layMaNV();
            layMaKH();
            dtimeNgayBan.Format = DateTimePickerFormat.Custom;
            dtimeNgayBan.CustomFormat = "dd-MM-yyyy";
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            layDL();
        }
        private void layDL()
        {
            string sql = @"
SELECT
	TRIM(Hoadon.SoHD) AS SoHD,
	Hoadon.Ngayban,
	TRIM(Nhanvien.MaNV) AS MaNV,
    TRIM(Hanghoa.Tenhang) AS Tenhang,
	TRIM(Nhanvien.TenNV) AS TenNV,
	TRIM(Khachhang.MaKH) AS MaKH,
	TRIM(Khachhang.TenKH) AS TenKH,
    TRIM(Hanghoa.Mahang) AS Mahang,
	CT_Hoadon.Soluong,
	TRIM(Hanghoa.Tenhang) AS Tenhang,
	Hanghoa.Dongia  
FROM 
    CT_Hoadon 
INNER JOIN 
    Hanghoa ON CT_Hoadon.Mahang = Hanghoa.Mahang
INNER JOIN 
    Hoadon ON CT_Hoadon.SoHD = Hoadon.SoHD
INNER JOIN 
    Khachhang ON Hoadon.MaKH = Khachhang.MaKH
INNER JOIN 
    Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV 
WHERE 
    Hoadon.SoHD = '" + soHD + "'";

            DataTable dt = DataAcess.getTable(sql);


            dtgv.DataSource = dt; 

            CustomizeDataGridView();

            if (dt.Rows.Count > 0)
            {
                DisplaySelectedRowData(dt.Rows[0]);
            }
            dtgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
            private void CustomizeDataGridView()
        {
            dtgv.Columns["Mahang"].HeaderText = "Mã hàng"; 
            dtgv.Columns["Tenhang"].HeaderText = "Tên hàng"; 
            dtgv.Columns["Soluong"].HeaderText = "Số lượng"; 
            dtgv.Columns["Dongia"].HeaderText = "Đơn giá"; 


            dtgv.Columns["SoHD"].Visible = false;
            dtgv.Columns["Ngayban"].Visible = false;
            dtgv.Columns["MaNV"].Visible = false;
            dtgv.Columns["TenNV"].Visible = false;
            dtgv.Columns["MaKH"].Visible = false;
            dtgv.Columns["TenKH"].Visible = false;

            dtgv.Columns["Dongia"].DefaultCellStyle.Format = "C0";
            dtgv.Columns["Dongia"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
        }

        private void DisplaySelectedRowData(DataRow row)
        {
            cbSoHD.Text = row["SoHD"].ToString(); 
            dtimeNgayBan.Value = Convert.ToDateTime(row["Ngayban"]); 
            cbMaNV.Text = row["MaNV"].ToString(); 
            txtTenNV.Text = row["TenNV"].ToString(); 
            cbMaKH.Text = row["MaKH"].ToString(); 
            txtTenKH.Text = row["TenKH"].ToString(); 
            cbMaHang.Text = row["Mahang"].ToString();
            txtSL.Text = row["Soluong"].ToString();
            txtTenHang.Text = row["Tenhang"].ToString();
            var cellDongia = row["Dongia"];
            txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", cellDongia);
        }

        private void dtgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cbMaHang.Text = dtgv.CurrentRow.Cells["Mahang"].Value.ToString();
            txtTenHang.Text = dtgv.CurrentRow.Cells["Tenhang"].Value.ToString();
            txtSL.Text = dtgv.CurrentRow.Cells["Soluong"].Value.ToString();
            var cellDongia = dtgv.CurrentRow.Cells["Dongia"].Value;
            txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", cellDongia);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string txtMaNV = cbMaNV.Text.Trim();
            bool matchFound = false;
            foreach (var item in cbMaNV.Items)
            {
                if (txtMaNV.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
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
            string dateformat = dtimeNgayBan.Value.ToString("yyyy-MM-dd");
            string sql = "UPDATE Hoadon set  Ngayban = N'" + dateformat +
                "', MaKH = N'" + cbMaKH.Text + "', MaNV = N'" + cbMaNV.Text + "' WHERE SoHD = N'" + cbSoHD.Text + "'";
            DataAcess.insertDeleteUpdate(sql);


            foreach (DataGridViewRow row in dtgv.Rows)
            {
                if (row.IsNewRow) continue;
                string maMH = row.Cells["Mahang"].Value.ToString();
                int soLuong = int.Parse(row.Cells["Soluong"].Value.ToString());
                string sql2 = "UPDATE CT_Hoadon set Soluong = N'" + soLuong + "' WHERE SoHD = N'" + cbSoHD.Text + "' AND Mahang = N'" + maMH + "'";
                DataAcess.insertDeleteUpdate(sql2);
            }
        }

        private void layTenNV(string maNV)
        {
           string sql = " SELECT Nhanvien.TenNV FROM Nhanvien WHERE Nhanvien.MaNV = N'"+ maNV + "' ";
           string tenNV = DataAcess.getValue(sql);

            txtTenNV.Text = tenNV;
        }

        private void layMaNV()
        {
            string sql = "SELECT Nhanvien.MaNV FROM Nhanvien";
            DataTable dt = DataAcess.getTable(sql);
            cbMaNV.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string maNV = row["MaNV"].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(maNV))
                {
                    cbMaNV.Items.Add(maNV);
                }
            }
        }

        private void cbMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string maNV = cbMaNV.SelectedItem.ToString();
            layTenNV(maNV);
        }

        private void cbMaNV_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void layTenKH(string maKH)
        {
            string sql = " SELECT Khachhang.TenKH FROM Khachhang WHERE Khachhang.MaKH = N'" + maKH + "' ";
            string tenKH = DataAcess.getValue(sql);

            txtTenKH.Text = tenKH;
        }

        private void layMaKH()
        {
            string sql = "SELECT Khachhang.MaKH FROM Khachhang";
            DataTable dt = DataAcess.getTable(sql);
            cbMaKH.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string maKH = row["MaKH"].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(maKH)) 
                {
                    cbMaKH.Items.Add(maKH);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSP = cbMaHang.Text;
            int soLuongMoi;
            if (!int.TryParse(txtSL.Text, out soLuongMoi) || soLuongMoi <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ.");
                return;
            }
            foreach (DataGridViewRow row in dtgv.Rows)
            {
                if (row.Cells["Mahang"].Value != null && row.Cells["Mahang"].Value.ToString() == maSP)
                {
                    row.Cells["Soluong"].Value = soLuongMoi;
                    MessageBox.Show("Sửa thành công.");
                    return;
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (cbMaHang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn người cần xóa");
            }
            else if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa hay không", "TB", MessageBoxButtons.YesNo))
            {
                foreach (DataGridViewRow row in dtgv.SelectedRows)
                {
                    string maSP = row.Cells["Mahang"].Value.ToString();
                    string sql2 = "DELETE FROM CT_Hoadon WHERE SoHD = N'" + cbSoHD.Text + "' AND Mahang = N'" + maSP + "'";
                    DataAcess.insertDeleteUpdate(sql2);
                }
            }
            layDL();
        }

        private void cbMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maKH = cbMaKH.SelectedItem.ToString();
            layTenKH(maKH);
        }

        private void FormLY_FormClosed(object sender, FormClosedEventArgs e)
        {
            formDetail?.LoadData();
        }
    }
}

