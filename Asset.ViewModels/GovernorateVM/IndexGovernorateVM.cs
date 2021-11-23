using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.GovernorateVM
{
    public class IndexGovernorateVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string NameAr { get; set; }

            public string Population { get; set; }

            public string Area { get; set; }
        }
    }
}
