using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class HospitalSupplierStatus
    {

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string NameAr { get; set; }
        [StringLength(50)]
        public string Color { get; set; }
        [StringLength(50)]
        public string Icon { get; set; }

    }
}
