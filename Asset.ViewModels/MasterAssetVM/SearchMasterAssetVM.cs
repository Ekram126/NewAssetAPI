using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetVM
{
   public class SearchMasterAssetVM
    {

        public int? ECRIId { get; set; }
        public int? OriginId { get; set; }
        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }



        public string Code { get; set; }
        public string ModelNumber { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }



    }
}
