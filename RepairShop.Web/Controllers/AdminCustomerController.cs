using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.Web;
using RepairShop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            {
                System.Diagnostics.Debug.WriteLine(employee.UserId);
                return HttpNotFound();

            }

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

        public ActionResult Register()
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Phone};
                var result = await UserManager.CreateAsync(user, "1234Aa=");
                if (result.Succeeded)
                {
                    customerDb.Add(new Customer { UserId = user.Id, });
                    return RedirectToAction("Index");
                }

                model.Errors = result.Errors;
            }

            return View(model);
        }

        public ActionResult edit(int id)
        {
            var c = customerDb.Get(id);
            var u = UserManager.FindById(c.UserId);
            var model = new EditCustomerViewModel
            {
                Id = c.Id,
                Email = u.Email,
                UserName = u.UserName,
                Phone = u.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(EditCustomerViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var C = customerDb.Get(Model.Id);
                var User = UserManager.FindById(C.UserId);
                User.Email = Model.Email;
                User.UserName = Model.UserName;
                User.PhoneNumber = Model.Phone;
                UserManager.Update(User);
                return RedirectToAction("Index");
            }

            return View(Model);
        }
    }
}