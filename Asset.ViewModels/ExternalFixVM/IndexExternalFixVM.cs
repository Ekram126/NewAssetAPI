using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixVM
{
    public class IndexExternalFixVM
    {

        public List<GetData> Results { get; set; }
        public int Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }

            public string SerialNumber { get; set; }
            public string Barcode { get; set; }
            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string ModelNumber { get; set; }
            public int? AssetDetailId { get; set; }

            public int MasterAssetId { get; set; }
            public int HospitalId { get; set; }
            public int BrandId { get; set; }
            public int DepartmentId { get; set; }
            public DateTime? ComingDate { get; set; }

            public DateTime? OutDate { get; set; }
            public DateTime? ExpectedDate { get; set; }
            public int? SupplierId { get; set; }
            public string Notes { get; set; }
            public string ComingNotes { get; set; }
            public string OutNumber { get; set; }



        }
    }
}
