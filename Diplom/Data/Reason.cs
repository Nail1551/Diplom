using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data
{
    [Table("Reason")]
    public class Reason
    {
        public int ID { get; set; }      
        public string ReasonName { get; set; }
    }
}
