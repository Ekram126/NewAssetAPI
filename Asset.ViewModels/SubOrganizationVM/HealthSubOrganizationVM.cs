using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SubOrganizationVM
{
    public class HealthSubOrganizationVM
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int OrganizationId { get; set; }
        public List<string> HospitaCodes { get; set; }
        public int Id { get; set; }
    }
}
