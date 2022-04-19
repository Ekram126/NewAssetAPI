using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WorkOrderAttachmentVM
{
    public class IndexWorkOrderAttachmentVM
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public int WorkOrderTrackingId { get; set; }
        public int? HospitalId { get; set; }
    }
}
