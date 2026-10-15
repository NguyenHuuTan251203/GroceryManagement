using hd;
using QuanLyTapHoa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using GroceryManagement.View;
using GioiThieu;


namespace GroceryManagement
{
    internal static class Program
    {
        public static string NameUser = "";
        public static string maNV  = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
