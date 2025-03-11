using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Diplom.ViewModels;

namespace Diplom.Data
{
    [Table("Employees")]
    public class Employees:BaseViewModel
    {
        
        private int _employeeid;
        [Key]
        public int EmployeeID
        {
            get { return _employeeid; }
            set
            {
                if (_employeeid != value)
                {
                    _employeeid = value;
                    OnPropertyChanged(nameof(EmployeeID));
                }
            }
        }

        private string _fio;
        public string FIO
        {
            get { return _fio; }
            set
            {
                if (_fio != value)
                {
                    _fio = value;
                    OnPropertyChanged(nameof(FIO));
                }
            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _position;
        public string Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }
    }
}
