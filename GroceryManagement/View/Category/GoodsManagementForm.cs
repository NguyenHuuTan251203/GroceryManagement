using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLyTapHoa.Controller;
using QuanLyTapHoa.Models;

namespace QuanLyTapHoa.View.GoodsManagement
{
    public partial class GoodsManagementForm : Form
    {
        GoodsController goodsController = new GoodsController();
        bool statusBtnAdd = false;
        bool statusBtnEdit = false;
        bool statusBtnDelete = false;
        bool statusBtnGoods = true;
        bool statusBtnCategoryOfGoods = false;
        bool statusSearch = false;
        int pageSize = 1;
        int currentPage = 1;
        int totalPage = 1;

        public GoodsManagementForm()
        {
            InitializeComponent();
        }

        private void header_HangHoa()
        {
            tableHangHoa.Columns[0].HeaderText = "Mã Hàng Hóa";
            tableHangHoa.Columns[1].HeaderText = "Loại Hàng Hóa";
            tableHangHoa.Columns[2].HeaderText = "Tên Hàng Hóa";
            tableHangHoa.Columns[3].HeaderText = "Đơn Vị Tính";
            tableHangHoa.Columns[4].HeaderText = "Số Lượng";
            tableHangHoa.Columns[5].HeaderText = "Đơn Giá";

            //tableHangHoa.Columns[0].Width = 100;
            tableHangHoa.Columns[1].Width = 150;
            tableHangHoa.Columns[2].Width = 150;
            //tableHangHoa.Columns[3].Width = 150;
            //tableHangHoa.Columns[4].Width = 70;
            //tableHangHoa.Columns[5].Width = 70;

            tableHangHoa.Columns[5].DefaultCellStyle.Format = "C0";
            tableHangHoa.Columns[5].DefaultCellStyle.FormatProvider = new System.Globalization.CultureInfo("vi-VN");

        }

        private void header_LoaiHangHoa()
        {
            tableHangHoa.Columns[0].HeaderText = "Mã Loại Hàng Hóa";
            tableHangHoa.Columns[1].HeaderText = "Tên Loại Hàng Hóa";
        }

        void ListLoaiHang()
        {
            cbbLoaiHH.DataSource = goodsController.getLoaiHangHoa();
            cbbLoaiHH.DisplayMember = "Tenloai";
            cbbLoaiHH.ValueMember = "Maloai";
            cbbLoaiHH.SelectedIndex = -1;
        }

        void listSearch()
        {
            cbbSearch.Items.Clear(); 

            if (statusBtnGoods)
            {
                cbbSearch.Items.Add("Tất cả");
                cbbSearch.Items.Add("Mã hàng hóa");
                cbbSearch.Items.Add("Loại hàng hóa");
                cbbSearch.Items.Add("Tên hàng hóa");
                cbbSearch.Items.Add("Đơn vị tính");
                cbbSearch.Items.Add("Số lượng");
                cbbSearch.Items.Add("Đơn giá");

                cbbSearch.SelectedIndex = 0; 
            }
            else if (statusBtnCategoryOfGoods)
            {
                cbbSearch.Items.Add("Tất cả");
                cbbSearch.Items.Add("Mã loại hàng hóa");
                cbbSearch.Items.Add("Tên loại hàng hóa");

                cbbSearch.SelectedIndex = 0;
            }
        }

        private void GoodsManagement_Load(object sender, EventArgs e)
        {
            ListLoaiHang();
            listSearch();

            cbbRecord.SelectedIndex = 0;
            //cbbSearch.SelectedIndex = 0;
            tableHangHoa.ClearSelection();
            
            loadDataGoods();
            disableTextBoxGoods(false);
            disableTextBoxCate(false);
            //disableTextBox(false);
            if (decimal.TryParse(txtDonGia.Text, out decimal value))
            {
                txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", value);
            }

        }

        void loadDataGoods()
        {
            pageSize = Convert.ToInt32(cbbRecord.Text);
            totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordHangHoa() / pageSize);
            DataTable table = new DataTable();
            table = goodsController.getTableHangHoa((currentPage - 1) * pageSize + 1, currentPage * pageSize); ;
            //table = goodsController.getTableHangHoa_OF(4, 4);
            //table = goodsController.getTableHangHoa_Fill(3, 4);
            tableHangHoa.DataSource = table;
            header_HangHoa();

            totalPage = totalPage == 0 ? 1 : totalPage;
            lblCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
        }

