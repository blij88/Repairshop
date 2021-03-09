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
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            var model = employeeDb.GetAll().Join(UserManager.Users,
               e => e.UserId,
               u => u.Id,
               (e, u) => new EmployeeIndexViewModel
               {
                   Name = u.UserName,
                   Email = u.Email,
                   Phone = u.PhoneNumber,
                   HourlyCost = e.HourlyCost,
                   Admin = e.Admin,
                   Id = e.Id
               });

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
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
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email , PhoneNumber = model.Phone};
                var result = await UserManager.CreateAsync(user, "1234Aa=");
                if (result.Succeeded)
                {
                    employeeDb.Add(new Employee { HourlyCost = model.HourlyCost, UserId = user.Id, Admin = model.Admin,});
                    return RedirectToAction("Index");
                }

                model.Errors = result.Errors;
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();
            
            
            var Employee = employeeDb.Get(id);
            var EmployeeUser = UserManager.FindById(Employee.UserId);
            var model = new EditEmployeeViewModel
                {
                Email = EmployeeUser.Email,
                Phone = EmployeeUser.PhoneNumber,
                Admin = Employee.Admin,
                HourlyCost = Employee.HourlyCost,
                Id = Employee.Id,
                Name = EmployeeUser.UserName
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditEmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {
                var E = employeeDb.Get(model.Id);
                E.Admin = model.Admin;
                E.HourlyCost = model.HourlyCost;
                employeeDb.Update(E);
                var User = UserManager.FindById(E.UserId);
                User.PhoneNumber = model.Phone;
                User.Email = model.Email;
                User.UserName = model.Name;
                UserManager.Update(User);
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}