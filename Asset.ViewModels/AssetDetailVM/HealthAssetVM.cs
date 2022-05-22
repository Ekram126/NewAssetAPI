using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class HealthAssetVM
    {
        public string HospitalArName { get; set; }
        public string HospitalEngName { get; set; }
        public int? HospitalId { get; set; }
        public string DepartmentArName { get; set; }
        public string DeviceInternData { get; set; }
        public string PurchaseDate { get; set; }
        public string DeviceArName { get; set; }
        public string DeviceEngName { get; set; }
        public int DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string DevicePrice { get; set; }
    }
}
