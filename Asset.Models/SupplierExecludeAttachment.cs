using System;
using System.Collections.Generic;
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



        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
