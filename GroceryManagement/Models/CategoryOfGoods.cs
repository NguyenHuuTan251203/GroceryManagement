using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTapHoa.Models
{
    class CategoryOfGoods
    {
        private string maLoaiHangHoa;
        private string tenLoaiHangHoa;

        public CategoryOfGoods() { }

        public CategoryOfGoods(string maLoaiHangHoa, string tenLoaiHangHoa)
        {
            this.MaLoaiHangHoa = maLoaiHangHoa;
            this.TenLoaiHangHoa = tenLoaiHangHoa;
        }

        public string MaLoaiHangHoa { get => maLoaiHangHoa; set => maLoaiHangHoa = value; }
        public string TenLoaiHangHoa { get => tenLoaiHangHoa; set => tenLoaiHangHoa = value; }
    }
}
