using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetVM
{
    public class CountMasterAssetBrands
    {
        public int Key { get; set; }
        public int Value { get; set; }

        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }
        public int CountOfMasterAssets { get; set; }
    }


    public class CountMasterAssetSuppliers
    {
        public string SupplierName { get; set; }
        public string SupplierNameAr { get; set; }
        public int CountOfMasterAssets { get; set; }
    }
}
