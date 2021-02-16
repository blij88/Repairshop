using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class RepairJobPart
    {
        public int Id { get; set; }
        public int RepairJobId { get; set; }
        public int PartId { get; set; }

        // Who requested this part?
        public int EmployeeId { get; set; }
        public int NumberUsed { get; set; }
    }
}
