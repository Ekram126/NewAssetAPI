using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTrackingVM
{
    public class LstWorkOrderFromTracking
    {
        public int Id { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public int WorkOrderStatusId { get; set; }
        public string WorkOrderStatusName { get; set; }
        public string WorkOrderStatusNameAr { get; set; }
        public string WorkOrderStatusIcon { get; set; }
        public string WorkOrderStatusColor { get; set; }
        public string WorkOrderSubject { get; set; }

        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }

        public int WorkOrderId { get; set; }
        public string Subject { get; set; }
        public string WorkOrderNumber { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Note { get; set; }
        public int WorkOrderPeriorityId { get; set; }
        public string WorkOrderPeriorityName { get; set; }
        public string WorkOrderPeriorityNameAr { get; set; }

        public string WorkOrderPeriorityIcon { get; set; }
        public string WorkOrderPeriorityColor { get; set; }

        public int WorkOrderTypeId { get; set; }
        public string WorkOrderTypeName { get; set; }
        public int RequestId { get; set; }
        public string RequestSubject { get; set; }
        public int WorkOrderTrackingId { get; set; }

        public string SerialNumber { get; set; }

        public int HospitalId { get; set; }
        public int SubOrganizationId { get; set; }
        public int OrganizationId { get; set; }
        public int CityId { get; set; }
        public int GovernorateId { get; set; }


        public string RoleName { get; set; }
        public string RoleId { get; set; }

        public string ClosedDate { get; set; }

    }
}
