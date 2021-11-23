using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
   public class SearchContractVM
    {

        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public string Subject { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? HospitalId { get; set; }




    }
}