        void loadDataCategoryOfGoods()
        {
            pageSize = Convert.ToInt32(cbbRecord.Text);
            totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordLoaiHangHoa() / pageSize);
            DataTable table = new DataTable();
            table = goodsController.getTableLoaiHangHoa((currentPage - 1) * pageSize + 1, currentPage * pageSize); ;
            tableHangHoa.DataSource = table;
            header_LoaiHangHoa();
            totalPage = totalPage == 0 ? 1 : totalPage;
            lblCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
        }

        private void btnHangHoa_Click(object sender, EventArgs e)
        {
            currentPage = 1;

            loadDataGoods();

            goodsControl.Visible = true;
            categoryOfGoodsControl.Visible = false;
            statusBtnGoods = true;
            statusBtnCategoryOfGoods = false;
            statusSearch = false;

            btnGoods.CustomBorderColor = System.Drawing.Color.OrangeRed;
            btnCategoryOfGoods.CustomBorderColor = System.Drawing.Color.Transparent;
            
            listSearch();
        }

        private void btnPhanLoaiHangHoa_Click(object sender, EventArgs e)
        {
            currentPage = 1;

            loadDataCategoryOfGoods();

            categoryOfGoodsControl.Visible = true;
            goodsControl.Visible = false;
            statusBtnCategoryOfGoods = true;
            statusBtnGoods = false;

            btnCategoryOfGoods.CustomBorderColor = System.Drawing.Color.OrangeRed;
            btnGoods.CustomBorderColor = System.Drawing.Color.Transparent;
            
            listSearch();
        }

        private void addUpdateDeleteControl1_Load(object sender, EventArgs e)
        {

        }

        bool isDecimalNumeric(string str)
        {
            decimal number;
            return Decimal.TryParse(str, out number);
        }

        bool KTDL_Goods(string mahh, string loaihh, string tenhh, string dvt, string soluong, string dongia)
        {
            if (String.IsNullOrWhiteSpace(mahh))
            {
                MessageBox.Show("Mã hàng hóa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(loaihh))
                {
                    MessageBox.Show("Loại hàng hóa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(tenhh))
                    {
                        MessageBox.Show("Tên hàng hóa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(dongia))
                        {
                            MessageBox.Show("Đơn giá không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            if (String.IsNullOrWhiteSpace(soluong))
                            {
                                MessageBox.Show("Số lượng không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else
                            {
                                if (String.IsNullOrWhiteSpace(dvt))
                                {
                                    MessageBox.Show("Đơn vị tính không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        bool KTDL_Cate(string maloaihh, string tenloaihh)
        {
            if (String.IsNullOrWhiteSpace(maloaihh))
            {
                MessageBox.Show("Mã hàng hóa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(tenloaihh))
                {
                    MessageBox.Show("Loại hàng hóa không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }


        bool isNumeric(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                if (!isDecimalNumeric(str))
                {
                    return false;
                }
            }
            return true;
        }

        bool addCate()
        {
            if (KTDL_Cate(txtMaLoaiHH.Text, txtTenLoaiHH.Text))
            {
                string maLoaiHH = txtMaLoaiHH.Text;
                string tenLoaiHH = txtTenLoaiHH.Text;

                CategoryOfGoods category = new CategoryOfGoods(maLoaiHH, tenLoaiHH);

                if (goodsController.addLoaiHangHoa(category))
                {
                    MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Thêm dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return false; 
        }

        bool updCate()
        {
            if (KTDL_Cate(txtMaLoaiHH.Text, txtTenLoaiHH.Text))
            {
                string maLoaiHH = txtMaLoaiHH.Text;
                string tenLoaiHH = txtTenLoaiHH.Text;

                CategoryOfGoods category = new CategoryOfGoods(maLoaiHH, tenLoaiHH);

                if (goodsController.updateLoaiHangHoa(category))
                {
                    MessageBox.Show("Cập nhập dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Cập nhập dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return false;
        }

        bool delCate()
        {
            if (!String.IsNullOrWhiteSpace(txtMaLoaiHH.Text))
            {
                string maLoaiHH = txtMaLoaiHH.Text.Trim();

                if (goodsController.deleteLoaiHangHoa(maLoaiHH))
                {
                    MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return false;
        }

        public void addUpdDelCate()
        {
            if (statusBtnAdd)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (addCate())
                    {
                        statusBtnAdd = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtMaLoaiHH.Focus();
                    }
                }
            }
            else if (statusBtnEdit)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (updCate())
                    {
                        statusBtnEdit = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtTenLoaiHH.Focus();
                    }
                }
            }
            else if (statusBtnDelete)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (delCate())
                    {
                        statusBtnDelete = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtMaLoaiHH.Focus();
                    }
                }
            }
        }

        public void cancelAddUpdDelCate()
        {
            if (statusBtnAdd)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (addCate())
                    {
                        statusBtnAdd = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtMaLoaiHH.Focus();
                    }
                }
                else
                {
                    resetTextBoxCate();
                    disableTextBoxCate(false);
                    statusBtnAdd = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                }
            }
            else if (statusBtnEdit)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (updCate())
                    {
                        statusBtnEdit = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        txtMaLoaiHH.ReadOnly = false;
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtTenLoaiHH.Focus();
                    }
                }
                else
                {
                    txtMaLoaiHH.ReadOnly = false;
                    resetTextBoxCate();
                    disableTextBoxCate(false);
                    statusBtnEdit = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                }
            }
            else if (statusBtnDelete)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (delCate())
                    {
                        statusBtnDelete = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataCategoryOfGoods();
                        ListLoaiHang();
                        txtMaLoaiHH.ReadOnly = false;
                        resetTextBoxCate();
                        disableTextBoxCate(false);
                    }
                    else
                    {
                        txtTenLoaiHH.Focus();
                    }
                }
                else
                {
                    txtMaLoaiHH.ReadOnly = false;
                    resetTextBoxCate();
                    disableTextBoxCate(false);
                    statusBtnDelete = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                }
            }
        }



        bool addGoods()
        {
            if (KTDL_Goods(txtMaHH.Text, cbbLoaiHH.Text, txtTenHH.Text, txtDVTinh.Text, txtSoLuong.Text, txtDonGia.Text))
            {
                string mahh = txtMaHH.Text;
                string loaihh = (string)cbbLoaiHH.SelectedValue;
                string tenhh = txtTenHH.Text;
                decimal soluong;
                decimal dongia;
                string dvt = txtDVTinh.Text;

                if (!isDecimalNumeric(txtDonGia.Text))
                {
                    MessageBox.Show("Yêu cầu nhập kí tự số cho trường đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else
                {
                    dongia = decimal.Parse(txtDonGia.Text, NumberStyles.Currency, new CultureInfo("vi-VN"));
                    //dongia = decimal.Parse(txtDonGia.Text);

                    if (!isDecimalNumeric(txtSoLuong.Text))
                    {
                        MessageBox.Show("Yêu cầu nhập kí tự số cho trường số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Focus();
                    }
                    else
                    {
                        soluong = Convert.ToDecimal(txtSoLuong.Text);

                        Goods addGoods = new Goods(mahh, loaihh, tenhh, soluong, dongia, dvt);

                        if (goodsController.addHangHoa(addGoods))
                        {
                            MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Thêm dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }

            }
            return false;
        }

        bool updGoods()
        {
            if (KTDL_Goods(txtMaHH.Text, cbbLoaiHH.Text, txtTenHH.Text, txtDVTinh.Text, txtSoLuong.Text, txtDonGia.Text))
            {
                string mahh = txtMaHH.Text;
                string loaihh = (string)cbbLoaiHH.SelectedValue;
                string tenhh = txtTenHH.Text;
                decimal soluong;
                decimal dongia;
                string dvt = txtDVTinh.Text;

                if (!isDecimalNumeric(txtDonGia.Text))
                {
                    MessageBox.Show("Yêu cầu nhập kí tự số cho trường đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else
                {
                    dongia = decimal.Parse(txtDonGia.Text, NumberStyles.Currency, new CultureInfo("vi-VN"));
                    //dongia = decimal.Parse(txtDonGia.Text);

                    if (!isDecimalNumeric(txtSoLuong.Text))
                    {
                        MessageBox.Show("Yêu cầu nhập kí tự số cho trường số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Focus();
                    }
                    else
                    {
                        soluong = Convert.ToDecimal(txtSoLuong.Text);

                        Goods updGoods = new Goods(mahh, loaihh, tenhh, soluong, dongia, dvt);

                        if (goodsController.updateHangHoa(updGoods))
                        {
                            MessageBox.Show("Cập nhập dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Cập nhập dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }

            }
            return false;
        }

        bool delGoods()
        {
            if (!String.IsNullOrWhiteSpace(txtMaHH.Text))
            {
                string mahh = txtMaHH.Text.Trim();

                if (goodsController.deleteHangHoa(mahh))
                {
                    MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return false;
        }

        public void addUpdDelGoods()
        {
            if (statusBtnAdd)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (addGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnAdd = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        txtMaHH.Focus();
                    }
                }
            }
            else if (statusBtnEdit)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (updGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnEdit = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        txtMaHH.ReadOnly = false;
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        cbbLoaiHH.Focus();
                    }
                }  
            }
            else if (statusBtnDelete)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (delGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnDelete = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        txtMaHH.ReadOnly = false;
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        txtMaHH.Focus();
                    }
                }
            }
        }

        public void cancelAddUpdDelGoods()
        {
            if (statusBtnAdd)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (addGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnAdd = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        cbbLoaiHH.Focus();
                    }
                }
                else
                {
                    cbbLoaiHH.SelectedIndex = -1;
                    statusBtnAdd = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                    resetTextBoxGoods();
                    disableTextBoxGoods(false);
                }
            }
            else if (statusBtnEdit)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (updGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnEdit = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        txtMaHH.ReadOnly = false;
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        cbbLoaiHH.Focus();
                    }
                }
                else
                {
                    cbbLoaiHH.SelectedIndex = -1;
                    statusBtnEdit = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                    txtMaHH.ReadOnly = false;
                    resetTextBoxGoods();
                    disableTextBoxGoods(false);
                }
            }
            else if (statusBtnDelete)
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (delGoods())
                    {
                        cbbLoaiHH.SelectedIndex = -1;
                        statusBtnDelete = false;
                        addEditDeleteControl.Visible = false;
                        saveCancelControl.Visible = true;
                        loadDataGoods();
                        txtMaHH.ReadOnly = false;
                        resetTextBoxGoods();
                        disableTextBoxGoods(false);
                    }
                    else
                    {
                        txtMaHH.Focus();
                    }
                }
                else
                {
                    cbbLoaiHH.SelectedIndex = -1;
                    statusBtnDelete = false;
                    addEditDeleteControl.Visible = false;
                    saveCancelControl.Visible = true;
                    txtMaHH.ReadOnly = false;
                    resetTextBoxGoods();
                    disableTextBoxGoods(false);
                }
            }
        }

        void resetTextBoxGoods()
        {
            txtMaHH.ResetText();
            txtTenHH.ResetText();
            txtDVTinh.ResetText();
            txtSoLuong.ResetText();
            txtDonGia.ResetText();
            //cbbLoaiHH.ResetText();
            cbbLoaiHH.SelectedIndex = -1;
        }

        void resetTextBoxCate()
        {
            txtMaLoaiHH.ResetText();
            txtTenLoaiHH.ResetText();
        }

        void disableTextBoxGoods(bool truefalse)
        {
            txtMaHH.Enabled = truefalse;
            cbbLoaiHH.Enabled = truefalse;
            txtTenHH.Enabled = truefalse;
            txtDonGia.Enabled = truefalse;
            txtSoLuong.Enabled = truefalse;
            txtDVTinh.Enabled = truefalse;
        }

        void disableTextBoxCate(bool truefalse)
        {
            txtMaLoaiHH.Enabled = truefalse;
            txtTenLoaiHH.Enabled = truefalse;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                statusBtnAdd = true;
                addEditDeleteControl.Visible = true;
                saveCancelControl.Visible = false;
                //disableTextBox(true);

                if (statusBtnGoods)
                {
                    txtMaHH.Text = goodsController.createMaHH();
                    disableTextBoxGoods(true);
                }
                else if (statusBtnCategoryOfGoods)
                {
                    txtMaLoaiHH.Text = goodsController.createMaLoaiHH();
                    disableTextBoxCate(true);
                }
            }
            catch(Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                statusBtnEdit = true;
                addEditDeleteControl.Visible = true;
                saveCancelControl.Visible = false;

                if (statusBtnGoods)
                {
                    disableTextBoxGoods(true);
                    txtMaHH.ReadOnly = true;
                }
                else if (statusBtnCategoryOfGoods)
                {
                    disableTextBoxCate(true);
                    txtMaLoaiHH.ReadOnly = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                statusBtnDelete = true;
                addEditDeleteControl.Visible = true;
                saveCancelControl.Visible = false;

                if (statusBtnGoods)
                {
                    disableTextBoxGoods(true);
                    txtMaHH.ReadOnly = true;
                }
                else if (statusBtnCategoryOfGoods)
                {
                    disableTextBoxCate(true);
                    txtMaLoaiHH.ReadOnly = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStorage_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusBtnGoods)
                    addUpdDelGoods();
                else if (statusBtnCategoryOfGoods)
                    addUpdDelCate();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusBtnGoods)
                    cancelAddUpdDelGoods();
                else if (statusBtnCategoryOfGoods)
                    cancelAddUpdDelCate();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tableHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (tableHangHoa.SelectedRows.Count > 0)
                {
                    if (statusBtnGoods)
                    {
                        txtMaHH.Text = tableHangHoa.SelectedRows[0].Cells[0].Value.ToString();
                        string loaiHH = tableHangHoa.SelectedRows[0].Cells[1].Value.ToString();
                        txtTenHH.Text = tableHangHoa.SelectedRows[0].Cells[2].Value.ToString();
                        txtDVTinh.Text = tableHangHoa.SelectedRows[0].Cells[3].Value.ToString();
                        txtSoLuong.Text = tableHangHoa.SelectedRows[0].Cells[4].Value.ToString();
                        //txtDonGia.Text = tableHangHoa.SelectedRows[0].Cells[5].Value.ToString();
                        var cellDongia = tableHangHoa.SelectedRows[0].Cells[5].Value;

                        cbbLoaiHH.SelectedIndex = cbbLoaiHH.FindStringExact(loaiHH);
                        txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", cellDongia);
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        txtMaLoaiHH.Text = tableHangHoa.SelectedRows[0].Cells[0].Value.ToString();
                        txtTenLoaiHH.Text = tableHangHoa.SelectedRows[0].Cells[1].Value.ToString();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage > 1)
                {
                    if (statusBtnGoods)
                    {
                        currentPage--;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchCate();
                        //txtCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage--;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoublePrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage > 2)
                {
                    if (statusBtnGoods)
                    {
                        currentPage -= 2;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchCate();
                        //txtCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage -= 2;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
                else if (currentPage > 1)
                {
                    if (statusBtnGoods)
                    {
                        currentPage--;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                        //txtCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage--;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage > 1)
                {
                    if (statusBtnGoods)
                    {
                        currentPage = 1;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                        //txtCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage = 1;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }    
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage < totalPage)
                {
                    if (statusBtnGoods)
                    {
                        currentPage++;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage++;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoubleNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage < totalPage - 1)
                {
                    if (statusBtnGoods)
                    {
                        currentPage += 2;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage += 2;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
                else if (currentPage < totalPage)
                {
                    if (statusBtnGoods)
                    {
                        currentPage++;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage++;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage < totalPage)
                {
                    if (statusBtnGoods)
                    {
                        currentPage = totalPage;
                        if (!statusSearch)
                            loadDataGoods();
                        else
                            searchGoods();
                    }
                    else if (statusBtnCategoryOfGoods)
                    {
                        currentPage = totalPage;
                        if (!statusSearch)
                            loadDataCategoryOfGoods();
                        else
                            searchCate();
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblCurrentPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (totalPage < 2)
                {
                    btnFirst.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnDoublePrevious.Enabled = false;
                    btnNext.Enabled = false;
                    btnDoubleNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                else if(currentPage < 2)
                {
                    btnFirst.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnDoublePrevious.Enabled = false;
                    btnNext.Enabled = true;
                    btnDoubleNext.Enabled = true;
                    btnLast.Enabled = true;
                }
                else if (currentPage > totalPage - 1)
                {
                    btnFirst.Enabled = true;
                    btnPrevious.Enabled = true;
                    btnDoublePrevious.Enabled = true;
                    btnNext.Enabled = false;
                    btnDoubleNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                else
                {
                    btnFirst.Enabled = true;
                    btnPrevious.Enabled = true;
                    btnDoublePrevious.Enabled = true;
                    btnNext.Enabled = true;
                    btnDoubleNext.Enabled = true;
                    btnLast.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                currentPage = 1;
                if (statusBtnGoods)
                    loadDataGoods();
                else if (statusBtnCategoryOfGoods)
                    loadDataCategoryOfGoods();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDonGia_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtDonGia.Text, out decimal value))
            {
                txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", value);
            }
        }

        private void btnImportingGoods_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusBtnGoods)
                {
                    if (tableHangHoa.SelectedRows.Count > 0)
                    {
                        string mahh = tableHangHoa.SelectedRows[0].Cells[0].Value.ToString();

                        ImportingGoodsForm f = new ImportingGoodsForm();
                        f.MaHH = mahh;
                        f.ShowDialog();

                        if (f.flag)
                        {
                            loadDataGoods();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn mặt hàng muốn nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void searchGoods()
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
            {
                loadDataGoods();
            }
            else
            {
                string mahh = txtSearch.Text;
                string tenhh = txtSearch.Text;
                string loaihh = txtSearch.Text;
                string dvt = txtSearch.Text;
                decimal soluong, dongia;

                if (isDecimalNumeric(txtSearch.Text))
                {
                    soluong = decimal.Parse(txtSearch.Text);
                    dongia = decimal.Parse(txtSearch.Text);
                }
                else
                {
                    soluong = -1;
                    dongia = -1;
                }

                Goods goods = new Goods
                {
                    MaHangHoa = mahh,
                    LoaiHangHoa = loaihh,
                    TenHangHoa = tenhh,
                    DonViTinh = dvt,
                    DonGia = dongia,
                    SoLuong = soluong
                };
                int index = cbbSearch.SelectedIndex;
                pageSize = Convert.ToInt32(cbbRecord.Text);
                DataTable tbl = new DataTable();

                int pageStart = (currentPage - 1) * pageSize + 1;
                int pageEnd = currentPage * pageSize;

                switch(index)
                {
                    case 1:
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByMaHH(mahh) / pageSize);
                        break;
                    case 2:
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByLoaiHH(loaihh) / pageSize);
                        break;
                    case 3:
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByTenHH(tenhh) / pageSize);
                        break;
                    case 4:
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByDVT(dvt) / pageSize);
                        break;
                    case 5:
                        if (!isDecimalNumeric(txtSearch.Text))
                        {
                            MessageBox.Show("Yêu cầu nhập kí tự số để tìm kiếm theo số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtSearch.Focus();
                        }
                        else
                        {
                            totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsBySoluong(soluong) / pageSize);
                        }
                        break;
                    case 6:
                        if (!isDecimalNumeric(txtSearch.Text))
                        {
                            MessageBox.Show("Yêu cầu nhập kí tự số để tìm kiếm theo đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtSearch.Focus();
                        }
                        else
                        {
                            totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByDongia(dongia) / pageSize);
                        }
                        break;
                    default:
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordGoodsByAll(goods) / pageSize);
                        break;
                }

                tbl = goodsController.getSearchGoods(goods, index, pageStart, pageEnd);
                tableHangHoa.DataSource = tbl;

                totalPage = totalPage == 0 ? 1 : totalPage;
                lblCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
            }
        }

        void searchCate()
        {
            if (String.IsNullOrWhiteSpace(txtSearch.Text))
            {
                loadDataCategoryOfGoods();
            }
            else
            {
                string maloaihh = txtSearch.Text;
                string tenloaihh = txtSearch.Text;

                CategoryOfGoods cate = new CategoryOfGoods
                {
                    MaLoaiHangHoa = maloaihh,
                    TenLoaiHangHoa = tenloaihh
                };

                int index = cbbSearch.SelectedIndex;
                pageSize = Convert.ToInt32(cbbRecord.Text);
                DataTable tbl = new DataTable();

                int pageStart = (currentPage - 1) * pageSize + 1;
                int pageEnd = currentPage * pageSize;

                if (index == 1)
                    totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordCateByMaLoaiHH(maloaihh) / pageSize);
                else if (index == 2)
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordCateByTenLoaiHH(tenloaihh) / pageSize);
                     else
                        totalPage = (int)Math.Ceiling((double)goodsController.getTotalRecordCateByAll(cate) / pageSize);


                tbl = goodsController.getSearchCate(cate, index, pageStart, pageEnd);
                tableHangHoa.DataSource = tbl;

                totalPage = totalPage == 0 ? 1 : totalPage;
                lblCurrentPage.Text = $"{currentPage.ToString()}/{totalPage.ToString()}";
            }
        }
        
        private void txtSearch_IconRightClick(object sender, EventArgs e)
        {
            try
            {
                currentPage = 1;
                statusSearch = true;
                if (statusBtnGoods)
                    searchGoods();
                else if (statusBtnCategoryOfGoods)
                    searchCate();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
