using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RoleVM
{
    public class IndexRoleVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string CategoryName { get; set; }
        }
    }
}
