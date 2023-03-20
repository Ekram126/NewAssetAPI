using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalSupplierStatusVM
{
    public class IndexHospitalSupplierStatusVM
    {

        public List<HospitalSupplierStatus> ListStatus { get; set; }
        public int? OpenStatus { get; set; }
        public int? ApproveStatus { get; set; }
        public int? RejectStatus { get; set; }
        public int? SystemRejectStatus { get; set; }
        public int? TotalStatus { get; set; }
    }
}
