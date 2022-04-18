using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class Employee
    {
        public int Id { get; set; } 
        
        [StringLength(5)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }

        [StringLength(14)]
        public string CardId { get; set; }

        public string Phone { get; set; } 
        [StringLength(15)]
        public string WhatsApp { get; set; }
        public DateTime? Dob { get; set; }
        public string EmpImg { get; set; }

        [StringLength(320)]
        public string Email { get; set; }
        public string Address { get; set; }

        public string AddressAr { get; set; }
        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
        public int? DepartmentId { get; set; }

        public int? ClassificationId { get; set; }

        public int? GenderId { get; set; }





    }
}
