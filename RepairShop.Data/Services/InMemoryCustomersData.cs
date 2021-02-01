using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    class InMemoryCustomersData : ICustomersData
    {
        List<Customer> customers;

        public InMemoryCustomersData()
        {
            customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Jan Janssen"},
                new Customer { Id = 2, Name = "Paul Paulusma"},
            };
        }

        public void Add(Customer customer)
        {
            customers.Add(customer);
            customer.Id = customers.Max(r => r.Id) + 1;
        }

        public void Delete(int id)
        {
            var existing = Get(id);
            if (existing != null)
            {
                customers.Remove(existing);
            }
        }

        public Customer Get(int id)
        {
            return customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return customers.OrderBy(c => c.Name);
        }

        public void Update(Customer customer)
        {
            var existing = Get(customer.Id);
            if (existing != null)
            {
                existing.Name = customer.Name;
            }
        }
    }
}
