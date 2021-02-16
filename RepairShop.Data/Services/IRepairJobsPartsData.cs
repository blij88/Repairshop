using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepairShop.Data.Models;

namespace RepairShop.Data.Services
{
    public interface IRepairJobsPartsData
    {
        IEnumerable<RepairJobPart> GetAll();
        RepairJobPart Get(int repairJobId, int partId);
        void Add(RepairJobPart entry);
        void Update(RepairJobPart repairJobPart);
        void Delete(int Id);
    }
}