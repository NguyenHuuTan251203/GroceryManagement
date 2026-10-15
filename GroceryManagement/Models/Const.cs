using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Write message error or notification ... in here   
namespace GroceryManagement.Models
{

    internal class Const
    {
        public static StringBuilder error = new StringBuilder();
        public const string Error_Contain_Letter = "The input can not include letter ";
        public const string Error_Contain_Number  = "The input can not include digit ";
        public const string Error_Contain_CharactersSpecial = "The input can not include characters special ";
        public const string Error_Input_Is_Null = "The input can not null ";


        public const string Error_Account_Not_Exist = "Account doesn't exist ";
        public const string Error_UserName_Exist = "This user name is already registered ";

        public const string Error_Incorrect_Account= "The UserName or Password is incorrect ";
        
        public const string Error_No_Selected_Row= "Please , Select any row in DataTable ";
        public const string Noti_Successful = "Successful ";
    }

   
}
