using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.HospitalApplicationVM
{
    public class CreateHospitalApplicationVM
    {

        public int Id { get; set; }
        public int? AssetId { get; set; }
        public int? StatusId { get; set; }
        public int? AppTypeId { get; set; }
        public int? HospitalId { get; set; }
        public string UserId { get; set; }
        public DateTime? AppDate { get; set; }
        public string DueDate { get; set; }
        public string AppNumber { get; set; }
        public string Comment { get; set; }
        public List<int> ReasonIds { get; set; }

        public string ActionDate { get; set; }

    }
}
