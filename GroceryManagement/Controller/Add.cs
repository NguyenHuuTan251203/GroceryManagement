using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryManagement.Models;

namespace GroceryManagement.Controller
{
    public partial class Add
    {
        public static List<Nhanvien> ls = DBConText.db.Nhanviens.ToList();
    }

    //Employee
    public partial class Add
    {
        public static void Account(string fullName, string userName, string password,string id,string address)
        {

            Nhanvien employee = new Nhanvien();
            employee.MaNV = id;
            employee.TenNV = fullName;
            employee.Tentaikhoan = userName;
            employee.Matkhau = password;
            employee.Quyen = "User";
            employee.Diachi = address;
            DBConText.db.Nhanviens.Add(employee);
            DBConText.db.SaveChanges();
            MessageBox.Show("ThanhCong");

        }
    }
}
