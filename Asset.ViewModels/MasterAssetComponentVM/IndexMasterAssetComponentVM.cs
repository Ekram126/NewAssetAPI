using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetComponentVM
{
    public class IndexMasterAssetComponentVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }
            public string Price { get; set; }
            public string PartNo { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }

        }
    }
}
