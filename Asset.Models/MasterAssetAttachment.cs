using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class MasterAssetAttachment
    {
        public int Id { get; set; }
        public int MasterAssetId { get; set; }

        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
    }
}
