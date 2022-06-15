using Asset.ViewModels.EmployeeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class SearchRequestDateVM
    {

        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }

        public string Lang { get; set; }

    }
}
