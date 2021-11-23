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
        public class GetData
        {
            public int Id { get; set; }
            public int AssetDetailId { get; set; }

            public DateTime? MovementDate { get; set; }
            public string BuildingName { get; set; }
            public string BuildingNameAr { get; set; }

            public string FloorName { get; set; }
            public string FloorNameAr { get; set; }

            public string RoomName { get; set; }
            public string RoomNameAr { get; set; }
        }
    }
}
