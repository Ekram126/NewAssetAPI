using Asset.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStockTakingVM
{
    public class IndexAssetStockTakingVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {
            public string UserName { get; set; }
            public string HospitalName { get; set; }
            public string AssetName { get; set; }
            public string BarCode { get; set; }
            public string URL { get; set; }
            public int Id { get; set; }
            public int? HospitalId { get; set; }
            public DateTime? CaptureDate { get; set; }
            public string UserId { get; set; }
            public int? AssetDetailId { get; set; }
            public int? STSchedulesId { get; set; }
            public decimal? Longtitude { get; set; }
            public decimal? Latitude { get; set; }

        }
    }
}









