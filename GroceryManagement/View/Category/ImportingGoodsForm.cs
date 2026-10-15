using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyTapHoa.Models;
using QuanLyTapHoa.Controller;

namespace QuanLyTapHoa.View.GoodsManagement
{
    public partial class ImportingGoodsForm : Form
    {
        public ImportingGoodsForm()
        {
            InitializeComponent();
        }

        public string MaHH;
        public bool flag = false;
        GoodsController controller = new GoodsController();

        void infomationGoods()
        {
            Goods goods = controller.getGoodsByMaHH(MaHH);
            txtMaHH.Text = goods.MaHangHoa;
            txtLoaiHH.Text = goods.LoaiHangHoa;
            txtTenHH.Text = goods.TenHangHoa;
            txtDVTinh.Text = goods.DonViTinh;
            txtSoLuong.Text = goods.SoLuong.ToString();
            txtDonGia.Text = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", goods.DonGia);
        }

        private void ImportingGoods_Load(object sender, EventArgs e)
        {
            infomationGoods();
            txtSoLuong.Focus();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    import();
                    flag = true;
                    //txtSoLuong.ResetText();
                    infomationGoods();
                }
                else
                {
                    flag = false;
                    txtSoLuong.Focus();
                }
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
                if (DialogResult.Yes == MessageBox.Show("Bạn có muốn lưu thay đổi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    if (!flag)
                    {
                        flag = true;
                        import();
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("Error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool import()
        {
            if (String.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Số lượng không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (!isDecimalNumeric(txtSoLuong.Text))
                {
                    MessageBox.Show("Yêu cầu nhập kí tự số cho trường số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                }
                else
                {
                    decimal soluong = decimal.Parse(txtSoLuong.Text);
                    string mahh = txtMaHH.Text;
                    string loaihh = txtLoaiHH.Text;
                    string tenhh = txtTenHH.Text;
                    decimal dongia = decimal.Parse(string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C0}", txtDonGia.Text));
                    string dvt = txtDVTinh.Text;

                    Goods goods = new Goods(mahh, loaihh, tenhh, soluong, dongia, dvt);

                    if (controller.importingGoods(goods))
                    {
                        MessageBox.Show("Nhập hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Nhập hàng thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            return false;
        }

        bool isDecimalNumeric(string str)
        {
            //decimal number;
            return Decimal.TryParse(str, out decimal number);
        }

        
    }
}
