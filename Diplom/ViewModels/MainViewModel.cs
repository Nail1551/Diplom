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
        #region Commands
        public ICommand ShowCarsCommand { get; set; }
        public ICommand ShowDevCommand { get; set; }
        public ICommand ShowReturnCommand { get; set; }
        #endregion
        public MainViewModel()
        {
            _navigation = new Navigation();
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
