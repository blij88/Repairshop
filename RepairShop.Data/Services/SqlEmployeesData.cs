using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly RepairShopDbContext db;

        public SqlEmployeesData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(Employee employee)
        {
            db.Employees.Add(employee);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.Employees.Find(id);
            db.Employees.Remove(entry);
            db.SaveChanges();
        }

        public Employee Get(int id)
        {
            return db.Employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return db.Employees;
        }

        public void Update(Employee employee)
        {
            var entry = db.Entry(employee);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
