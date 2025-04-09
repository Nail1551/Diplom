using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diplom;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    public class Navigation:BaseViewModel   
    {
        private object _currentView;
        private string _test;
      
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }
        #region Commands
        public ICommand ShowCarsCommand { get; set; }
        public ICommand ShowDevCommand { get; set; }
        public ICommand ShowReturnCommand { get; set; }
        #endregion

        public Navigation()
        {

            _currentView = new LoginViewModel(this);
            ShowCarsCommand = new RelayCommand(Cars);
            ShowDevCommand = new RelayCommand(DevCar);
            ShowReturnCommand = new RelayCommand(AccCar);
        }

        private void Cars(object obj)
        {
            CurrentView = new CarListViewModel(this);
        }
        private void DevCar(object obj)
        {
            CurrentView = new DevCarViewModel(this);
           
        }
        private void AccCar(object obj)
        {
            CurrentView = new AcceptViewModel(this);

        }
    }
}
