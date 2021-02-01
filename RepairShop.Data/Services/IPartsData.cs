using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    public interface IPartsData
    {
        IEnumerable<Part> GetAll();
        Part Get(int id);
        void Add(Part part);
        void Update(Part part);
        void Delete(int id);
    }
}
