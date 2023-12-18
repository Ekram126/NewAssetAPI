using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailVM
{
    public class SortAndFilterVM
    {
        public Sort SortObj { get; set; }
        public SearchAssetDetailVM SearchObj { get; set; }
        public bool IsSearchAndSort { get; set; }
    }
}
