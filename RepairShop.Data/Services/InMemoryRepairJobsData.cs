using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class InMemoryRepairJobsData : IRepairJobsData
    {
        List<RepairJob> repairJobs;

        public InMemoryRepairJobsData()
        {
            repairJobs = new List<RepairJob>
            {
                new RepairJob { Id = 1, StartDate = new DateTime(2021, 2, 12), EndDate = new DateTime(2021, 2, 22), Status = RepairStatus.Pending, CustomerId = 1},
                new RepairJob { Id = 2, StartDate = new DateTime(2021, 3, 1), EndDate = new DateTime(2021, 3, 10), Status = RepairStatus.InProgress, CustomerId = 2},
                new RepairJob { Id = 3, StartDate = new DateTime(2021, 2, 12), EndDate = new DateTime(2021, 2, 22), Status = RepairStatus.WaitingForComponents, CustomerId = 3},
                new RepairJob { Id = 4, StartDate = new DateTime(2021, 3, 1), EndDate = new DateTime(2021, 3, 10), Status = RepairStatus.Done, CustomerId = 4}
            };
        }

        public void Add(RepairJob repairJob)
        {
            repairJobs.Add(repairJob);
            repairJob.Id = repairJobs.Max(r => r.Id) + 1;
            repairJob.CustomerId = repairJobs.Max(repairJobs => repairJobs.CustomerId) + 1;
            repairJob.Status = RepairStatus.Pending;
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
            var existing = Get(repairJob.Id);
            if (existing != null)
            {
                existing.StartDate = repairJob.StartDate;
                existing.EndDate = repairJob.EndDate;
                existing.Status = repairJob.Status;
                existing.CustomerId = repairJob.CustomerId;
                existing.HoursWorkedByEmployee = repairJob.HoursWorkedByEmployee;
                existing.JobDescription = repairJob.JobDescription;
                existing.RequiredParts = repairJob.RequiredParts;
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

        public decimal GetPrice(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsLate(int id)
        {
            var entry = Get(id);
            var today = DateTime.Today;
            return ElementIsLate(entry, today);
        }

        public IEnumerable<bool> IsLate()
        {
            var today = DateTime.Today;
            return repairJobs.Select(r => ElementIsLate(r, today));
        }

        bool ElementIsLate(RepairJob job, DateTime today)
        {
            return (job.StartDate < today && job.Status == RepairStatus.Pending) ||
                (job.EndDate < today && job.Status != RepairStatus.Done);
        }
    }
}
