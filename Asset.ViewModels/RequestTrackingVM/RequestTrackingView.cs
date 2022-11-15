using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestTrackingVM
{
    public class RequestTrackingView
    {
        public int Id { get; set; }
        public int? HospitalId { get; set; }
        public int RequestId { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public DateTime? DescriptionDate { get; set; }
        public int RequestStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
        public string CreatedById { get; set; }
        public string UserName { get; set; }
    }
}
