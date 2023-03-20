using Asset.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalAssetMovementVM
{
    public class IndexExternalAssetMovementVM
    {
        public List<GetData> Results { get; set; }
        public int Count { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public int AssetDetailId { get; set; }
            public DateTime? MovementDate { get; set; }
            public string HospitalName { get; set; }
            public string Notes { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string BarCode { get; set; }
            public string SerialNumber { get; set; }
            public string ModelNumber { get; set; }
        }

    }
}
