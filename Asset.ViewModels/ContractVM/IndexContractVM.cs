using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
   public class IndexContractVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string AssetName { get; set; }  
            public string AssetNameAr { get; set; }
            public string ResponseTime { get; set; }
            public string HasSpareParts { get; set; }
            public int? MasterContractId { get; set; }

            public int? AssetDetailId { get; set; }


            public DateTime? ContractDate { get; set; }

       

        }
    }
}
