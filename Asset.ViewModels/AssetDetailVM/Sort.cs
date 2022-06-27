using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class Sort
    {
        public int Id { get; set; }

        public int StatusId { get; set; }
        public int? HospitalId { get; set; }

        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? MasterAssetId { get; set; }
        public int? AssetModel { get; set; }
        public int? OriginId { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }


        public string UserId { get; set; }
        public string Code { get; set; }
        public string BarCode { get; set; }
        public string BarCodeValue { get; set; }
        public string SerialValue { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }
        public string CityName { get; set; }
        public string CityNameAr { get; set; }
        public string OrgName { get; set; }
        public string OrgNameAr { get; set; }

        public string SubOrgName { get; set; }
        public string SubOrgNameAr { get; set; }
        public string SortStatus { get; set; }

        public string Model { get; set; }
        public string Serial { get; set; }


        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }


        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }
    }
}
