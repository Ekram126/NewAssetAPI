using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixVM
{
   public class SearchExternalFixVM
    {
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
        public string ModelNumber { get; set; }

        public string StrSearch { get; set; }
        public decimal? Price { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public string Start { get; set; }
        public string End { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }




        public string CommingStart { get; set; }
        public string CommingEnd { get; set; }
        public DateTime? CommingStartDate { get; set; }
        public DateTime? CommingEndDate { get; set; }



        public string ExpectedStart { get; set; }
        public string ExpectedEnd { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }


    }
}
