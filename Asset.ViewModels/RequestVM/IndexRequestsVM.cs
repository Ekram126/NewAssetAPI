using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class IndexRequestsVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string RequestCode { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestTime { get; set; }
        public int RequestModeId { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public int ProblemId { get; set; }
        public int SubProblemId { get; set; }
        public string SubProblemName { get; set; }
        public string SubProblemNameAr { get; set; }
        public int MasterAssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public int AssetDetailId { get; set; }
        public string SerialNumber { get; set; }
        public int RequestPeriorityId { get; set; }
        public string PeriorityNameAr { get; set; }
        public string PeriorityName { get; set; }
        public string CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public string RequestTypeNameAr { get; set; }
        public int RequestTrackingId { get; set; }
        public int RequestStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string UserId { get; set; }

        public int HospitalId { get; set; }
        public int SubOrganizationId { get; set; }
        public int OrganizationId { get; set; }
        public int CityId { get; set; }
        public int GovernorateId
        {
            get; set;
        }
    }
}
