using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Utility
{
    public class ServiceCarClass
    {
        public int RepairID { get; set; }
        public int CarID { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReasonID { get; set; }
        public string ReasonName { get; set; }
        public string Description { get; set; }
        public System.DateTime RepairDate { get; set; }
    }
}
