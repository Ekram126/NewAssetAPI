using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.VisitVM
{
    public class SearchVisitVM
    {
       
            public int Id { get; set; }
        public int? HospitalId { get; set; }

        public int? VisitTypeId { get; set; }

        public int? EngineerId { get; set; }

        public DateTime? FromVisitDate { get; set; }

        public DateTime? ToVisitDate { get; set; }

        public int? StatusId { get; set; }
    }
}
