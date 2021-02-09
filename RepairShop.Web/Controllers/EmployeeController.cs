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
    public class EmployeeController : Controller
    {
        IEmployeesData employeeDb;
        private ApplicationUserManager _userManager;

        public EmployeeController(IEmployeesData employeeDb)
        {
            this.employeeDb = employeeDb;
        }

        public EmployeeController(IEmployeesData employeeDb, ApplicationUserManager userManager)
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
            var Model = employeeDb.GetAll() ;
            return View(Model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateEmployeeViewModel view)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = view.Employee.Name, Email = view.Employee.Email };
                var result = await UserManager.CreateAsync(user, "1234Aa=");
                if (result.Succeeded)
                {
                    employeeDb.Add(view.Employee);
                    return RedirectToAction("Index");
                }

                view.Errors = result.Errors;
            }

            return View(view);
        }
    }
}