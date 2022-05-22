using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.OrganizationVM
{
    public class HealthOrganizationVM
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string HospitalCode { get; set; }
        public int Id { get; set; }
        public int subOrganizationId { get; set; }
        public List<Hospital> hospitals { get; set; }
    }
}
