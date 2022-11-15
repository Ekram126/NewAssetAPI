using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class MasterContract
    {
        public int Id { get; set; }

        public int? TotalVisits { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }



        [StringLength(50)]
        public string Serial { get; set; }
 

        [StringLength(100)]
        public string Subject { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ContractDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Cost { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        public string Notes { get; set; }
    }
}
