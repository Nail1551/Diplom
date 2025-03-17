using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    public class CarListViewModel:BaseViewModel
    {
        private CarListModel _carlistmodel;
        private Navigation _navigation;

        private ObservableCollection<CarClass> _cars;
        public ObservableCollection<CarClass> Cars
        {
            get => _cars;
            set
            {
                if (_cars != value)
                {
                    _cars = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TransferClass> _acts;
        public ObservableCollection<TransferClass> Acts
        {
            get => _acts;
            set
            {
                if (_acts != value)
                {
                    _acts = value;
                    OnPropertyChanged();
                }
            }
        }



        private List<string> _statuses;
        public List<string> Statuses
        {
            get { return _statuses; }
            set { _statuses = value; OnPropertyChanged(); }
        }

        private string _currentStatus;
        public string CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; OnPropertyChanged(); changeCarByFilter(); }
        }

        private CarClass _currentCar;
        public CarClass CurrentCar
        {
            get {return _currentCar; }
            set { _currentCar = value; OnPropertyChanged(); FilterAct(); }
        }

        #region Commands
        public ICommand ClearFiltersCommand { get; set; }
        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public ICommand deleteCommand { get; set; }
        #endregion
        public CarListViewModel(Navigation navigation) 
        {
            // получаем и заполняем данные
            _navigation = navigation;
            _carlistmodel = new CarListModel();
            // заполняем список пользователей
            fillCars();
            

            Statuses = _carlistmodel.getStatus();

            ClearFiltersCommand = new RelayCommand(clearFilters);
            addCommand = new RelayCommand(addCar);
            editCommand = new RelayCommand(editCar);
            deleteCommand = new RelayCommand(deleteCar);
        }
        private void fillCars()
        {
            // получаем их
            var cars = _carlistmodel.getCars();
            // создаем список и заполняем его
            Cars = new ObservableCollection<CarClass>();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
        }
        private void fillActs()
        {
            // получаем их
            var acts = _carlistmodel.getActs();
            // создаем список и заполняем его
            Acts = new ObservableCollection<TransferClass>();
            foreach (var act in acts)
            {
                Acts.Add(act);
            }
        }
        public void changeCarByFilter()
        {
            var cars = _carlistmodel.changeCarByFilter(CurrentStatus);
            Cars = new ObservableCollection<CarClass>();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
        }

        public void FilterAct()
        {
            if (CurrentCar != null)
            {
                var acts = _carlistmodel.changeActByCar(CurrentCar.CarID);
                Acts = new ObservableCollection<TransferClass>();

                foreach (var act in acts)
                {
                    Acts.Add(act);
                }
            }
            else
            {
                fillActs();
            }
        }

        private void clearFilters(object obj)
        {
            CurrentStatus = _carlistmodel.clearStatus();
            
        }

        private void addCar(object obj)
        {
            _navigation.CurrentView = new CarAddEditViewModel(_navigation, null);
        }
        private void editCar(object obj)
        {
            if (_carlistmodel.checkSelectedItem(CurrentCar))
            {
                _navigation.CurrentView = new CarAddEditViewModel(_navigation, CurrentCar);
            }
            else
            {
                MessageBox.Show("Выберите авто!");
            }
        }
        private void deleteCar(object obj)
        {
            if (_carlistmodel.checkSelectedItem(CurrentCar))
            {
                var result = MessageBox.Show("Вы действительно хотите удалить этот автомобиль?", "Подтверждение удаления",
                                             MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    _carlistmodel.deleteCar(CurrentCar);
                    Cars.Remove(CurrentCar);
                    MessageBox.Show("Удаление выполнено успешно", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите авто", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
