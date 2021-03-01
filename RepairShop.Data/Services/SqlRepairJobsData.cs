using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlRepairJobsData : IRepairJobsData
    {
        private readonly RepairShopDbContext db;

        public SqlRepairJobsData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(RepairJob repairJob)
        {
            db.RepairJobs.Add(repairJob);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.RepairJobs.Find(id);
            db.RepairJobs.Remove(entry);
            db.SaveChanges();
        }

        public RepairJob Get(int id)
        {
            return db.RepairJobs.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<RepairJob> GetAll()
        {
            return db.RepairJobs.OrderBy(r => r.EndDate);
        }

        public void Update(RepairJob repairJob)
        {
            var entry = db.Entry(repairJob);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public int AmountWithStatus(RepairStatus status)
        {
            return db.RepairJobs.Count(r => r.Status == status);
        }

        public Dictionary<RepairStatus, int> StatusAmounts()
        {
            var statusDict = new Dictionary<RepairStatus, int>();
            foreach (RepairStatus status in Enum.GetValues(typeof(RepairStatus)))
            {
                statusDict[status] = AmountWithStatus(status);
            }

            return statusDict;
        }

        public decimal GetPrice(int id)
        {
            var job = Get(id);

            decimal wageCost = 0M;
            foreach (var rje in db.RepairJobsEmployees.Where(r => r.RepairJobId == id).ToArray())
            {
                var employee = db.Employees.Find(rje.EmployeeId);
                wageCost += employee.HourlyCost * rje.HoursWorked;
            }

            decimal materialCost = 0M;
            foreach (var rjp in db.RepairJobsParts.Where(r => r.RepairJobId == id).ToArray())
            {
                var part = db.Parts.Find(rjp.PartId);
                materialCost += part.UnitCost * rjp.NumberUsed;
            }

            return wageCost + materialCost;
        }

    }
}
