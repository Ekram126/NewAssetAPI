using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string RequestCode { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }
        //public bool? IsAssigned { get; set; }
        //public bool? IsSolved { get; set; }
        public int RequestModeId { get; set; }
        [ForeignKey("RequestModeId")]
        public virtual RequestMode RequestMode { get; set; }
        //public int SubCategoryId { get; set; }
        //[ForeignKey("SubCategoryId")]
        //public virtual SubCategory SubCategory { get; set; }
        public int AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }



        public int RequestPeriorityId { get; set; }
        [ForeignKey("RequestPeriorityId")]
        public virtual RequestPeriority RequestPeriority { get; set; }
        //public int EmployeeId { get; set; }
        //[ForeignKey("EmployeeId")]
        //public virtual Employee Employee { get; set; }
        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser User { get; set; }
        public int SubProblemId { get; set; }
        [ForeignKey("SubProblemId")]
        public virtual SubProblem SubProblem { get; set; }
        public int RequestTypeId { get; set; }
        [ForeignKey("RequestTypeId")]
        public virtual RequestType RequestType { get; set; }
    }
}
