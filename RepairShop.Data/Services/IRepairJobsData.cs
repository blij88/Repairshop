using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public interface IRepairJobsData
    {
        IEnumerable<RepairJob> GetAll();
        RepairJob Get(int id);
        void Add(RepairJob repairJob);
        void Update(RepairJob repairJob);
        void Delete(int id);
        int AmountWithStatus(RepairStatus status);
        Dictionary<RepairStatus, int> StatusAmounts();
        decimal GetPrice(int id);
        bool IsLate(int id);
    }
}
