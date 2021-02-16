using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlRepairJobsPartsData : IRepairJobsPartsData
    {
        private readonly RepairShopDbContext db;

        public SqlRepairJobsPartsData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(RepairJobPart entry)
        {
            db.RepairJobsParts.Add(entry);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.RepairJobsParts.Find(id);
            db.RepairJobsParts.Remove(entry);
            db.SaveChanges();
        }

        public RepairJobPart Get(int repairId, int partId)
        {
            return db.RepairJobsParts.FirstOrDefault(e => e.RepairJobId == repairId && e.PartId == partId);
        }

        public IEnumerable<RepairJobPart> GetAll()
        {
            return db.RepairJobsParts;
        }

        public void Update(RepairJobPart repairJobPart)
        {
            var entry = db.Entry(repairJobPart);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
