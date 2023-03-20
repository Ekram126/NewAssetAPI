using Org.BouncyCastle.Utilities.IO.Pem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ManufacturerPMAssetVM
{
    public class IndexUnScheduledManfacturerPMAssetVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {
            public int AssetDetailId { get; set; }
            public string UnscheduledReason { get; set; }
            public string UnscheduledReasonAR { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAR { get; set; }
            public string BrandCode { get; set; }
            public string ModelNumber { get; set; }
            public string SerialNumber { get; set; }
            public string Barcode { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAR { get; set; }
        }
    }
}
