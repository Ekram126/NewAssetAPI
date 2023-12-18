using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ManufacturerPMAssetVM
{
    public class IndexManfacturerPMAssetVM
    {
        public List<GetData> Results { get; set; }
        public int? YearQuarter { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int CountDone { get; set; }
        public int CountNotDone { get; set; }
        public class GetData
        {

            public int Id { get; set; }
            public string TypeName { get; set; }
            public string TypeNameAr { get; set; }
            public int StatusId { get; set; }
            public string BarCode { get; set; }
            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string SerialNumber { get; set; }
            public string ModelNumber { get; set; }

            public int? SupplierId { get; set; }
            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }

            public int? AssetId { get; set; }
            public int? MasterAssetId { get; set; }
            public int? HospitalId { get; set; }


            public int? BrandId { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }


            public int? DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }







            public DateTime? PMDate { get; set; }

            public DateTime? DoneDate { get; set; }

            public DateTime? DueDate { get; set; }
            public bool IsDone { get; set; }
        }
    }
}
