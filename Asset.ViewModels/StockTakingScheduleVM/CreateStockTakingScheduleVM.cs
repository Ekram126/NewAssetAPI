using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.StockTakingScheduleVM
{
        public class CreateStockTakingScheduleVM {
        public int Id { get; set; }
        //public int? HospitalId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string UserId { get; set; }
        public List<int> ListHospitalIds { get; set; }
        public string STCode { get; set; }

    }
}
