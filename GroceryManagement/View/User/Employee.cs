using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement.View;
using GroceryManagement.Models;
using System.Text.RegularExpressions;
using System.Numerics;
using hd;
using GroceryManagement.View.User;

namespace GroceryManagement.View.User
{
    public partial class EmployeeForm : Form
    {
        private BindingList<ShoppingBasket> giohangList = new BindingList<ShoppingBasket>();
        static List<string> selectedValues = new List<string>();
        private bool isUpdatingTotalRow = false;
        private int soLuongHangHoa = 0;
        public static string thoiGianHienTai = "";
        public static string soHoaDon;
        public static string soKhachHang;
        public string error = "";

        BigInteger thanhtien = 0;
        public EmployeeForm()
        {
            InitializeComponent();
            GetTenKhachHang();
            layMaHoaDon();
            layThoiGian();
            ClearText();
            ganGiaTriMacDinh();
            dtgvSP.AllowUserToAddRows = false;
            dtgvSP.RowHeadersVisible = false;

        }
        public void ganGiaTriMacDinh()
        {
            foreach (DataGridViewRow row in dtgvSP.Rows)
            {
                if (!row.IsNewRow) // Kiểm tra nếu hàng không phải là hàng trống cuối cùng
                {
                    row.Cells["SLMua"].Value = "0"; // Gán giá trị 0 cho cột SLMua
                }
            }
        }

        public bool CheckMoney()
        {
            if (!Validation.IsNotNull(txtTienKhachDua.Text))
            {
                error = "Vui lòng nhập tiền khách hàng";
                return false;
            }

            if (double.Parse(Regex.Replace(txtTienKhachDua.Text, @"\D", "")) < double.Parse(Regex.Replace(lblTotal.Text, @"\D", "")))
            {
                error = "Đưa thiếu tiền";
                return false;
            }

            return true;

        }
        void GetTenKhachHang()
        {
            foreach (var item in DBConText.db.Khachhangs.ToList())
            {
                cbbKhachHang.Items.Add(item.TenKH);
            }
        }
        public void ClearText()
        {
            txtTenSP.ResetText();
            cbbKhachHang.ResetText();
            foreach (DataGridViewRow row in dtgvSP.Rows)
            {
                row.Cells["SLMua"].Value = "0";
                row.Cells["Chon"].Value = false;
            }
        }
        public void layMaHoaDon()
        {
            var hd = DBConText.db.Hoadons.ToList();
            soHoaDon = "HD" + (int.Parse(Regex.Match((hd[hd.Count - 1].SoHD), @"\d+").Value) + 1);
        }
        public void layMaKhacHang()
        {
            var kh = DBConText.db.Hoadons.ToList();
            soKhachHang = "KH" + (int.Parse(Regex.Match((kh[kh.Count - 1].SoHD), @"\d+").Value) + 1);
        }
        public static void layThoiGian()
        {
            DateTime ngayGioHienTai = DateTime.Now;
            thoiGianHienTai = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private void Employee_Load(object sender, EventArgs e)
        {
            lbName.Text = Program.NameUser;
            loadHH();
            ganGiaTriMacDinh();

            dtgvSP.Columns["Dongia"].DefaultCellStyle.Format = "C0";
            dtgvSP.Columns["Dongia"].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
        }
        private void loadHH()
        {
            string sql = "SELECT Hanghoa.Mahang, Hanghoa.Tenhang, Hanghoa.Dongia, Hanghoa.DVT, Loaihang.Tenloai, HangHoa.Soluong FROM Hanghoa Inner join Loaihang on Hanghoa.Loaihang = Loaihang.Maloai WHERE Soluong > 0";
            DataTable dt = DataAcess.getTable(sql);
            dtgvSP.DataSource = dt;
        }
        private void btLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm f = new LoginForm();
            f.Show();
        }

