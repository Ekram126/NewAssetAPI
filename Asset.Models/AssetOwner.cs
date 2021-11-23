using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class AssetOwner
    {

        public int Id { get; set; }
        public int? AssetDetailId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
