using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.PMAssetTaskVM
{
    public class CreatePMAssetTaskVM
    {
        public int Id { get; set; }
        public int MasterAssetId { get; set; }
        public string TaskName { get; set; }

        public string TaskNameAr { get; set; }

    }
}
