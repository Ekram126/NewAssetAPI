using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ManufacturerPMAssetVM
{
    public class SearchManfacturerAssetTimeVM
    {

        public string BarCode { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string UserId { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? HospitalId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BrandId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? PMDate { get; set; }

    }
}
