using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class PMAssetTaskSchedule
    {
        public int Id { get; set; }

        public int? PMAssetTimeId { get; set; }
        [ForeignKey("PMAssetTimeId")]
        public virtual PMAssetTime PMAssetTime { get; set; }


        public int? PMAssetTaskId { get; set; }
        [ForeignKey("PMAssetTaskId")]
        public virtual PMAssetTask PMAssetTask { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

    }
}
