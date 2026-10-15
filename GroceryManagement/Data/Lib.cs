using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GroceryManagement
{
    internal class Lib
    {

        public static void ExportExcel(DataGridView dataGridView, string title)
        {
            int rowHeight = 20;
            string defaultFolder = ExportFolderPath();
            if (!Directory.Exists(defaultFolder))
            {
                Directory.CreateDirectory(defaultFolder);
            }

            string filePath = Path.Combine(defaultFolder, $"{title}.xlsx");

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn Nơi Lưu Tập Tin";
                sfd.FileName = $"{title}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filePath = sfd.FileName;
                }
                else
                {
                    DialogResult saveToDefault = MessageBox.Show("Bạn có muốn lưu tập tin Excel vào thư mục ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (saveToDefault == DialogResult.No) return;
                }
            }

            MessageBox.Show("Đang xử lý xuất dữ liệu ra Excel...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                // Sử dụng mảng tạm thời để lưu dữ liệu
                int totalRows = dataGridView.Rows.Count + 3; // Thêm dòng tiêu đề và dòng tổng
                string[,] tempData = new string[totalRows, dataGridView.Columns.Count];

                // Đặt tiêu đề cho sheet và các cột
                tempData[0, 0] = $"Danh Sách {title}";
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    tempData[1, i] = dataGridView.Columns[i].HeaderText;
                }

                // Biến để lưu tổng tiền
                decimal totalAmount = 0;
                int totalColumnIndex = -1;

                // Lấy dữ liệu từ DataGridView vào mảng và tính tổng
                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < dataGridView.Columns.Count; colIndex++)
                    {
                        string value = dataGridView.Rows[rowIndex].Cells[colIndex].Value?.ToString() ?? string.Empty;

                        // Xác định cột tổng tiền và tính tổng
                        if (dataGridView.Columns[colIndex].HeaderText == "Tổng Tiền") // Thay thế "Tổng Tiền" bằng tên cột phù hợp
                        {
                            totalColumnIndex = colIndex;
                            if (decimal.TryParse(value, out decimal amount))
                            {
                                totalAmount += amount;
                                value = ConvertPrice(value, false);
                            }
                        }

                        tempData[rowIndex + 2, colIndex] = value;
                    }
                }

                // Thêm dòng tổng vào mảng
                if (totalColumnIndex != -1)
                {
                    tempData[dataGridView.Rows.Count + 2, totalColumnIndex - 1] = "Tổng Cộng"; // Ô bên trái cột tổng
                    tempData[dataGridView.Rows.Count + 2, totalColumnIndex] = totalAmount.ToString(); // Ô tổng tiền
                }

                // Cơ chế retry để tránh lỗi RPC_E_CALL_REJECTED
                int retryCount = 5;
                bool success = false;
                for (int attempt = 0; attempt < retryCount; attempt++)
                {
                    try
                    {
                        Excel.Range dataRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[totalRows, dataGridView.Columns.Count]];
                        dataRange.Value = tempData;
                        success = true;
                        break;
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        if (attempt == retryCount - 1) throw;
                        System.Threading.Thread.Sleep(500);
                    }
                }

                if (!success)
                {
                    MessageBox.Show("Không thể xuất dữ liệu do lỗi Excel.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Định dạng tiêu đề
                Excel.Range titleRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, dataGridView.Columns.Count]];
                titleRange.Merge();
                titleRange.Font.Bold = true;
                titleRange.Interior.Color = Color.FromArgb(180, 198, 231);
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                titleRange.Rows.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                titleRange.Rows.RowHeight = 30;

                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                {
                    Excel.Range rowData = worksheet.Range[worksheet.Cells[rowIndex + 3, 1], worksheet.Cells[rowIndex + 3, dataGridView.Columns.Count]];
                    rowData.Interior.Color = rowIndex % 2 == 0 ? Color.White : Color.FromArgb(230, 247, 255);
                }

                // Định dạng dòng tổng
                if (totalColumnIndex != -1)
                {
                    Excel.Range totalRowRange = worksheet.Range[worksheet.Cells[dataGridView.Rows.Count + 3, 1], worksheet.Cells[dataGridView.Rows.Count + 3, dataGridView.Columns.Count]];
                    totalRowRange.Font.Bold = true;
                    totalRowRange.Interior.Color = Color.LightYellow;
                    totalRowRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                worksheet.Name = $"Danh Sách {title}";

                // Điều chỉnh kích thước
                worksheet.UsedRange.Columns.AutoFit();
                worksheet.UsedRange.Rows.AutoFit();
                worksheet.UsedRange.Rows.RowHeight = rowHeight;
                worksheet.UsedRange.Rows.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // Thêm đường viền cho các ô
                worksheet.UsedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                worksheet.UsedRange.Borders.Weight = Excel.XlBorderWeight.xlThin;
                worksheet.UsedRange.Borders.Color = Color.FromArgb(122, 181, 192);

                if (File.Exists(filePath)) File.Delete(filePath);
                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (workbook != null) workbook.Close(false);
                if (excelApp != null) excelApp.Quit();

                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static string ExportFolderPath()
        {
            return Path.Combine(CurrentPath(), $"Export");
        }
        // lấy đường dẫn đến thư mục .exe của chương trình
        private static string CurrentPath()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        }
        public static string ConvertPrice(string price, bool isFormat)
        {
            try
            {
                if (string.IsNullOrEmpty(price)) return string.Empty;

                return isFormat
                    ? FormatNumber(price)
                    : price.Replace(",", "");
            }
            catch
            {
                return price;
            }
        }
        private static string FormatNumber(string number)
        {
            var result = new StringBuilder();

            int count = 0;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                result.Insert(0, number[i]);
                count++;
                if ((count % 3 == 0)
                    && (i != 0))
                {
                    result.Insert(0, ',');
                }
            }

            return result.ToString();
        }
    }
}
