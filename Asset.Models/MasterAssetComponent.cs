using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class MasterAssetComponent
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string NameAr { get; set; }
        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(20)]
        public string PartNo { get; set; }

        public string Description { get; set; }
        public string DescriptionAr { get; set; }

        public int? MasterAssetId { get; set; }
        [ForeignKey("MasterAssetId")]
        public virtual MasterAsset MasterAsset { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }


    }
}
