using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class FilterHospitalAssetAge
    {

        public int? OriginId { get; set; }
        public int? BrandId { get; set; }
        public int? SupplierId { get; set; }
        public int? HospitalId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? AssetId { get; set; }
        public string UserId { get; set; }
        public string AssetName { get; set; }
        public string Serial { get; set; }
        public string Model { get; set; }
    }
}
