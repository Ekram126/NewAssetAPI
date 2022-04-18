using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class ContractAttachment
    {
        public int Id { get; set; }
        public int? MasterContractId { get; set; }
        [ForeignKey("MasterContractId")]
        public virtual MasterContract MasterContract { get; set; }
        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string DocumentName { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
