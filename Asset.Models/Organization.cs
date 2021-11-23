using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class Organization
    {

        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }


        public string Address { get; set; }

        public string AddressAr { get; set; }

        [StringLength(320)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string DirectorName { get; set; }

        [StringLength(50)]
        public string DirectorNameAr { get; set; }

    }
}
