using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class RepairJob
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public RepairStatus Status { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public string Description { get; set; }

        // Dictionary with keys corresponding to PartId and values corresponding to amount of that part.
        public Dictionary<int, int> RequiredParts { get; set; }

    }
}
