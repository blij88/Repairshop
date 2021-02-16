using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
    public class AdminEmployeeController : Controller
    {
        IEmployeesData employeeDb;
        private ApplicationUserManager _userManager;

        public AdminEmployeeController(IEmployeesData employeeDb)
        {
            this.employeeDb = employeeDb;
        }

        public AdminEmployeeController(IEmployeesData employeeDb, ApplicationUserManager userManager)
        {
            this.employeeDb = employeeDb;
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

        // GET: Employee
        public ActionResult Index()
        {

            var model = employeeDb.GetAll().Join(UserManager.Users,
               e => e.UserId,
               u => u.Id,
               (e, u) => new EmployeeIndexViewModel
               {
                   Name = u.UserName,
                   Email = u.Email,
                   Phone = u.PhoneNumber,
                   HourlyCost = e.HourlyCost,
                   Admin = e.Admin
               });

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await UserManager.CreateAsync(user, "1234Aa=");
                if (result.Succeeded)
                {
                    employeeDb.Add(new Employee { HourlyCost = model.HourlyCost, UserId = user.Id, Admin = model.Admin});
                    return RedirectToAction("Index");
                }

                model.Errors = result.Errors;
            }

            return View(model);
        }
    }
}