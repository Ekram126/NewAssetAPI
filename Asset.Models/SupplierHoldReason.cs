using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class SupplierHoldReason
    {

        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string NameAr { get; set; }
    }
}
