using System.Collections.Generic;

namespace Asset.ViewModels.HospitalVM
{
    public class IndexHospitalDepartmentVM
    {



        public List<GetData> Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }

            public string DepartmentName { get; set; }
            public string DepartmentNameAr { get; set; }

            public string HospitalName { get; set; }
            public string HospitalNameAr { get; set; }

            public int? DepartmentId { get; set; }
            public int? HospitalId { get; set; }

            public bool IsActive { get; set; }
        }

    }
}
