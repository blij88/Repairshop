using RepairShop.Data.Models;
using RepairShop.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class CustomerController : Controller
    {
        ICustomersData db1;

        public CustomerController(ICustomersData db1)
        {
            this.db1 = db1;
        }
        // GET: Customer
        public ActionResult Overview()
        {
            var Model = db1.GetAll(); ;
            return View(Model);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            db1.Add(customer);
            return RedirectToAction("overview");
        }
    }
}