﻿using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class AdminHomeIndexViewModel
    {
        public IEnumerable<AdminQueryRepairJob> RepairJobs { get; set; }
        public Dictionary<RepairStatus, int> RepairStatus { get; set; }
        public List<string> HeaderNames { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
    }

    public class AdminQueryRepairJob
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Customer { get; set; }
        public bool IsLate { get; set; }
        public RepairStatus Status {get; set; }
    }

    public class AdminHomeDetailsViewModel
    {
        public RepairJob RepairJob { get; set; }
        public Customer Customer { get; set; }
        public decimal Price { get; set; }
    }

    public class AdminJobEditViewModel
    {
        public RepairJob Job { get; set; }
        public RepairJobEmployee JobEmployee { get; set; }
    }

    public class AdminCreateJobViewModel
    {
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("End date")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public RepairStatus Status { get; set; }

        [DisplayName("Description")]
        public string JobDescription { get; set; }

        public int CustomerId { get; set; }
    }
}