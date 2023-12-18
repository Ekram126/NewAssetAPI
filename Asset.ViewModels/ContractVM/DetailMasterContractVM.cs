using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
    public class DetailMasterContractVM
    {

        public int Id { get; set; }
        public int? TotalVisits { get; set; }
        public int? SupplierId { get; set; }
        public int? HospitalId { get; set; }
        public string Serial { get; set; }

        public string Subject { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public decimal? Cost { get; set; }

        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }
        public string Notes { get; set; }

        public List<ContractDetailVM> ListDetails { get; set; }


    }

    public class ContractDetailVM
    {
        public string BarCode { get; set; }
        public string SerialNumber { get; set; }
        public string BrandNameAr { get; set; }
        public string BrandName { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentName { get; set; }
        public int? ResponseTime { get; set; }
        public bool? HasSpareParts { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
    }
}
