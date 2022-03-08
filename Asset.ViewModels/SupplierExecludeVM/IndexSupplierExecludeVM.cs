using Asset.Models;
using System;
using System.Collections.Generic;

namespace Asset.ViewModels.SupplierExecludeVM
{
   public class IndexSupplierExecludeVM
    {

        public List<GetData> Results { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }


            public int ReasonId { get; set; }
            public string ReasonName { get; set; }
            public string ReasonNameAr { get; set; }

            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }

            public List<SupplierExecludeAttachment> Attachments { get; set; }
        }
    }
}
