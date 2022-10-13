using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PMAssetTaskScheduleVM
{
    public class ListPMAssetTaskScheduleVM
    {
        public List<GetData> Results { get; set; }
        public class GetData
        {
            public int Id { get; set; }
            public int PMAssetTimeId { get; set; }
            public int PMAssetTaskId { get; set; }
            public bool Checked { get; set; }
            public int MasterAssetId { get; set; }
            public string TaskName { get; set; }
            public string TaskNameAr { get; set; }

        }
    }
}
