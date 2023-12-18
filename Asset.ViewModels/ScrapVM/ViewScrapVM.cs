using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ScrapVM
{
   public class ViewScrapVM
    {
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }
        public int Id { get; set; }
        public string ScrapDate { get; set; }
        public string ScrapNo { get; set; }
        public string ScrapReasonName { get; set; }
        public string ScrapReasonNameAr { get; set; }
        public string Model { get; set; }
        public string Comment { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandNameAr { get; set; }


        public string DepartmentName { get; set; }
        public string DepartmentNameAr { get; set; }
    }
}
