using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ScrapVM
{
    public class CreateScrapVM
    {
        public int Id { get; set; }
        public string ScrapNo { get; set; }
        public int? AssetDetailId { get; set; }

        public string Comment { get; set; }
        public List<ScrapAttachment> ListAttachments { get; set; }

        public DateTime ScrapDate { get; set; }

        public string StrScrapDate { get; set; }
        public DateTime SysDate { get; set; }

        public int[] ReasonIds { get; set; }
    }
}
