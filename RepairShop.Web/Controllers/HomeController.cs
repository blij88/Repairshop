
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
    public class HomeController : Controller
    {
        IRepairJobsData jobsDb;
        IEmployeesData employeeDb;
        ICustomersData customerDb;
        IRepairJobsEmployeesData jobEmployeeDb;
        ApplicationUserManager _userManager;

        public HomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
    IRepairJobsEmployeesData jobEmployeeDb)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
            this.jobEmployeeDb = jobEmployeeDb;
        }

        public HomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
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
            var ViewModel = new HomeIndexViewModel()
            {
                RepairJobs = jobsDb.GetAll().Join(customerDb.GetAll(), r => r.CustomerId, c => c.Id, (r, c) => new {r, c}).
                    Join(UserManager.Users, rc => rc.c.UserId, u => u.Id,
                    (rc, u) => new QueryRepairJob
                    {
                        Id = rc.r.Id,
                        StartDate = rc.r.StartDate,
                        EndDate = rc.r.EndDate,
                        Customer = u.UserName,
                        IsLate = rc.r.IsLate,
                        Status = rc.r.Status
                    }),
                RepairStatus = jobsDb.StatusAmounts(),
            };
            
            return View(ViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

            var model = new JobEditViewModel
            {
               Job = job,
               JobEmployee = jobEmployee
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobEditViewModel model)
        {
            jobsDb.Update(model.Job);
            jobEmployeeDb.Update(model.JobEmployee);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var job = jobsDb.Get(id);
            var viewModel = new HomeDetailsViewModel()
            {
                RepairJob = job,
                Customer = customerDb.Get(job.CustomerId),
                Price = jobsDb.GetPrice(id)
            };
            return View(viewModel);
        }
    }
}