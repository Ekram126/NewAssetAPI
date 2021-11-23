using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ProblemVM
{
    public class IndexProblemVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int MasterAssetId { get; set; }
        public string MasterAssetName { get; set; }
    }
}
