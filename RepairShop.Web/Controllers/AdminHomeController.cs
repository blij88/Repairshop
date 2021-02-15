
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
            jobsDb.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var job = jobsDb.Get(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);

            // TODO: Redirect to appropriate message.
            if (employee == null)
                return RedirectToAction("Index");

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
            var job = jobsDb.Get(id);
            var viewModel = new AdminHomeDetailsViewModel()
            {
                RepairJob = job,
                Customer = customerDb.Get(job.CustomerId),
                Price = jobsDb.GetPrice(id)
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var customer = customerDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (customer == null)
                return RedirectToAction("Index");

            var ViewModel = new AdminCreateJobViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CustomerId = customer.Id
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminCreateJobViewModel model)
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
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}