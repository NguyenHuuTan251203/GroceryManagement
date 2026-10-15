using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.Entity.Infrastructure;
using static System.Data.Entity.Infrastructure.Design.Executor;
using GroceryManagement.Models;
using System.Collections;
using GroceryManagement.DataAccess;

namespace GroceryManagement.View
{
    public partial class ThongKeForm : Form
    {
       public string connectionString = DataAcess.path;
        public ThongKeForm()
        {
            InitializeComponent();
        }

        private void ThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            DoanhThu();
            LoadNhanVienToComboBox();
            cbFilter_QuantityOrTotal.SelectedIndex = 0;
            cbFilter_Bill.SelectedIndex = 0;
            cbName.SelectedIndex = 0;
            cbDate.SelectedIndex = 0;
            dtpDate.Value = DateTime.Now;
            LoadDataIntoDgvLoaiHang();
            LoadDateOptions();
            LoadLoaiHangToComboBox();
        }

        private void tbExport_Click(object sender, EventArgs e)
        {
            Lib.ExportExcel(dgvDoanhThu, "Doanh Thu");
        }

        private void dgvDoanhThu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvDoanhThu.CurrentRow.Index;
            tbDate.Text = dgvDoanhThu.Rows[i].Cells[0].Value.ToString();
            tbCode.Text = dgvDoanhThu.Rows[i].Cells[1].Value.ToString();
            tbKhachHang.Text = dgvDoanhThu.Rows[i].Cells[2].Value.ToString();
            tbNhanVien.Text = dgvDoanhThu.Rows[i].Cells[3].Value.ToString();
            tbQuantity.Text = dgvDoanhThu.Rows[i].Cells[4].Value.ToString();
            var cellTongTien = dgvDoanhThu.Rows[i].Cells[5].Value;

