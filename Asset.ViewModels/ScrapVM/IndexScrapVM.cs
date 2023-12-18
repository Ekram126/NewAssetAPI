using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ScrapVM
{
    public class IndexScrapVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {

            public int Id { get; set; }
            public string ScrapDate { get; set; }
            public DateTime? SortScrapDate { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string SerialNumber { get; set; }
            public string Barcode { get; set; }
            public string ScrapNo { get; set; }
            public string Comment { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }
            public string ScrapReasonName { get; set; }
            public string ScrapReasonNameAr { get; set; }
            public string Model { get; set; }

            public int? BrandId { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }


            public string Lang { get; set; }
            public string OrgName { get; set; }
            public string OrgNameAr { get; set; }

            public string SubOrgName { get; set; }
            public string SubOrgNameAr { get; set; }

            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }


   
            public string AssetPeriorityName { get; set; }
            public string AssetPeriorityNameAr { get; set; }

            public int? PeriorityId { get; set; }
            public int? OriginId { get; set; }
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

            public DateTime? InstallationDate { get; set; }
            public string BarCode { get; set; }
            public string AssetBarCode { get; set; }
            public string MasterAssetName { get; set; }
            public string MasterAssetNameAr { get; set; }
            public int? AssetStatusId { get; set; }
            public DateTime? WarrantyStart { get; set; }
            public DateTime? WarrantyEnd { get; set; }
            public string EndWarrantyDate { get; set; }

            public string AssetStatus { get; set; }
            public string AssetStatusAr { get; set; }


            public string GovernorateName { get; set; }
            public string GovernorateNameAr { get; set; }
            public string CreatedBy { get; set; }

            public string CityName { get; set; }
            public string CityNameAr { get; set; }

            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }

            public string AssetImg { get; set; }
            public int? MasterAssetId { get; set; }

    public string PrintedBy { get; set; }
        }
    }



    public class ListOfScrap
    {
        public int Id { get; set; }
        public string ScrapDate { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }
        public string ScrapNo { get; set; }
        public string Comment { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string ScrapReasonName { get; set; }
        public string ScrapReasonNameAr { get; set; }
        public string Model { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }
        public string Lang { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }
        public int? DepartmentId { get; set; }
        public int? SupplierId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? AssetId { get; set; }
        public string QrFilePath { get; set; }
        public string QrData { get; set; }
        public string CreatedBy { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string PrintedBy { get; set; }
    }
}
