
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
    public class EmployeeHomeController : Controller
    {
        IRepairJobsData jobsDb;
        IEmployeesData employeeDb;
        ICustomersData customerDb;
        IPartsData partDb;
        IRepairJobsEmployeesData jobEmployeeDb;
        IRepairJobsPartsData jobPartDb;
        ApplicationUserManager _userManager;

        public EmployeeHomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
            IPartsData partDb, IRepairJobsEmployeesData jobEmployeeDb, IRepairJobsPartsData jobPartDb)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
            this.partDb = partDb;
            this.jobEmployeeDb = jobEmployeeDb;
            this.jobPartDb = jobPartDb;
        }

        public EmployeeHomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
            IPartsData partDb, IRepairJobsEmployeesData jobEmployeeDb, IRepairJobsPartsData jobPartDb,
            ApplicationUserManager userManager)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
            this.partDb = partDb;
            this.jobEmployeeDb = jobEmployeeDb;
            this.jobPartDb = jobPartDb;
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
            // This page should only be accessible to employees.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null)
                return HttpNotFound();

            // Show only unfinished jobs to repair employees.
            var repairJobs = jobsDb.GetAll().
                Where(r => r.Status != RepairStatus.Done).
                Join(customerDb.GetAll(), r => r.CustomerId, c => c.Id, (r, c) => new { r, c }).
                Join(UserManager.Users, rc => rc.c.UserId, u => u.Id,
                (rc, u) => new EmployeeQueryRepairJob
                {
                    Id = rc.r.Id,
                    StartDate = rc.r.StartDate,
                    EndDate = rc.r.EndDate,
                    Customer = u.UserName,
                    IsLate = rc.r.IsLate,
                    Status = rc.r.Status
                });

            var ViewModel = new EmployeeHomeIndexViewModel()
            {
                RepairJobs = repairJobs,
                RepairStatus = jobsDb.StatusAmounts(),
            };

            return View(ViewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Check if this is a valid request.
            var job = jobsDb.Get(id);
            if (job == null)
                return HttpNotFound();

            // This page should only be accessible to employees.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null)
                return HttpNotFound();

            // Get data for number of hours worked by this employee.
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

            // Get data for parts used for this job.
            var jobParts = jobPartDb.GetAll().Where(jp => jp.RepairJobId == id).
                Join(partDb.GetAll(), jp => jp.PartId, p => p.Id,
                (jp, p) => new EmployeeQueryPart
                {
                    Id = jp.Id,
                    PartId = p.Id,
                    Name = p.Name,
                    Amount = jp.NumberUsed,
                    InStock = jp.NumberUsed <= p.AmountInStore
                }).ToArray();

            var model = new EmployeeJobEditViewModel
            {
                Job = job,
                JobEmployee = jobEmployee,
                Parts = jobParts
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeJobEditViewModel model)
        {
            jobsDb.Update(model.Job);
            jobEmployeeDb.Update(model.JobEmployee);
            
            foreach (var part in model.Parts)
            {
                if (part.Amount == 0)
                    jobPartDb.Delete(part.Id);
                else
                {
                    jobPartDb.Update(new RepairJobPart()
                    {
                        Id = part.Id,
                        PartId = part.PartId,
                        RepairJobId = model.Job.Id,
                        EmployeeId = model.JobEmployee.EmployeeId,
                        NumberUsed = part.Amount
                    });
                }
            }
            if (Request.Form["update"] != null)
                return RedirectToAction("Edit", model.Job.Id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            // This page should only be accessible to employees.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null)
                return HttpNotFound();

            var job = jobsDb.Get(id);
            var viewModel = new EmployeeHomeDetailsViewModel()
            {
                RepairJob = job,
                Customer = customerDb.Get(job.CustomerId),
                Price = jobsDb.GetPrice(id)
            };
            return View(viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult AddPartToJob(int id)
        {
            // This page should only be accessible to employees.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null)
                return HttpNotFound();

            var model = new EmployeeAddPartViewModel()
            {
                EmployeeId = employee.Id,
                JobId = id,
                AllParts = partDb.GetAll().
                    Select(p => new EmployeeQueryPart()
                    {
                        PartId = p.Id,
                        Name = p.Name
                    }),
                Amount = 1
            };

           return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPartToJob(EmployeeAddPartViewModel model)
        {
            if (ModelState.IsValid)
            {
                jobPartDb.Add(new RepairJobPart()
                {
                    RepairJobId = model.JobId,
                    PartId = model.PartId,
                    EmployeeId = model.EmployeeId,
                    NumberUsed = model.Amount
                });
                return RedirectToAction("Edit",  new { id = model.JobId });
            }

            return PartialView(model);
        }
    }
}