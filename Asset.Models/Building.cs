using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class Building
    {
        public int Id { get; set; }

        [StringLength(15)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }

        [StringLength(500)]
        public string Brief { get; set; }

        [StringLength(500)]
        public string BriefAr { get; set; }

        [ForeignKey("HospitalId")]
        public int HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }

    }
}
