using Asset.ViewModels.HospitalExecludeReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IHospitalExecludeReasonRepository
    {
        IEnumerable<IndexHospitalExecludeReasonVM.GetData> GetAll();
        EditHospitalExecludeReasonVM GetById(int id);
        int Add(CreateHospitalExecludeReasonVM reasonObj);
        int Update(EditHospitalExecludeReasonVM reasonObj);
        int Delete(int id);
    }
}
