using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
    public class CreateMasterContractVM
    {

        public int Id { get; set; }
        public int? TotalVisits { get; set; }
        public int? SupplierId { get; set; }
        public int? HospitalId { get; set; }
        public string Serial { get; set; }

        public string Subject { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public decimal? Cost { get; set; }

        public string Notes { get; set; }
        public List<ContractDetail> lstDetails { get; set; }

       
    }
}
