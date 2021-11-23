using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderTaskVM
{
    public class IndexWorkOrderTaskVM
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int WorkOrderId { get; set; }
        public string WorkOrderSubject { get; set; }
        public int AssetWorkOrderTaskId { get; set; }
        public string AssetWorkOrderTaskName { get; set; }

    }
}
