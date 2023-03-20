using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.StockTakingScheduleVM
{
    public class RelatedHospital
    {
        public string Name { get; set; }
        public string NameAr { get; set; }

    }
    public class IndexStockTakingScheduleVM
    {

        public List<GetData> Results { get; set; }
         public int Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public List<RelatedHospital> RelatedHospitals { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? CreationDate { get; set; }
            public string UserName { get; set; }
            public string STCode { get; set; }

            public List<StockTakingHospital> ListHospitals { get; set; }

        }
    }
}
