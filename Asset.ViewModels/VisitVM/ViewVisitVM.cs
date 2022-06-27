using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.VisitVM
{
    public class ViewVisitVM
    {

        public int Id { get; set; }
        public string VisitDate { get; set; }

        public string VisitDescr { get; set; }

        public int StatusId { get; set; }


        public string HospitalName { get; set; }
        public string HospitalNameAr { get; set; }
        public string EngineerName { get; set; }

        public string EngineerNameAr { get; set; }
        public string VisitTypeName { get; set; }

        public string VisitTypeNameAr { get; set; }
 public string Code { get; set; }
    }
}
