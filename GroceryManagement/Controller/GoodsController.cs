using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QuanLyTapHoa.Models;

namespace QuanLyTapHoa.Controller
{
    class GoodsController
    {
        DBAccessGoods dbGoods = new DBAccessGoods();
        
        public DataTable getTableHangHoa(int pageStart, int pageEnd)
        {
            return dbGoods.getListHangHoa_RowNumber(pageStart, pageEnd);
        }

        public DataTable getTableHangHoa_OF(int pageSize, int page)
        {
            return dbGoods.getListHangHoa_OffsetFech(pageSize, page);
        }

        public DataTable getTableHangHoa_Fill(int pageSize, int page)
        {
            return dbGoods.getListHangHoa_Fill(pageSize, page);
        }

        public DataTable getTableLoaiHangHoa(int pageSize, int page)
        {
            return dbGoods.getListLoaiHangHoa(pageSize, page);
        }

        public DataTable getLoaiHangHoa()
        {
            return dbGoods.LoaiHangHoa();
        }

        public int getTotalRecordHangHoa()
        {
            return dbGoods.totalRecordHangHoa();
        }

        public int getTotalRecordLoaiHangHoa()
        {
            return dbGoods.totalRecordLoaiHangHoa();
        }

        public string createMaHH()
        {
            string maHH = "";
            int total = getTotalRecordHangHoa() + 1;
            if (total < 10)
                maHH += "HH00" + total;
            else if (total < 100)
                maHH += "HH0" + total;
            else
                maHH += "HH" + total;
            return maHH;
        }

        public string createMaLoaiHH()
        {
            string maLoaiHH = "";
            int total = getTotalRecordLoaiHangHoa() + 1;
            if (total < 10)
                maLoaiHH += "LH00" + total;
            else if (total < 100)
                maLoaiHH += "LH0" + total;
            else
                maLoaiHH += "LH" + total;
            return maLoaiHH;
        }

        public bool addHangHoa(Goods addGoods)
        {
            return dbGoods.addHangHoa(addGoods);
        }

        public bool updateHangHoa(Goods updGoods)
        {
            return dbGoods.updateHangHoa(updGoods);
        }

        public bool deleteHangHoa(string mahh)
        {
            return dbGoods.deleteHangHoa(mahh);
        }

        public bool addLoaiHangHoa(CategoryOfGoods add)
        {
            return dbGoods.addLoaiHangHoa(add);
        }

        public bool updateLoaiHangHoa(CategoryOfGoods upd)
        {
            return dbGoods.updateLoaiHangHoa(upd);
        }

        public bool deleteLoaiHangHoa(string maloaihh)
        {
            return dbGoods.deleteLoaiHangHoa(maloaihh);
        }

        public Goods getGoodsByMaHH(string mahh)
        {
            return dbGoods.getGoodsByMaHH(mahh);
        }

        public bool importingGoods(Goods imp)
        {
            return dbGoods.importingGoods(imp);
        }

        public DataTable getSearchGoods(Goods goods, int index, int pageStart, int pageEnd)
        {
            return dbGoods.searchGoods(goods, index, pageStart, pageEnd);
        }

        public DataTable getSearchCate(CategoryOfGoods cate, int index, int pageStart, int pageEnd)
        {
            return dbGoods.searchCate(cate, index, pageStart, pageEnd);
        }

        public int getTotalRecordGoodsByMaHH(string mahh)
        {
            return dbGoods.totalRecordGoodsByMaHH(mahh);
        }

        public int getTotalRecordGoodsByLoaiHH(string loaihh)
        {
            return dbGoods.totalRecordGoodsByLoaiHH(loaihh);
        }

        public int getTotalRecordGoodsByTenHH(string tenhh)
        {
            return dbGoods.totalRecordGoodsByTenHH(tenhh);
        }

        public int getTotalRecordGoodsByDVT(string dvt)
        {
            return dbGoods.totalRecordGoodsByMaHH(dvt);
        }

        public int getTotalRecordGoodsByDongia(decimal dongia)
        {
            return dbGoods.totalRecordGoodsByDongia(dongia);
        }

        public int getTotalRecordGoodsBySoluong(decimal soluong)
        {
            return dbGoods.totalRecordGoodsBySoluong(soluong);
        }

        public int getTotalRecordGoodsByAll(Goods goods)
        {
            return dbGoods.totalRecordGoodsByAll(goods);
        }

        public int getTotalRecordCateByAll(CategoryOfGoods cate)
        {
            return dbGoods.totalRecordCateByAll(cate);
        }

        public int getTotalRecordCateByMaLoaiHH(string maloaihh)
        {
            return dbGoods.totalRecordCateByMaLoaiHH(maloaihh);
        }

        public int getTotalRecordCateByTenLoaiHH(string tenloaihh)
        {
            return dbGoods.totalRecordCateByTenLoaiHH(tenloaihh);
        }

        bool isDecimalNumeric(string number)
        {
            decimal num;
            return Decimal.TryParse(number, out num);
        }

        bool isEmpty(string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        bool isNull(string str)
        {
            return String.IsNullOrEmpty(str);
        }
    }
}
