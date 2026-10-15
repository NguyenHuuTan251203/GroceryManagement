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

namespace hd
{
    public partial class DetailBillForm : Form
    {
        private string soHD;
        public BillManagerForm fromBill;
        public DetailBillForm(string soHD)
        {
            InitializeComponent();
            this.soHD = soHD;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            if (DataAcess.ht == 1)
            {
                btnSua.Visible = true;
                btnXoa.Visible = true;
            }
            dtimeNgayBan.Format = DateTimePickerFormat.Custom;
            dtimeNgayBan.CustomFormat = "dd-MM-yyyy"; 

        }

        public void LoadData()
        {
            string sql = @"
            SELECT 
    Hanghoa.Tenhang,
    Hanghoa.DVT,
    Hanghoa.Dongia,
	CT_Hoadon.Soluong,
	CT_Hoadon.Soluong * Hanghoa.Dongia AS ThanhTien,
    Khachhang.TenKH,
    Nhanvien.TenNV,
    Hoadon.Ngayban,
    Hoadon.SoHD
    
FROM 
    CT_Hoadon 
INNER JOIN 
    Hanghoa ON CT_Hoadon.Mahang = Hanghoa.Mahang
INNER JOIN 
    Hoadon ON CT_Hoadon.SoHD = Hoadon.SoHD
INNER JOIN 
    Khachhang ON Hoadon.MaKH = Khachhang.MaKH
INNER JOIN 
    Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV WHERE Hoadon.SoHD = '" + soHD + "'";

            DataTable dt = DataAcess.getTable(sql);

            decimal totalAmount = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalAmount += Convert.ToDecimal(row["ThanhTien"]);
            }

            // Thêm dòng "Tổng cộng" vào cuối DataTable
            DataRow totalRow = dt.NewRow();
            totalRow["Tenhang"] = "Tổng cộng";
            totalRow["ThanhTien"] = totalAmount;
            dt.Rows.Add(totalRow);


            dtgv.DataSource = dt; 

            CustomizeDataGridView();

            if (dt.Rows.Count > 0)
            {
                DisplaySelectedRowData(dt.Rows[0]);
            }
            dtgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dtgv.Columns["ThanhTien"].DefaultCellStyle.Format = "C0";
            dtgv.Columns["ThanhTien"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
            dtgv.Columns["DonGia"].DefaultCellStyle.Format = "C0";
            dtgv.Columns["DonGia"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
        }

            private void CustomizeDataGridView()
        {
            dtgv.Columns["Tenhang"].HeaderText = "Tên mặt hàng"; // Đặt tên cột
            dtgv.Columns["DVT"].HeaderText = "ĐV tính"; // Đặt tên cột
            dtgv.Columns["Dongia"].HeaderText = "Đơn giá"; // Đặt tên cột
            dtgv.Columns["Soluong"].HeaderText = "Số lượng"; // Đặt tên cột
            dtgv.Columns["ThanhTien"].HeaderText = "Thành tiền"; // Đặt tên cột

            dtgv.Columns["SoHD"].Visible = false;
            dtgv.Columns["TenKH"].Visible = false;
            dtgv.Columns["TenNV"].Visible = false;
            dtgv.Columns["Ngayban"].Visible = false;

            foreach (DataGridViewRow row in dtgv.Rows)
            {
                if (row.Cells["Tenhang"].Value?.ToString() == "Tổng cộng")
                {
                    row.DefaultCellStyle.Font = new Font(dtgv.Font, FontStyle.Bold); // Bôi đậm dòng "Tổng cộng"
                }
            }
        }

        private void DisplaySelectedRowData(DataRow row)
        {
            txtSHD.Text = row["SoHD"].ToString();
            txtTenKH.Text = row["TenKH"].ToString(); 
            txtTenNV.Text = row["TenNV"].ToString(); 
            dtimeNgayBan.Value = Convert.ToDateTime(row["Ngayban"]); 
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            string soHD = txtSHD.Text.ToString();

            FormLY formSua = new FormLY(soHD);
            formSua.formDetail = this;
            formSua.ShowDialog();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            DialogResult rs = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {

                string sql = "Delete CT_Hoadon WHERE SoHD = '" + txtSHD.Text + "'";
                DataAcess.insertDeleteUpdate(sql);

                string sql2 = "Delete Hoadon WHERE SoHD = '" + txtSHD.Text + "'";
                DataAcess.insertDeleteUpdate(sql2);

                fromBill?.Loaddulieu();
                this.Close();
            }
        }

        private void DetailBillForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            fromBill?.Loaddulieu();
        }
    }
}
