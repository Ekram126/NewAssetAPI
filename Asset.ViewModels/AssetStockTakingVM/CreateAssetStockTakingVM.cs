using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStockTakingVM
{
    public class CreateAssetStockTakingVM
    {
       
        public string URL { get; set; }
        public int? Id { get; set; }
        public int? HospitalId { get; set; }
        public DateTime? CaptureDate { get; set; }
        public string UserId { get; set; }
        public int? AssetDetailId { get; set; }
        public int? STSchedulesId { get; set; }
        public string STCode { get; set; }
        public decimal? Longtitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}
