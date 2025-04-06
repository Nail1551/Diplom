using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ObservableCollection<DevClass> DevCars
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
        public DevCarViewModel(Navigation navigation) 
        {
            _navigation = navigation;
            _cardevmodel = new CarDevModel();
            fillDevs();
        }

        private void fillDevs()
        {
            // получаем их
            var devs = _cardevmodel.getDev();
            // создаем список и заполняем его
            DevCars = new ObservableCollection<DevClass>();
            foreach (var dev in devs)
            {
                DevCars.Add(dev);
            }
        }
    }
}
