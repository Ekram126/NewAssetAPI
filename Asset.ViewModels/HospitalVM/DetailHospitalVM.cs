using System.Collections.Generic;

namespace Asset.ViewModels.HospitalVM
{
    public class DetailHospitalVM
    {
        public int Id { get; set; }

        public string Type { get; set; }

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


        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }


        public string CityName { get; set; }
        public string CityNameAr { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationNameAr { get; set; }


        public string SubOrganizationName { get; set; }
        public string SubOrganizationNameAr { get; set; }

        public List<int> Departments { get; set; }

        public List<EnableDisableDepartment> EnableDisableDepartments { get; set; }


    }
}
