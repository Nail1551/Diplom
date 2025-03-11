using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Data;

namespace Diplom.Utility
{
    public static class DbManager
    {
        public static bool CheckLogin(string login, string password)
        {
            using (var context = new ApplicationContext())
            {

                return context.Employees.Where(b => b.Login == login && b.Password == password).Any();
            }
        }
        public static List<CarClass> getCars()
        {
            using (var context = new ApplicationContext())
            {
                // создаем список из вспомогательного класса
                List<CarClass> cars = new List<CarClass>();
                foreach (var car in context.Cars)
                {
                    // создаем экземпляр для которого заполняем данные и добавляем в список
                    CarClass userObj = new CarClass
                    {
                        CarID = car.CarID,
                        Brand = car.Brand,
                        Model = car.Model,
                        VIN = car.VIN,
                        LicensePlate = car.LicensePlate,
                        Mileage = car.Mileage,
                        CarStatus = getStatusById(car.CarStatusID)
                    };

                    cars.Add(userObj);
                }

                return cars;
            }
        }

        public static List<TransferClass> getTransfers()
        {
            using (var context = new ApplicationContext())
            {
                // создаем список из вспомогательного класса
                List<TransferClass> transfers = new List<TransferClass>();
                foreach (var act in context.Transfers)
                {
                    // создаем экземпляр для которого заполняем данные и добавляем в список
                    TransferClass actObj = new TransferClass
                    {
                        TransferID = act.TransferID,
                        CarID= act.CarID,
                        FromEmployeeFIO = GetFIOById(act.FromEmployeeID),
                        ToEmployeeFIO = GetFIOById(act.ToEmployeeID),
                        TransferDate = act.TransferDate,
                        Condition = act.Condition,
                    };

                    transfers.Add(actObj);
                }

                return transfers;
            }
        }

        public static string GetFIOById(int id)
        {
            using (var context = new ApplicationContext())
            {
                return context.Employees.Where(ch => ch.EmployeeID == id).First().FIO;
            }
        }

        public static string getStatusById(int id)
        {
            using (var context = new ApplicationContext())
            {
                return context.CarStatus.Where(ch => ch.ID == id).First().Name;
            }
        }

        public static List<string> getStatus()
        {
            using (var context = new ApplicationContext())
            {
                return context.CarStatus.Select(ch => ch.Name).ToList();
            }
        }

        public static int getStatusIdByName(string name)
        {
            using (var context = new ApplicationContext())
            {
                
                return  context.CarStatus.Where(ch => ch.Name == name).First().ID;
            }
        }

        public static void createСar(string brand, string model, string vin, string licenseplate, string carstatus, int mileage)
        {
            using (var context = new ApplicationContext())
            {
                // создаем нового пользователя и заполняем его данными
                Cars car = new Cars();
                {
                    car.Brand = brand;
                    car.Model = model;
                    car.VIN = vin;
                    car.LicensePlate = licenseplate;
                    car.Mileage = mileage;
                    car.CarStatusID = getStatusIdByName(carstatus);
                };

                // добавляем его в список, сохраняем изменения и берем его айди
                context.Cars.Add(car);
                context.SaveChanges();
                int id = car.CarID;
                context.SaveChanges();
            }
        }
        public static void editCar(int id, string brand, string model, string vin, string licenseplate, string carstatus, int mileage)
        {
            using (var context = new ApplicationContext())
            {
                // берем по айди пользователя из списка
                Cars car = context.Cars.Where(ch => ch.CarID == id).First();
                // меняем его данные
                car.Brand = brand;
                car.Model = model;
                car.VIN = vin;
                car.LicensePlate = licenseplate;
                car.Mileage = mileage;
                car.CarStatusID = getStatusIdByName(carstatus);

                // сохраняем изменения
                context.SaveChanges();

            }
        }
        public static void deleteCarById(int id)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.Where(ch => ch.CarID == id).First();
                context.Cars.Remove(car);
                context.SaveChanges();
            }
        }
    }
}