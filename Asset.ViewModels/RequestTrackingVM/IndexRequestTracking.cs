using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestTrackingVM
{
    public class IndexRequestTracking
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AssetCode { get; set; }
        public string BarCode { get; set; }
        public DateTime? DescriptionDate { get; set; }
        public int RequestStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }

        public string CreatedById { get; set; }
        public string UserName { get; set; }
        public int RequestId { get; set; }
        public string RequestName { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestTime { get; set; }
        public bool? IsSolved { get; set; }
        public bool? IsAssigned { get; set; }
        public int RequestModeId { get; set; }
        public string ModeName { get; set; }
        public int? AssetDetailId { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public int RequestPeriorityId { get; set; }
        public string PeriorityName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Subject { get; set; }

        public int? HospitalId { get; set; }
        public int GovernorateId { get; set; }
        public int CityId { get; set; }
        public int OrganizationId { get; set; }
        public int SubOrganizationId { get; set; }
        public string RoleId { get; set; }

    }









    public class IndexRequestTrackingVM
    {
        public List<GetData> Results { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public DateTime? Date { get; set; }
            public int StatusId { get; set; }
            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }
            public string StatusColor{ get; set; }
            public string StatusIcon { get; set; }
            public bool isExpanded { get; set; }
            public string CreatedById { get; set; }
            public string UserName { get; set; }
            public List<RequestDocument> ListDocuments { get; set; }
        }
    }
}
