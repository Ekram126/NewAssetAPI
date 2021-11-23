﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderVM
{
    public class IndexWorkOrderVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
       
        public string WorkOrderNumber { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Note { get; set; }
        public string CreatedById { get; set; }
        public string TrackCreatedById { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }


        public int WorkOrderPeriorityId { get; set; }
        public string WorkOrderPeriorityName { get; set; }

        public string PeriorityName { get; set; }
        public string PeriorityNameAr { get; set; }

        public int WorkOrderTypeId { get; set; }
        public string WorkOrderTypeName { get; set; }
        public string WorkOrderTypeNameAr { get; set; }
        public string TypeName { get; set; }
        public string TypeNameAr { get; set; }



        public int RequestId { get; set; }
        public string RequestSubject { get; set; }
        public int WorkOrderTrackingId { get; set; }
        public int WorkOrderStatusId { get; set; }

        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
    public string statusColor { get; set; }

        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }

        public string UserName { get; set; }

        public bool ExistStatusId { get; set; }


        public int? MasterAssetId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public string RoleId { get; set; }

    }
}
