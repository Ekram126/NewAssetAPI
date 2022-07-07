using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
   public class ReportRequestVM
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public string StartRequestDate { get; set; }
        public string InitialWorkOrderDate { get; set; }
        public string StartWorkOrderDate { get; set; }
        public string DurationBetweenStartRequestWorkOrder { get; set; }




        public string FirstStepInTrackWorkOrderInProgress { get; set; }
          public string LastStepInTrackWorkOrderInProgress { get; set; }
        public string DurationBetweenWorkOrders { get; set; }




        public string ClosedWorkOrderDate { get; set; }
        public string CloseRequestDate { get; set; }
        public string DurationTillCloseDate { get; set; }

    }
}
