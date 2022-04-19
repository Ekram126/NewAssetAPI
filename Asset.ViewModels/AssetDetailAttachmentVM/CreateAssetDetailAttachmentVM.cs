using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.AssetDetailAttachmentVM
{
    public class CreateAssetDetailAttachmentVM
    {
        public int AssetDetailId { get; set; }
        public int HospitalId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
