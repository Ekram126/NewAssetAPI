using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixVM
{
    public class EditExternalFixVM
    {
        public int Id { get; set; }
        public DateTime? ComingDate { get; set; }
        public string ComingNotes { get; set; }
        public string StrComingDate { get; set; }
    }
}
