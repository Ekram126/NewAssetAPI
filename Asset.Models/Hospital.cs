using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class Hospital
    {

        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameAr { get; set; }



        [StringLength(320)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string ManagerName { get; set; }

        [StringLength(50)]
        public string ManagerNameAr { get; set; }



        public float? Latitude { get; set; }

        public float? Longtitude { get; set; }

        public string Address { get; set; }

        public string AddressAr { get; set; }


        public string ContractName { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }


        public int? GovernorateId { get; set; }
        [ForeignKey("GovernorateId")]
        public virtual Governorate Governorate { get; set; }

        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }



        public int? SubOrganizationId { get; set; }
        [ForeignKey("SubOrganizationId")]
        public virtual SubOrganization SubOrganization { get; set; }
    }
}
