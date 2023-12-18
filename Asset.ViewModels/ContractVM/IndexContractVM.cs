using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.ViewModels.ContractVM
{
   public class IndexContractVM
    {

        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public string ContractName { get; set; }
            public string AssetName { get; set; }  
            public string AssetNameAr { get; set; }
            public string ResponseTime { get; set; }
            public string HasSpareParts { get; set; }
            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }
            public string BarCode { get; set; }
            public string SerialNumber { get; set; }
            public int? MasterContractId { get; set; }

            public int? AssetDetailId { get; set; }

            public int? HospitalId { get; set; }
            public DateTime? ContractDate { get; set; }



            public string BrandName { get; set; }
            public string BrandNameAr { get; set; }


            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }

        }
    }
}
