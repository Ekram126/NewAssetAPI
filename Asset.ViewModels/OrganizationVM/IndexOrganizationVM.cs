using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.OrganizationVM
{
    public class IndexOrganizationVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Mobile { get; set; }
            public string NameAr { get; set; }
        }
    }
}
