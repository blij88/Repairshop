using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class RepairJobEmployee
    {
        public int Id { get; set; }
        public int RepairJobId { get; set; }
        public int EmployeeId { get; set; }
        public int HoursWorked { get; set; }
    }
}
