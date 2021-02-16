using Microsoft.AspNet.Identity;
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
    public class AdminCustomerController : Controller
    {
        ICustomersData customerDb;
        IEmployeesData employeeDb;
        IRepairJobsData jobsDb;
        ApplicationUserManager _userManager;

        public AdminCustomerController(IEmployeesData employeeDb, ICustomersData customerDb, IRepairJobsData jobsDb)
        {
            this.customerDb = customerDb;
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
        }

        public AdminCustomerController(IEmployeesData employeeDb, ICustomersData customerDb, IRepairJobsData jobsDb,
            ApplicationUserManager userManager)
        {
            this.customerDb = customerDb;
            this.employeeDb = employeeDb;
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
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

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
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            var ViewModel = new AdminCreateJobViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CustomerId = id
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestJob(AdminCreateJobViewModel model)
        {
            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError(nameof(model.StartDate), "start date must be earlier then end date");
            }
            if (DateTime.Now.Date > model.StartDate)
            {
                ModelState.AddModelError(nameof(model.StartDate), "start date must not be in the past");
            }

            if (ModelState.IsValid)
            {
                jobsDb.Add(new RepairJob()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CustomerId = model.CustomerId,
                    Status = RepairStatus.Pending,
                    JobDescription = model.JobDescription
                });
                return RedirectToAction("Index","AdminHome");
            }

            return View(model);
        }
    }
}