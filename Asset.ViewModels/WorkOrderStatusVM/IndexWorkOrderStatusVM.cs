using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderStatusVM
{
    public class IndexWorkOrderStatusVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Color { get; set; }
  public string Icon { get; set; }

        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }



        public int? CountAssigned { get; set; }
        public int? CountInProgress { get; set; }
        public int? CountExternalSupport { get; set; }
        public int? CountSparePart { get; set; }
        public int? CountEscalate { get; set; }
        public int? CountPending { get; set; }
        public int? CountDone { get; set; }
        public int? CountReview { get; set; }
        public int? CountReAssigned { get; set; }
        public int? CountTechApprove { get; set; }
        public int? CountUserApprove { get; set; }
        public int? CountClosed { get; set; }
        public int? CountAll{ get; set; }


        public List<WorkOrderStatus> ListStatus { get; set; }

    }
}
