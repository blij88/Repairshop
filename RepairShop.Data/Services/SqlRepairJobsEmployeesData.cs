using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlRepairJobsEmployeesData : IRepairJobsEmployeesData
    {
        private readonly RepairShopDbContext db;

        public SqlRepairJobsEmployeesData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(RepairJobEmployee entry)
        {
            db.RepairJobsEmployees.Add(entry);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.RepairJobsEmployees.Find(id);
            db.RepairJobsEmployees.Remove(entry);
            db.SaveChanges();
        }

        public RepairJobEmployee Get(int repairId, int employeeId)
        {
            return db.RepairJobsEmployees.FirstOrDefault(e => e.RepairJobId == repairId && e.EmployeeId == employeeId);
        }

        public IEnumerable<RepairJobEmployee> GetAll()
        {
            return db.RepairJobsEmployees;
        }

        public void Update(RepairJobEmployee repairJobEmployee)
        {
            var entry = db.Entry(repairJobEmployee);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
