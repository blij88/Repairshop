using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlCustomersData : ICustomersData
    {
        private readonly RepairShopDbContext db;

        public SqlCustomersData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.Customers.Find(id);
            db.Customers.Remove(entry);
            db.SaveChanges();
        }

        public Customer Get(int id)
        {
            return db.Customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.OrderBy(c => c.Name);
        }

        public void Update(Customer customer)
        {
            var entry = db.Entry(customer);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
