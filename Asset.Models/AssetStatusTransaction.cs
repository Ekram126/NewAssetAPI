using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class AssetStatusTransaction
    {
        public int Id { get; set; }




        public int AssetDetailId { get; set; }

        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public int AssetStatusId { get; set; }
        [ForeignKey("AssetStatusId")]
        public virtual AssetStatu AssetStatus { get; set; }


        public DateTime? StatusDate { get; set; }


    }
}
