using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GroceryManagement
{
    internal class DataAcess
    {
        public static int ht = 0;
        public static string path = @"Data Source=DESKTOP-KGMTGTC\SQLEXPRESS;Initial Catalog=QuanLyTapHoa;Integrated Security=True;";

        public static SqlConnection TaoKetNot()
        {
            return new SqlConnection(path);
        }
        public static DataTable getTable(string sql)
        {

            DataTable dt = new DataTable();
            SqlConnection conn = TaoKetNot();
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(sql, conn);
            ad.Fill(dt);
            conn.Close();
            ad.Dispose();
            return dt;
        }

        public static void insertDeleteUpdate(string sql)
        {
            SqlConnection conn = TaoKetNot();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd.Dispose();
        }

        public static string getValue(string sql)
        {
            SqlConnection conn = TaoKetNot();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            object kq = cmd.ExecuteScalar();
            conn.Close();
            cmd.Dispose();
            if (kq == null)
            {
                return "";
            }
            else
                return kq.ToString();
        }
    }
}
