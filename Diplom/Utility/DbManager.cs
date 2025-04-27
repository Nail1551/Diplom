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
                        CarStatus = getStatusById(car.CarStatusID),
                        PhotoPath = car.PhotoPath
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
                List<DevClass> devs = new List<DevClass>();

                foreach (var dev in context.DevCar)
                {
                    DevClass devObj = new DevClass
                    {
                        DevID = dev.DevID,
                        CarID = dev.CarID,
                        LicensePlate = getCarPlateById(dev.CarID),
                        ToEmployeeID = dev.ToEmployeeID,
                        FIO = GetFIOById(dev.ToEmployeeID),
                        Condition = dev.Condition,
                        Odometr = dev.Odometr,
                        TransferDate = dev.TransferDate
                    };

                    devs.Add(devObj);
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
                var car = context.Cars.FirstOrDefault(c => c.CarID == carId);
                int currentMileage = car?.Mileage ?? 0;

                var newDev = new DevCar
                {
                    CarID = carId,
                    ToEmployeeID = toEmployeeId,
                    Condition = condition,
                    Odometr = currentMileage,         // сохраняем пробег только при создании
                    TransferDate = transferDate
                };

                context.DevCar.Add(newDev);

                if (car != null)
                {
                    car.CarStatusID = 2; // "Занят"
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
                List<AcceptCarClass> accepts = new List<AcceptCarClass>();

                foreach (var accept in context.AcceptCar)
                {
                    AcceptCarClass acceptObj = new AcceptCarClass
                    {
                        AcceptID = accept.AcceptID,
                        DevID = accept.DevID,
                        CarID = accept.CarID,
                        LicensePlate = getCarPlateById(accept.CarID),
                        EmployeeID = accept.EmployeeID,
                        FIO = GetFIOById(accept.EmployeeID),
                        Comment = accept.Comment,
                        Odometr = accept.Odometr, // ← фиксированное значение из приёмки
                        AcceptDate = accept.AcceptDate
                    };

                    accepts.Add(acceptObj);
                }

                return accepts;
            }
        }


        public static List<CarClass> GetCarsAvailableForAccept(int employeeId)
        {
            using (var context = new ApplicationContext())
            {
                var result = (from dev in context.DevCar
                              join car in context.Cars on dev.CarID equals car.CarID
                              where car.CarStatusID == 2
                                    && dev.ToEmployeeID == employeeId
                                    && dev.TransferDate == context.DevCar
                                         .Where(d => d.CarID == car.CarID)
                                         .Max(d => d.TransferDate)
                              select new CarClass
                              {
                                  CarID = car.CarID,
                                  LicensePlate = car.LicensePlate
                              }).ToList();

                return result;
            }
        }
        public static List<CarClass> GetAllBusyCars()
        {
            using (var context = new ApplicationContext())
            {
                return (from car in context.Cars
                        where car.CarStatusID == 2
                        select new CarClass
                        {
                            CarID = car.CarID,
                            LicensePlate = car.LicensePlate
                        }).ToList();
            }
        }

        public static DevCar GetLastDevByCar(int carId)
        {
            using (var context = new ApplicationContext())
            {
                return context.DevCar
                    .Where(d => d.CarID == carId)
                    .OrderByDescending(d => d.TransferDate)
                    .FirstOrDefault();
            }
        }

        public static void AddAccept(int carId, int employeeId, string comment, DateTime acceptDate, int odometer)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.FirstOrDefault(c => c.CarID == carId);
                if (car == null) return;

                var dev = context.DevCar
                    .Where(d => d.CarID == carId)
                    .OrderByDescending(d => d.TransferDate)
                    .FirstOrDefault();

                var accept = new AcceptCar
                {
                    CarID = carId,
                    DevID = dev?.DevID ?? 0,
                    EmployeeID = employeeId,
                    Comment = comment,
                    Odometr = odometer,
                    AcceptDate = acceptDate
                };

                context.AcceptCar.Add(accept);

                // Обновляем пробег в таблице Cars
                if (odometer > car.Mileage)
                {
                    car.Mileage = odometer;
                }

                // Обновляем статус авто
                car.CarStatusID = (car.Mileage % 15000 == 0) ? 3 : 1;

                context.SaveChanges();
            }
        }

        public static void UpdateAccept(int acceptId, int employeeId, string comment, DateTime acceptDate, int odometer)
        {
            using (var context = new ApplicationContext())
            {
                var accept = context.AcceptCar.FirstOrDefault(a => a.AcceptID == acceptId);
                if (accept == null) return;

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

                return context.CarStatus.Where(ch => ch.Name == name).First().ID;
            }
        }


        public static void createСar(string brand, string model, string vin, string licenseplate, string carstatus, int mileage, string photopath)
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
                    car.PhotoPath = photopath;
                };

                // добавляем его в список, сохраняем изменения и берем его айди
                context.Cars.Add(car);
                context.SaveChanges();
                int id = car.CarID;
                context.SaveChanges();
            }
        }
        public static void editCar(int id, string brand, string model, string vin, string licenseplate, string carstatus, int mileage, string photopath)
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
                car.PhotoPath= photopath;

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
        public static string GetReasonNameById(int reasonId)
        {
            using (var context = new ApplicationContext())
            {
                var reason = context.Reason.FirstOrDefault(r => r.ID == reasonId);
                return reason != null ? reason.ReasonName : "Неизвестно";
            }
        }

        public static List<ServiceCarClass> GetRepair()
        {
            using (var context = new ApplicationContext())
            {
                List<ServiceCarClass> repairs = new List<ServiceCarClass>();

                var carsOnService = context.Cars
                    .Where(c => c.CarStatusID == 3)
                    .ToList(); // только машины на ТО

                foreach (var car in carsOnService)
                {
                    ServiceCarClass repairObj = new ServiceCarClass
                    {
                        RepairID = 0, // Условно, реального ремонта нет
                        CarID = car.CarID,
                        LicensePlate = car.LicensePlate,
                        Brand = car.Brand,
                        Model = car.Model,
                        ReasonID = 1, // Плановое ТО
                        ReasonName = "Плановое ТО",
                        Description = "На плановом техническом обслуживании",
                        RepairDate = DateTime.Now
                    };

                    repairs.Add(repairObj);
                }

                return repairs;
            }
        }
        public static void CompleteService(ServiceCarClass selectedCar)
        {
            using (var context = new ApplicationContext())
            {
                var car = context.Cars.FirstOrDefault(c => c.CarID == selectedCar.CarID);

                if (car != null)
                {
                    car.CarStatusID = 1; // Меняем статус на Свободно
                    context.SaveChanges();
                }
            }
        }

        public static List<string> getReasons()
        {
            using (var context = new ApplicationContext())
            {
                return context.Reason.Select(r => r.ReasonName).ToList();
            }
        }

        // Найти ID причины ремонта по её названию
        public static int getReasonIdByName(string name)
        {
            using (var context = new ApplicationContext())
            {
                var reason = context.Reason.FirstOrDefault(r => r.ReasonName == name);
                return reason != null ? reason.ID : 0;
            }
        }
     
       
    }
}