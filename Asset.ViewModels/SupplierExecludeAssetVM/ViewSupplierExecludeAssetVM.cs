using Asset.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
    public class ViewSupplierExecludeAssetVM
    {


        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ExecludeDate { get; set; }
        public string ExNumber { get; set; }
        public List<SupplierExecludeReason> ReasonNames { get; set; }
        public List<SupplierHoldReason> HoldReasonNames { get; set; }

        public string appTypeName { get; set; }
        public string appTypeNameAr { get; set; }

        public string assetName { get; set; }
        public string assetNameAr { get; set; }


        public string GovName { get; set; }
        public string GovNameAr { get; set; }




        public string CityName { get; set; }
        public string CityNameAr { get; set; }


        public string OrgName { get; set; }
        public string OrgNameAr { get; set; }

        public string SubOrgName { get; set; }
        public string SubOrgNameAr { get; set; }

        public int? HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }

        public string Comment { get; set; }

    }
}
