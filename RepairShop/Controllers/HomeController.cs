
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
        
        public HomeController(IRepairJobsData db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            var ViewModel = new HomeIndexViewModel()
            {
                RepairJobs = db.GetAll(),
                RepairStatus = db.StatusAmounts(),
            };
            
            return View(ViewModel);
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
        public  ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RepairJob Repair)
        {
            db.Add(Repair);
            return View();
        }
    }
}