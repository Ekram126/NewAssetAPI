using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestVM
{
    public class SortRequestVM
    {
        public string Code { get; set; }
        public string AssetName { get; set; }
        public string AssetNameAr { get; set; }
        public string Subject { get; set; }
        public string RequestDate { get; set; }
        public string PeriorityNameAr { get; set; }
        public string PeriorityName { get; set; }
        public string StatusName { get; set; }
        public string StatusNameAr { get; set; }
        public string ModeName { get; set; }
        public string ModeNameAr { get; set; }
        public string SortStatus { get; set; }
    }
}
