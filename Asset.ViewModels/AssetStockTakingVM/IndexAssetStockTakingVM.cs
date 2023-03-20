using Asset.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStockTakingVM
{
    public class IndexAssetStockTakingVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {
            public string UserName { get; set; }
            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }
            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }

            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }

            public string BarCode { get; set; }
            public string SerialNumber { get; set; }
            public string URL { get; set; }
            public int Id { get; set; }
            public int? HospitalId { get; set; }
            public DateTime? CaptureDate { get; set; }
            public string UserId { get; set; }
            public int? AssetDetailId { get; set; }
            public int? STSchedulesId { get; set; }
            public decimal? Longtitude { get; set; }
            public decimal? Latitude { get; set; }
            // public int? PeriorityId { get; set; }

            public int? BrandId { get; set; }
            public int? DepartmentId { get; set; }
            //public int? SupplierId { get; set; }
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }
            public int? AssetId { get; set; }
            public int? MasterAssetId { get; set; }
            public string ModelNumber { get; set; }

        }
    }
}









