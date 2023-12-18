using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.VisitVM
{
    public class CreateVisitVM
    {
        public int Id { get; set; }

        //public int? EngineerId { get; set; }

        public int? HospitalId { get; set; }

        public DateTime VisitDate { get; set; }

        public int? VisitTypeId { get; set; }

        public string VisitDescr { get; set; }

        public int? StatusId { get; set; }

        public string Code { get; set; }

        public string UserId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public bool? IsMode { get; set; }

        public List<VisitAttachment> ListAttachments { get; set; }


    }
}
