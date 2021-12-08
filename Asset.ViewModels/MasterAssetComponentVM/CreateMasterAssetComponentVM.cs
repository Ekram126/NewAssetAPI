using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetComponentVM
{
    public class CreateMasterAssetComponentVM
    {
        public int Id { get; set; }
        public string CompName { get; set; }
        public string CompNameAr { get; set; }
        public string CompCode { get; set; }
        public string PartNo { get; set; }
        public string CompDescription { get; set; }
        public string CompDescriptionAr { get; set; }
        public int? MasterAssetId { get; set; }
        public decimal? Price { get; set; }
    }
}
