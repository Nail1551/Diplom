using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;

namespace Diplom.Models
{
    public class ServiceModel
    {
        private List<ServiceCarClass> Services;

        public ServiceModel()
        {
            Services = DbManager.GetRepair();
        }

        public List<ServiceCarClass> getRepair()
        {
            return Services;
        }

        public bool checkSelectedItem(ServiceCarClass service)
        {
            return service != null;
        }
       
    }
}
