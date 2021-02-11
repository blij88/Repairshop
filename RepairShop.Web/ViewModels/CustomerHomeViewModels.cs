using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class CustomerQueryRepairJob
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HasStarted { get; set; }
        public RepairStatus Status { get; set; }
    }

    public class CustomerHomeDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RepairStatus Status { get; set; }
        public bool HasStarted { get; set; }
        public string JobDescription { get; set; }
        public string RepairNotes { get; set; }
        public decimal Price { get; set; }
    }

    public class CustomerHomeEditViewModel
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

        [DisplayName("Description")]
        public string JobDescription { get; set; }

        public int CustomerId { get; set; }
    }
}