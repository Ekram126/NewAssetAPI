using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.RequestDocumentVM
{
    public class IndexRequestDocument
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public int RequestTrackingId { get; set; }
        public string Subject { get; set; }

    }
}
