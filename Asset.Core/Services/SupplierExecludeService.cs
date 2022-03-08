using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.SupplierExecludeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SupplierExecludeService : ISupplierExecludeService
    {
        private IUnitOfWork _unitOfWork;

        public SupplierExecludeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(SupplierExeclude SupplierExecludeAssetObj)
        {
            return _unitOfWork.SupplierExecludeRepository.Add(SupplierExecludeAssetObj);
        }

          public int Delete(int id)
        {
            var SupplierExecludeAssetObj = _unitOfWork.SupplierExecludeRepository.GetById(id);
            _unitOfWork.SupplierExecludeRepository.Delete(SupplierExecludeAssetObj.Id);
            return SupplierExecludeAssetObj.Id;
        }

      
        public IEnumerable<SupplierExeclude> GetAll()
        {
            return _unitOfWork.SupplierExecludeRepository.GetAll();
        }

        public List<IndexSupplierExecludeVM.GetData> GetAttachmentBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            return _unitOfWork.SupplierExecludeRepository.GetAttachmentBySupplierExecludeAssetId(supplierExecludeAssetId);
        }

        public SupplierExeclude GetById(int id)
        {
            return _unitOfWork.SupplierExecludeRepository.GetById(id);
        }

        public IEnumerable<SupplierExeclude> GetSupplierExecludesBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            throw new NotImplementedException();
        }

        public int Update(SupplierExeclude SupplierExecludeAssetObj)
        {
            return _unitOfWork.SupplierExecludeRepository.Update(SupplierExecludeAssetObj);
        }

      
    }
}
