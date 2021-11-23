using Asset.ViewModels.HospitalHoldReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IHospitalHoldReasonRepository
    {
        IEnumerable<IndexHospitalHoldReasonVM.GetData> GetAll();
        EditHospitalHoldReasonVM GetById(int id);
        int Add(CreateHospitalHoldReasonVM reasonObj);
        int Update(EditHospitalHoldReasonVM reasonObj);
        int Delete(int id);
    }
}
