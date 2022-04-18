using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class WorkOrderTracking
    {
        public int Id { get; set; }
        public DateTime? WorkOrderDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Notes { get; set; }
        public string AssignedTo { get; set; }

        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("AssignedTo")]
        public virtual ApplicationUser AssignedToUser { get; set; }

        public int WorkOrderStatusId { get; set; }
        [ForeignKey("WorkOrderStatusId")]
        public virtual WorkOrderStatus WorkOrderStatus { get; set; }
        public int WorkOrderId { get; set; }
        [ForeignKey("WorkOrderId")]
        public virtual WorkOrder WorkOrder { get; set; }


        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }


        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
