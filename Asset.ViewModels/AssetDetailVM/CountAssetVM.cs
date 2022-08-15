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
        public int CountAssetsByGovernorate { get; set; }
        public int CountAssetsByCity{ get; set; }



        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }

        public string CityName { get; set; }
        public string CityNameAr { get; set; }

    }
}
