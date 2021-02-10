using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class JobEditViewModel
    {
        public RepairJob Job { get; set; }
        public int EmployeeId { get; set; }
        public int HoursWorked { get; set; }
    }
}