﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class MasterContract
    {
        public int Id { get; set; }


        public int? SupplierId { get; set; }

        [StringLength(50)]
        public string Serial { get; set; }


        [StringLength(100)]
        public string Subject { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ContractDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }


        public decimal? Cost { get; set; }
    }
}
