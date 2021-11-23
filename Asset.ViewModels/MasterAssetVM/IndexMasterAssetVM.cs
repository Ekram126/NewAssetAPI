using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetVM
{
    public class IndexMasterAssetVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }

            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }



            public string PMColor { get; set; }
            public string PMBGColor { get; set; }



            public int? ECRIId { get; set; }
            public string ECRIName { get; set; }
            public string ECRINameAr { get; set; }
            public string Model { get; set; }
            public int? OriginId { get; set; }
            public string OriginName { get; set; }
            public string OriginNameAr { get; set; }
            public int? BrandId { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }

            public int? CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryNameAr { get; set; }

            public int? SubCategoryId { get; set; }
            public string SubCategoryName { get; set; }
            public string SubCategoryNameAr { get; set; }






        }
    }
}
