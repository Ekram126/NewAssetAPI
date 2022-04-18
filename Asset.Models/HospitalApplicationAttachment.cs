using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class HospitalApplicationAttachment
    {

        public int Id { get; set; }

        public int? HospitalReasonTransactionId { get; set; }
        [ForeignKey("HospitalReasonTransactionId")]
        public virtual HospitalReasonTransaction HospitalReasonTransaction { get; set; }


        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
