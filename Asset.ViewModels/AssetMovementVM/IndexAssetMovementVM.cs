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


            public int? BrandId { get; set; }
            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }

            public int? SupplierId { get; set; }
            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }



            public int?    DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }

            public int? MasterAssetId { get; set; }

            public int? PeriorityId { get; set; }

            public int? OriginId { get; set; }


            
            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }

        }
    }
}
