using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset.Models
{
   public class ScrapAttachment
    {
        public int Id { get; set; }
        public int? ScrapId { get; set; }
        [ForeignKey("ScrapId")]
        public virtual Scrap Scrap { get; set; }

        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
    }
}
