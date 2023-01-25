using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public int? OrganizationId { get; set; }
        public int? SubOrganizationId { get; set; }
        public int? HospitalId { get; set; }
        [StringLength(450)]
        public string RoleId { get; set; }
        public int? RoleCategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? CommetieeMemberId { get; set; }


        [NotMapped]
        public List<string> RoleIds { get; set; }

        [NotMapped]
        public List<ApplicationRole> userRoleIds { get; set; }
    }
}
