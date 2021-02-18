using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RepairShop.Web.ViewModels
{
    public class EmployeeHomeIndexViewModel
    {
        public IEnumerable<EmployeeQueryRepairJob> RepairJobs { get; set; }
        public Dictionary<RepairStatus, int> RepairStatus { get; set; }
        public List<string> HeaderNames { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
    }

    public class EmployeeQueryRepairJob
    {
        public int Id { get; set; }
        [DisplayName ("Start date")]
        public DateTime StartDate { get; set; }
        [DisplayName ("End date")]
        public DateTime EndDate { get; set; }
        public string Customer { get; set; }
        public bool IsLate { get; set; }
        public RepairStatus Status { get; set; }
    }

    public class EmployeeJobEditViewModel
    {
        public RepairJob Job { get; set; }
        public RepairJobEmployee JobEmployee { get; set; }
        public IEnumerable<EmployeeQueryPart> Parts { get; set; }
    }

    public class EmployeeQueryPart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool InStock { get; set; }
    }

    public class EmployeeHomeDetailsViewModel
    {
        public RepairJob RepairJob { get; set; }
        public Customer Customer { get; set; }
        public decimal Price { get; set; }
    }

    public class EmployeeAddPartViewModel
    {
        public int PartId { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public int Amount { get; set; }
        public IEnumerable<EmployeeQueryPart> AllParts { get; set; }
    }
}