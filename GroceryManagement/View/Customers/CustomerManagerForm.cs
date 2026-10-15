using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using System.Xml.Linq;
using GroceryManagement;
using GroceryManagement.Models;
using GroceryManagement.Service;
namespace QuanLyTapHoa
{
    public partial class CustomerManagerForm : Form
    {
        private string path = DataAcess.path;
        public CustomerManagerForm()
        {
            InitializeComponent();
        }
        void vohieuhoa(bool gt)
        {
            txtMa.Enabled = gt;
            txtTen.Enabled = gt;
            txtQuan.Enabled = gt;
            txtDC.Enabled = gt;
            txtThanhPho.Enabled = gt;
            //txtTimKiem.Enabled = gt;
        }

        void HienThiLai()
        {
            SqlConnection consql = new SqlConnection(path);
            consql.Open();
            SqlDataAdapter ad = new SqlDataAdapter("select * from KhachHang", consql);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dgvKH.DataSource = dt;
            consql.Close();
            ad.Dispose();
        }
        void xoaText() => TextProcess.ResetText(txtMa, txtTen, txtQuan, txtDC,txtQuan);
        bool ktradauvao() => !Validation.IsNotNull(txtMa.Text, txtTen.Text, txtDC.Text, txtQuan.Text);
        public bool KiemTraMaKH(string ma)
        {
            SqlConnection conn = new SqlConnection(path);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter("select * from KhachHang where MaKH ='" + ma + "'", conn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            conn.Close();
            if (dt.Rows.Count > 0) return true;
            return false; //không trùng

        }

        private void Form_QLKH_Load(object sender, EventArgs e)
        {
            vohieuhoa(false);
            SqlConnection conn = new SqlConnection(path);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter("select * from KhachHang", conn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dgvKH.DataSource = dt;
            conn.Close();
            ad.Dispose();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (btnThem.Text == "Thêm")
            {
                vohieuhoa(true);
                txtMa.Focus();
                btnThem.Text = "Lưu";
            }
            else
            {
                btnThem.Text = "Lưu";
                if (ktradauvao() == true)
                {
                    MessageBox.Show("Chưa nhập đủ dữ liệu");
                }
                else
                {
                    if (KiemTraMaKH(txtMa.Text) == true)
                    {
                        MessageBox.Show("Trùng mã khách hàng");
                        txtMa.Focus();
                    }
                    else
                    {
                        btnThem.Text = "Thêm";
                        vohieuhoa(false);
                        SqlConnection conn = new SqlConnection(path);
                        conn.Open();
                        string sql = "insert into KhachHang values('" +
                            txtMa.Text + "',N'" + txtTen.Text + "',N'" +
                            txtDC.Text + "','" + txtQuan.Text + "' ,'" + txtThanhPho.Text + "' )";
                        MessageBox.Show("Đã thêm khách hàng " + txtTen.Text);
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        cmd.Dispose();
                        //hiển thị lại
                        HienThiLai();
                        xoaText();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMa.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn khách hàng cần sửa");
            }
            else if (btnSua.Text == "Sửa")
            {
                vohieuhoa(true);
                btnSua.Text = "Cập nhật";
                txtMa.Enabled = false;
            }
            else if (ktradauvao() == true)
            {
                MessageBox.Show("Chưa nhập đủ dữ liệu");
                btnSua.Text = "Cập nhật";
            }
            else
            {
                SqlConnection conn = new SqlConnection(path);
                conn.Open();
                string sql = "update KhachHang set TenKH = N'" +
                    txtTen.Text + "', Diachi = N'" + txtDC.Text +
                    "', Quan = '" + txtQuan.Text + "', ThanhPho = '" + txtThanhPho.Text + "' where MaKH = '" + txtMa.Text + "'";
                MessageBox.Show("Đã sửa khách hàng " + txtTen.Text);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd.Dispose();

                //Hiển Thị Lại
                HienThiLai();
                xoaText();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Bạn chưa chọn khách hàng cần xóa");
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa khách hàng " + txtTen.Text + " không?", "Thông báo", MessageBoxButtons.YesNo))
                {

                    SqlConnection conn = new SqlConnection(path);
                    conn.Open();

                    // Xóa dữ liệu liên quan trong bảng CT_HoaDon
                    string deleteCT_HoaDon = "DELETE FROM CT_HoaDon WHERE SoHD IN (SELECT SoHD FROM HoaDon WHERE MaKH = @MaKH)";
                    SqlCommand cmd1 = new SqlCommand(deleteCT_HoaDon, conn);
                    cmd1.Parameters.AddWithValue("@MaKH", txtMa.Text);
                    cmd1.ExecuteNonQuery();

                    MessageBox.Show("Xóa khách hàng và các hóa đơn liên quan thành công.");
                    string xoa = "delete from HoaDon where MaKH = '" +
                        txtMa.Text + "'";
                    SqlCommand cm = new SqlCommand(xoa, conn);
                    cm.ExecuteNonQuery();

                    string sql = "delete from KhachHang where MaKH = '" +
                        txtMa.Text + "'";
                    MessageBox.Show("Đã xóa khách hàng ");
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd.Dispose();
                    xoaText();
                    HienThiLai();
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên khách hàng cần tìm");
            }
            else
            {
                SqlConnection consql = new SqlConnection(path);
                consql.Open();
                SqlDataAdapter ad = new SqlDataAdapter("select * from KhachHang where TenKH LIKE N'%" + txtTimKiem.Text + "%'", consql);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dgvKH.DataSource = dt;
                consql.Close();
                ad.Dispose();
            }

        }

        private void btnHienThiLai_Click(object sender, EventArgs e)
        {
            HienThiLai();
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            xoaText();
            vohieuhoa(false);
        }
        private void dgvKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvKH.CurrentRow.Index;
            txtMa.Text = dgvKH.Rows[i].Cells[0].Value.ToString();
            txtTen.Text = dgvKH.Rows[i].Cells[1].Value.ToString();
        }
    }
}
