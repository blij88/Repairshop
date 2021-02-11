using Microsoft.AspNet.Identity.Owin;
using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.Web;
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
        ApplicationUserManager _userManager;

        public CustomerController(ICustomersData customerDb, IRepairJobsData jobsDb)
        {
            this.customerDb = customerDb;
            this.jobsDb = jobsDb;
        }

        public CustomerController(ICustomersData customerDb, IRepairJobsData jobsDb, ApplicationUserManager userManager)
        {
            this.customerDb = customerDb;
            this.jobsDb = jobsDb;
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Customer
        public ActionResult Index()
        {
            var model = customerDb.GetAll().Join(UserManager.Users, c => c.UserId, u => u.Id,
                (c, u) => new CustomerIndexViewModel
                {
                    Id = c.Id,
                    Name = u.UserName,
                    Phone = u.PhoneNumber,
                    Email = u.Email
                });
            return View(model);
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