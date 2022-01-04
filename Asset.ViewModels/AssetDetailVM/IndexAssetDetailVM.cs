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


        public class GetData
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public int? MasterAssetId { get; set; }
            public string Code { get; set; }
            public string Serial { get; set; }
            public string Model { get; set; }
            public string SerialNumber { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public DateTime? PurchaseDate { get; set; }
            public decimal? Price { get; set; }
            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }

            public string GovernorateName { get; set; }
            public string GovernorateNameAr { get; set; }


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

            public List<int> ListAssetIds { get; set; }

            public string MasterImg { get; set; }


            public DateTime? InstallationDate{ get; set; }


        }
    }
}
