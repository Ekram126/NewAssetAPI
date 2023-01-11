using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.StockTakingHospitalVM
{
    public class IndexStockTakingHospitalVM
    {
        public List<GetData> Results;
        public int Count;
        public class GetData
        {
            public int Id { get; set; }
            public int? HospitalId { get; set; }

            public int? STSchedulesId { get; set; }
        }
    }
}
