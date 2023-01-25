using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class SearchOpenRequestVM
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public int? HospitalId { get; set; }
        public string UserId { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }

        public string Logo { get; set; }
        public string Lang { get; set; }


        public string HospitalNameAr { get; set; }
        public string HospitalName { get; set; }


        public string PrintedBy { get; set; }

        public int? AppTypeId { get; set; }
        public int? StatusId { get; set; }

    }
}
