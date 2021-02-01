using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    class InMemoryPartsData : IPartsData
    {
        List<Part> parts;

        public InMemoryPartsData()
        {
            parts = new List<Part>
            {
                new Part { Id = 1, Name = "Bolt M6", UnitCost = 0.20M},
                new Part { Id = 2, Name = "Gizmo", UnitCost = 249M},
            };
        }

        public void Add(Part part)
        {
            parts.Add(part);
            part.Id = parts.Max(p => p.Id) + 1;
        }

        public void Delete(int id)
        {
            var existing = Get(id);
            if (existing != null)
            {
                parts.Remove(existing);
            }
        }

        public Part Get(int id)
        {
            return parts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Part> GetAll()
        {
            return parts.OrderBy(p => p.Name);
        }

        public void Update(Part part)
        {
            var existing = Get(part.Id);
            if (existing != null)
            {
                existing.Name = part.Name;
                existing.UnitCost = part.UnitCost;
            }
        }
    }
}
