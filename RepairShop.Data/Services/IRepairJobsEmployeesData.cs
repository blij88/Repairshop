using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepairShop.Data.Models;

namespace RepairShop.Data.Services
{
    public interface IRepairJobsEmployeesData
    {
        IEnumerable<RepairJobEmployee> GetAll();
        RepairJobEmployee Get(int repairJobId, int employeeId);
        void Add(RepairJobEmployee entry);
        void Update(RepairJobEmployee repairJobEmployee);
        void Delete(int Id);
    }
}
