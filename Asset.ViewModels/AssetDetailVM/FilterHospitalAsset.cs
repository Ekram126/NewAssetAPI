using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class FilterHospitalAsset
    {
        public int? StatusId { get; set; }
        public int? BrandId { get; set; }
        public int? SupplierId { get; set; }
        public int? DepartmentId { get; set; }
        public int? MasterAssetId { get; set; }
        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public DateTime? purchaseDateFrom { get; set; }
        public DateTime? purchaseDateTo { get; set; }



        public string Start { get; set; }
        public string End { get; set; }
        public string Lang { get; set; }
        public string PrintedBy { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string selectedElement { get; set; }

        public int? CategoryId { get; set; }
        public int? PeriorityId { get; set; }
    }
}
