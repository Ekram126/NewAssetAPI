using System;
using System.Collections.Generic;

namespace Asset.ViewModels.AssetDetailVM
{
    public class EditAssetDetailVM
    {
        public int Id { get; set; }

        public string AssetName { get; set; }


        public string AssetNameAr { get; set; }
        public string AssetImg { get; set; }

        public string Code { get; set; }
        public string PurchaseDate { get; set; }


        public decimal? Price { get; set; }

        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }

        public string Barcode { get; set; }
        public string BarCode { get; set; }

        public string InstallationDate { get; set; }

        public string WarrantyExpires { get; set; }
        public int? DepartmentId { get; set; }
        public int? SupplierId { get; set; }

        public int? HospitalId { get; set; }
        public int? MasterAssetId { get; set; }

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


        public List<int?> ListOwners { get; set; }
        public string QrFilePath { get; set; }

        public string BuildName { get; set; }
        public string BuildNameAr { get; set; }
        public string FloorName { get; set; }
        public string FloorNameAr { get; set; }
        public string RoomNameAr { get; set; }
        public string RoomName { get; set; }
        public string HospitalNameAr { get; set; }
        public string HospitalName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentName { get; set; }

        public int AssetStatusId { get; set; }
        public string AssetStatus { get; set; }
        public string AssetStatusAr { get; set; }





        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }

        public string SupplierNameAr { get; set; }
        public string SupplierName { get; set; }

        public string CreatedBy { get; set; }
        public int? BrandId { get; set; }
        public string BrandNameAr { get; set; }
        public string BrandName
        {
            get; set;
        }
        public decimal? FixCost { get; set; }
       public string DomainName { get; set; }

    }
}
