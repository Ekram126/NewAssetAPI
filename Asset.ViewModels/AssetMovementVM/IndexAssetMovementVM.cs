using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetMovementVM
{
    public class IndexAssetMovementVM
    {

        public List<GetData> Results { get; set; }
        public int Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public int AssetDetailId { get; set; }
            public int? HospitalId { get; set; }
            public string MoveDesc { get; set; }
            public DateTime? MovementDate { get; set; }
            public string BuildingName { get; set; }
            public string BuildingNameAr { get; set; }

            public string FloorName { get; set; }
            public string FloorNameAr { get; set; }

            public string RoomName { get; set; }
            public string RoomNameAr { get; set; }


            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }

            public string ModelNumber { get; set; }
            public string BarCode { get; set; }
            public string SerialNumber { get; set; }
        }
    }
}
