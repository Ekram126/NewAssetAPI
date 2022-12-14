using Asset.ViewModels.ExternalFixFileVM;
using System;
using System.Collections.Generic;

namespace Asset.ViewModels.ExternalFixVM
{
    public class ViewExternalFixVM
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
            public int HospitalId { get; set; }


    


        public int AssetStatusId { get; set; }
        public DateTime OutDate { get; set; }

        public string ComingNotes { get; set; }

        public int SupplierId { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string Notes { get; set; }
        public DateTime? ComingDate { get; set; }
        public string OutNumber { get; set; }

        public IEnumerable<IndexExternalFixFileVM.GetData> ListExternalFixFiles { get; set; }


    }
}
