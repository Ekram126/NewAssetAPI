using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PMAssetTaskScheduleVM
{
    public class IndexPMAssetTaskScheduleVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string AssetName { get; set; }
            public string AssetNameAr { get; set; }
            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }
            public string Serial { get; set; }
            public int HospitalId { get; set; }
            public List<PMAssetTask> ListTasks { get; set; }
            public string start { get; set; }
            public string end { get; set; }


            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool allDay { get; set; }
            public string title { get; set; }
            public string titleAr { get; set; }

            public string color { get; set; }
            public string textColor { get; set; }

        }
    }



}
