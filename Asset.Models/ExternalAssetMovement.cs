using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class ExternalAssetMovement
    {
        public int Id { get; set; }

        public int AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail  { get; set; }

        public DateTime? MovementDate { get; set; }


        [StringLength(500)]
        public string HospitalName { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

     

    }
}
