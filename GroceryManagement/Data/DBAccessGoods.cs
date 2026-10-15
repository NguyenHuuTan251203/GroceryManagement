using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace QuanLyTapHoa.Models
{
    class DBAccessGoods
    {
        SqlConnection conn;

        public DBAccessGoods()
        {
            string path = @"Data Source=DESKTOP-KGMTGTC\SQLEXPRESS;Initial Catalog=QuanLyTapHoa;Integrated Security=True;";
          
            conn = new SqlConnection(path);
        }

        public DataTable getListHangHoa_RowNumber(int pageStart, int pageEnd)
        {
            conn.Open();
            string sql = @"select Mahang, TenLoai, Tenhang, DVT, Soluong, Dongia 
                from 
                (
                    select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                    from Hanghoa
					Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                ) AS RowConstrainedResult
				where RowNum BETWEEN @StartRow AND @EndRow
                order by RowNum";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@StartRow", pageStart);
            cmd.Parameters.AddWithValue("@EndRow", pageEnd);

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            cmd.Dispose();
            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public DataTable getListHangHoa_OffsetFech(int pageSize, int page)
        {
            conn.Open();
            string sql = @"SELECT Mahang, Loaihang, Tenhang, Tenloai, DVT, Soluong, Dongia 
                from 
                (
                    SELECT Hanghoa.*, Loaihang.* 
                    FROM Hanghoa
					Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                ) AS RowConstrainedResult
                ORDER BY Mahang
                OFFSET @page ROWS
                FETCH NEXT @pagesize ROW ONLY";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pagesize", pageSize);
            cmd.Parameters.AddWithValue("@page", page);

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            cmd.Dispose();
            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public DataTable getListHangHoa_Fill(int pageSize, int page)
        {
            conn.Open();
            string sql = @"SELECT Mahang, Loaihang, Tenhang, Tenloai, DVT, Soluong, Dongia 
                        FROM Hanghoa
					    Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                        ORDER BY Mahang";

            SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
            DataTable tbl = new DataTable();
            adap.Fill(pageSize, page, tbl);

            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public DataTable getListLoaiHangHoa(int pageStart, int pageEnd)
        {
            conn.Open();
            string sql = @"select Maloai, TenLoai
                from 
                (
                    select row_number() over (order by Maloai) AS RowNum, * 
                    from Loaihang
                ) AS RowConstrainedResult
				where RowNum BETWEEN @StartRow AND @EndRow
                order by RowNum";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@StartRow", pageStart);
            cmd.Parameters.AddWithValue("@EndRow", pageEnd);

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            cmd.Dispose();
            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public int totalRecordLoaiHangHoa()
        {
            conn.Open();

            string sql = @"select count(distinct Maloai) from Loaihang";
            SqlCommand cmd = new SqlCommand(sql, conn);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordHangHoa()
        {
            conn.Open();

            string sql = @"select count(distinct Mahang) from Hanghoa";
            SqlCommand cmd = new SqlCommand(sql, conn);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public DataTable LoaiHangHoa()
        {
            conn.Open();
            string sql = @"select distinct * from Loaihang";

            SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public Goods getGoodsByMaHH(string mahh)
        {
            Goods goods = null;
            conn.Open();

            string sql = @"select * from Hanghoa where Mahang=@mahang";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mahang", mahh);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                goods = new Goods
                {
                    MaHangHoa = reader["Mahang"].ToString(),
                    LoaiHangHoa = reader["Loaihang"] as string,
                    TenHangHoa = reader["Tenhang"] as string,
                    DonViTinh = reader["DVT"] as string,
                    SoLuong = (decimal)(reader["Soluong"] != DBNull.Value ? Convert.ToDecimal(reader["Soluong"]) : (decimal?)null),
                    DonGia = (decimal)(reader["Dongia"] != DBNull.Value ? Convert.ToDecimal(reader["Dongia"]) : (decimal?)null)
                };
            }

            cmd.Dispose();
            reader.Close();
            conn.Close();

            return goods;
        }

        public bool KT_MaHangHoa(string maHangHoa)
        {
            conn.Open();

            string sql = @"select Mahang from Hanghoa where Mahang=@mahang";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mahang", maHangHoa);
            SqlDataReader reader = cmd.ExecuteReader();

            bool result = reader.Read();

            cmd.Dispose();
            reader.Close();
            conn.Close();

            return result;
        }

        public bool KT_MaLoaiHangHoa(string maLoaiHangHoa)
        {
            conn.Open();

            string sql = @"select Maloai from Loaihang where Maloai=@maloai";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maloai", maLoaiHangHoa);
            SqlDataReader reader = cmd.ExecuteReader();

            bool result = reader.Read();

            cmd.Dispose();
            reader.Close();
            conn.Close();

            return result;
        }

        public bool KT_MaLoaiHangHoa_InHangHoa(string maLoaiHangHoa)
        {
            conn.Open();

            string sql = @"select Loaihang from Hanghoa where Loaihang=@maloai";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maloai", maLoaiHangHoa);
            SqlDataReader reader = cmd.ExecuteReader();

            bool result = reader.Read();

            cmd.Dispose();
            reader.Close();
            conn.Close();

            return result;
        }

        public bool KT_MaLoaiHangHoa_InCTHoaDon(string maHangHoa)
        {
            conn.Open();

            string sql = @"select Mahang from CT_Hoadon where Mahang=@mahang";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mahang", maHangHoa);
            SqlDataReader reader = cmd.ExecuteReader();

            bool result = reader.Read();

            cmd.Dispose();
            reader.Close();
            conn.Close();

            return result;
        }

        public bool addHangHoa(Goods goods)
        {
            //string mahh = goods.MaHangHoa;
            //string loaihh = String.IsNullOrEmpty(goods.LoaiHangHoa) ? null : goods.LoaiHangHoa;

            if (!KT_MaHangHoa(goods.MaHangHoa))
            {
                conn.Open();
                string sql = @"insert into Hanghoa(Mahang, Loaihang, Tenhang, DVT, Soluong, Dongia)
                               values (@mahang, @loaihang, @tenhang, @dvt, @soluong, @dongia)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mahang", goods.MaHangHoa);
                cmd.Parameters.AddWithValue("@loaihang", goods.LoaiHangHoa);
                //cmd.Parameters.Add("@loaihang", SqlDbType.VarChar).Value = loaihh ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@tenhang", goods.TenHangHoa);
                cmd.Parameters.AddWithValue("@dvt", goods.DonViTinh);
                cmd.Parameters.AddWithValue("@soluong", goods.SoLuong);
                cmd.Parameters.AddWithValue("@dongia", goods.DonGia);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool updateHangHoa(Goods goods)
        {
            if (KT_MaHangHoa(goods.MaHangHoa))
            {
                conn.Open();

                string sql = @"update Hanghoa 
                               set Loaihang=@loaihang, Tenhang=@tenhang, DVT=@dvt, Soluong=@soluong, Dongia=@dongia 
                               where Mahang=@mahang";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mahang", goods.MaHangHoa);
                cmd.Parameters.AddWithValue("@loaihang", goods.LoaiHangHoa);
                cmd.Parameters.AddWithValue("@tenhang", goods.TenHangHoa);
                cmd.Parameters.AddWithValue("@dvt", goods.DonViTinh);
                cmd.Parameters.AddWithValue("@soluong", goods.SoLuong);
                cmd.Parameters.AddWithValue("@dongia", goods.DonGia);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool deleteHangHoa(string maHangHoa)
        {
            if (!KT_MaLoaiHangHoa_InCTHoaDon(maHangHoa))
            {
                conn.Open();

                string sql = @"delete from Hanghoa where Mahang=@mahang";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mahang", maHangHoa);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }


        public bool addLoaiHangHoa(CategoryOfGoods categoryOfGoods)
        {
            if (!KT_MaLoaiHangHoa(categoryOfGoods.MaLoaiHangHoa))
            {
                conn.Open();
                string sql = @"insert into Loaihang(Maloai, Tenloai)
                               values (@maloai, @tenloai)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maloai", categoryOfGoods.MaLoaiHangHoa);
                cmd.Parameters.AddWithValue("@tenloai", categoryOfGoods.TenLoaiHangHoa);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool updateLoaiHangHoa(CategoryOfGoods categoryOfGoods)
        {
            if (KT_MaLoaiHangHoa(categoryOfGoods.MaLoaiHangHoa))
            {
                conn.Open();
                string sql = @"update Loaihang set Tenloai=@tenloai where Maloai=@maloai";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maloai", categoryOfGoods.MaLoaiHangHoa);
                cmd.Parameters.AddWithValue("@tenloai", categoryOfGoods.TenLoaiHangHoa);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool deleteLoaiHangHoa(string maLoaiHangHoa)
        {
            if (!KT_MaLoaiHangHoa_InHangHoa(maLoaiHangHoa))
            {
                conn.Open();

                string sql = @"delete from Loaihang where Maloai=@maloai";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@maloai", maLoaiHangHoa);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool importingGoods(Goods goods)
        {
            if (KT_MaHangHoa(goods.MaHangHoa))
            {
                conn.Open();

                string sql = @"update Hanghoa set Soluong=Soluong+@soluong where Mahang=@mahang";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mahang", goods.MaHangHoa);
                cmd.Parameters.AddWithValue("@soluong", goods.SoLuong);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            return false;
        }

        public DataTable searchGoods(Goods goods, int index, int pageStart, int pageEnd)
        {
            conn.Open();
            string sql;
            SqlCommand cmd;

            switch (index)
            {
                case 1:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where Mahang like '%' + @mahang + '%' and RowNum BETWEEN @StartRow AND @EndRow
                            order by RowNum";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@mahang", goods.MaHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 2:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where Tenloai like '%' + @loaihang + '%' and RowNum BETWEEN @StartRow AND @EndRow
                            order by RowNum";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@loaihang", goods.LoaiHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 3:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where Tenhang like '%' + @tenhang + '%' and RowNum BETWEEN @StartRow AND @EndRow
                            order by RowNum";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tenhang", goods.TenHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 4:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where DVT like '%' + @donvitinh + '%' and RowNum BETWEEN @StartRow AND @EndRow
                            order by RowNum";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@donvitinh", goods.DonViTinh);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 5:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where Soluong<=@soluong and RowNum BETWEEN @StartRow AND @EndRow
                            order by Soluong desc";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@soluong", goods.LoaiHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 6:
                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where Dongia<=@dongia and RowNum BETWEEN @StartRow AND @EndRow
                            order by Dongia desc";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@dongia", goods.DonGia);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                default:

                    decimal? dongia = goods.DonGia == -1 ? (decimal?)null : goods.DonGia;
                    decimal? soluong = goods.SoLuong == -1 ? (decimal?)null : goods.DonGia;

                    sql = @"select Mahang, Tenloai, Tenhang, DVT, Soluong, Dongia 
                            from 
                            (
                                select row_number() over (order by Hanghoa.Mahang) AS RowNum, Hanghoa.*, Loaihang.* 
                                from Hanghoa
					            Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
                            ) AS RowConstrainedResult
				            where (RowNum BETWEEN @StartRow AND @EndRow)
                            and 
                            (
                                (@mahang IS NULL OR Mahang like '%' + @mahang + '%')
                                or (@loaihang IS NULL OR Tenloai like '%' + @loaihang + '%')
                                or (@tenhang IS NULL OR Tenhang like '%' + @tenhang + '%')
                                or (@donvitinh IS NULL OR DVT like '%' + @donvitinh + '%')
                                or (Dongia IS NULL OR Dongia <= @dongia)
                                or (Soluong IS NULL OR Soluong <= @soluong)
                            )
                            order by RowNum";

                    cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@mahang", String.IsNullOrEmpty(goods.MaHangHoa) ? (object)DBNull.Value : goods.MaHangHoa);
                    cmd.Parameters.AddWithValue("@loaihang", String.IsNullOrEmpty(goods.LoaiHangHoa) ? (object)DBNull.Value : goods.LoaiHangHoa);
                    cmd.Parameters.AddWithValue("@tenhang", String.IsNullOrEmpty(goods.TenHangHoa) ? (object)DBNull.Value : goods.TenHangHoa);
                    cmd.Parameters.AddWithValue("@donvitinh", String.IsNullOrEmpty(goods.DonViTinh) ? (object)DBNull.Value : goods.DonViTinh);
                    cmd.Parameters.AddWithValue("@dongia", dongia.HasValue ? (object)dongia.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@soluong", soluong.HasValue ? (object)soluong.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
            }

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            cmd.Dispose();
            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public DataTable searchCate(CategoryOfGoods cate, int index, int pageStart, int pageEnd)
        {
            conn.Open();
            string sql;
            SqlCommand cmd;

            switch(index)
            {
                case 1:
                    sql = @"select Maloai, TenLoai
                            from 
                            (
                                select row_number() over (order by Maloai) AS RowNum, * 
                                from Loaihang
                            ) AS RowConstrainedResult
				            where RowNum BETWEEN @StartRow AND @EndRow
                            and Maloai like '%' + @maloai + '%'
                            order by Maloai asc";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maloai", cate.MaLoaiHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                case 2:
                    sql = @"select Maloai, TenLoai
                            from 
                            (
                                select row_number() over (order by Maloai) AS RowNum, * 
                                from Loaihang
                            ) AS RowConstrainedResult
				            where RowNum BETWEEN @StartRow AND @EndRow
                            and Tenloai like '%' + @tenloai + '%'
                            order by Maloai asc";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tenloai", cate.TenLoaiHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
                default:
                    sql = @"select Maloai, TenLoai
                            from 
                            (
                                select row_number() over (order by Maloai) AS RowNum, * 
                                from Loaihang
                            ) AS RowConstrainedResult
				            where RowNum BETWEEN @StartRow AND @EndRow
                            and 
                            (Maloai like '%' + @maloai + '%')
                            or (Tenloai like '%' + @tenloai + '%')
                            order by Maloai asc";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maloai", cate.MaLoaiHangHoa);
                    cmd.Parameters.AddWithValue("@tenloai", cate.TenLoaiHangHoa);
                    cmd.Parameters.AddWithValue("@StartRow", pageStart);
                    cmd.Parameters.AddWithValue("@EndRow", pageEnd);
                    break;
            }

            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adap.Fill(tbl);

            cmd.Dispose();
            adap.Dispose();
            conn.Close();

            return tbl;
        }

        public int totalRecordCateByMaLoaiHH(string maloaihh)
        {
            conn.Open();

            string sql = @"select Count (*)
                           from Loaihang
				           where Maloai like '%' + @maloai + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maloai", maloaihh);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordCateByTenLoaiHH(string tenloaihh)
        {
            conn.Open();

            string sql = @"select Count (*)
                           from Loaihang
				           where Tenloai like '%' + @tenloai + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tenloai", tenloaihh);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordCateByAll(CategoryOfGoods cate)
        {
            conn.Open();

            string sql = @"select count (*)
                           from Loaihang
				           where
                            Maloai like '%' + @maloai + '%'
                            or Tenloai like '%' + @tenloai + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@maloai", cate.MaLoaiHangHoa);
            cmd.Parameters.AddWithValue("@tenloai", cate.TenLoaiHangHoa);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByMaHH(string mahh)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where Mahang like '%' + @mahang + '%'";
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mahang", mahh);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByLoaiHH(string loaihh)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where Tenloai like '%' + @loaihang + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@loaihang", loaihh);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByTenHH(string tenhh)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where Tenhang like '%' + @tenhang + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@tenhang", tenhh);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByDVT(string dvt)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where DVT like '%' + @dvt + '%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dvt", dvt);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByDongia(decimal dongia)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where Dongia<=@dongia";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dongia", dongia);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsBySoluong(decimal soluong)
        {
            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where Soluong<=@soluong";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@soluong", soluong);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

        public int totalRecordGoodsByAll(Goods goods)
        {
            decimal? dongia = goods.DonGia == -1 ? (decimal?)null : goods.DonGia;
            decimal? soluong = goods.SoLuong == -1 ? (decimal?)null : goods.DonGia;

            conn.Open();

            string sql = @"select count (*) 
                           from Hanghoa
					       Inner Join Loaihang on Loaihang.Maloai = Hanghoa.LoaiHang
				           where
                                (@mahang IS NULL OR Mahang like '%' + @mahang + '%')
                                or (@loaihang IS NULL OR Tenloai like '%' + @loaihang + '%')
                                or (@tenhang IS NULL OR Tenhang like '%' + @tenhang + '%')
                                or (@donvitinh IS NULL OR DVT like '%' + @donvitinh + '%')
                                or (Dongia IS NULL OR Dongia <= @dongia)
                                or (Soluong IS NULL OR Soluong <= @soluong)";

            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@mahang", String.IsNullOrEmpty(goods.MaHangHoa) ? (object)DBNull.Value : goods.MaHangHoa);
            cmd.Parameters.AddWithValue("@loaihang", String.IsNullOrEmpty(goods.LoaiHangHoa) ? (object)DBNull.Value : goods.LoaiHangHoa);
            cmd.Parameters.AddWithValue("@tenhang", String.IsNullOrEmpty(goods.TenHangHoa) ? (object)DBNull.Value : goods.TenHangHoa);
            cmd.Parameters.AddWithValue("@donvitinh", String.IsNullOrEmpty(goods.DonViTinh) ? (object)DBNull.Value : goods.DonViTinh);
            cmd.Parameters.AddWithValue("@dongia", dongia.HasValue ? (object)dongia.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@soluong", soluong.HasValue ? (object)soluong.Value : DBNull.Value);

            int total = (int)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Close();

            return total;
        }

    }
}
