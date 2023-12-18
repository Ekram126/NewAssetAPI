using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class SupplierExecludeAsset
    {
        public int Id { get; set; }

        public int? AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual HospitalSupplierStatus HospitalSupplierStatus { get; set; }

        public int? AppTypeId { get; set; }
        [ForeignKey("AppTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual ApplicationUser MemberUser { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? ExecludeDate { get; set; }
        public DateTime? ActionDate { get; set; }

        [StringLength(50)]
        public string ExNumber { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
