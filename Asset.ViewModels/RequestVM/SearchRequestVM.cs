using Asset.ViewModels.EmployeeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class SearchRequestVM
    {

        public int? StatusId { get; set; }
        public int? PeriorityId { get; set; }
        public int? ModeId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetId { get; set; }
        public int? AssetOwnerId { get; set; }
        public string AssetOwnerCreatedById { get; set; }
        public List<IndexEmployeeVM> ListEmployees { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }


        public string Start { get; set; }
        public string End { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
