using Asset.Models;
using Asset.ViewModels.HospitalReasonTransactionVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface IHospitalReasonTransactionRepository
    {
        IEnumerable<HospitalReasonTransaction> GetAll();
        HospitalReasonTransaction GetById(int id);
        List<IndexHospitalReasonTransactionVM.GetData> GetAttachmentByHospitalApplicationId(int transId);
        int Add(CreateHospitalReasonTransactionVM reasonTransaction);
        int Update(HospitalReasonTransaction reasonTransaction);
        int Delete(int id);
    }
}
