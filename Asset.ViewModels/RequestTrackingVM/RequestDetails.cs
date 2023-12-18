using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestTrackingVM
{
    public class RequestDetails
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestTime { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
        public string UserName { get; set; }

        public int RequestModeId { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }


        public int AssetDetailId { get; set; }
        public int MasterAssetId { get; set; }


        public int RequestStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }



        public string Barcode { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }

        public int RequestPeriorityId { get; set; }
        public string PeriorityName { get; set; }
        public string PeriorityNameAr { get; set; }

        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public string RequestTypeNameAr { get; set; }

        public int SubProblemId { get; set; }
        public string SubProblemName { get; set; }
        public string SubProblemNameAr { get; set; }

        public int ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string ProblemNameAr { get; set; }


        public string departmentName { get; set; }
        public string departmentNameAr { get; set; }


        public string WONotes { get; set; }

        public int? HospitalId { get; set; }

        public List<RequestTrackingView> lstTracking { get; set; }
    }
}
