using Asset.ViewModels.PMAssetTaskScheduleVM;
using Asset.ViewModels.PMAssetTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PmAssetTimeVM
{
    public class PmAssetTimeVM
    {
        public int Id { get; set; }
        public int? AssetDetailId { get; set; }
        public DateTime? PMDate { get; set; }

        public string Code { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? Price { get; set; }

        public string SerialNumber { get; set; }
    }
    public class PmDateGroupVM
    {
        public int Id { get; set; }
        public int AssetDetailId { get; set; }
        public int MasterAssetId { get; set; }

        public int PMAssetTimeId { get; set; }
        public DateTime? PMDate { get; set; }
        public List<ListPMAssetTaskScheduleVM.GetData> AssetSchduleList { get; set; }
        //   public List<CreatePMAssetTaskVM> AssetTasksList { get; set; }
    }
}
