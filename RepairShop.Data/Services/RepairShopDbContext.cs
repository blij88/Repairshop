using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Services
{
    class RepairShopDbContext :DbContext
    {
        public DbSet<RepairJob> RepairJobs { get; set; }
    }
}
