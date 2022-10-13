using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WNPMAssetTimes
{
    public class SortWNPMAssetTimeVM
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int AssetId { get; set; }
        public string UserId { get; set; }
        public string BarCode { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SortStatus { get; set; }

        public string StrBarCode { get; set; }
        public string StrSerialNumber { get; set; }
        public string StrModelNumber { get; set; }


        public string PMDate { get; set; }
        public string DoneDate { get; set; }
        public string DueDate { get; set; }
        public string IsDone { get; set; }

    }
}
