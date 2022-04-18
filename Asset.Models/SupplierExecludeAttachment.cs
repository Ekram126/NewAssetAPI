using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class SupplierExecludeAttachment
    {

        public int Id { get; set; }

        public int? SupplierExecludeId { get; set; }
        [ForeignKey("SupplierExecludeId")]
        public virtual SupplierExeclude SupplierExeclude { get; set; }





        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string Title { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
