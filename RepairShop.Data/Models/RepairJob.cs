using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class RepairJob
    {
        public int Id { get; set; }
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("End date")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public RepairStatus Status { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DisplayName("Type")]
        public DeviceType DeviceType { get; set; }

        // This should be filled in by the customer.
        [DisplayName("Description")]
        public string JobDescription { get; set; }

        // This should be filled in by the employees to describe the work done.
        [DisplayName("Repair notes")]
        public string RepairNotes { get; set; }

        // Dictionary with keys corresponding to PartId and values corresponding to amount of that part.
        public Dictionary<int, int> RequiredParts = new Dictionary<int, int>();

        public bool IsLate
        {
            get
            {
                var today = DateTime.Today;
                return (StartDate < today && Status == RepairStatus.Pending) ||
                    (EndDate < today && Status != RepairStatus.Done);
            }
        }

    }
}
