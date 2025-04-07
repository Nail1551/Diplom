using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels;
using Diplom.Utility;
using Diplom.Data;

namespace Diplom.Models
{
    public class CarDevModel
    {
        private List<DevClass> DevCar;
        public List<DevClass> getDev()
        {
            return DevCar;
        }
        public CarDevModel() 
        {
            DevCar = DbManager.getDev();
        }
        public bool checkSelectedItem(DevClass dev)
        {
            return dev != null;
        }
    }
}
