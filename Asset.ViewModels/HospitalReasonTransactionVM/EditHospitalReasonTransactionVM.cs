using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalReasonTransactionVM
{
    public class EditHospitalReasonTransactionVM
    {

        public int Id { get; set; }
        public int? HospitalApplicationId { get; set; }
        public int? ReasonId { get; set; }
    }
}
