using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SubOrganizationVM
{
    public class CreateSubOrganizationVM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }


        public string Address { get; set; }

        public string AddressAr { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }

        public string DirectorName { get; set; }

        public string DirectorNameAr { get; set; }

        public int OrganizationId { get; set; }
    }
}
