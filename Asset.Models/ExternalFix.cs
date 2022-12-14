using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class ExternalFix
    {
        public int Id { get; set; }
        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ComingDate { get; set; }
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }
        public string Notes { get; set; }
        public string ComingNotes { get; set; }
        public string OutNumber { get; set; }


    }
}
