using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixVM
{
    public class SortExternalFixVM
    {
        public int Id { get; set; }
        public string SortStatus { get; set; }

        public int StatusId { get; set; }
        public int? HospitalId { get; set; }
        public int? MasterAssetId { get; set; }
        public int? AssetModel { get; set; }
        public int? SupplierId { get; set; }
        public int? BrandId { get; set; }


        public string UserId { get; set; }
        public string Code { get; set; }
        public string BarCode { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

        public string Model { get; set; }
        public string Serial { get; set; }
        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }

        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string OutDate { get; set; }
    }
}
