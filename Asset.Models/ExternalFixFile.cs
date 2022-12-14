using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class ExternalFixFile
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(25)]
        public string FileName { get; set; }
        public int? ExternalFixId { get; set; }
        [ForeignKey("ExternalFixId")]
        public virtual ExternalFix ExternalFix { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }



    }
}
