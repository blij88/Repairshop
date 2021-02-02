using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public decimal UnitCost { get; set; }
        public int AmountInStore { get; set; }
        public int AmountInBackOrder { get; set; }
    }
}
