using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTrackingVM
{
    public class WorkOrderDetails
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
        public string CreatedBy { get; set; }
        public int WorkOrderPeriorityId { get; set; }
        public string WorkOrderPeriorityName { get; set; }
        public string WorkOrderPeriorityNameAr { get; set; }
        public int WorkOrderTypeId { get; set; }
        public string WorkOrderTypeName { get; set; }
        public int RequestId { get; set; }
        public string RequestSubject { get; set; }
        public int WorkOrderTrackingId { get; set; }
        public int MasterAssetId { get; set; }

        public string AssetSerial { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
   



        public List<IndexWorkOrderTrackingVM> LstWorkOrderTracking { get; set; }
    }
}
