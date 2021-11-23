using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
    public class IndexMasterContractVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public int? SupplierId { get; set; }
            public int? HospitalId { get; set; }
            public string ContractNumber { get; set; }
            public string Subject { get; set; }
            public string ContractDate { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }

            public string Cost { get; set; }

            public string SupplierName { get; set; }
            public string SupplierNameAr { get; set; }
        }
    }
}
