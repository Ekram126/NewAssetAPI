using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderVM
{
    public class EditWorkOrderVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Note { get; set; }
        public string CreatedById { get; set; }
        public int WorkOrderPeriorityId { get; set; }
        public int WorkOrderTypeId { get; set; }
        public int RequestId { get; set; }
    }
}
