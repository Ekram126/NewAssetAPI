using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class VisitAttachment
    {
        public int Id { get; set; }
        public int? VisitId { get; set; }
        [ForeignKey("VisitId")]
        public virtual Visit Visit { get; set; }

        [StringLength(25)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string Title { get; set; }

        [NotMapped]
        public IFormFile FileToUpload { get; set; }
    }
}
