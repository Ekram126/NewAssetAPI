using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.SupplierExecludeAssetVM
{
    public class SortSupplierExecludeAssetVM
    {
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string AppNumber { get; set; }
        public string Date { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string DueDate { get; set; }
        public string ReasonHoldTitles { get; set; }
        public string ReasonHoldTitlesAr { get; set; }
        public string SortStatus { get; set; }
        public string TypeName { get; set; }
        public string TypeNameAr { get; set; }
        public string AppDate { get; set; }
        public string ReasonExTitles { get; set; }
        public string ReasonExTitlesAr { get; set; }
        public string UserId { get; set; }
        public int HospitalId { get; set; }
        public int GovernorateId { get; set; }
        public int CityId { get; set; }
        public int OrganizationId { get; set; }
        public int SubOrganizationId { get; set; }
    }
}
