using Asset.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.StockTakingHospitalVM
{
    public class StockTakingHospitalVM
    {
        public int Id { get; set; }
        public int? HospitalId { get; set; }
   
        public int? STSchedulesId { get; set; }
       
      

    }
}
