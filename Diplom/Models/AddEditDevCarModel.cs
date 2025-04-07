using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Utility;

namespace Diplom.Models
{
    public class AddEditDevCarModel
    {
        public void createDev(int carid, int toemployeeid, string condition,  DateTime transferdate)
        {
            DbManager.AddDev(carid,toemployeeid,condition,transferdate);
        }

        public void editDev(int id, int carid, int toemployeeid, string condition, DateTime transferdate)
        {
            DbManager.editDev(id,carid,toemployeeid,condition,transferdate);
        }
    }
}
