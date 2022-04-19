using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetStatusTransactionVM
{
    public class CreateAssetStatusTransactionVM
    {
        public int AssetDetailId { get; set; }

        public int AssetStatusId { get; set; }

        public string StatusDate { get; set; }

        public int? HospitalId { get; set; }
    }
}
