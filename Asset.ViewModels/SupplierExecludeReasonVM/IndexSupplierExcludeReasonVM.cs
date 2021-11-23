using System;
using System.Collections.Generic;

namespace Asset.ViewModels.SupplierExecludeReasonVM
{
   public class IndexSupplierExcludeReasonVM
    {

        public List<GetData> Results { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }
        }
    }
}
