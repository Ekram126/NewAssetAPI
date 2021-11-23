using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class WorkOrderTask
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int WorkOrderId { get; set; }
        [ForeignKey("WorkOrderId")]
        public virtual WorkOrder WorkOrder { get; set; }
        public int AssetWorkOrderTaskId { get; set; }
        [ForeignKey("AssetWorkOrderTaskId")]
        public virtual AssetWorkOrderTask AssetWorkOrderTask { get; set; }
    }
}
