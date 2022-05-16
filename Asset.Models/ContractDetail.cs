using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class ContractDetail
    {
        public int Id { get; set; }

        public int? MasterContractId { get; set; }
        [ForeignKey("MasterContractId")]
        public virtual MasterContract MasterContract { get; set; }

        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public bool? HasSpareParts { get; set; }


        [DataType(DataType.Date)]
        public DateTime? ContractDate { get; set; }

        public int? ResponseTime { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
