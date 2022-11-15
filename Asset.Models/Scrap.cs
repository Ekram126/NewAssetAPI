using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Asset.Models
{
    public class Scrap
    {
        public int Id { get; set; }
        public string ScrapNo { get; set; }
        public int? AssetDetailId { get; set; }
        [ForeignKey("AssetDetailId")]
        public virtual AssetDetail AssetDetail { get; set; }

        public string Comment { get; set; }
        public List<ScrapAttachment> ListAttachments { get; set; }
        //public List<ScrapReason> ListReasons { get; set; }
        //public virtual ScrapReason ScrapReason { get; set; }
        public DateTime? ScrapDate { get; set; }
        public DateTime? SysDate { get; set; }


    }
}
