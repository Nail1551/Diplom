using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;
using Diplom.Data;

namespace Diplom.Models
{
    public class AcceptModel
    {
        private List<AcceptCarClass> AcceptCar;
        public List<AcceptCarClass> getAccept()
        {
            return AcceptCar;
        }
        public AcceptModel()
        {
            AcceptCar = DbManager.getAccept();
        }
        public bool checkSelectedItem(AcceptCarClass accept)
        {
            return accept != null;
        }
    }
}
