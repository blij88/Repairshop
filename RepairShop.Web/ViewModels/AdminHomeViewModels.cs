using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<QueryRepairJob> RepairJobs { get; set; }
        public Dictionary<RepairStatus, int> RepairStatus { get; set; }
        public List<string> HeaderNames { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
    }

    public class QueryRepairJob
    {
        public int Id { get; set; }
        [DisplayName ("Start date")]
        public DateTime StartDate { get; set; }
        [DisplayName ("End date")]
        public DateTime EndDate { get; set; }
        public string Customer { get; set; }
        public bool IsLate { get; set; }
        public RepairStatus Status {get; set; }
    }
}