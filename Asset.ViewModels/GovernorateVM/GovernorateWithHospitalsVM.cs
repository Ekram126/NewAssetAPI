using Asset.ViewModels.HospitalVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.GovernorateVM
{
    public class GovernorateWithHospitalsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int HospitalsCount { get; set; }
        public decimal? Population { get; set; }
        public decimal? Area { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitude { get; set; }
        public string Logo { get; set; }
    }
}
