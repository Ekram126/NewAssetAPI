using Asset.Models;
using Asset.ViewModels.HospitalReasonTransactionVM;
using System.Collections.Generic;

namespace Asset.Domain.Services
{
    public interface IHospitalReasonTransactionService
    {
        IEnumerable<HospitalReasonTransaction> GetAll();
        List<IndexHospitalReasonTransactionVM.GetData> GetAttachmentByHospitalApplicationId(int appId);
        HospitalReasonTransaction GetById(int id);
        int Add(CreateHospitalReasonTransactionVM reasonTransaction);
        int Update(HospitalReasonTransaction reasonTransaction);
        int Delete(int id);
    }
}
