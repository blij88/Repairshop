
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
    public class AdminHomeController : Controller
    {
        IRepairJobsData jobsDb;
        IEmployeesData employeeDb;
        ICustomersData customerDb;
        IRepairJobsEmployeesData jobEmployeeDb;
        ApplicationUserManager _userManager;

        public AdminHomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
    IRepairJobsEmployeesData jobEmployeeDb)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
            this.jobEmployeeDb = jobEmployeeDb;
        }

        public AdminHomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
            IRepairJobsEmployeesData jobEmployeeDb, ApplicationUserManager userManager)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
            this.jobEmployeeDb = jobEmployeeDb;
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

        public ActionResult Index()
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            var repairJobs = jobsDb.GetAll().
                Join(customerDb.GetAll(), r => r.CustomerId, c => c.Id, (r, c) => new { r, c }).
                Join(UserManager.Users, rc => rc.c.UserId, u => u.Id,
                (rc, u) => new AdminQueryRepairJob
                {
                    Id = rc.r.Id,
                    StartDate = rc.r.StartDate,
                    EndDate = rc.r.EndDate,
                    Customer = u.UserName,
                    IsLate = rc.r.IsLate,
                    Status = rc.r.Status
                });

            var ViewModel = new AdminHomeIndexViewModel()
            {
                RepairJobs = repairJobs,
                RepairStatus = jobsDb.StatusAmounts(),
            };
            
            return View(ViewModel);
        }

        public ActionResult Delete(int id)
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            jobsDb.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Check if the request is valid.
            var job = jobsDb.Get(id);
            if (job == null)
                return HttpNotFound();

            // Only admins should see this view.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            var jobEmployee = jobEmployeeDb.Get(id, employee.Id);
            if (jobEmployee == null)
            {
                jobEmployee = new RepairJobEmployee
                {
                    RepairJobId = id,
                    EmployeeId = employee.Id,
                    HoursWorked = 0
                };
                jobEmployeeDb.Add(jobEmployee);
            }

            var model = new AdminJobEditViewModel
            {
               Job = job,
               JobEmployee = jobEmployee
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminJobEditViewModel model)
        {
            jobsDb.Update(model.Job);
            jobEmployeeDb.Update(model.JobEmployee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            // Check if the request is valid.
            var job = jobsDb.Get(id);
            if (job == null)
                return HttpNotFound();

            var viewModel = new AdminHomeDetailsViewModel()
            {
                RepairJob = job,
                Customer = UserManager.FindById(customerDb.Get(job.CustomerId).UserId),
                Price = jobsDb.GetPrice(id)
            };
            return View(viewModel);
        }
    }
}