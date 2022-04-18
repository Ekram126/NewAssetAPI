using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public  class HospitalReasonTransaction
    {

        public int Id { get; set; }


        public int? HospitalApplicationId { get; set; }
        [ForeignKey("HospitalApplicationId")]
        public virtual HospitalApplication HospitalApplication { get; set; }

        public int? ReasonId { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
