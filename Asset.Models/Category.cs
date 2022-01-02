using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class Category
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }



        public int? CategoryTypeId { get; set; }
        [ForeignKey("CategoryTypeId")]
        public virtual CategoryType CategoryType { get; set; }
    }
}
