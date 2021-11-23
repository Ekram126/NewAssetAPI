using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class ContractDetail
    {
        public int Id { get; set; }
        public int? MasterContractId { get; set; }

        public int? AssetDetailId { get; set; }
        public bool? HasSpareParts { get; set; }


        [DataType(DataType.Date)]
        public DateTime? ContractDate { get; set; }

        public int? ResponseTime { get; set; }
    }
}
