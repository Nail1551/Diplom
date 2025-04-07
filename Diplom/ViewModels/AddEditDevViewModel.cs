using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    public class AddEditDevViewModel:BaseViewModel
    {
        #region properties
        private AddEditDevCarModel _addEditDevCarModel;

        private Navigation _navigation;

        private DevClass _dev;
        public List<CarClass> Cars => DbManager.getFreeCars();
        
        public List<Employees> Employees => DbManager.getEmployees()  ;


        private int _carID;
        public int CarID
        {
            get => _carID;
            set
            {
                if (_carID != value)
                {
                    _carID = value;
                    Odometr = DbManager.getOdometrbyID(_carID)?.ToString(); // подтягиваем пробег при выборе
                    OnPropertyChanged();
                }
            }
        }

        private int _toEmployeeID;
        public int ToEmployeeID
        {
            get => _toEmployeeID;
            set
            {
                if (_toEmployeeID != value)
                {
                    _toEmployeeID = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _condition;
        public string Condition
        {
            get => _condition;
            set
            {
                if (_condition != value)
                {
                    _condition = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _odometr;
        public string Odometr
        {
            get => _odometr;
            set
            {
                if (_odometr != value)
                {
                    _odometr = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _transferDate;
        public DateTime TransferDate
        {
            get => _transferDate;
            set
            {
                if (_transferDate != value)
                {
                    _transferDate = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public ICommand CloseCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public AddEditDevViewModel(Navigation navigation,DevClass dev)
        {
            _navigation = navigation;
            _addEditDevCarModel=new AddEditDevCarModel();
            _dev = null;
            if (dev != null)
            {
                _dev = dev;
                CarID = dev.CarID;
                ToEmployeeID = dev.ToEmployeeID;
                TransferDate = dev.TransferDate;
                Condition = dev.Condition;
                Odometr = dev.Odometr.ToString();
            }
            else
            {
                TransferDate = DateTime.Now;
            }
           
            

            CloseCommand = new RelayCommand(GoBack);
            SaveCommand = new RelayCommand(SaveDevCar);
        }
        private void GoBack(object obj)
        {
            _navigation.CurrentView = new DevCarViewModel(_navigation);
        }
        private void SaveDevCar(object obj)
        {
            int parsedOdometer = 0;
            int.TryParse(Odometr, out parsedOdometer);
            if (_dev == null)
            {
                // Добавление
                _addEditDevCarModel.createDev(CarID, ToEmployeeID,Condition,TransferDate);

            }
            else
            {
                // Обновление
                _addEditDevCarModel.editDev(_dev.DevID, CarID, ToEmployeeID, Condition, TransferDate);

            }

            _navigation.CurrentView = new DevCarViewModel(_navigation);
        }
    }
}
