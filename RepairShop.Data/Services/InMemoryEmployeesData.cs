using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        List<Employee> employees;

        public InMemoryEmployeesData()
        {
            employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Janine Janssen", HourlyCost = 8M},
                new Employee { Id = 2, Name = "Paula Paulusma", HourlyCost = 10M},
            };
        }

        public void Add(Employee employee)
        {
            employees.Add(employee);
            employee.Id = employees.Max(e => e.Id) + 1;
        }

        public void Delete(int id)
        {
            var existing = Get(id);
            if (existing != null)
            {
                employees.Remove(existing);
            }
        }

        public Employee Get(int id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return employees.OrderBy(c => c.Name);
        }

        public void Update(Employee employee)
        {
            var existing = Get(employee.Id);
            if (existing != null)
            {
                existing.Name = employee.Name;
            }
        }
    }
}
