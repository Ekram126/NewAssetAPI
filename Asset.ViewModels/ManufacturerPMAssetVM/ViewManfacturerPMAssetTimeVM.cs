using Asset.ViewModels.PMAssetTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ManufacturerPMAssetVM
{
    public class ViewManfacturerPMAssetTimeVM
    {
        public int Id { get; set; }
        public int MasterAssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string BarCode { get; set; }
        public string AssetStatus { get; set; }
        public string AssetStatusAr { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }

        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }

        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }

        public int? HospitalId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? PMDate { get; set; }
        public int? AssetDetailId { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? DueDate { get; set; }
        public string Comment { get; set; }


        public List<IndexPMAssetTaskVM.GetData> ListMasterAssetTasks { get; set; }
    }
}
