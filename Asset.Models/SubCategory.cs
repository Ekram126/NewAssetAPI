using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
  public  class SubCategory
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string NameAr { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}
