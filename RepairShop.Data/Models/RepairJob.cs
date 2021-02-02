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
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public RepairStatus Status { get; set; }

        // Dictionary with keys corresponding to EmployeeID and values corresponding to number of hours worked on this job by that employee.
        public Dictionary<int, int> HoursWorkedByEmployee { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DeviceType DeviceType { get; set; }

        // This should be filled in by the customer.
        public string JobDescription { get; set; }

        // This should be filled in by the employees to describe the work done.
        public string RepairNotes { get; set; }

        // Dictionary with keys corresponding to PartId and values corresponding to amount of that part.
        public Dictionary<int, int> RequiredParts { get; set; }

    }
}
