using Asset.ViewModels.RequestTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class OpenRequestVM
    {

        public List<GetData> Results { get; set; }

        public int Count { get; set; }
        public string Lang { get; set; }
        public string PrintedBy { get; set; }

        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }


        public string HospitalNameAr { get; set; }
        public string HospitalName { get; set; }



        public class GetData
        {
            public int Id { get; set; }
            public DateTime RequestDate { get; set; }
            public string RequestCode { get; set; }
            public string CreatedById { get; set; }
            public string CreatedBy { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }

            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }

            public string Barcode { get; set; }
            public string SerialNumber { get; set; }
            public int? AssetDetailId { get; set; }
            public int? HospitalId { get; set; }
            public string ModelNumber { get; set; }
            public decimal? FixCost { get; set; }
            public decimal? CostPerDay { get; set; }
            public double? AllDays { get; set; }
            public DateTime? LastWOFixedDate { get; set; }
            public decimal? TotalCost { get; set; }
            public int RequestStatusId { get; set; }
            public string RequestStatusName { get; set; }
            public string RequestStatusNameAr { get; set; }
            public string RequestStatusColor { get; set; }
            public string RequestStatusIcon { get; set; }


            public int WorkOrderStatusId { get; set; }
            public string WorkOrderStatusName { get; set; }
            public string WorkOrderStatusNameAr { get; set; }
            public string WorkOrderStatusColor { get; set; }
            public string WorkOrderStatusIcon { get; set; }
            public string BGColor { get; set; }

            public int? GovernorateId { get; set; }
            public int? CityId { get; set; }
            public int? OrganizationId { get; set; }
            public int? SubOrganizationId { get; set; }



            public DateTime? AppDate { get; set; }


        }

    }
}
