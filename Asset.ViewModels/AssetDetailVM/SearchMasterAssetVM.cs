using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class SearchMasterAssetVM
    {

        public int? WarrantyTypeId { get; set; }
        public int? ContractTypeId { get; set; }
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
        public int? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetId { get; set; }
        public int? MasterAssetId { get; set; }


        public string MasterAssetName { get; set; }
        public string MasterAssetNameAr { get; set; }

        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string Serial { get; set; }

        public string Code { get; set; }
        public string BarCode { get; set; }
        public string Model { get; set; }

        public string StrSearch { get; set; }
        public decimal? Price { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }



        public DateTime? PurchaseDate { get; set; }
        public DateTime? FromPurchaseDate { get; set; }
        public DateTime? ToPurchaseDate { get; set; }


        public string Start { get; set; }
        public string End { get; set; }

        public string ContractStart { get; set; }
        public string ContractEnd { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }




        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        public string SortField { get; set; }

        public string SortStatus { get; set; }
    }

    public class SearchAssetDetailVM
    {

        public int? WarrantyTypeId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? PeriorityId { get; set; }
        public int? OriginId { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SupplierId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetId { get; set; }
        public int? MasterAssetId { get; set; }
        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string Serial { get; set; }
        public string Code { get; set; }
        public string BarCode { get; set; }
        public string Model { get; set; }
        public string StrSearch { get; set; }
        public decimal? Price { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? FromPurchaseDate { get; set; }
        public DateTime? ToPurchaseDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string ContractStart { get; set; }
        public string ContractEnd { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string SortField { get; set; }
        public string SortStatus { get; set; }
    }

}
