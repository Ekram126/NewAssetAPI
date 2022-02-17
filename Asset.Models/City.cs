﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class City
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }

        public int? GovernorateId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitude { get; set; }
    }
}
