using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Overview()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}