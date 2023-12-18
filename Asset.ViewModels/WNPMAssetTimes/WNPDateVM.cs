using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.WNPMAssetTimes
{
    public class WNPDateVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Start { get; set; }
        public string End { get; set; }
    }
}
