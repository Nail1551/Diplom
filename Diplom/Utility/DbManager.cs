using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
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

        public static List<CarClass> getFreeCars()
        {
            using (var context = new ApplicationContext())
            {
                // Сначала из базы
                var cars = context.Cars
                                  .Where(c => c.CarStatusID == 1)
                                  .ToList(); // Загружаем в память

                // Потом преобразуем через метод
                return cars.Select(car => new CarClass
                {
                    CarID = car.CarID,
                    Brand = car.Brand,
                    Model = car.Model,
                    VIN = car.VIN,
                    LicensePlate = car.LicensePlate,
                    Mileage = car.Mileage,
                    CarStatus = getStatusById(car.CarStatusID) // Теперь можно
                }).ToList();

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
                        DevID = dev.DevID,
                        CarID = dev.CarID,  // сохраняем как int
                        LicensePlate = getCarPlateById(dev.CarID), // для отображения
                        ToEmployeeID =dev.ToEmployeeID,
                        FIO=GetFIOById(dev.ToEmployeeID),
                        Condition = dev.Condition,
                        Odometr=getOdometrbyID(dev.CarID),
                        TransferDate = dev.TransferDate,
                    };

                    devs.Add(userObj);
                }

                return devs;
            }
        }
        public static List<Employees> getEmployees()
        {
            using (var context = new ApplicationContext())
            {
                return context.Employees.ToList();
            }
        }
        public static void AddDev(int carId, int toEmployeeId, string condition, DateTime transferDate)
        {
            using (var context = new ApplicationContext())
            {
                // Получаем машину
                var car = context.Cars.FirstOrDefault(c => c.CarID == carId);
                int currentMileage = car?.Mileage ?? 0;

                // Создаем акт передачи
                var newDev = new DevCar
                {
                    CarID = carId,
                    ToEmployeeID = toEmployeeId,
                    Condition = condition,
                    Odometr = currentMileage,
                    TransferDate = transferDate
                };

                context.DevCar.Add(newDev);

                // Обновляем статус машины
                if (car != null)
                {
                    car.CarStatusID = 2; // Статус "Занят"
                }

                context.SaveChanges();
            }
        }

        public static void editDev(int id, int carId, int toEmployeeId, string condition, DateTime transferDate)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.FirstOrDefault(c => c.CarID == carId);
                int currentMileage = car?.Mileage ?? 0;

                // Находим и обновляем акт передачи
                var dev = context.DevCar.FirstOrDefault(d => d.DevID == id);
                if (dev != null)
                {
                    dev.CarID = carId;
                    dev.ToEmployeeID = toEmployeeId;
                    dev.Condition = condition;
                    dev.Odometr = currentMileage;
                    dev.TransferDate = transferDate;
                }

                // Обновляем статус авто
                if (car != null)
                {
                    car.CarStatusID = 2; // Статус "Занят"
                }

                context.SaveChanges();
            }
        }

        public static List<AcceptCarClass> getAccept()
        {
            using (var context = new ApplicationContext())
            {
                // создаем список из вспомогательного класса
                List<AcceptCarClass> accepts = new List<AcceptCarClass>();

                foreach (var accept in context.AcceptCar)
                {
                    // создаем экземпляр для которого заполняем данные и добавляем в список
                    AcceptCarClass acceptObj = new AcceptCarClass
                    {
                        AcceptID = accept.AcceptID,
                        DevID = accept.DevID,
                        CarID = accept.CarID,  // сохраняем как int
                        LicensePlate = getCarPlateById(accept.CarID), // для отображения
                        EmployeeID = accept.EmployeeID,
                        FIO = GetFIOById(accept.EmployeeID),
                        Comment = accept.Comment,
                        Odometr = getOdometrbyID(accept.CarID),
                        AcceptDate = accept.AcceptDate
                    };

                    accepts.Add(acceptObj);
                }

                return accepts;
            }
        }



        public static void AddAccept(int carId, int employeeId, string comment, DateTime acceptDate, int odometer)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.FirstOrDefault(c => c.CarID == carId);
                if (car == null)
                    return;

                
                if (odometer > car.Mileage)
                {
                    car.Mileage = odometer;
                }

                
                if (car.Mileage % 15000 == 0)
                {
                    car.CarStatusID = 3; 
                }
                else
                {
                    car.CarStatusID = 1; 
                }

                
                var lastDev = context.DevCar
                    .Where(d => d.CarID == carId)
                    .OrderByDescending(d => d.TransferDate)
                    .FirstOrDefault();

                int? devId = lastDev?.DevID;

                
                var accept = new AcceptCar
                {
                    CarID = carId,
                    DevID = devId ?? 0,
                    EmployeeID = employeeId,
                    Comment = comment,
                    Odometr = odometer,
                    AcceptDate = acceptDate
                };

                context.AcceptCar.Add(accept);
                context.SaveChanges();
            }
        }
        public static void UpdateAccept(int acceptId, int employeeId, string comment, DateTime acceptDate, int odometer)
        {
            using (var context = new ApplicationContext())
            {
                var accept = context.AcceptCar.FirstOrDefault(a => a.AcceptID == acceptId);
                if (accept == null)
                    return;

                var car = context.Cars.FirstOrDefault(c => c.CarID == accept.CarID);
                if (car == null)
                    return;

                // Обновляем пробег автомобиля, если он увеличился
                if (odometer > car.Mileage)
                {
                    car.Mileage = odometer;
                }

                // Обновляем статус автомобиля
                if (car.Mileage % 15000 == 0)
                {
                    car.CarStatusID = 3; // На ТО
                }
                else
                {
                    car.CarStatusID = 1; // Свободен
                }

                // Обновляем данные приёма
                accept.EmployeeID = employeeId;
                accept.Comment = comment;
                accept.AcceptDate = acceptDate;
                accept.Odometr = odometer;

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