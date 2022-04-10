using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class HospitalDepartment
    {

        public int Id { get; set; }

        [ForeignKey("HospitalId")]
        public int HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }

        [ForeignKey("HospitalId")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public bool IsActive { get; set; }
    }
}
