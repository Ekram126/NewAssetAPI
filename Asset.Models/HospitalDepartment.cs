using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class HospitalDepartment
    {

        public int Id { get; set; }

        public int HospitalId { get; set; }
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }
    }
}
