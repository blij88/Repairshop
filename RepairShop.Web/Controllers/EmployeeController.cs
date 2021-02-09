using RepairShop.Data.Models;
using RepairShop.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeesData db1;

        public EmployeeController(IEmployeesData db1)
        {
            this.db1 = db1;
        }
        // GET: Employee
        public ActionResult Overview()
        {
            var Model = db1.GetAll() ;
            return View(Model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee Employee)
        {
            db1.Add(Employee);
            return RedirectToAction("Overview");
        }
    }
}