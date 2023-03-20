using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.StockTakingScheduleVM
{
    public class SortStockTakingScheduleVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string STCode { get; set; }
        public string SortStatus { get; set; }
    }
}
