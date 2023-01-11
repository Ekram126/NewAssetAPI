using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.UserVM
{
    public class LoggedUserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }
        public string CityName { get; set; }
        public string CityNameAr { get; set; }
        public string HospitalCode { get; set; }
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? HospitalId { get; set; }

        public int? SupplierId { get; set; }
        public int? CommetieeMemberId { get; set; }


        public string Token { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
