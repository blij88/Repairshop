using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class CustomerIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
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

    public class RegisterCustomerViewModel
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

            public IEnumerable<string> Errors { get; set; }
        }
    }
}