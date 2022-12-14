using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixFileVM
{
    public class IndexExternalFixFileVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string FileName { get; set; }
            public int? ExternalFixId { get; set; }

            public int? HospitalId { get; set; }





        }
    }
}
