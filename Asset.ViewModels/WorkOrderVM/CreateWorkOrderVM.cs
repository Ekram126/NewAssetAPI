using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderVM
{
    public class CreateWorkOrderVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
   
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }


        public string Note { get; set; }
        public string CreatedById { get; set; }
        public int WorkOrderPeriorityId { get; set; }
        public int WorkOrderTypeId { get; set; }
        public int RequestId { get; set; }
        public int HospitalId { get; set; }
    }
}
