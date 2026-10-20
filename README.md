GroceryManagement --- Ứng dụng Quản lý Cửa hàng Tạp hóa
Ứng dụng desktop hỗ trợ quản lý hoạt động cửa hàng tạp hóa, bao gồm hàng
hóa, nhập hàng, bán hàng, tài khoản, khách hàng, hóa đơn và thống kê.
Công nghệ sử dụng
- Ngôn ngữ: C#
- Giao diện: Windows Forms (WinForms), Guna UI2
- Cơ sở dữ liệu: Microsoft SQL Server
- Truy cập dữ liệu: Entity Framework 6 và ADO.NET
Chức năng chính
- Quản lý hàng hóa và danh mục: thêm, sửa, xóa, tìm kiếm và phân
  trang dữ liệu hàng hóa/danh mục.
- Nhập hàng và tồn kho: ghi nhận nghiệp vụ nhập hàng và cập nhật
  số lượng tồn kho.
- Bán hàng: chọn sản phẩm, quản lý giỏ hàng, tính tổng tiền, kiểm
  tra số tiền thanh toán và lập hóa đơn.
- Quản lý tài khoản: quản lý tài khoản người dùng; dự án có các
  màn hình đăng nhập và đăng ký.
- Quản lý khách hàng và hóa đơn: quản lý thông tin khách hàng, hóa
  đơn và chi tiết hóa đơn.
- Thống kê: hiển thị thống kê doanh thu và số lượng hàng bán theo
  thời gian, biểu đồ và chức năng xuất dữ liệu Excel.
Phần thực hiện
Các phần đã được phát triển trong dự án bao gồm giao diện người dùng,
đăng nhập/đăng ký, quản lý tài khoản, xử lý ngoại lệ ở một số chức năng
và quản lý thanh toán.
Việc xử lý ngoại lệ có thể chưa đồng đều ở mọi chức năng. Khi giới
thiệu dự án, chỉ nên mô tả cụ thể những phần bạn đã trực tiếp triển
khai và kiểm thử.

Cài đặt và chạy chương trình
Yêu cầu
- Windows
- Visual Studio có hỗ trợ C# và Windows Forms
- Microsoft SQL Server
- SQL Server Management Studio (SSMS), hoặc công cụ tương đương
Các bước thực hiện
1. Clone hoặc tải mã nguồn từ GitHub:
   git clone https://github.com/NguyenHuuTan251203/GroceryManagement.git
2. Mở solution/project bằng Visual Studio.
3. Tạo hoặc khôi phục cơ sở dữ liệu theo cấu trúc mà dự án sử dụng.
4. Kiểm tra các chuỗi kết nối và cập nhật cho phù hợp với SQL Server
   trên máy của bạn.
5. Khôi phục/cài đặt các package NuGet cần thiết nếu Visual Studio yêu
   cầu.
6. Build solution, sau đó chạy ứng dụng từ Visual Studio.
Lưu ý: Cần kiểm tra mã nguồn để xác định chính xác tên database,
cấu trúc bảng, chuỗi kết nối và cách khởi tạo dữ liệu. README này
không tự đặt các thông tin chưa được xác minh.

Mã nguồn
GitHub: https://github.com/NguyenHuuTan251203/GroceryManagement
Mục đích dự án
Dự án được thực hiện nhằm thực hành xây dựng ứng dụng quản lý desktop
bằng C#, thiết kế giao diện WinForms, làm việc với SQL Server và triển
khai các quy trình quản lý hàng hóa, nhập hàng, bán hàng và thống kê.
