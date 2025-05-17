using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Diplom.Data;
using Diplom.Models;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    class CarAddEditViewModel : BaseViewModel, IDataErrorInfo
    {
        private static readonly Regex _vinRegex =
            new Regex("^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _plateRegex =
            new Regex("^[АВЕКМНОРСТУХ]\\d{3}[АВЕКМНОРСТУХ]{2}\\d{2,3}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly CarAddEditModel _model;
        private readonly Navigation _nav;
        private readonly CarClass _car;

        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public ICommand CloseCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand SelectPhotoCommand { get; }

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
                    ValidateAll();
                }
            }
        }

        private string _modelName;
        public string Model
        {
            get => _modelName;
            set
            {
                if (_modelName != value)
                {
                    _modelName = value;
                    OnPropertyChanged();
                    ValidateAll();
                }
            }
        }

        private string _vin;
        public string VIN
        {
            get => _vin;
            set
            {
                var up = (value ?? "").ToUpper();
                if (_vin != up)
                {
                    _vin = up;
                    OnPropertyChanged();
                    ValidateAll();
                }
            }
        }

        private string _licensePlate;
        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                var up = (value ?? "").ToUpper();
                if (_licensePlate != up)
                {
                    _licensePlate = up;
                    OnPropertyChanged();
                    ValidateAll();
                }
            }
        }

        private string _carStatus;
        public string CarStatus
        {
            get => _carStatus;
            set
            {
                if (_carStatus != value)
                {
                    _carStatus = value;
                    OnPropertyChanged();
                    ValidateAll();
                }
            }
        }

        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                if (_photoPath != value)
                {
                    _photoPath = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _statuses;
        public List<string> Statuses
        {
            get => _statuses;
            private set
            {
                _statuses = value;
                OnPropertyChanged();
            }
        }

        public CarAddEditViewModel(Navigation navigation, CarClass car)
        {
            _nav = navigation;
            _model = new CarAddEditModel();
            _car = car;

            Statuses = _model.FillStatus();

            if (_car != null)
            {
                Brand = _car.Brand;
                Model = _car.Model;
                VIN = _car.VIN;
                LicensePlate = _car.LicensePlate;
                CarStatus = _car.CarStatus;
                PhotoPath = _car.PhotoPath;
            }

            CloseCommand = new RelayCommand(_ => GoBack());
            ConfirmCommand = new RelayCommand(_ => Confirm());
            SelectPhotoCommand = new RelayCommand(_ => SelectPhoto());

            ValidateAll();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string err = null;

                string vin = (VIN ?? "").Trim();
                string plate = (LicensePlate ?? "").Trim();
                int? currentId = _car?.CarID;

                switch (columnName)
                {
                    case nameof(Brand):
                        if (string.IsNullOrWhiteSpace(Brand))
                            err = "Марка не заполнена";
                        break;

                    case nameof(Model):
                        if (string.IsNullOrWhiteSpace(Model))
                            err = "Модель не заполнена";
                        break;

                    case nameof(VIN):
                        if (string.IsNullOrWhiteSpace(vin))
                            err = "VIN не заполнен";
                        else if (!_vinRegex.IsMatch(vin))
                            err = "VIN должен быть 17 символов (лат. буквы/цифры)";
                        else if (_model.VinExists(vin, currentId))
                            err = "Такой VIN уже существует";
                        break;

                    case nameof(LicensePlate):
                        if (string.IsNullOrWhiteSpace(plate))
                            err = "Госномер не заполнен";
                        else if (!_plateRegex.IsMatch(plate))
                            err = "Формат номера: А123ВС77";
                        else if (_model.PlateExists(plate, currentId))
                            err = "Такой госномер уже существует";
                        break;

                    case nameof(CarStatus):
                        if (string.IsNullOrWhiteSpace(CarStatus))
                            err = "Статус не выбран";
                        break;
                }

                if (err == null)
                    _errors.Remove(columnName);
                else
                    _errors[columnName] = err;

                return err;
            }
        }

        private void ValidateAll()
        {
            _ = this[nameof(Brand)];
            _ = this[nameof(Model)];
            _ = this[nameof(VIN)];
            _ = this[nameof(LicensePlate)];
            _ = this[nameof(CarStatus)];
        }

        private void GoBack()
        {
            _nav.CurrentView = new CarListViewModel(_nav);
        }

        private void Confirm()
        {
            ValidateAll();

            if (_errors.Count > 0)
            {
                var sb = new System.Text.StringBuilder("Пожалуйста, исправьте следующие ошибки:\n\n");
                foreach (var msg in _errors.Values)
                    sb.AppendLine(" • " + msg);

                MessageBox.Show(
                    sb.ToString(),
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            int defaultMileage = 0;
            string path = string.IsNullOrWhiteSpace(PhotoPath) ? null : PhotoPath;

            if (_car == null)
            {
                _model.createCar(
                    Brand,
                    Model,
                    VIN,
                    LicensePlate,
                    CarStatus,
                    defaultMileage,
                    path
                );
                MessageBox.Show("Успешно сохранено");
            }
            else
            {
                _model.editCar(
                    _car.CarID,
                    Brand,
                    Model,
                    VIN,
                    LicensePlate,
                    CarStatus,
                    defaultMileage,
                    path
                );
                MessageBox.Show("Изменение сохранено");
            }

            GoBack();
        }

        private void SelectPhoto()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };
            if (dlg.ShowDialog() == true)
                PhotoPath = dlg.FileName;
        }
    }
}
