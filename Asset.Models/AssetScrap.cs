using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset.Models
{
   public class AssetScrap
    {
        public int Id { get; set; }
        public int? ScrapId { get; set; }
        [ForeignKey("ScrapId")]
        public virtual Scrap Scrap { get; set; }
        public int? ScrapReasonId { get; set; }
        [ForeignKey("ScrapReasonId")]
        public virtual ScrapReason ScrapReason { get; set; }
    }
}
