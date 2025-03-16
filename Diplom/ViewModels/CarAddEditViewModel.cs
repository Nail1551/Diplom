using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;

namespace Diplom.ViewModels
{
     class CarAddEditViewModel:BaseViewModel
    {
        #region properties
        private CarAddEditModel _caraddeditmodel;
        
        private Navigation _navigation;
        
        private CarClass _car;

        private string _brand;
        public string Brand
        {
            get => _brand;
            set
            {
                if (_brand != value)
                {
                    _brand = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _model;
        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _vin;

        public string VIN
        {
            get { return _vin; }
            set { _vin = value; }
        }

        private string _licenseplate;

        public string LicensePlate
        {
            get { return _licenseplate; }
            set { _licenseplate = value; }
        }

        private int _mileage;

        public int Mileage
        {
            get { return _mileage; }
            set { _mileage = value; }
        }


        private string _carstatus;
        public string CarStatus
        {
            get { return _carstatus; }
            set { _carstatus = value; }
        }

        private List<string> _statuses;

        public List<string> Statuses
        {
            get { return _statuses; }
            set { _statuses = value; }
        }


        public ICommand CloseCommand { get; set; }
        public ICommand confirmCommand { get; set; }
        

        #endregion
        public CarAddEditViewModel(Navigation navigation,CarClass car) 
        {
            _navigation= navigation;
            _caraddeditmodel = new CarAddEditModel();
            Statuses = _caraddeditmodel.FillStatus();

            _car = null;
            if (car != null)
            {
                _car = car;
                Brand = car.Brand;
                Model = car.Model;
                VIN = car.VIN;
                LicensePlate = car.LicensePlate;
                Mileage = car.Mileage;
                CarStatus = car.CarStatus;
            }
            CloseCommand = new RelayCommand(goBack);
            confirmCommand = new RelayCommand(confirm);

        }
        private void goBack(object obj)
        {
            _navigation.CurrentView = new CarListViewModel(_navigation);
        }

        // метод подтверждения 
        private void confirm(object obj)
        {
            // если юзера нет(тоесть не передали при переходе), то создаем
            if (_car == null)
            {
                _caraddeditmodel.createCar( Brand,  Model,  VIN,  LicensePlate,  CarStatus, Mileage);
            }
            // иначе изменяем
            else
            {
                _caraddeditmodel.editCar(_car.CarID, Brand, Model, VIN, LicensePlate,CarStatus, Mileage);
            }
            // возвращаем на страницу с таблицей
            _navigation.CurrentView = new CarListViewModel(_navigation);
        }
    }
}