        private async void btPay_Click(object sender, EventArgs e)
        {
            var progressForm = new ProcessbarForm();
            progressForm.Show();
            await Task.Delay(900);
            if (giohangList.Count <= 0)
            {
                new MessageCustomForm("Không có sản phẩm nào trong giỏ hàng!").Show();
            }
            else if (!CheckMoney())
            {
                new MessageCustomForm(error).Show();
            }
            else if (KiemTra())
            {
                double ThanhTien = double.Parse(Regex.Replace(lblTotal.Text.Remove(0, 1), @"\D", ""));
                double TienKhach = double.Parse(Regex.Replace(txtTienKhachDua.Text, @"\D", ""));
                ThemHD();
                lbTienTraKhach.Text = "$" + (Math.Abs(ThanhTien - TienKhach)).ToString();

                new MessageCustomForm(Const.Noti_Successful).Show();
                DetailBillForm frm = new DetailBillForm(soHoaDon);
                frm.ShowDialog();
                giohangList.Clear();
                ClearText();
                lbSoLuongSanPham.Text = getSoLuongSP().ToString();
                lblTotal.Text = "$";
                loadHH();
            }
            else
            {
                new MessageCustomForm("Khách Hàng Không Được Null Và Không Chứa Ký Tự Đặc Biệt").Show();
            }

        }
        private bool KiemTra() => Validation.IsNotNull(cbbKhachHang.Text) && Validation.IsNotContainsCharactersSpecial(cbbKhachHang.Text);
        private void ThemHD()
        {
            layMaHoaDon();
            layThoiGian();
            layMaKhacHang();
            List<string> query = new List<string>();
            var MaNV = DBConText.db.Nhanviens.FirstOrDefault(x => x.TenNV.Equals(lbName.Text));
            var MaKH = DBConText.db.Khachhangs.FirstOrDefault(x => x.TenKH.Equals(cbbKhachHang.Text));
            if (MaKH == null)
            {

                query.Add($@"INSERT INTO Khachhang (MaKH,TenKH) 
                          VALUES ( '{soKhachHang}' , '{cbbKhachHang.Text}')
                         ");

            }

            query.Add($@"
                             INSERT INTO Hoadon (SoHD,NgayBan,MaKH,MaNV) 
                             VALUES ('{soHoaDon}' , '{thoiGianHienTai}' , '{MaKH.MaKH}' , '{MaNV.MaNV}')
                     ");

            foreach (var item in giohangList)
            {
                query.Add($@"INSERT INTO CT_Hoadon(SoHD, Mahang, Soluong, DonGiaBan) 
                            VALUES ('{soHoaDon}', '{item.MaSanPham}', '{item.SoLuong}', '{item.DonGia}')");

                query.Add($@"UPDATE Hanghoa
                            SET Soluong = Soluong - {item.SoLuong}
                            WHERE Mahang = '{item.MaSanPham}'");
            }

            query.ForEach(item => DataAcess.insertDeleteUpdate(item));
        }
        private void btnTK_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                string sql = "SELECT *  FROM Hanghoa WHERE Soluong > 0";
                DataTable dt = DataAcess.getTable(sql);
                dtgvSP.DataSource = dt;
            }
            else
            {
                string sql = "SELECT *  FROM Hanghoa WHERE Tenhang LIKE N'%" + txtTenSP.Text + "%' AND Soluong > 0";
                DataTable dt = DataAcess.getTable(sql);
                dtgvSP.DataSource = dt;
            }

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dtgvSP.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["Chon"] as DataGridViewCheckBoxCell;
                bool isCheckBoxCell = Convert.ToBoolean(checkBoxCell.Value);

                if (checkBoxCell != null && isCheckBoxCell)
                {
                    string maSP = row.Cells["Mahang"].Value.ToString();
                    string tenSP = row.Cells["Tenhang"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["SLMua"].Value);
                    int soLuongTonKho = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    string donViTinh = row.Cells["DVT"].Value.ToString();
                    decimal donGia = Convert.ToDecimal(row.Cells["Dongia"].Value);
                    decimal thanhtien = soLuong * donGia;

                    ShoppingBasket giohang = giohangList.FirstOrDefault(g => g.MaSanPham == maSP);
                    if (giohang != null)
                    {
                        if ((giohang.SoLuong + soLuong) > soLuongTonKho)
                            new MessageCustomForm("Xin lỗi quý khách sản phẩm này trong kho không đủ số lượng mua cho quý khách!!").Show();
                        else
                            giohang.SoLuong += soLuong;
                    }
                    else
                    {
                        ShoppingBasket newGioHang = new ShoppingBasket
                        {
                            MaSanPham = maSP,
                            TenSanPham = tenSP,
                            SoLuong = soLuong,
                            DonViTinh = donViTinh,
                            DonGia = donGia,
                            ThanhTien = thanhtien,
                            SLTonKho = soLuongTonKho
                        };
                        giohangList.Add(newGioHang);
                    }
                }
            }

            lbSoLuongSanPham.Text = getSoLuongSP().ToString();
            ClearText();
        }

        int getSoLuongSP()
        {
            return giohangList.Count();
        }

        private void dtgvTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int soLuong = 0;
                //laySoSanPham();
                if (dtgvSP.Columns[e.ColumnIndex] is DataGridViewButtonColumn && dtgvSP.Columns[e.ColumnIndex].Name == "Them")
                {
                    if (dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value != null)
                    {
                        soLuong = Convert.ToInt32(dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value);
                    }
                    soLuong++;
                    if (soLuong > Convert.ToInt32(dtgvSP.Rows[e.RowIndex].Cells["SoLuong"].Value))
                        new MessageCustomForm("Xin lỗi quý khách sản phẩm này trong kho không đủ số lượng mua cho quý khách!!").Show();
                    else
                    {
                        thanhtien += 1 * Convert.ToInt32(dtgvSP.Rows[e.RowIndex].Cells["Dongia"].Value);
                        lblTotal.Text = "$" + thanhtien.ToString("N0");
                        // Cập nhật lại giá trị vào cột số lượng
                        dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value = soLuong;
                    }
                }
                if (dtgvSP.Columns[e.ColumnIndex] is DataGridViewButtonColumn && dtgvSP.Columns[e.ColumnIndex].Name == "Tru")
                {
                    if (dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value != null)
                    {
                        soLuong = Convert.ToInt32(dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value);
                    }
                    if (soLuong > 0)
                    {
                        soLuong--;
                        thanhtien -= 1 * Convert.ToInt32(dtgvSP.Rows[e.RowIndex].Cells["Dongia"].Value);
                        lblTotal.Text = "$" + thanhtien.ToString("N0");
                        dtgvSP.Rows[e.RowIndex].Cells["SLMua"].Value = soLuong;
                    }
                    else
                        new MessageCustomForm("Không thể bớt số lượng bằng 0").Show();
                }
            }
        }

