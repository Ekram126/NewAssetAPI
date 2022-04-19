using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStatusTransactionVM
{
    public class IndexAssetStatusTransactionVM
    {

        public List<GetData> Results { get; set; }
        public class GetData
        {
            public int Id { get; set; }
            public int AssetDetailId { get; set; }
            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }
            public string StatusDate { get; set; }
            public int HospitalId { get; set; }
        }
    }
}
