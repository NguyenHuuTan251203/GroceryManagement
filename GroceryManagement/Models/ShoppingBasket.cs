using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryManagement.Models
{
    class ShoppingBasket
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public string DonViTinh { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public int SLTonKho { get; set; }
    }
}
