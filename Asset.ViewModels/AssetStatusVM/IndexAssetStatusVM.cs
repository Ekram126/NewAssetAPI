using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStatusVM
{
    public class IndexAssetStatusVM
    {

        public List<GetData> Results { get; set; }





        public class GetData
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }




            public int? HospitalId { get; set; }
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }
            public List<AssetStatu> ListStatus { get; set; }
            public int? CountNeedRepair { get; set; }
            public int? CountInActive { get; set; }
            public int? CountWorking { get; set; }
            public int? CountUnderMaintenance { get; set; }
            public int? CountUnderInstallation { get; set; }
            public int? CountNotWorking { get; set; }
            public int? CountShutdown { get; set; }
            public int? CountExecluded { get; set; }
            public int? CountHold { get; set; }
        }
    }
}
