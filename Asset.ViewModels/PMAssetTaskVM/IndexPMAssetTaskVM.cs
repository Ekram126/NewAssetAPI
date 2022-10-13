using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PMAssetTaskVM
{
    public class IndexPMAssetTaskVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {

            public int Id { get; set; }
            public int MasterAssetId { get; set; }
            public string TaskName { get; set; }
            public string TaskNameAr { get; set; }
        }

    }
}
