using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
    public class EditSupplierExecludeAssetVM
    {


        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public string UserId { get; set; }
        public string MemberId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ExecludeDate { get; set; }
        public string ActionDate { get; set; }
        public string ExNumber { get; set; }
        public int? AppTypeId { get; set; }
        public List<int> ReasonIds { get; set; }
        public List<int> HoldReasonIds { get; set; }
        public string SerialNumber { get; set; }
        public string BarCode { get; set; }
        public string assetName { get; set; }
        public string assetNameAr { get; set; }

        public int? HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

        public string appTypeName { get; set; }
        public string appTypeNameAr { get; set; }

        public string Comment { get; set; }

    }
}
