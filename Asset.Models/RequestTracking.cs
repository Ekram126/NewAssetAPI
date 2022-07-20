using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class RequestTracking
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? DescriptionDate { get; set; }
        public int? RequestStatusId { get; set; }
        [ForeignKey("RequestStatusId")]
        public virtual RequestStatus RequestStatus { get; set; }
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser User { get; set; }

        public bool? IsOpened { get; set; }


        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
