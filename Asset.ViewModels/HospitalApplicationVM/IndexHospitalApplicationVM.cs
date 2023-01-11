using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalApplicationVM
{
    public class IndexHospitalApplicationVM
    {

        public List<GetData> Results { get; set; }
        public int Count { get; set; }
        public class GetData
        {
            public int Id { get; set; }
            public int? AssetId { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }

            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }


            public string BarCode { get; set; }

            public string SerialNumber { get; set; }

            public string ModelNumber { get; set; }
            public int? StatusId { get; set; }
            public string StatusName { get; set; }
            public string StatusNameAr { get; set; }
            public string StatusColor { get; set; }
            public string StatusIcon { get; set; }


            public int? AppTypeId { get; set; }

            public string TypeName { get; set; }
            public string TypeNameAr { get; set; }

            public string UserName { get; set; }
            public string Date { get; set; }
            public string DueDate { get; set; }
            public string AppNumber { get; set; }


            public string ReasonExTitles { get; set; }
            public string ReasonExTitlesAr { get; set; }


            public string ReasonHoldTitles { get; set; }
            public string ReasonHoldTitlesAr { get; set; }

            public int DiffMonths { get; set; }

            public bool IsMoreThan3Months { get; set; }
            public int? HospitalId { get; set; }


            public int GovernorateId { get; set; }
            public int CityId { get; set; }
            public int OrganizationId { get; set; }
            public int SubOrganizationId { get; set; }





            public int? OpenStatus { get; set; }
            public int? ApproveStatus { get; set; }
            public int? RejectStatus { get; set; }
            public int? SystemRejectStatus { get; set; }


            public decimal? FixCost { get; set; }
            public decimal? TotalCost { get; set; }
            public decimal? CostPerDay { get; set; }
            public double? AllDays { get; set; }
        }
    }
}
