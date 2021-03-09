using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class EmployeeIndexViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [DisplayName("Hourly wage")]
        public decimal HourlyCost { get; set; }
        public bool Admin { get; set; }
    }

    public class CreateEmployeeViewModel
    {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }
        [Required]
        [DisplayName("Hourly wage")]
        public decimal HourlyCost { get; set; }
        [Required]
        public bool Admin { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }

    public class EditEmployeeViewModel
    {

        [Required]
        [MaxLength(24)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }
        [Required]
        [DisplayName("Hourly wage")]
        public decimal HourlyCost { get; set; }
        [Required]
        public bool Admin { get; set; }
        [Required]
        public int Id { get; set; }
    }
}