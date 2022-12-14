using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.WorkOrderTrackingVM;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class ExportRequestVM
    {


        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int RequestId { get; set; }
        public string Subject { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public int ModeId { get; set; }

        public int? AssetDetailId { get; set; }
        public int? MasterAssetId { get; set; }

        public string Barcode { get; set; }
        public string Serial { get; set; }

        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public int PeriorityId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }

        public string PeriorityName { get; set; }
        public string PeriorityNameAr { get; set; }
        public string PeriorityColor { get; set; }
        public string PeriorityIcon { get; set; }


        public string ClosedDate { get; set; }

        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public string ModelNumber { get; set; }
        public string AssetOwnerCreatedById { get; set; }
        public List<Employee> ListEmployees { get; set; }
        public int? HospitalId { get; set; }
        public int? AssetHospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public string RoleId { get; set; }
        public int CountListTracks { get; set; }
        public int CountWorkOrder { get; set; }
        public int LatestWorkOrderStatusId { get; set; }

        public string WorkOrderSubject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string WorkOrderNote { get; set; }
        public string AssignedTo { get; set; }
        public string WOCreatedBy { get; set; }
        public string WOPeriorityName { get; set; }
        public string WOPeriorityNameAr { get; set; }
        public string WorkOrderTypeName { get; set; }
        public string WorkOrderTypeNameAr { get; set; }
        public string WorkOrderStatusName { get; set; }
        public string WorkOrderStatusNameAr { get; set; }
        public string WorkOrderStatusIcon { get; set; }
        public string WorkOrderStatusColor { get; set; }
        public string ElapsedTime { get; set; }
        public DateTime? DescriptionDate { get; set; }
        public string WOLastTrackDescription { get; set; }
        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public List<IndexRequestTrackingVM.GetData> ListTracks { get; set; }
        public List<IndexWorkOrderVM> ListWorkOrder { get; set; }

        public List<ListWorkOrderVM.GetData> ListWorkOrders { get; set; }
        public List<LstWorkOrderFromTracking> ListWorkOrderTracking { get; set; }




        public string BGColor { get; set; }
        public string Lang { get; set; }
        public string PrintedBy { get; set; }

        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

    }
}
