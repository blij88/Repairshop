using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class CreateEmployeeViewModel
    {
        public Employee Employee { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}