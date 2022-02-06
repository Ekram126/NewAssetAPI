using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeAssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class SupplierExecludeAssetService : ISupplierExecludeAssetService
    {
        private IUnitOfWork _unitOfWork;

        public SupplierExecludeAssetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateSupplierExecludeAssetVM SupplierExecludeAssetObj)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.Add(SupplierExecludeAssetObj);
           // return _unitOfWork.CommitAsync();
        }

        public int CreateSupplierExecludAttachments(SupplierExecludeAttachment attachObj)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.CreateSupplierExecludAttachments(attachObj);
        }

        public int Delete(int id)
        {
            var SupplierExecludeAssetObj = _unitOfWork.SupplierExecludeAssetRepository.GetById(id);
            _unitOfWork.SupplierExecludeAssetRepository.Delete(SupplierExecludeAssetObj.Id);
            _unitOfWork.CommitAsync();
            return SupplierExecludeAssetObj.Id;
        }

        public int DeleteSupplierExecludeAttachment(int id)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.DeleteSupplierExecludeAttachment(id);
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll()
        {
            return _unitOfWork.SupplierExecludeAssetRepository.GetAll();
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusId(int statusId)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.GetAllByStatusId(statusId);
        }

        public IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int assetId)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.GetAttachmentBySupplierExecludeAssetId(assetId);
        }

        public EditSupplierExecludeAssetVM GetById(int id)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.GetById(id);
        }

        public ViewSupplierExecludeAssetVM GetSupplierExecludeAssetDetailById(int id)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.GetSupplierExecludeAssetDetailById(id);
        }

        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> SortSuplierApp(SortSupplierExecludeAssetVM sortObj)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.SortSuplierApp(sortObj);
        }

        public int Update(EditSupplierExecludeAssetVM SupplierExecludeAssetObj)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.Update(SupplierExecludeAssetObj);
          //  _unitOfWork.CommitAsync();
          //  return SupplierExecludeAssetObj.Id;
        }

        public int UpdateExcludedDate(EditSupplierExecludeAssetVM execludeAssetObj)
        {
            return _unitOfWork.SupplierExecludeAssetRepository.UpdateExcludedDate(execludeAssetObj);
        }
    }
}
