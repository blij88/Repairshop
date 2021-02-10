
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RepairShop.Data.Models;
using RepairShop.Data.Services;
using RepairShop.Web;
using RepairShop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShop.Controllers
{
    public class HomeController : Controller
    {
        IRepairJobsData jobsDb;
        IEmployeesData employeeDb;
        ICustomersData customerDb;

        public HomeController(IRepairJobsData jobsDb, IEmployeesData employeeDb, ICustomersData customerDb)
        {
            this.jobsDb = jobsDb;
            this.employeeDb = employeeDb;
            this.customerDb = customerDb;
        }

        public ActionResult Index()
        {
            var ViewModel = new HomeIndexViewModel()
            {
                RepairJobs = jobsDb.GetAll().Join(customerDb.GetAll(),
                    r => r.CustomerId, c => c.Id,
                    (r, c) => new QueryRepairJob
                    {
                        Id = r.Id,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate,
                        Customer = c.Name,
                        IsLate = r.IsLate,
                        Status = r.Status
                    }),
                RepairStatus = jobsDb.StatusAmounts(),
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

        public ActionResult Delete(int id)
        {
            jobsDb.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var job = jobsDb.Get(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            var employee = employeeDb.GetAll().FirstOrDefault(e => e.UserId == userId);

            // TODO: Redirect to appropriate message.
            if (employee == null)
                return RedirectToAction("Index");
            
            if (!job.HoursWorkedByEmployee.ContainsKey(employee.Id))
                job.HoursWorkedByEmployee[employee.Id] = 0;

            var model = new JobEditViewModel
            {
               Job = job,
               EmployeeId = employee.Id,
               HoursWorked = job.HoursWorkedByEmployee[employee.Id],
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobEditViewModel model)
        {
            model.Job.HoursWorkedByEmployee[model.EmployeeId] = model.HoursWorked;
            jobsDb.Update(model.Job);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var job = jobsDb.Get(id);
            var viewModel = new HomeDetailsViewModel()
            {
                RepairJob = job,
                Customer = customerDb.Get(job.CustomerId),
                Price = jobsDb.GetPrice(id)
            };
            return View(viewModel);
        }
    }
}