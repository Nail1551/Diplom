using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diplom.Models;
using Diplom.Views;
using Diplom.ViewModels;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    
    public class MainViewModel:BaseViewModel
    {
        private Navigation _navigation;
        private MainModel _mainmodel;
        #region Commands
        public ICommand ShowCarsCommand { get; set; }
        public ICommand ShowDevCommand { get; set; }
        public ICommand ShowReturnCommand { get; set; }
        #endregion
        public MainViewModel(Navigation navigation)
        {
            _navigation = new Navigation();
            _mainmodel = new MainModel();
            ShowCarsCommand = new RelayCommand(Cars);
            ShowDevCommand = new RelayCommand(Dev);
            //ShowReturnCommand = new RelayCommand(deleteCar);
        }

        private void Cars(object obj)
        {
            _navigation.CurrentView = new CarListViewModel(_navigation);
        }
        private void Dev(object obj)
        {
            _navigation.CurrentView = new DevCarViewModel(_navigation);
        }
    }
}
