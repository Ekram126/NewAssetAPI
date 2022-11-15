using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class ExternalAssetMovementAttachment
    {

        public int Id { get; set; }

        public int? ExternalAssetMovementId { get; set; }
        [ForeignKey("ExternalAssetMovementId")]
        public virtual ExternalAssetMovement ExternalAssetMovement { get; set; }


        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string Title { get; set; }

    }
}
