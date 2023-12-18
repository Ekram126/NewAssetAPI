using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ScrapVM
{
    public class SearchScrapVM
    {
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
        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string BarCode { get; set; }
        public string ModelNumber { get; set; }

        public string StrSearch { get; set; }
        public decimal? Price { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }


        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? FromPurchaseDate { get; set; }
        public DateTime? ToPurchaseDate { get; set; }


        public string Start { get; set; }
        public string End { get; set; }

        public string SortField { get; set; }

        public string SortStatus { get; set; }

        public string Lang { get; set; }
        public string PrintedBy { get; set; }


        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }

        public string StartDate2 { get; set; }
        public string EndDate2 { get; set; }


        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }


    }
}
