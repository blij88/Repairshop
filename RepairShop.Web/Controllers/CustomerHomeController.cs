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

namespace RepairShop.Web.Controllers
{
    public class CustomerHomeController : Controller
    {
        IRepairJobsData jobsDb;
        ICustomersData customerDb;
        ApplicationUserManager _userManager;

        public CustomerHomeController(IRepairJobsData jobsDb, ICustomersData customerDb)
        {
            this.jobsDb = jobsDb;
            this.customerDb = customerDb;
        }

        public CustomerHomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb,
            ApplicationUserManager userManager)
        {
            this.jobsDb = jobsDb;
            this.customerDb = customerDb;
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
            var userId = User.Identity.GetUserId();
            var customer = customerDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (customer == null)
                return RedirectToAction("Error");

            var repairJobs = jobsDb.GetAll().Where(r => r.CustomerId == customer.Id).
                Select(r => new CustomerQueryRepairJob()
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status,
                    HasStarted = r.Status != RepairStatus.Pending
                });

            return View(repairJobs);
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            // This page should be inaccesible when the job has already started.
            var job = jobsDb.Get(id);
            if (job == null || job.Status != RepairStatus.Pending)
                return HttpNotFound();

            // Make sure only the original customer can access this page.
            var userId = User.Identity.GetUserId();
            var customer = customerDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (customer == null || customer.Id != job.CustomerId)
                return HttpNotFound();

            var model = new CustomerHomeEditViewModel
            {
                Id = job.Id,
                StartDate = job.StartDate,
                EndDate = job.EndDate,
                CustomerId = job.CustomerId,
                Status = RepairStatus.Pending,
                JobDescription = job.JobDescription
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerHomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                jobsDb.Update(new RepairJob()
                {
                    Id = model.Id,
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            var job = jobsDb.Get(id);
            if (job == null)
                return HttpNotFound();

            // Make sure we only show this to the customer that requested the job.
            var userId = User.Identity.GetUserId();
            var customer = customerDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (customer == null || customer.Id != job.CustomerId)
                return HttpNotFound();

            // Show only the data that the customer needs to see.
            var viewModel = new CustomerHomeDetailsViewModel()
            {
                Id = job.Id,
                StartDate = job.StartDate,
                EndDate = job.EndDate,
                Status = job.Status,
                JobDescription = job.JobDescription,
                RepairNotes = job.RepairNotes,
                HasStarted = job.Status != RepairStatus.Pending,
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

            var ViewModel = new CustomerCreateJobViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CustomerId = customer.Id
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCreateJobViewModel model)
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