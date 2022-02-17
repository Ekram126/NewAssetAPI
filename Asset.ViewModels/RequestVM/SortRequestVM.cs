using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class SortRequestVM
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int AssetId { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string Subject { get; set; }
        public string RequestDate { get; set; }
        public string PeriorityNameAr { get; set; }
        public string PeriorityName { get; set; }
        public string PeriorityColor { get; set; }
        public string PeriorityIcon { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public string SortStatus { get; set; }
        public int CountListTracks { get; set; }
        public int CountWorkOrder { get; set; }
        public List<IndexRequestTrackingVM.GetData> ListTracks { get; set; }

       

    }
}
