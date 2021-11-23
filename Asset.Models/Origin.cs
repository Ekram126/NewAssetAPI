using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
  public  class Origin
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
