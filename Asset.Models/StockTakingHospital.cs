using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Models
{
    public class StockTakingHospital
    {
        public int Id { get; set; }
        public int? HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
        public int? STSchedulesId { get; set; }
        [ForeignKey("STSchedulesId")]
        public virtual StockTakingSchedule StockTakingSchedule { get; set; }




    }
}
