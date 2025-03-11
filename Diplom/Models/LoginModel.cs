using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;

namespace Diplom.Models
{
    public class LoginModel
    {
        public bool CheckLogin(string Login, string Password)
        {
            return DbManager.CheckLogin(Login, Password);
        }
    }
}
