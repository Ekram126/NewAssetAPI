using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
    public class CreateSupplierExecludeAssetVM
    {

        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public int? HospitalId { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string ExecludeDate { get; set; }
        public string ExNumber { get; set; }
        public string Comment { get; set; }
        public int? AppTypeId { get; set; }
        public List<int> ReasonIds { get; set; }
        public string ActionDate { get; set; }
    }
}
