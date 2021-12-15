using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int? MasterAssetId { get; set; }
        [ForeignKey("MasterAssetId")]
        public virtual MasterAsset MasterAsset { get; set; }

    }
}
