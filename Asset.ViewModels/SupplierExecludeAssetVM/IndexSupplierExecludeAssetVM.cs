﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
   public class IndexSupplierExecludeAssetVM
    {

        public List<GetData> Results { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public int? StatusId { get; set; }
            public string UserName { get; set; }
            public string Date { get; set; }
            public string ExecludeDate { get; set; }
            public string ExNumber { get; set; }



            public string ReasonExTitles { get; set; }
            public string ReasonExTitlesAr { get; set; }

            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }



      
            public int? OpenStatus { get; set; }
            public int? ApproveStatus { get; set; }
            public int? RejectStatus { get; set; }
            public int? SystemRejectStatus { get; set; }




            public int DiffMonths { get; set; }

            public bool IsMoreThan3Months { get; set; }

            
        }
    }
}
