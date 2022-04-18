using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class SupplierExeclude
    {

        public int Id { get; set; }

        public int? SupplierExecludeAssetId { get; set; }
        [ForeignKey("SupplierExecludeAssetId")]
        public virtual SupplierExecludeAsset SupplierExecludeAsset { get; set; }




        public int? ReasonId { get; set; }
        [ForeignKey("ReasonId")]
        public virtual SupplierExecludeReason SupplierExecludeReason { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
