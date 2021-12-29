using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalVM
{
    public class HospitalWithAssetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int AssetCount { get; set; }
        public decimal? Assetprice { get; set; }
    }
}
