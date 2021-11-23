using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetDetailAttachment
    {
        public int Id { get; set; }
        public int? AssetDetailId { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string Title { get; set; }
    }
}
