using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalVM
{
    public class CreateHospitalVM
    {
     //   public int Id { get; set; }

        public string Code { get; set; }


        public string Name { get; set; }


        public string NameAr { get; set; }




        public string Email { get; set; }


        public string Mobile { get; set; }


        public string ManagerName { get; set; }

        public string ManagerNameAr { get; set; }


        public double? Latitude { get; set; }

        public double? Longtitude { get; set; }

        public string Address { get; set; }

        public string AddressAr { get; set; }


        public int? GovernorateId { get; set; }

        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }

        public List<int> Departments { get; set; }
    }
}
