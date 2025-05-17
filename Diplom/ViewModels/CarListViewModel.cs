using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Diplom.Models;
using Diplom.Utility;
using Diplom.Data;
using System.Collections.Generic;

namespace Diplom.ViewModels
{
    public class CarListViewModel : BaseViewModel
    {
        private readonly CarListModel _carlistmodel;
        private readonly Navigation _navigation;

        private ObservableCollection<CarClass> _cars;
        public ObservableCollection<CarClass> Cars
        {
            get => _cars;
            set
            {
                _cars = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private ObservableCollection<CarClass> _filteredCars;
        public ObservableCollection<CarClass> FilteredCars
        {
            get => _filteredCars;
            set { _filteredCars = value; OnPropertyChanged(); }
        }

        private CarClass _currentCar;
        public CarClass CurrentCar
        {
            get => _currentCar;
            set { _currentCar = value; OnPropertyChanged(); }
        }

        private string _currentStatus;
        public string CurrentStatus
        {
            get => _currentStatus;
            set { _currentStatus = value; OnPropertyChanged(); ApplyFilters(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public List<string> Statuses { get; set; }

        public ICommand ClearFiltersCommand { get; set; }
        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public ICommand deleteCommand { get; set; }

        public CarListViewModel(Navigation navigation)
        {
            _navigation = navigation;
            _carlistmodel = new CarListModel();

            LoadCars();
            Statuses = _carlistmodel.getStatus(); 

            ClearFiltersCommand = new RelayCommand(clearFilters);
            addCommand = new RelayCommand(addCar);
            editCommand = new RelayCommand(editCar);
            deleteCommand = new RelayCommand(deleteCar);

            SearchText = string.Empty;
            CurrentStatus = null; 
        }

        private void LoadCars()
        {
            Cars = new ObservableCollection<CarClass>(_carlistmodel.getCars());
        }

        private void ApplyFilters()
        {
            if (Cars == null) return;

            var filtered = Cars.Where(car =>
                (string.IsNullOrWhiteSpace(SearchText) || car.LicensePlate.Contains(SearchText)) &&
                (string.IsNullOrWhiteSpace(CurrentStatus) || car.CarStatus == CurrentStatus));

            FilteredCars = new ObservableCollection<CarClass>(filtered);
        }

        private void clearFilters(object obj)
        {
            SearchText = string.Empty;
            CurrentStatus = null; 
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
                    ApplyFilters();
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
