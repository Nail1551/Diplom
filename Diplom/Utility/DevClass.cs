﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;

namespace Diplom.Utility
{
    public class DevClass:BaseViewModel
    {
        private int _devID;
        public int DevID
        {
            get => _devID;
            set
            {
                if (_devID != value)
                {
                    _devID = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _carID;
        public string CarID
        {
            get => _carID;
            set
            {
                if (_carID != value)
                {
                    _carID = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _toEmployeeID;
        public string ToEmployeeID
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

        private int? _odometr;
        public int? Odometr
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

    }
}

