using System;
using System.Collections.Generic;

namespace Asset.ViewModels.HospitalVM
{
    public class EditHospitalVM
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string ManagerName { get; set; }

        public string ManagerNameAr { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public string Address { get; set; }

        public string AddressAr { get; set; }

        public int? GovernorateId { get; set; }

        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }

        public string ContractName { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }
        public string StrContractStart { get; set; }
        public string StrContractEnd { get; set; }



        public List<int> Departments { get; set; }



      public List<EnableDisableDepartment> EnableDisableDepartments { get; set; }
    }

    public class EnableDisableDepartment
    {
        public int? DepartmentId { get; set; }
        public bool IsActive { get; set; }

    }
}
