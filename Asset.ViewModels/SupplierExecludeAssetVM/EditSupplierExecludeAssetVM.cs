using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
   public class EditSupplierExecludeAssetVM
    {


        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ExecludeDate { get; set; }
        public string ExNumber { get; set; }


       // [NotMapped]
        public List<int> ReasonIds { get; set; }

        public string assetName { get; set; }

        public string assetNameAr { get; set; }


    }
}
