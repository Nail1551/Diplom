using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Diplom.Models;
using Diplom.Utility;
using Diplom.Data;

namespace Diplom.ViewModels
{
    public class AddEditAcceptViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;
        private readonly AddEditAcceptModel _addEditAcceptModel;
        private AcceptCarClass _accept;
        private bool _isInternalUpdate = false;

        public AddEditAcceptViewModel(Navigation navigation, AcceptCarClass accept)
        {
            _navigation = navigation;
            _addEditAcceptModel = new AddEditAcceptModel();
            _accept = accept;

            Employees = new ObservableCollection<Employees>(DbManager.getEmployees());

            if (_accept != null)
            {
                AcceptID = _accept.AcceptID;
                _employeeId = _accept.EmployeeID;
                _carId = _accept.CarID;
                Comment = _accept.Comment;
                Odometr = _accept.Odometr?.ToString();
                AcceptDate = _accept.AcceptDate;

                Cars = new ObservableCollection<CarClass>(DbManager.GetCarsAvailableForAccept(_employeeId));

                OnPropertyChanged(nameof(EmployeeID));
                OnPropertyChanged(nameof(CarID));
            }
            else
            {
                AcceptDate = DateTime.Now;
                Cars = new ObservableCollection<CarClass>(DbManager.GetAllBusyCars());
            }

            CloseCommand = new RelayCommand(GoBack);
            SaveCommand = new RelayCommand(SaveAccept);
        }

        public ObservableCollection<CarClass> Cars { get; set; }
        public ObservableCollection<Employees> Employees { get; set; }

        public int AcceptID { get; set; }
        public int DevID { get; set; }

        private int _carId;
        public int CarID
        {
            get => _carId;
            set
            {
                if (_carId != value)
                {
                    _carId = value;
                    OnPropertyChanged();
                    LoadByCar(_carId);
                }
            }
        }

        private int _employeeId;
        public int EmployeeID
        {
            get => _employeeId;
            set
            {
                if (_employeeId != value)
                {
                    _employeeId = value;
                    OnPropertyChanged();
                    LoadCarsByEmployee(_employeeId);
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged(); }
        }

        private string _odometr;
        public string Odometr
        {
            get => _odometr;
            set { _odometr = value; OnPropertyChanged(); }
        }

        private DateTime _acceptDate;
        public DateTime AcceptDate
        {
            get => _acceptDate;
            set { _acceptDate = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private void GoBack(object obj)
        {
            _navigation.CurrentView = new AcceptViewModel(_navigation);
        }

        private void LoadByCar(int carId)
        {
            if (_isInternalUpdate) return;

            var dev = DbManager.GetLastDevByCar(carId);
            _isInternalUpdate = true;

            if (dev != null)
            {
                EmployeeID = dev.ToEmployeeID;
                DevID = dev.DevID;
                Odometr = DbManager.getOdometrbyID(carId)?.ToString();
                Comment = "";
            }
            else
            {
                EmployeeID = 0;
                DevID = 0;
                Odometr = "";
                Comment = "";
            }

            _isInternalUpdate = false;
        }

        private void LoadCarsByEmployee(int employeeId)
        {
            if (_isInternalUpdate) return;

            var filtered = DbManager.GetCarsAvailableForAccept(employeeId);
            Cars = new ObservableCollection<CarClass>(filtered);
            OnPropertyChanged(nameof(Cars));

            _isInternalUpdate = true;
            CarID = 0;
            DevID = 0;
            Odometr = "";
            Comment = "";
            _isInternalUpdate = false;

            if (filtered.Count == 0)
            {
                MessageBox.Show("У выбранного сотрудника нет выданных автомобилей в статусе 'Занято'.",
                                "Нет доступных машин", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveAccept(object obj)
        {
            if (!int.TryParse(Odometr, out int parsedOdometr))
            {
                MessageBox.Show("Введите корректный пробег.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EmployeeID == 0 || CarID == 0)
            {
                MessageBox.Show("Выберите сотрудника и автомобиль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Cars.Any(c => c.CarID == CarID))
            {
                MessageBox.Show("Машина недоступна для приёма. Проверьте, что она выдана сотруднику и в статусе 'Занято'.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_accept == null)
            {
                _addEditAcceptModel.createAcc(CarID, EmployeeID, Comment, AcceptDate, parsedOdometr);
            }
            else
            {
                _addEditAcceptModel.editAcc(AcceptID, EmployeeID, Comment, AcceptDate, parsedOdometr);
            }

            MessageBox.Show("Операция успешно завершена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigation.CurrentView = new AcceptViewModel(_navigation);
        }
    }
}
