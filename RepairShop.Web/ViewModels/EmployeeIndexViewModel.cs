using System;
using System.Collections.Generic;
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
    }
}