            tbTotal.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", cellTongTien);
        }

        private void cbFilter_Bill_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void cbFilter_QuantityOrTotal_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEmployeeOrStoreChart();

        }

        private void cbDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEmployeeOrStoreChart();

        }


        private void cbDate2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharts();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateCharts();
        }
        public void DoanhThu()
        {
            string sql = @"
                            SELECT Hoadon.SoHD AS [Mã Hóa Đơn], 
                                   Hoadon.Ngayban AS [Ngày Bán], 
                                   Khachhang.TenKH AS [Tên Khách Hàng], 
                                   Nhanvien.TenNV AS [Tên Nhân Viên], 
                                   SUM(CT_Hoadon.Soluong) AS [Tổng Số Lượng Bán], 
                                   SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền] 
                            FROM Hoadon 
                            JOIN Khachhang ON Hoadon.MaKH = Khachhang.MaKH 
                            JOIN Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV 
                            JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD 
                            GROUP BY Hoadon.SoHD, Hoadon.Ngayban, Khachhang.TenKH, Nhanvien.TenNV 
                            ORDER BY Hoadon.Ngayban;
                        ";
            DataTable dt = DataAcess.getTable(sql);
            dgvDoanhThu.DataSource = dt;
            decimal tongTienTatCa = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (decimal.TryParse(row["Tổng Tiền"].ToString(), out decimal tongTien))
                {
                    tongTienTatCa += tongTien;
                }
            }

            dgvDoanhThu.Columns[5].DefaultCellStyle.Format = "C0";
            dgvDoanhThu.Columns[5].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");

            txtTongTien.Text = tongTienTatCa.ToString("N0");
        }
        private void LoadDateOptions()
        {
            cbDate2.Items.Clear();
            cbDate2.Items.AddRange(new string[] { "Ngày", "Tháng", "Năm" });
            cbDate2.SelectedIndex = 2;
        }

        private void dgvLoaiHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvLoaiHang.CurrentRow.Index;
            tbMaLoai.Text = dgvLoaiHang.Rows[i].Cells[0].Value.ToString();
            tbTenLoai.Text = dgvLoaiHang.Rows[i].Cells[1].Value.ToString();
            tbSoLuong.Text = dgvLoaiHang.Rows[i].Cells[2].Value.ToString();
            var cellDonGia = dgvLoaiHang.Rows[i].Cells[3].Value;

            tbTongTienLH.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", cellDonGia);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LoadDataIntoDgvLoaiHang();
            lbMaLoai.Text = "Mã Loại";
            lbTenLoai.Text = "Tên Loại";
            lbSoLuong.Text = "Số lượng";
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            DoanhThu();
        }

        private void chbNhanVien_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNhanVien.Checked)
            {
                cbNhanVien.Visible = true;
            }
            else
                cbNhanVien.Visible = false;
        }

        private void chbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDate.Checked)
            {
                lblDate.Visible = true;
                dtpFrom.Visible = true;
                dtpTo.Visible = true;
            }
            else
            {
                lblDate.Visible = false;
                dtpFrom.Visible = false;
                dtpTo.Visible = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Lib.ExportExcel(dgvLoaiHang, "Hang Hoa");
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            tbSearch.Clear();
            cbNhanVien.SelectedItem = null;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
        }
        private void LoadLoaiHangToComboBox()
        {
            string query = "SELECT Maloai, Tenloai FROM Loaihang";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Dictionary<string, string> loaiHangDict = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        // Lấy giá trị của Maloai và Tenloai từ CSDL
                        string maLoai = reader["Maloai"].ToString();
                        string tenLoai = reader["Tenloai"].ToString();

                        // Thêm vào ComboBox
                        loaiHangDict.Add(maLoai, tenLoai);
                    }

                    // Thiết lập DataSource của ComboBox với Dictionary vừa tạo
                    cbLoaiHang.DataSource = new BindingSource(loaiHangDict, null);
                    cbLoaiHang.DisplayMember = "Value"; // Hiển thị Tenloai
                    cbLoaiHang.ValueMember = "Key";     // Giá trị là Maloai
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu vào ComboBox: " + ex.Message);
                }
            }
        }
        private void UpdateChart()
        {
            if (cbFilter_QuantityOrTotal.SelectedItem == null || cbFilter_Bill.SelectedItem == null)
            {
                //MessageBox.Show("Vui lòng chọn trước khi cập nhật biểu đồ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filterQuantityOrTotal = cbFilter_QuantityOrTotal.SelectedItem.ToString();
            string filterBill = cbFilter_Bill.SelectedItem.ToString();

        

            string sqlQuery = "";

            if (filterQuantityOrTotal == "Số hóa đơn")
            {
                if (filterBill == "Ngày")
                    sqlQuery = "SELECT CONVERT(DATE, Ngayban) AS Ngay, COUNT(SoHD) AS SoHoaDon FROM HoaDon GROUP BY CONVERT(DATE, Ngayban)";
                else if (filterBill == "Tháng")
                    sqlQuery = "SELECT YEAR(Ngayban) AS Nam, MONTH(Ngayban) AS Thang, COUNT(SoHD) AS SoHoaDon FROM HoaDon GROUP BY YEAR(Ngayban), MONTH(Ngayban)";
                else if (filterBill == "Năm")
                    sqlQuery = "SELECT YEAR(Ngayban) AS Nam, COUNT(SoHD) AS SoHoaDon FROM HoaDon GROUP BY YEAR(Ngayban)";
            }
            else if (filterQuantityOrTotal == "Tổng tiền")
            {
                if (filterBill == "Ngày")
                    sqlQuery = @"
                                SELECT CONVERT(DATE, HoaDon.Ngayban) AS Ngay, SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                                FROM HoaDon 
                                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                                GROUP BY CONVERT(DATE, HoaDon.Ngayban)";
                else if (filterBill == "Tháng")
                    sqlQuery = @"
                                SELECT YEAR(HoaDon.Ngayban) AS Nam, MONTH(HoaDon.Ngayban) AS Thang, SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                                FROM HoaDon 
                                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                                GROUP BY YEAR(HoaDon.Ngayban), MONTH(HoaDon.Ngayban)";
                else if (filterBill == "Năm")
                    sqlQuery = @"
                                SELECT YEAR(HoaDon.Ngayban) AS Nam, SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                                FROM HoaDon 
                                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                                GROUP BY YEAR(HoaDon.Ngayban)";
            }

            if (string.IsNullOrEmpty(sqlQuery))
            {
                MessageBox.Show("Vui lòng chọn các bộ lọc thích hợp trước khi cập nhật biểu đồ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                chart1.Series.Clear();
                var series = chart1.Series.Add("Thống Kê");

                series.ChartType =  SeriesChartType.Column;
                series.XValueType = ChartValueType.String;

                foreach (DataRow row in dataTable.Rows)
                {
                    string xValue = "";
                    decimal yValue = 0;

                    if (filterQuantityOrTotal == "Số hóa đơn")
                    {
                        if (filterBill == "Ngày")
                        {
                            xValue = Convert.ToDateTime(row["Ngay"]).ToString("dd/MM/yyyy");
                            yValue = Convert.ToInt32(row["SoHoaDon"]);
                        }
                        else if (filterBill == "Tháng")
                        {
                            xValue = row["Thang"].ToString() + "/" + row["Nam"].ToString();
                            yValue = Convert.ToInt32(row["SoHoaDon"]);
                        }
                        else if (filterBill == "Năm")
                        {
                            xValue = row["Nam"].ToString();
                            yValue = Convert.ToInt32(row["SoHoaDon"]);
                        }

                        int pointIndex = series.Points.AddXY(xValue, yValue);
                        series.Points[pointIndex].Label = $"({yValue})";
                    }
                    else if (filterQuantityOrTotal == "Tổng tiền")
                    {
                        if (filterBill == "Ngày")
                        {
                            xValue = Convert.ToDateTime(row["Ngay"]).ToString("dd/MM/yyyy");
                            yValue = Convert.ToDecimal(row["TongTien"]);
                        }
                        else if (filterBill == "Tháng")
                        {
                            xValue = row["Thang"].ToString() + "/" + row["Nam"].ToString();
                            yValue = Convert.ToDecimal(row["TongTien"]);
                        }
                        else if (filterBill == "Năm")
                        {
                            xValue = row["Nam"].ToString();
                            yValue = Convert.ToDecimal(row["TongTien"]);
                        }

                        int pointIndex = series.Points.AddXY(xValue, yValue);
                        series.Points[pointIndex].Label = $"{yValue:N0}";
                    }
                }
            }
        }
        private void UpdateEmployeeOrStoreChart()
        {
            if (cbName.SelectedItem == null || cbDate.SelectedItem == null)
            {
                return;
            }

            string filterName = cbName.SelectedItem.ToString();
            string filterDate = cbDate.SelectedItem.ToString();
            string connectionString = DataAcess.path;
            string sqlQuery = "";

            if (filterName == "Toàn cửa hàng")
            {
                if (filterDate == "Ngày")
                {
                    sqlQuery = @"
                SELECT CONVERT(DATE, HoaDon.Ngayban) AS Ngay, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                GROUP BY CONVERT(DATE, HoaDon.Ngayban)";
                }
                else if (filterDate == "Tháng")
                {
                    sqlQuery = @"
                SELECT YEAR(HoaDon.Ngayban) AS Nam, MONTH(HoaDon.Ngayban) AS Thang, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                GROUP BY YEAR(HoaDon.Ngayban), MONTH(HoaDon.Ngayban)";
                }
                else if (filterDate == "Năm")
                {
                    sqlQuery = @"
                SELECT YEAR(HoaDon.Ngayban) AS Nam, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                GROUP BY YEAR(HoaDon.Ngayban)";
                }
            }
            else if (filterName == "Nhân viên")
            {
                if (filterDate == "Ngày")
                {
                    sqlQuery = @"
                SELECT NhanVien.TenNV AS TenNhanVien, CONVERT(DATE, HoaDon.Ngayban) AS Ngay, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                JOIN NhanVien ON HoaDon.MaNV = NhanVien.MaNV
                GROUP BY NhanVien.TenNV, CONVERT(DATE, HoaDon.Ngayban)";
                }
                else if (filterDate == "Tháng")
                {
                    sqlQuery = @"
                SELECT NhanVien.TenNV AS TenNhanVien, YEAR(HoaDon.Ngayban) AS Nam, MONTH(HoaDon.Ngayban) AS Thang, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                JOIN NhanVien ON HoaDon.MaNV = NhanVien.MaNV
                GROUP BY NhanVien.TenNV, YEAR(HoaDon.Ngayban), MONTH(HoaDon.Ngayban)";
                }
                else if (filterDate == "Năm")
                {
                    sqlQuery = @"
                SELECT NhanVien.TenNV AS TenNhanVien, YEAR(HoaDon.Ngayban) AS Nam, 
                       SUM(CT_HoaDon.SoLuong * CT_HoaDon.DonGiaBan) AS TongTien
                FROM HoaDon 
                JOIN CT_HoaDon ON HoaDon.SoHD = CT_HoaDon.SoHD 
                JOIN NhanVien ON HoaDon.MaNV = NhanVien.MaNV
                GROUP BY NhanVien.TenNV, YEAR(HoaDon.Ngayban)";
                }
            }

            if (string.IsNullOrEmpty(sqlQuery))
            {
                MessageBox.Show("Vui lòng chọn các bộ lọc thích hợp trước khi cập nhật biểu đồ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                chart2.Series.Clear();
                var series = chart2.Series.Add("Doanh Thu");

                // Đặt kiểu biểu đồ
                series.ChartType = SeriesChartType.Column;
                series.XValueType = ChartValueType.String;

                foreach (DataRow row in dataTable.Rows)
                {
                    string xValue = "";
                    decimal yValue = Convert.ToDecimal(row["TongTien"]);

                    if (filterName == "Toàn cửa hàng")
                    {
                        if (filterDate == "Ngày")
                        {
                            xValue = Convert.ToDateTime(row["Ngay"]).ToString("dd/MM/yyyy");
                        }
                        else if (filterDate == "Tháng")
                        {
                            xValue = row["Thang"].ToString() + "/" + row["Nam"].ToString();
                        }
                        else if (filterDate == "Năm")
                        {
                            xValue = row["Nam"].ToString();
                        }
                    }
                    else if (filterName == "Nhân viên")
                    {
                        xValue = row["TenNhanVien"].ToString();
                        if (filterDate == "Tháng")
                        {
                            xValue += " - " + row["Thang"].ToString() + "/" + row["Nam"].ToString();
                        }
                        else if (filterDate == "Năm")
                        {
                            xValue += " - " + row["Nam"].ToString();
                        }
                    }

                    int pointIndex = series.Points.AddXY(xValue, yValue);
                    series.Points[pointIndex].Label = $"{yValue:N0}";
                }
            }
        }

        private void UpdateCharts()
        {
            if (cbDate2.SelectedItem == null)
            {
                //MessageBox.Show("Vui lòng chọn kiểu lọc thời gian trước khi cập nhật biểu đồ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDateType = cbDate2.SelectedItem.ToString();
            DateTime selectedDate = dtpDate.Value;
            int selectedYear = selectedDate.Year;
            int selectedMonth = selectedDate.Month;

            string sqlQuery = "";
            if (selectedDateType == "Ngày")
            {
                sqlQuery = $@"
SELECT Loaihang.Tenloai AS TenLoai,
       SUM(CT_Hoadon.Soluong) AS SoLuong,
       SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS DoanhThu
FROM Hoadon
JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
JOIN Hanghoa ON CT_Hoadon.Mahang = Hanghoa.Mahang
JOIN Loaihang ON Hanghoa.Loaihang = Loaihang.Maloai
WHERE CONVERT(DATE, Hoadon.Ngayban) = '{selectedDate:yyyy-MM-dd}'
GROUP BY Loaihang.Tenloai";
            }
            else if (selectedDateType == "Tháng")
            {
                sqlQuery = $@"
SELECT Loaihang.Tenloai AS TenLoai,
       SUM(CT_Hoadon.Soluong) AS SoLuong,
       SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS DoanhThu
FROM Hoadon
JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
JOIN Hanghoa ON CT_Hoadon.Mahang = Hanghoa.Mahang
JOIN Loaihang ON Hanghoa.Loaihang = Loaihang.Maloai
WHERE MONTH(Hoadon.Ngayban) = {selectedMonth} AND YEAR(Hoadon.Ngayban) = {selectedYear}
GROUP BY Loaihang.Tenloai";
            }
            else if (selectedDateType == "Năm")
            {
                sqlQuery = $@"
SELECT Loaihang.Tenloai AS TenLoai,
       SUM(CT_Hoadon.Soluong) AS SoLuong,
       SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS DoanhThu
FROM Hoadon
JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
JOIN Hanghoa ON CT_Hoadon.Mahang = Hanghoa.Mahang
JOIN Loaihang ON Hanghoa.Loaihang = Loaihang.Maloai
WHERE YEAR(Hoadon.Ngayban) = {selectedYear}
GROUP BY Loaihang.Tenloai";
            }

            if (string.IsNullOrEmpty(sqlQuery))
            {
                MessageBox.Show("Chưa chọn đúng kiểu lọc thời gian.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DataAcess.TaoKetNot())
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để hiển thị trên biểu đồ cho khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    UpdateChart(char3, dataTable, "SoLuong", "Số lượng hàng đã bán");
                    UpdateChart(char4, dataTable, "DoanhThu", "Doanh thu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối cơ sở dữ liệu hoặc xử lý dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateChart(Chart chart, DataTable dataTable, string yValueColumn, string seriesName)
        {
            chart.Series.Clear();
            var series = chart.Series.Add(seriesName);
            series.ChartType = SeriesChartType.Column;
            series.XValueType = ChartValueType.String;

            foreach (DataRow row in dataTable.Rows)
            {
                string xValue = row["TenLoai"].ToString();
                decimal yValue = Convert.ToDecimal(row[yValueColumn]);

                int pointIndex = series.Points.AddXY(xValue, yValue);
                series.Points[pointIndex].Label = $"{yValue:N0}";
            }
        }

        private void LoadDataIntoDgvLoaiHang()
        {
            string connectionString = DataAcess.path;
            string query = @"
               SELECT Loaihang.Maloai AS [Mã Loại Hàng],
               Loaihang.Tenloai AS [Tên Loại Hàng],
               SUM(CT_Hoadon.Soluong) AS [Số Lượng],
               SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS [Tổng tiền]
                FROM Loaihang
                JOIN Hanghoa ON Loaihang.Maloai = Hanghoa.Loaihang 
                JOIN CT_Hoadon ON Hanghoa.Mahang = CT_Hoadon.Mahang
                JOIN Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
                GROUP BY Loaihang.Maloai, Loaihang.Tenloai
                HAVING SUM(CT_Hoadon.Soluong) > 0";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvLoaiHang.DataSource = dataTable;

                    decimal tongTien = 0;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (decimal.TryParse(row["Tổng tiền"].ToString(), out decimal giaTri))
                        {
                            tongTien += giaTri;
                        }
                    }

                    tbTongTien.Text = tongTien.ToString("N0"); // Định dạng số để có dấu phân cách

                    dgvLoaiHang.Columns[3].DefaultCellStyle.Format = "C0";
                    dgvLoaiHang.Columns[3].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }


        private void btnTim_Click(object sender, EventArgs e)
        {
            // Lấy Maloai từ ComboBox cbLoaiHang
            string maLoai = ((KeyValuePair<string, string>)cbLoaiHang.SelectedItem).Key;

            string query = @"
            SELECT Hanghoa.Tenhang AS [Tên Hàng],
                   SUM(CT_Hoadon.Soluong) AS [Số Lượng],
                   CT_Hoadon.DonGiaBan AS [Đơn Giá],
                   SUM(CT_Hoadon.Soluong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền]
            FROM Hanghoa
            JOIN CT_Hoadon ON Hanghoa.Mahang = CT_Hoadon.Mahang
            JOIN Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
            WHERE Hanghoa.Loaihang = @Maloai
            GROUP BY Hanghoa.Tenhang, CT_Hoadon.DonGiaBan
            HAVING SUM(CT_Hoadon.Soluong) > 0";
            lbMaLoai.Text = "Tên Hàng";
            lbTenLoai.Text = "Số lượng";
            lbSoLuong.Text = "Đơn Giá";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Maloai", maLoai);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvLoaiHang.DataSource = dataTable;

                    // Tính tổng tiền và hiển thị trên tbTongTien
                    decimal tongTien = 0;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (decimal.TryParse(row["Tổng Tiền"].ToString(), out decimal giaTri))
                        {
                            tongTien += giaTri;
                        }
                    }
                    tbTongTien.Text = tongTien.ToString("N2"); // Định dạng số để có dấu phân cách
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }


        public void LoadNhanVienToComboBox()
        {
            string connectionString = DataAcess.path;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TenNV FROM Nhanvien";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    cbNhanVien.Items.Clear();
                    while (reader.Read())
                    {
                        cbNhanVien.Items.Add(reader["TenNV"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            if (!chbNhanVien.Checked && !chbDate.Checked)
            {
                string searchValue = tbSearch.Text.Trim();

                string query = @"SELECT Hoadon.SoHD AS [Mã Hóa Đơn], 
                            Hoadon.Ngayban AS [Ngày Bán], 
                            Khachhang.TenKH AS [Tên Khách Hàng], 
                            Nhanvien.TenNV AS [Tên Nhân Viên], 
                            SUM(CT_Hoadon.SoLuong) AS [Tổng Số Lượng Bán], 
                            SUM(CT_Hoadon.SoLuong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền]
                     FROM Hoadon
                     JOIN Khachhang ON Hoadon.MaKH = Khachhang.MaKH
                     JOIN Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV
                     JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
                     WHERE Khachhang.TenKH LIKE @searchValue
                     GROUP BY Hoadon.SoHD, Hoadon.Ngayban, Khachhang.TenKH, Nhanvien.TenNV";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();
                        adapter.Fill(dt);
                        dgvDoanhThu.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else if (chbNhanVien.Checked && !chbDate.Checked)
            {
                string selectedNhanVien = cbNhanVien.SelectedItem?.ToString() ?? "";

                string query = @"SELECT Hoadon.SoHD AS [Mã Hóa Đơn], 
                    Hoadon.Ngayban AS [Ngày Bán], 
                    Khachhang.TenKH AS [Tên Khách Hàng], 
                    Nhanvien.TenNV AS [Tên Nhân Viên], 
                    SUM(CT_Hoadon.SoLuong) AS [Tổng Số Lượng Bán], 
                    SUM(CT_Hoadon.SoLuong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền]
             FROM Hoadon
             JOIN Khachhang ON Hoadon.MaKH = Khachhang.MaKH
             JOIN Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV
             JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
             WHERE Nhanvien.TenNV = @tenNhanVien";

                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    query += " AND Khachhang.TenKH LIKE @searchValue";
                }

                query += " GROUP BY Hoadon.SoHD, Hoadon.Ngayban, Khachhang.TenKH, Nhanvien.TenNV";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tenNhanVien", selectedNhanVien);
                    if (!string.IsNullOrEmpty(tbSearch.Text))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", "%" + tbSearch.Text.Trim() + "%");
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();
                        adapter.Fill(dt);
                        dgvDoanhThu.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else if (!chbNhanVien.Checked && chbDate.Checked)
            {
                DateTime fromDate = dtpFrom.Value;
                DateTime toDate = dtpTo.Value;

                string query = @"SELECT Hoadon.SoHD AS [Mã Hóa Đơn], 
                    Hoadon.Ngayban AS [Ngày Bán], 
                    Khachhang.TenKH AS [Tên Khách Hàng], 
                    Nhanvien.TenNV AS [Tên Nhân Viên], 
                    SUM(CT_Hoadon.SoLuong) AS [Tổng Số Lượng Bán], 
                    SUM(CT_Hoadon.SoLuong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền]
             FROM Hoadon
             JOIN Khachhang ON Hoadon.MaKH = Khachhang.MaKH
             JOIN Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV
             JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
             WHERE Hoadon.Ngayban BETWEEN @fromDate AND @toDate";

                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    query += " AND Khachhang.TenKH LIKE @searchValue";
                }

                query += " GROUP BY Hoadon.SoHD, Hoadon.Ngayban, Khachhang.TenKH, Nhanvien.TenNV";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    if (!string.IsNullOrEmpty(tbSearch.Text))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", "%" + tbSearch.Text.Trim() + "%");
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();
                        adapter.Fill(dt);
                        dgvDoanhThu.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else if (chbNhanVien.Checked && chbDate.Checked)
            {
                string selectedNhanVien = cbNhanVien.SelectedItem?.ToString() ?? "";
                DateTime fromDate = dtpFrom.Value;
                DateTime toDate = dtpTo.Value;

                string query = @"SELECT Hoadon.SoHD AS [Mã Hóa Đơn], 
                    Hoadon.Ngayban AS [Ngày Bán], 
                    Khachhang.TenKH AS [Tên Khách Hàng], 
                    Nhanvien.TenNV AS [Tên Nhân Viên], 
                    SUM(CT_Hoadon.SoLuong) AS [Tổng Số Lượng Bán], 
                    SUM(CT_Hoadon.SoLuong * CT_Hoadon.DonGiaBan) AS [Tổng Tiền]
             FROM Hoadon
             JOIN Khachhang ON Hoadon.MaKH = Khachhang.MaKH
             JOIN Nhanvien ON Hoadon.MaNV = Nhanvien.MaNV
             JOIN CT_Hoadon ON Hoadon.SoHD = CT_Hoadon.SoHD
             WHERE Nhanvien.TenNV = @tenNhanVien AND Hoadon.Ngayban BETWEEN @fromDate AND @toDate";

                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    query += " AND Khachhang.TenKH LIKE @searchValue";
                }

                query += " GROUP BY Hoadon.SoHD, Hoadon.Ngayban, Khachhang.TenKH, Nhanvien.TenNV";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tenNhanVien", selectedNhanVien);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    if (!string.IsNullOrEmpty(tbSearch.Text))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", "%" + tbSearch.Text.Trim() + "%");
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    try
                    {
                        conn.Open();
                        adapter.Fill(dt);
                        dgvDoanhThu.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        private void char3_Click(object sender, EventArgs e)
        {

        }

        private void cbDate2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateCharts();
        }
    }
}
