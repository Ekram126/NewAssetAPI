using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.ExternalAssetMovementVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class ExternalAssetMovementService : IExternalAssetMovementService
    {

        private IUnitOfWork _unitOfWork;


        public ExternalAssetMovementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(ExternalAssetMovement movementObj)
        {
            return _unitOfWork.ExternalAssetMovementRepository.Add(movementObj);
        }

        public int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj)
        {
            return _unitOfWork.ExternalAssetMovementRepository.CreateExternalAssetMovementAttachments(attachObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.ExternalAssetMovementRepository.Delete(id);
        }

        public EditExternalAssetMovementVM GetById(int id)
        {
            return _unitOfWork.ExternalAssetMovementRepository.GetById(id);
        }

        public IndexExternalAssetMovementVM GetExternalAssetMovements(int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalAssetMovementRepository.GetExternalAssetMovements(pageNumber, pageSize);
        }



        public IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId)
        {
            return _unitOfWork.ExternalAssetMovementRepository.GetExternalMovementAttachmentByExternalAssetMovementId(externalAssetMovementId);
        }

        public IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId)
        {
            return _unitOfWork.ExternalAssetMovementRepository.GetExternalMovementsByAssetDetailId(assetId);
        }

        public IndexExternalAssetMovementVM SearchExternalAssetMovement(SearchExternalAssetMovementVM searchObj, int pageNumber, int pageSize)
        {
            return _unitOfWork.ExternalAssetMovementRepository.SearchExternalAssetMovement(searchObj, pageNumber, pageSize);
        }

        public int Update(ExternalAssetMovement movementObj)
        {
            return _unitOfWork.ExternalAssetMovementRepository.Update(movementObj);
        }
    }
}
