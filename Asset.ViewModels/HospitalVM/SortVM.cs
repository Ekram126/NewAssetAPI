﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalVM
{
    public class SortVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }

        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string GovernorateName { get; set; }
        public string GovernorateNameAr { get; set; }
        public string CityName { get; set; }
        public string CityNameAr { get; set; }
        public string OrgName { get; set; }
        public string OrgNameAr { get; set; }

        public string SubOrgName { get; set; }
        public string SubOrgNameAr { get; set; }
        public string SortStatus { get; set; }

        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
    }
}
