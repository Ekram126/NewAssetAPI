using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Note { get; set; }
        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser User { get; set; }


        public int? WorkOrderPeriorityId { get; set; }
        [ForeignKey("WorkOrderPeriorityId")]
        public virtual WorkOrderPeriority WorkOrderPeriority { get; set; }
        public int? WorkOrderTypeId { get; set; }
        [ForeignKey("WorkOrderTypeId")]
        public virtual WorkOrderType WorkOrderType { get; set; }
        public int? RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
