using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class CreateRequestVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string RequestCode { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        public string StrRequestDate { get; set; }
        public string RequestTime { get; set; }
        public int RequestModeId { get; set; }
        public int? SubProblemId { get; set; }
        public int AssetDetailId { get; set; }
        public string SerialNumber { get; set; }
        public int RequestPeriorityId { get; set; }
        public string CreatedById { get; set; }
        public int RequestTypeId { get; set; }
        public int HospitalId { get; set; } 
        public bool IsOpened { get; set; }
    }
}
