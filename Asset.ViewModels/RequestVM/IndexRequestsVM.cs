using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class IndexRequestsVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string AssetCode { get; set; }
        public string RequestCode { get; set; }
        public string Description { get; set; }
        public string RequestTrackDescription { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestTime { get; set; }
        public int RequestModeId { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public int ProblemId { get; set; }
        public int? SubProblemId { get; set; }
        public string SubProblemName { get; set; }
        public string SubProblemNameAr { get; set; }
        public int MasterAssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public int AssetDetailId { get; set; }
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }
        public int RequestPeriorityId { get; set; }
        public string PeriorityNameAr { get; set; }
        public string PeriorityName { get; set; }
        public string PeriorityColor { get; set; }
        public string PeriorityIcon { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public string RequestTypeNameAr { get; set; }
        public int RequestTrackingId { get; set; }
        public int RequestStatusId { get; set; }

        public string ClosedDate { get; set; }
        public string ModelNumber { get; set; }


        public string StatusIcon { get; set; }
        public int StatusId { get; set; }
        public string StatusColor { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string UserId { get; set; }

        public int HospitalId { get; set; }
        public int SubOrganizationId { get; set; }
        public int OrganizationId { get; set; }
        public int CityId { get; set; }
        public int AssetHospitalId { get; set; }
        public int GovernorateId
        {
            get; set;
        }

        public int CountListTracks { get; set; }
        public int CountWorkOrder { get; set; }

        public string SupplierNameAr { get; set; }
        public string SupplierName { get; set; }


        public string BrandNameAr { get; set; }
        public string BrandName { get; set; }

        public string DepartmentNameAr { get; set; }
        public string DepartmentName { get; set; }


        public int LatestWorkOrderStatusId { get; set; }

        public string WOLastTrackDescription { get; set; }





        public string BuildName { get; set; }
        public string BuildNameAr { get; set; }
        public string FloorName { get; set; }
        public string FloorNameAr { get; set; }
        public string RoomNameAr { get; set; }
        public string RoomName { get; set; }
        public string PurchaseDate { get; set; }
        public string InstallationDate { get; set; }
        public string WarrantyExpires { get; set; }
        public string WarrantyStart { get; set; }
        public string WarrantyEnd { get; set; }
        public int? BuildingId { get; set; }
        public int? RoomId { get; set; }
        public int? FloorId { get; set; }

        public string OperationDate { get; set; }
        public string ReceivingDate { get; set; }
        public string PONumber { get; set; }
        public decimal? DepreciationRate { get; set; }
        public string CostCenter { get; set; }
        public string Remarks { get; set; }



        public List<IndexRequestTrackingVM.GetData> ListTracks { get; set; }
    }
}
