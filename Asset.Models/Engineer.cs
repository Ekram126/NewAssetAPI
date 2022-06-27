using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class Engineer
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
        public string EngImg { get; set; }

        [StringLength(320)]
        public string Email { get; set; }
        public string Address { get; set; }

        public string AddressAr { get; set; }
        public int? GenderId { get; set; }

    }
}
