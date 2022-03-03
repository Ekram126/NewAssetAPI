using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using Asset.ViewModels.HospitalReasonTransactionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class HospitalReasonTransactionService : IHospitalReasonTransactionService
    {
        private IUnitOfWork _unitOfWork;

        public HospitalReasonTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateHospitalReasonTransactionVM reasonTransaction)
        {
            return _unitOfWork.HospitalReasonTransactionRepository.Add(reasonTransaction);
        }
        public int Delete(int id)
        {
            var HospitalApplicationObj = _unitOfWork.HospitalReasonTransactionRepository.GetById(id);
            _unitOfWork.HospitalApplicationRepository.Delete(HospitalApplicationObj.Id);
            _unitOfWork.CommitAsync();
            return HospitalApplicationObj.Id;
        }
        public IEnumerable<HospitalReasonTransaction> GetAll()
        {
            return _unitOfWork.HospitalReasonTransactionRepository.GetAll();
        }

        public List<IndexHospitalReasonTransactionVM.GetData> GetAttachmentByHospitalApplicationId(int transId)
        {
            return _unitOfWork.HospitalReasonTransactionRepository.GetAttachmentByHospitalApplicationId(transId);
        }

        public HospitalReasonTransaction GetById(int id)
        {
            return _unitOfWork.HospitalReasonTransactionRepository.GetById(id);
        }
        public int Update(HospitalReasonTransaction reasonTransaction)
        {
            return _unitOfWork.HospitalReasonTransactionRepository.Update(reasonTransaction);
        }
    }
}
