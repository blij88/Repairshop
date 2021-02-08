using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult WorkOrders()
        {
            return View();
        }
    }
}