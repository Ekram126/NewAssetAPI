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
    public class SearchAssetStockTakingVM
    {
        public int? PeriorityId { get; set; }
        public int? OriginId { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SupplierId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? HospitalId { get; set; }
        public int? AssetId { get; set; }
        public int? MasterAssetId { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string BarCode { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}









