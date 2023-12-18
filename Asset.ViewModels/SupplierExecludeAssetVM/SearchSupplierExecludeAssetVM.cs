using Asset.ViewModels.EmployeeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
    public class SearchSupplierExecludeAssetVM
    {
        public int? DepartmentId { get; set; }
        public int? StatusId { get; set; }
        public int? PeriorityId { get; set; }
        public int? ModeId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? AssetDetailId { get; set; }
        public int? MasterAssetId { get; set; }
        public int? AppTypeId { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        public string BarCode { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }

        public string Lang { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }




        public string PrintedBy { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string StatusName { get; set; }

    }
}
