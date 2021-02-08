using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepairShop.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<RepairJob> RepairJobs { get; set; }
        public Dictionary<RepairStatus, int> RepairStatus { get; set; }
        public List<string> HeaderNames { get; set; }
        public Employee Employee { get; set; }
    }
}