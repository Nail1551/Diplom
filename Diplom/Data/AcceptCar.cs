using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;

namespace Diplom.Data
{
    [Table("AcceptCar")]
    public class AcceptCar:BaseViewModel 
    {
        
        private int _acceptID;
        [Key]
        public int AcceptID
        {
            get => _acceptID;
            set
            {
                if (_acceptID != value)
                {
                    _acceptID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _devID;

        public int DevID
        {
            get { return _devID; }
            set { _devID = value; }
        }


        private int _carID;
        public int CarID
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

       
        public int EmployeeID { get; set; }
        

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
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

        private DateTime _acceptDate;
        public DateTime AcceptDate
        {
            get => _acceptDate;
            set
            {
                if (_acceptDate != value)
                {
                    _acceptDate = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
