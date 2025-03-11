using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;

namespace Diplom.Utility
{
    public class TransferClass:BaseViewModel
    {
        private int _transferID;

        public int TransferID
        {
            get { return _transferID; }
            set { _transferID = value; }
        }

        private int _carID;

        public int CarID
        {
            get { return _carID; }
            set { _carID = value; }
        }

        private string _fromEmployeeFIO;

        public string FromEmployeeFIO
        {
            get { return _fromEmployeeFIO; }
            set { _fromEmployeeFIO = value; }
        }
        private string _toEmployeeFIO;

        public string ToEmployeeFIO
        {
            get { return _toEmployeeFIO; }
            set { _toEmployeeFIO = value; }
        }

        private DateTime _transferDate;

        public DateTime TransferDate
        {
            get { return _transferDate; }
            set { _transferDate = value; }
        }

        private string _condition;

        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

    }
}
