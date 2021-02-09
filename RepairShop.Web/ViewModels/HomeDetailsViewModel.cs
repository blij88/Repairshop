using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class HomeDetailsViewModel
    {
        public RepairJob RepairJob { get; set; }
        public Customer Customer { get; set; }
        public decimal Price { get; set; }
    }
}