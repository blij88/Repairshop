using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class CustomerController : Controller
    {
        ICustomersData customerDb;
        IRepairJobsData jobsDb;

        public CustomerController(ICustomersData customerDb, IRepairJobsData jobsDb)
        {
            this.customerDb = customerDb;
            this.jobsDb = jobsDb;
        }
        // GET: Customer
        public ActionResult Index()
        {
            var Model = customerDb.GetAll(); ;
            return View(Model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            customerDb.Add(customer);
            return RedirectToAction("overview");
        }

        [HttpGet]
        public ActionResult RequestJob(int id)
        {
            var ViewModel = new CreateJobViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ThisCustomer = customerDb.Get(id)
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestJob(RepairJob repair, int id)
        {
            if (repair.StartDate > repair.EndDate)
            {
                ModelState.AddModelError(nameof(repair.StartDate), "start date must be earlier then end date");
            }
            if (DateTime.Now.Date > repair.StartDate)
            {
                ModelState.AddModelError(nameof(repair.StartDate), "start date must not be in the past");
            }

            repair.CustomerId = id;

            if (ModelState.IsValid)
            {
                jobsDb.Add(repair);
                return RedirectToAction("Index");
            }

            var ViewModel = new CreateJobViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ThisCustomer = customerDb.Get(id)
            };
            return View(ViewModel);
        }
    }
}