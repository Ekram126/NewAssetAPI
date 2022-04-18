using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class PMAssetTime
    {

        public int Id { get; set; }

        public DateTime? PMDate { get; set; }

        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
