using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ScrapVM
{
    public class EditScrapVM
    {
        public int Id { get; set; }
        
        public int? AssetDetailId { get; set; }
        public string Comment { get; set; }
        public DateTime ScrapDate { get; set; }
        public DateTime SysDate { get; set; }
    }
}
