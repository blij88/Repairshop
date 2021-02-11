using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class CustomerIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}