using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.WorkOrderTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class PrintServiceRequestVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MasterAssetCode { get; set; }
        public string AssetCode { get; set; }
        public string AssetBarCode { get; set; }
        public string WorkOrderSubject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Note { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string PeriorityName { get; set; }
        public string PeriorityNameAr { get; set; }
        public string TypeName { get; set; }
        public string TypeNameAr { get; set; }

        public int RequestId { get; set; }
        public string RequestSubject { get; set; }
        public string RequestDate { get; set; }
        public string LastRequestDate { get; set; }
        public string DescriptionDate { get; set; }
        public string RequestCode { get; set; }
        public string RequestNote { get; set; }
        public string ProblemName { get; set; }
        public string ProblemNameAr { get; set; }
        public string SubProblemName { get; set; }
        public string SubProblemNameAr { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public string RequestTypeName { get; set; }
        public string RequestTypeNameAr { get; set; }


        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string UserName { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public int? MasterAssetId { get; set; }
        public int? AssetHospitalId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public string RoleId { get; set; }


        public string BuildingName { get; set; }
        public string BuildingNameAr { get; set; }
        public string FloorName { get; set; }
        public string FloorNameAr { get; set; }
        public string RoomName { get; set; }
        public string RoomNameAr { get; set; }
        public string ModelNumber { get; set; }


        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }


        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }

        public List<LstWorkOrderFromTracking> LstWorkOrderTracking { get; set; }

        public List<IndexRequestTracking> RequestTrackingList { get; set; }
    }
}
