using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class PMAssetTask
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameAr { get; set; }

        public int? MasterAssetId { get; set; }
        [ForeignKey("MasterAssetId")]
        public virtual MasterAsset MasterAsset { get; set; }

    }
}
