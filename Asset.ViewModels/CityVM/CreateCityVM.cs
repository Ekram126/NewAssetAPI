using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.CityVM
{
    public class CreateCityVM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int GovernorateId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longtitude { get; set; }
    }
}
