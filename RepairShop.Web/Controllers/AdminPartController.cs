using Microsoft.AspNet.Identity;
using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.ViewModels;
using RepairShop.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class AdminPartController : Controller
    {
        IPartsData partDb;
        IEmployeesData employeeDb;

        public AdminPartController(IPartsData partDb, IEmployeesData employeeDb)
        {
            this.partDb = partDb;
            this.employeeDb = employeeDb;
        }

        // GET: Employee
        public ActionResult Index()
        {
            // Only admins should be able to see this.
            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);
            if (employee == null || !employee.Admin)
                return HttpNotFound();

            var model = partDb.GetAll();
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
        public ActionResult Create(CreatePartViewModel model)
        {
            if (ModelState.IsValid)
            {
                partDb.Add(new Part { Name = model.Name, UnitCost = model.UnitCost });
                return RedirectToAction("Index");
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

            var model = partDb.Get(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Part model)
        {
            if (ModelState.IsValid)
            {
                partDb.Update(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}