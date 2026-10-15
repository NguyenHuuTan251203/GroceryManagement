using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

//Write validate in here
namespace GroceryManagement.Models
{
    public partial class Validation
    {
        public static bool IsNotNumber(string input) => Regex.IsMatch(input, @"\d");
        public static bool IsNotContainsLetter(string input) => Regex.IsMatch(input, @"[a-zA-Z]");
        public static bool IsNotContainsCharactersSpecial(string input) => !Regex.IsMatch(input, @"[^\w\s]");
        public static bool IsNotNull(params string[] controls) => controls.All(control => !string.IsNullOrWhiteSpace(control));
    }

    //Validate Login and Register
    public partial class Validation
    {
        public static Nhanvien CheckUserNameExists(string userName)
            => DBConText.db.Nhanviens.FirstOrDefault(account => account.Tentaikhoan.Equals(userName));

        public static Nhanvien CheckAccount(string userName, string password, string authorization)
            => DBConText.db.Nhanviens.AsNoTracking()
               .FirstOrDefault
               (
                account =>
                account.Tentaikhoan == userName &&
                account.Matkhau == password &&
                account.Quyen == authorization
               );

    }
}
