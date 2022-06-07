using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTrackingVM
{
    public class IndexWorkOrderTrackingVM
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public DateTime? WorkOrderDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Notes { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }

        public int WorkOrderStatusId { get; set; }
        public string WorkOrderStatusName { get; set; }
        public string WorkOrderStatusNameAr { get; set; }
        public string WorkOrderStatusColor { get; set; }
        public string WorkOrderStatusIcon { get; set; }
        public int WorkOrderId { get; set; }
        public string WorkOrderSubject { get; set; }
        public string CreatedToId { get; set; }
        public string CreatedTo { get; set; }


        public string PeriorityName { get; set; }
        public string PeriorityNameAr { get; set; }

        public string TypeName { get; set; }
        public string TypeNameAr { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
