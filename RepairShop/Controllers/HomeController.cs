
using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class HomeController : Controller
    {
        IRepairJobsData db;
        IEmployeesData EmployeeDB;
        ICustomersData CustomerDB;
        
        public HomeController(IRepairJobsData db, IEmployeesData EmployeeDB, ICustomersData CustomerDB)
        {
            this.db = db;
            this.EmployeeDB = EmployeeDB;
            this.CustomerDB = CustomerDB;
        }

        public ActionResult Index()
        {
            var ViewModel = new HomeIndexViewModel()
            {
                RepairJobs = db.GetAll(),
                RepairStatus = db.StatusAmounts(),
                Customer = CustomerDB.GetAll()
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

        [HttpGet]
        public  ActionResult RequestJob(int id)
        {
            var ViewModel = new HomeCreateViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ThisCustomer = CustomerDB.Get(id)
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestJob(RepairJob Repair, int id)
        {
            if (Repair.StartDate > Repair.EndDate)
            {
                ModelState.AddModelError(nameof(Repair.StartDate), "start date must be earlier then end date");
            }
            if (DateTime.Now.Date > Repair.StartDate)
            {
                ModelState.AddModelError(nameof(Repair.StartDate), "start date must be in the present");
            }

            if (ModelState.IsValid)
            {
            db.Add(Repair);
            return RedirectToAction("Index");
            }
            
            var ViewModel = new HomeCreateViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ThisCustomer = CustomerDB.Get(id)
            };
            return View(ViewModel);
        }

        public ActionResult Delete(int id)
        {
            db.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = db.Get(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RepairJob Repair)
        {
            db.Update(Repair);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = db.Get(id);
            return View(model);
        }

        public ActionResult GetPrice(int id)
        {
            var Model = db.GetPrice(id);
            return View(Model);
        }
    }
}