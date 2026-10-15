using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTapHoa.Models
{
    class Goods
    {
        private string maHangHoa;
        private string loaiHangHoa;
        private string tenHangHoa;
        private decimal soLuong;
        private decimal donGia;
        private string donViTinh;

        public Goods() { }

        public Goods(string maHangHoa, string maLoaiHangHoa, string tenHangHoa, decimal soLuong, decimal donGia, string donViTinh)
        {
            this.maHangHoa = maHangHoa;
            this.loaiHangHoa = maLoaiHangHoa;
            this.tenHangHoa = tenHangHoa;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.donViTinh = donViTinh;
        }

        public string MaHangHoa { get => maHangHoa; set => maHangHoa = value; }
        public string TenHangHoa { get => tenHangHoa; set => tenHangHoa = value; }
        public decimal SoLuong { get => soLuong; set => soLuong = value; }
        public decimal DonGia { get => donGia; set => donGia = value; }
        public string DonViTinh { get => donViTinh; set => donViTinh = value; }
        public string LoaiHangHoa { get => loaiHangHoa; set => loaiHangHoa = value; }
    }
}
