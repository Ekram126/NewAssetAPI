using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalReasonTransactionVM
{
  public  class IndexHospitalReasonTransactionVM
    {
        public List<GetData> Results { get; set; }
        public class GetData
        {

            public int ReasonId { get; set; }
            public string ReasonName { get; set; }
            public string ReasonNameAr { get; set; }

            public List<HospitalApplicationAttachment> Attachments { get; set; }
        }
    }
}
