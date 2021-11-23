using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.MasterAssetAttachmentVM
{
    public class CreateMasterAssetAttachmentVM
    {
        public int MasterAssetId { get; set; }

        public string Title { get; set; }
        public string FileName { get; set; }

    }
}
