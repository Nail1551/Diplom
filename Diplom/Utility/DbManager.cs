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

        public static List<DevClass> getDev()
        {
            using (var context = new ApplicationContext())
            {
                // создаем список из вспомогательного класса
                List<DevClass> devs = new List<DevClass>();
                foreach (var dev in context.DevCar)
                {
                    // создаем экземпляр для которого заполняем данные и добавляем в список
                    DevClass userObj = new DevClass
                    {
                        CarID = getCarPlateById(dev.CarID),
                        ToEmployeeID=GetFIOById(dev.ToEmployeeID),
                        Condition = dev.Condition,
                        Odometr=getOdometrbyID(dev.CarID),
                        TransferDate = dev.TransferDate,
                    };

                    devs.Add(userObj);
                }

                return devs;
            }
        }
        public static void AddDev(string carId, string toEmployeeId, string condition, DateTime transferDate)
        {
            using (var context = new ApplicationContext())
            {
                // Получаем ID машины и сотрудника
                int carIDInt = getCarIdByPlate(carId);
                int employeeIDInt = getFIOIdByName(toEmployeeId);

                // Получаем актуальный пробег из Cars
                var car = context.Cars.FirstOrDefault(c => c.CarID == carIDInt);
                int currentMileage = car?.Mileage ?? 0;

                // 1. Добавляем акт выдачи
                var newDev = new DevCar
                {
                    CarID = carIDInt,
                    ToEmployeeID = employeeIDInt,
                    Condition = condition,
                    Odometr = currentMileage,
                    TransferDate = transferDate,
                    
                };

                context.DevCar.Add(newDev);

                // 2. Обновляем только статус авто на "Занят"
                if (car != null)
                {
                    car.CarStatusID = 2; // Статус "Занят"
                }

                // 3. Сохраняем изменения
                context.SaveChanges();
            }
        }
        public static void editDev(int id, string carId, string toEmployeeId, string condition, DateTime transferDate)
        {
            using (var context = new ApplicationContext())
            {
                int carIDInt = getCarIdByPlate(carId);
                int employeeIDInt = getFIOIdByName(toEmployeeId);

                // Получаем текущий пробег из Cars
                var car = context.Cars.FirstOrDefault(c => c.CarID == carIDInt);
                int currentMileage = car?.Mileage ?? 0;

                // Находим запись акта по DevID
                var dev = context.DevCar.FirstOrDefault(d => d.DevID == id);
                if (dev != null)
                {
                    dev.CarID = carIDInt;
                    dev.ToEmployeeID = employeeIDInt;
                    dev.Condition = condition;
                    dev.Odometr = currentMileage; // обновляем пробег на основании записи из Cars
                    dev.TransferDate = transferDate;
                    
                }

                // Обновляем только статус машины
                if (car != null)
                {
                    car.CarStatusID = 2; // Статус "Занят"
                }

                context.SaveChanges();
            }
        }


        public static string getCarPlateById(int id)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.FirstOrDefault(ch => ch.CarID == id);
                return car != null ? car.LicensePlate : null;
            }
        }
        public static int getCarIdByPlate(string name)
        {
            using (var context = new ApplicationContext())
            {

                return context.Cars.Where(ch => ch.LicensePlate == name).First().CarID;
            }
        }
        public static int? getOdometrbyID(int id)
        {
            using (var context = new ApplicationContext())
            {
                return context.Cars.FirstOrDefault(ch => ch.CarID == id)?.Mileage;
            }
        }
        public static string GetFIOById(int id)
        {
            using (var context = new ApplicationContext())
            {
                return context.Employees.Where(ch => ch.EmployeeID == id).First().FIO;
            }
        }
        public static int getFIOIdByName(string name)
        {
            using (var context = new ApplicationContext())
            {

                return context.Employees.Where(ch => ch.FIO == name).First().EmployeeID;
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