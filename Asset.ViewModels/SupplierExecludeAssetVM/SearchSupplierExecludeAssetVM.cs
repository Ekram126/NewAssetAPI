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

        public int? StatusId { get; set; }
        public int? PeriorityId { get; set; }
        public int? ModeId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetDetailId { get; set; }
        public int? MasterAssetId { get; set; }



        public string strStartDate { get; set; }
        public string strEndDate { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
