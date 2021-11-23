using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class RequestPhase
    {
        public int Id { get; set; }
        public int PhaseId { get; set; }

        [ForeignKey("PhaseId")]
        public virtual Phase Phase { get; set; }
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
