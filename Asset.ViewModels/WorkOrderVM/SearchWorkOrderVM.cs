using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderVM
{
    public class SearchWorkOrderVM
    {

        public int? StatusId { get; set; }
        public int? PeriorityId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetDetailId { get; set; }
        public int? MasterAssetId { get; set; }

        public string Subject { get; set; }
        public string WONumber { get; set; }
        public string UserId { get; set; }
        public string BarCode { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }

        public string Start { get; set; }
        public string End { get; set; }


        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
    }
}
