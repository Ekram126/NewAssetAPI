using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class CommetieeMember
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }

        public string Mobile { get; set; }

        [StringLength(2083)]
        public string Website { get; set; }

        [StringLength(320)]
        public string EMail { get; set; }

        public string Address { get; set; }
        public string AddressAr { get; set; }
    }
}
