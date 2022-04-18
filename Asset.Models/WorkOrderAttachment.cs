using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class WorkOrderAttachment
    {
        public int Id { get; set; } 
        [StringLength(100)]
        public string DocumentName { get; set; }
        
        [StringLength(25)]
        public string FileName { get; set; }
        public int? WorkOrderTrackingId { get; set; }
        [ForeignKey("WorkOrderTrackingId")]
        public virtual WorkOrderTracking WorkOrderTracking { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
