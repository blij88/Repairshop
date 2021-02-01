using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public class SqlPartsData : IPartsData
    {
        private readonly RepairShopDbContext db;

        public SqlPartsData(RepairShopDbContext db)
        {
            this.db = db;
        }

        public void Add(Part part)
        {
            db.Parts.Add(part);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entry = db.Parts.Find(id);
            db.Parts.Remove(entry);
            db.SaveChanges();
        }

        public Part Get(int id)
        {
            return db.Parts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Part> GetAll()
        {
            return db.Parts.OrderBy(p => p.Name);
        }

        public void Update(Part part)
        {
            var entry = db.Entry(part);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