        private void btBill_Click(object sender, EventArgs e)
        {
            BillManagerForm frm = new BillManagerForm();
            frm.ShowDialog();
        }
        public void laySoSanPham()
        {
            int selectedCount = dtgvSP.Rows
                                  .Cast<DataGridViewRow>()
                                  .Count(row => row.Cells[0].Value != null && (bool)row.Cells[0].Value == true);
            lbSoLuongSanPham.Text = selectedCount.ToString();
        }

        private void btnShoppingBasket_Click(object sender, EventArgs e)
        {
            ShoppingBasketForm f = new ShoppingBasketForm();
            f.list = giohangList;
            f.FormClosed += new FormClosedEventHandler(ShoppingBasketForm_FormClosed);
            f.ShowDialog();
            lbSoLuongSanPham.Text = getSoLuongSP().ToString();
            lblTotal.Text = "$ " + tinhTongTien().ToString("N0") + " đ";
        }

        internal void capnhapgiohang(BindingList<ShoppingBasket> newgiohang)
        {
            giohangList = newgiohang;
        }

        private void ShoppingBasketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShoppingBasketForm f = sender as ShoppingBasketForm;
            if (f != null)
            {
                capnhapgiohang(f.list);
            }
        }

        decimal tinhTongTien()
        {
            decimal tongtien = 0;
            foreach (var item in giohangList)
            {
                tongtien += item.ThanhTien;
            }
            return tongtien;
        }

        private void guna2Separator3_Click(object sender, EventArgs e)
        {

        }
    }
}
