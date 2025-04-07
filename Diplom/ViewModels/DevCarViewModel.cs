using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    public class DevCarViewModel:BaseViewModel
    {
        private CarDevModel _cardevmodel;
        private Navigation _navigation;

        private ObservableCollection<DevClass> _devcars;
        public ObservableCollection<DevClass> DevCar
        {
            get => _devcars;
            set
            {
                if (_devcars != value)
                {
                    _devcars = value;
                    OnPropertyChanged();
                }
            }
        }
        private DevClass _currentDev;
        public DevClass CurrentDev
        {
            get { return _currentDev; }
            set { _currentDev = value; OnPropertyChanged(); }
        }

        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public DevCarViewModel(Navigation navigation) 
        {
            _navigation = navigation;
            _cardevmodel = new CarDevModel();
            fillDevs();
            addCommand = new RelayCommand(addDev);
            editCommand = new RelayCommand(editDev);
        }

        private void fillDevs()
        {
            // получаем их
            var devs = _cardevmodel.getDev();
            // создаем список и заполняем его
            DevCar = new ObservableCollection<DevClass>();
            foreach (var dev in devs)
            {
                DevCar.Add(dev);
            }
        }

        private void addDev(object obj)
        {
            _navigation.CurrentView = new AddEditDevViewModel(_navigation, null);
        }
        private void editDev(object obj)
        {
            if (_cardevmodel.checkSelectedItem(CurrentDev))
            {
                _navigation.CurrentView = new AddEditDevViewModel(_navigation, CurrentDev);
            }
            else
            {
                MessageBox.Show("Выберите акт!");
            }
        }
    }
}
