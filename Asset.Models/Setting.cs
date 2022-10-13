using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
   public class Setting
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string KeyName { get; set; }
        [StringLength(100)]
        public string KeyValue{ get; set; }
        [StringLength(100)]
        public string KeyValueAr { get; set; }

    }
}
