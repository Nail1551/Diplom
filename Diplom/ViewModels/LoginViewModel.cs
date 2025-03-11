using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;
using Diplom.ViewModels;
using Diplom.Views;

using System.Windows;

namespace Diplom.ViewModels
{
    public class LoginViewModel:BaseViewModel
    {
        private LoginModel _loginModel ;
        private Navigation _navigation;
        private string _login;
        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        public ICommand CheckLoginCommand { get; set; }
        public LoginViewModel(Navigation navigation)
        {
            _loginModel = new LoginModel();
            _navigation = navigation;
            CheckLoginCommand = new RelayCommand(CheckLogin);
            
        }
        private void CheckLogin(object obj)
        {
            bool auth = _loginModel.CheckLogin(Login, Password);
            if (auth == true)
            {
                _navigation.CurrentView = new CarListViewModel(_navigation);
                
            }
            else
            {
                MessageBox.Show("DealTime auth", "Не верный логин или пароль!");
            }

        }

    }
}
