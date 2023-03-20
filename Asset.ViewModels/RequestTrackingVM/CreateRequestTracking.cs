using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestTrackingVM
{
    public class CreateRequestTracking
    {
        public int Id { get; set; }
        public string Description { get; set; }
  public string StrDescriptionDate { get; set; }

        public DateTime DescriptionDate { get; set; }
        public int RequestStatusId { get; set; }
        public int RequestId { get; set; }
        public string CreatedById { get; set; }
        public int HospitalId { get; set; }
    }
}
