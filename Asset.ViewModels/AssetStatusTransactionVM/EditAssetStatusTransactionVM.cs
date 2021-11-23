using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStatusTransactionVM
{
    public class EditAssetStatusTransactionVM
    {

        public int Id { get; set; }
        public int AssetDetailId { get; set; }

        public int AssetStatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}
