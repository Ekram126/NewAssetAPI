using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeReasonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SupplierExecludeReasonService : ISupplierExecludeReasonService
    {
        private IUnitOfWork _unitOfWork;

        public SupplierExecludeReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateSupplierExcludeReasonVM SupplierExecludeReasonObj)
        {
            _unitOfWork.SupplierExecludeReasonRepository.Add(SupplierExecludeReasonObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var SupplierExecludeReasonObj = _unitOfWork.SupplierExecludeReasonRepository.GetById(id);
            _unitOfWork.SupplierExecludeReasonRepository.Delete(SupplierExecludeReasonObj.Id);
            _unitOfWork.CommitAsync();
            return SupplierExecludeReasonObj.Id;
        }

        public IEnumerable<IndexSupplierExcludeReasonVM.GetData> GetAll()
        {
            return _unitOfWork.SupplierExecludeReasonRepository.GetAll();
        }

      

        public EditSupplierExcludeReasonVM GetById(int id)
        {
            return _unitOfWork.SupplierExecludeReasonRepository.GetById(id);
        }

        

        public int Update(EditSupplierExcludeReasonVM SupplierExecludeReasonObj)
        {
            _unitOfWork.SupplierExecludeReasonRepository.Update(SupplierExecludeReasonObj);
            _unitOfWork.CommitAsync();
            return SupplierExecludeReasonObj.Id;
        }
    }
}
