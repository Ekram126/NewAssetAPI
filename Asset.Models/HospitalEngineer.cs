using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class HospitalEngineer
    {
        public int Id { get; set; }
        public int? EngineerId { get; set; }
        [ForeignKey("EngineerId")]
        public virtual Engineer Engineer { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
