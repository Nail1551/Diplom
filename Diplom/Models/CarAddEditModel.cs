using System;
using System.Collections.Generic;
using System.Linq;
using Diplom.Utility;

namespace Diplom.Models
{
    class CarAddEditModel
    {
        private List<string> statuses;

        public List<string> FillStatus()
        {
            statuses = DbManager.getStatus();
            return statuses;
        }

        public void createCar(string brand, string model, string vin, string licenseplate, string carstatus, int mileage, string photopath)
        {
            DbManager.createСar(brand, model, vin, licenseplate, carstatus, mileage, photopath);
        }

        public void editCar(int id, string brand, string model, string vin, string licenseplate, string carstatus, int mileage, string photopath)
        {
            DbManager.editCar(id, brand, model, vin, licenseplate, carstatus, mileage, photopath);
        }

        // 🔽 ПРОВЕРКА УНИКАЛЬНОСТИ VIN
        public bool VinExists(string vin, int? excludeId = null)
        {
            var cars = DbManager.getCars();
            return cars.Any(car =>
                car.VIN.Equals(vin, StringComparison.OrdinalIgnoreCase) &&
                (!excludeId.HasValue || car.CarID != excludeId.Value));
        }

        // 🔽 ПРОВЕРКА УНИКАЛЬНОСТИ НОМЕРА
        public bool PlateExists(string plate, int? excludeId = null)
        {
            var cars = DbManager.getCars();
            return cars.Any(car =>
                car.LicensePlate.Equals(plate, StringComparison.OrdinalIgnoreCase) &&
                (!excludeId.HasValue || car.CarID != excludeId.Value));
        }

        public CarAddEditModel() { }
    }
}
