using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class WNPMAssetTime
    {

        public int Id { get; set; }

        public DateTime? PMDate { get; set; }

        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }


        public DateTime? DoneDate { get; set; }

        public DateTime? SysDoneDate { get; set; }

        public bool? IsDone { get; set; }


        public DateTime? DueDate { get; set; }


        public string Comment { get; set; }

        public int? AgencyId { get; set; }
        [ForeignKey("AgencyId")]
        public virtual Supplier Supplier { get; set; }


        [NotMapped]
        public string strDueDate { get; set; }

        [NotMapped]
        public string strDoneDate { get; set; }
    }
}
