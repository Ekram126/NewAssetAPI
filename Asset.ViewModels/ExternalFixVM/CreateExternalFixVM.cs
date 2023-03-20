using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ExternalFixVM
{
        public class CreateExternalFixVM { 
        public int Id { get; set; }
        public int AssetDetailId { get; set; }

        public int AssetStatusId { get; set; }
        public DateTime OutDate { get; set; }
      
        public string ComingNotes { get; set; }
        public int HospitalId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string Notes { get; set; }
        public DateTime? ComingDate { get; set; }
        public string OutNumber { get; set; }


        public string StrOutDate { get; set; }
        public string StrExpectedDate { get; set; }

    }
}
