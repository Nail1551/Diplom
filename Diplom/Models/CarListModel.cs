using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;
using Diplom.ViewModels;

namespace Diplom.Models
{
    public class CarListModel
    {
        private List<string> statuses;
        private List<CarClass> Cars;
        private List<TransferClass> Acts;
        private Navigation _navigation;
        public List<CarClass> getCars()
        {
            return Cars;
        }
        public List<TransferClass> getActs()
        {
            return Acts;
        }
        public CarListModel()
        {
            Cars=DbManager.getCars(); 
            statuses = DbManager.getStatus();
            Acts=DbManager.getTransfers();
        }
        public List<string> getStatus()
        {
            return statuses;
        }

        public List<CarClass> changeCarByFilter(string statuses)
        {
            List<CarClass> cars = DbManager.getCars();
            if (string.IsNullOrEmpty(statuses) == false) cars = cars.Where(ch => ch.CarStatus == statuses).ToList();
            return cars;
        }

      

        public string clearStatus() => null;


        public bool checkSelectedItem(CarClass car)
        {
            return car != null;
        }
        public void deleteCar(CarClass car)
        {
            DbManager.deleteCarById(car.CarID);
        }
    }
}
