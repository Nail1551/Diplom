using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.Data;
using Diplom.Utility;

namespace Diplom.Models
{
    public class AddEditAcceptModel
    {
        public void createAcc(int carid,int Employeeid,string comment,DateTime acceptdate,int odometr)
        {
            DbManager.AddAccept(carid, Employeeid, comment, acceptdate, odometr);
        }

        public void editAcc(int acceptId, int employeeId, string comment, DateTime acceptDate, int odometer)
        {
            DbManager.UpdateAccept(acceptId, employeeId, comment, acceptDate, odometer);
        }
    }
}
