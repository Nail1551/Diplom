using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;

namespace Diplom.Utility
{
    public class CarClass:BaseViewModel
    {
        private int _carid;
        public int CarID
        {
            get => _carid;
            set
            {
                if (_carid != value)
                {
                    _carid = value;
                    OnPropertyChanged();
                }
            }
        }

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

    }
}
