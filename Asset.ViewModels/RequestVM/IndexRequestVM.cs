using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
using Asset.ViewModels.WorkOrderTrackingVM;
using Asset.ViewModels.WorkOrderVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class IndexRequestVM
    {
        //public int Id { get; set; }
        //public string RequestName { get; set; }
        //public string RequestCode { get; set; }
        //public string Description { get; set; }
        //public DateTime RequestDate { get; set; }
        //public string RequestTime { get; set; }
        //public bool? IsSolved { get; set; }
        //public bool? IsAssigned { get; set; }
        //public int RequestModeId { get; set; }
        //public string ModeName { get; set; }
        //public int SubCategoryId { get; set; }
        //public string SubCategoryName { get; set; }
        //public int AssetDetailId { get; set; }
        //public string SerialNumber { get; set; }
        //public int RequestPeriorityId { get; set; }
        //public string PeriorityName { get; set; }
        //public int EmployeeId { get; set; }
        //public string EmployeeName { get; set; }
        //public string CreatedById { get; set; }
        //public string CreatedBy { get; set; }



        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public int RequestId { get; set; }
            public string Subject { get; set; }
            public string Code { get; set; }
            public DateTime RequestDate { get; set; }
            public int ModeId { get; set; }
         
            public int AssetDetailId { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string SerialNumber { get; set; }
            public int PeriorityId { get; set; }
  
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string UserName { get; set; }
            public string CreatedById { get; set; }
            public string CreatedBy { get; set; }

            public int StatusId { get; set; }
            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }
            public string StatusColor { get; set; }


            public string PeriorityName { get; set; }
            public string PeriorityNameAr { get; set; }

            public string ModeName { get; set; }
            public string ModeNameAr { get; set; }


            public int? HospitalId { get; set; }
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }
            public string RoleId { get; set; }

            public int CountListTracks{ get; set; }


            public int CountWorkOrder { get; set; }

            //public string LatestWorkOrderStatus { get; set; }
            public int LatestWorkOrderStatusId { get; set; }

            public List<IndexRequestTrackingVM.GetData> ListTracks { get; set; }


            public List<IndexWorkOrderVM> ListWorkOrder { get; set; }

            public List<LstWorkOrderFromTracking> ListWorkOrderTracking { get; set; }
        }
    }
}
