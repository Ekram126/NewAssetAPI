using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalApplicationVM
{
   public class ViewHospitalApplicationVM
    {


        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public int? AppTypeId { get; set; }
        public string UserId { get; set; }
        public DateTime? AppDate { get; set; }
        public string DueDate { get; set; }
        public string AppNumber { get; set; }
        public string Comment { get; set; }

        public List<HospitalExecludeReason> ReasonNames { get; set; }
        public List<HospitalHoldReason> HoldReasonNames { get; set; }

    

        public int HospitalId { get; set; }

        public string assetName { get; set; }
        public string assetNameAr { get; set; }

        public string AppTypeName { get; set; }
        public string AppTypeNameAr { get; set; }
        public string BarCode { get; set; }
        public string SerialNumber { get; set; }

    }
}
