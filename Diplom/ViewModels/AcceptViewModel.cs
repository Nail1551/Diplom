using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Models;
using Diplom.Utility;
using System.Windows.Input;
using Diplom.Data;

namespace Diplom.ViewModels
{
    public class AcceptViewModel:BaseViewModel
    {
        private AcceptModel _acceptmodel;
        private Navigation _navigation;

        private ObservableCollection<AcceptCarClass> _accept;
        public ObservableCollection<AcceptCarClass> Accept
        {
            get => _accept;
            set
            {
                if (_accept != value)
                {
                    _accept = value;
                    OnPropertyChanged();
                }
            }
        }
        private AcceptCarClass _currentAcc;
        public AcceptCarClass CurrentAcc
        {
            get { return _currentAcc; }
            set { _currentAcc = value; OnPropertyChanged(); }
        }

        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public AcceptViewModel(Navigation navigation) 
        {
            _navigation = navigation;
            _acceptmodel = new AcceptModel();
            fillAcc();
            //addCommand = new RelayCommand(addAcc);
            //editCommand = new RelayCommand(editAcc);
        }
        private void fillAcc()
        {
            // получаем их
            var accepts = _acceptmodel.getAccept();
            // создаем список и заполняем его
            Accept = new ObservableCollection<AcceptCarClass>();
            foreach (var accept in accepts)
            {
                Accept.Add(accept);
            }
        }

        //private void addAcc(object obj)
        //{
        //    _navigation.CurrentView = new AddEditDevViewModel(_navigation, null);
        //}
        //private void editDev(object obj)
        //{
        //    if (_cardevmodel.checkSelectedItem(CurrentDev))
        //    {
        //        _navigation.CurrentView = new AddEditDevViewModel(_navigation, CurrentDev);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выберите акт!");
        //    }
        //}
    }
}
