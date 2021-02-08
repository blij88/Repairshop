
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
        IEmployeesData db1;
        ICustomersData db2;
        
        public HomeController(IRepairJobsData db, IEmployeesData db1, ICustomersData db2)
        {
            this.db = db;
            this.db1 = db1;
            this.db2 = db2;
        }

        public ActionResult Index()
        {
            var ViewModel = new HomeIndexViewModel()
            {
                RepairJobs = db.GetAll(),
                RepairStatus = db.StatusAmounts(),
                Customer = db2.GetAll()
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
                CustomerId = id,
                ThisCustomer = db2.Get(id)
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestJob(RepairJob Repair)
        {
            if (Repair.StartDate > Repair.EndDate)
            {
                ModelState.AddModelError(nameof(Repair.StartDate), "start date must be earlier then end date");
            }
            if (Repair.StartDate < DateTime.Now)
            {
                ModelState.AddModelError(nameof(Repair.StartDate), "start date must be in the present");
            }

            if (ModelState.IsValid)
            {
            db.Add(Repair);
            return RedirectToAction("Index");
            }
            return View();
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