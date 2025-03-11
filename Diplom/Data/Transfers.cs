using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;

namespace Diplom.Data
{
    public class Transfers:BaseViewModel
    {
		
		 private int _transferID;
        [Key]
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

		private int _fromEmployeeID;

		public int FromEmployeeID
        {
			get { return _fromEmployeeID; }
			set { _fromEmployeeID = value; }
		}
		private int _toEmployeeID;

		public int ToEmployeeID
        {
			get { return _toEmployeeID; }
			set { _toEmployeeID = value; }
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
