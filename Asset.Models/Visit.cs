using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int? EngineerId { get; set; }
        [ForeignKey("EngineerId")]
        public virtual Engineer Engineer { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]  
        public virtual Hospital Hospital { get; set; }

        public DateTime? VisitDate { get; set; }
        public int? VisitTypeId { get; set; }
        [ForeignKey("VisitTypeId")]
        public virtual VisitType VisitType { get; set; }
        public string VisitDescr { get; set; }
        public int? StatusId { get; set; }
        public List<VisitAttachment> ListAttachments { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Longtitude { get; set; }
        public bool? IsMode { get; set; }

    }
}
