﻿using Microsoft.AspNet.Identity.Owin;
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
        IRepairJobsData jobsDb;
        ApplicationUserManager _userManager;

        public AdminCustomerController(ICustomersData customerDb, IRepairJobsData jobsDb)
        {
            this.customerDb = customerDb;
            this.jobsDb = jobsDb;
        }

        public AdminCustomerController(ICustomersData customerDb, IRepairJobsData jobsDb, ApplicationUserManager userManager)
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
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}