﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
