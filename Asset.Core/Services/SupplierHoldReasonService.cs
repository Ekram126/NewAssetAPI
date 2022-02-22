using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierHoldReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SupplierHoldReasonService : ISupplierHoldReasonService
    {
        private IUnitOfWork _unitOfWork;

        public SupplierHoldReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateSupplierHoldReasonVM SupplierHoldReasonObj)
        {
            _unitOfWork.SupplierHoldReasonRepository.Add(SupplierHoldReasonObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var SupplierHoldReasonObj = _unitOfWork.SupplierHoldReasonRepository.GetById(id);
            _unitOfWork.SupplierHoldReasonRepository.Delete(SupplierHoldReasonObj.Id);
            _unitOfWork.CommitAsync();
            return SupplierHoldReasonObj.Id;
        }

        public IEnumerable<IndexSupplierHoldReasonVM.GetData> GetAll()
        {
            return _unitOfWork.SupplierHoldReasonRepository.GetAll();
        }

      

        public EditSupplierHoldReasonVM GetById(int id)
        {
            return _unitOfWork.SupplierHoldReasonRepository.GetById(id);
        }

        

        public int Update(EditSupplierHoldReasonVM SupplierHoldReasonObj)
        {
            _unitOfWork.SupplierHoldReasonRepository.Update(SupplierHoldReasonObj);
            _unitOfWork.CommitAsync();
            return SupplierHoldReasonObj.Id;
        }
    }
}
