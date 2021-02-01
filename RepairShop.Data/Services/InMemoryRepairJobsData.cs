using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    class InMemoryRepairJobsData : IRepairJobsData
    {
        List<RepairJob> repairJobs;

        public InMemoryRepairJobsData()
        {
            repairJobs = new List<RepairJob>
            {
                new RepairJob { Id = 1, StartDate = new DateTime(2021, 2, 12), EndDate = new DateTime(2021, 2, 22), Status = RepairStatus.Pending},
                new RepairJob { Id = 1, StartDate = new DateTime(2021, 3, 1), EndDate = new DateTime(2021, 3, 10), Status = RepairStatus.Pending}
            };
        }

        public void Add(RepairJob repairJob)
        {
            repairJobs.Add(repairJob);
            repairJob.Id = repairJobs.Max(r => r.Id) + 1;
        }

        public void Delete(int id)
        {
            var existing = Get(id);
            if (existing != null)
            {
                repairJobs.Remove(existing);
            }
        }

        public RepairJob Get(int id)
        {
            return repairJobs.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<RepairJob> GetAll()
        {
            return repairJobs.OrderBy(r => r.EndDate);
        }

        public void Update(RepairJob repairJob)
        {
            var existing = this.Get(repairJob.Id);
            if (existing != null)
            {
                existing.StartDate = repairJob.StartDate;
                existing.EndDate = repairJob.EndDate;
                existing.Status = repairJob.Status;
            }
        }

        public int AmountWithStatus(RepairStatus status)
        {
            return repairJobs.Count(r => r.Status == status);
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
    }
}
