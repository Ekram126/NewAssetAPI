using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class WorkOrderAssign
    {
        public int Id { get; set; }
        public int WOTId{ get; set; }
        public int? SupplierId{ get; set; }
        public string Notes{ get; set; }
        public string CreatedBy { get; set; }
        public string UserId  { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public DateTime? CreatedDate{ get; set; }
    }
}
