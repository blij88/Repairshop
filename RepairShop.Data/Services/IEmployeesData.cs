using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();
        Employee Get(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
    }
}
