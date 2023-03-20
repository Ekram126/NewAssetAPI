using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetStockTaking
    {
        public int Id { get; set; }
        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        public DateTime? CaptureDate { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }
        public int? STSchedulesId { get; set; }
        [ForeignKey("STSchedulesId")]
        public virtual StockTakingSchedule StockTakingSchedule { get; set; }
        //public string STCode { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Longtitude { get; set; }


    }
}
