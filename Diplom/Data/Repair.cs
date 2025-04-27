using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Data
{
    [Table("Repair")]
    public class Repair
    {
        
        public int RepairID { get; set; }  
        public int CarID { get; set; }      
        public int ReasonID { get; set; }   
        public string Description { get; set; } 
        public DateTime RepairDate { get; set; } 
    }
}
