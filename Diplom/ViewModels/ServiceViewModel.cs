using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;
using System.Windows.Input;
using Diplom.ViewModels;
using System.Windows;
using Diplom.Models;

namespace Diplom.ViewModels
{
    public class ServiceViewModel : BaseViewModel
    {
        private ServiceModel _serviceModel;
        private Navigation _navigation;

        private ObservableCollection<ServiceCarClass> _service;
        public ObservableCollection<ServiceCarClass> Service
        {
            get => _service;
            set
            {
                _service = value;
                OnPropertyChanged();
            }
        }

        private ServiceCarClass _currentService;
        public ServiceCarClass CurrentService
        {
            get => _currentService;
            set
            {
                _currentService = value;
                OnPropertyChanged();
            }
        }

        public ICommand CompleteServiceCommand { get; set; }
        public ICommand editCommand { get; set; }

        public ServiceViewModel(Navigation navigation)
        {
            _navigation = navigation;
            _serviceModel = new ServiceModel();
            fillService();
            CompleteServiceCommand = new RelayCommand(CompleteService);

        }

        private void CompleteService(object obj)
        {
            if (CurrentService != null)
            {
                DbManager.CompleteService(CurrentService); 
                fillService(); 
            }
            else
            {
                MessageBox.Show("Выберите автомобиль для завершения ТО!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void fillService()
        {
            var list = DbManager.GetRepair(); // загружаем машины со статусом "На ТО"

            if (Service == null)
            {
                Service = new ObservableCollection<ServiceCarClass>();
            }
            else
            {
                Service.Clear(); // Очищаем текущую коллекцию
            }

            foreach (var item in list)
            {
                Service.Add(item); // Добавляем вручную
            }
        }


    }
}
