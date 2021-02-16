using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public decimal HourlyCost { get; set; }
        [Required]
        public bool Admin { get; set; }
        
        public IEnumerable<string> Errors { get; set; }
    }
}