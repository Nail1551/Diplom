﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void createCar(string brand, string model, string vin, string licenseplate, string carstatus, int mileage)
        {
            DbManager.createСar(brand, model,  vin,  licenseplate,  carstatus,  mileage);
        }

        public void editCar(int id, string brand, string model, string vin, string licenseplate, string carstatus, int mileage)
        {
            DbManager.editCar(id,brand,model,vin,licenseplate,carstatus,mileage);
        }

        public CarAddEditModel()
        {
            
        }

    }
}
