using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class RequestDocument
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public int RequestTrackingId { get; set; }
        [ForeignKey("RequestTrackingId")]
        public virtual RequestTracking RequestTracking { get; set; }
    }
}
