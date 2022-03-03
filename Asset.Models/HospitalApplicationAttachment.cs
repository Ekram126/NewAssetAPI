using System;
using System.Collections.Generic;
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



        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
