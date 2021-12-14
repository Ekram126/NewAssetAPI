using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestStatusVM
{
    public class IndexRequestStatusVM
    {
        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }
            public string CreatedBy { get; set; }
            public int? RequestId { get; set; }
            public int? CountOpen { get; set; }
            public int? CountClosed { get; set; }
            public int? CountInProgress { get; set; }
            public int? CountSolved { get; set; }
            public int? CountApproved { get; set; }
            public int? HospitalId { get; set; }
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }
            public List<RequestStatus> ListStatus{ get; set; }

    }
    }
}
