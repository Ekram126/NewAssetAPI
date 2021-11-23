using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
   public class CountAssetVM
    {
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public decimal AssetPrice { get; set; }
        public int CountAssetsByHospital { get; set; }

    }
}
