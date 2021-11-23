using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTrackingVM
{
    public class EditWorkOrderTrackingVM
    {
        public int Id { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
        public string CreatedById { get; set; }
        public int WorkOrderStatusId { get; set; }
        public int WorkOrderId { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
