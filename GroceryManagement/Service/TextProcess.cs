using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GroceryManagement.Service
{
    internal class TextProcess
    {
        public static void ResetText(params Control[] control)
        {
            foreach (var textBox in control)
            {
                textBox.ResetText();
            }
        }
    }
}
