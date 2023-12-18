using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class IndexAssetDetailVM
    {

        public List<GetData> Results { get; set; }

        public int Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public int? MasterAssetId { get; set; }
            public int? MasterContractId { get; set; }
            public string Code { get; set; }
            public string Serial { get; set; }
            public string Model { get; set; }
            public string SerialNumber { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string AssetImg { get; set; }
      
            public decimal? Price { get; set; }
            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }

            public string GovernorateName { get; set; }
            public string GovernorateNameAr { get; set; }
            public string CreatedBy { get; set; }

            public string CityName { get; set; }
            public string CityNameAr { get; set; }



            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }

            public string OrgName { get; set; }
            public string OrgNameAr { get; set; }

            public string SubOrgName { get; set; }
            public string SubOrgNameAr { get; set; }

            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }


            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }

            public string CategoryName { get; set; }
            public string CategoryNameAr { get; set; }
            public string AssetPeriorityName { get; set; }
            public string AssetPeriorityNameAr { get; set; }


            public int? EmployeeId { get; set; }
            public int? PeriorityId { get; set; }
            public int? OriginId { get; set; }
            public int? BrandId { get; set; }
            public int? CategoryId { get; set; }
            public int? SubCategoryId { get; set; }
            public int? DepartmentId { get; set; }
            public int? SupplierId { get; set; }
            public int? HospitalId { get; set; }
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }
            public int? AssetId { get; set; }
            public string QrFilePath { get; set; }
            public string QrData { get; set; }
            public string MasterImg { get; set; }


            public List<int> ListAssetIds { get; set; }


            public DateTime? PurchaseDate { get; set; }
            public DateTime? OperationDate { get; set; }
            public DateTime? InstallationDate { get; set; }
            public string BarCode { get; set; }
            public string Barcode { get; set; }
            public string AssetBarCode { get; set; }
            public string MasterAssetName { get; set; }
            public string MasterAssetNameAr { get; set; }
            public int? AssetStatusId { get; set; }


            public DateTime? WarrantyStart { get; set; }
            public DateTime? WarrantyEnd { get; set; }
            public string EndWarrantyDate { get; set; }

            public int Count { get; set; }

            public string AssetStatus { get; set; }
            public string AssetStatusAr { get; set; }

            public DateTime? ContractDate { get; set; }
            public DateTime? ContractStartDate { get; set; }
            public DateTime? ContractEndDate { get; set; }




            public string StrPurchaseDate { get; set; }
            public string StrOperationDate { get; set; }
            public string StrInstallationDate { get; set; }


            public string InContract { get; set; }
            public string InContractAr { get; set; }



            public string ContractFrom { get; set; }
            public string ContractTo { get; set; }

    
            public int? StatusId { get; set; }

            public string StrReceivingDate { get; set; }

            public DateTime? ReceivingDate { get; set; }

            public string WarrantyExpires { get; set; }


            public int? BuildingId { get; set; }
            public int? RoomId { get; set; }
            public int? FloorId { get; set; }

            public string BuildName { get; set; }
            public string BuildNameAr { get; set; }
            public string FloorName { get; set; }
            public string FloorNameAr { get; set; }
            public string RoomNameAr { get; set; }
            public string RoomName { get; set; }


            public string PONumber { get; set; }
            public decimal? DepreciationRate { get; set; }
            public string CostCenter { get; set; }

        }
    }
}
