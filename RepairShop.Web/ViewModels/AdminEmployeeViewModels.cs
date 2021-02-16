﻿using System;
using System.Collections.Generic;
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
        public decimal HourlyCost { get; set; }
        public bool Admin { get; set; }
    }

    public class CreateEmployeeViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public decimal HourlyCost { get; set; }
        [Required]
        public bool Admin { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}