using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalReasonTransactionVM
{
    public class CreateHospitalReasonTransactionVM
    {
        public int Id { get; set; }
        public int? HospitalApplicationId { get; set; }
        public int? ReasonId { get; set; }
        public int? HospitalId { get; set; }
        //  public List<int> ReasonIds { get; set; }
    }
}
