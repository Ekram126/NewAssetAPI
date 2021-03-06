using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class AssetMovementService : IAssetMovementService
    {

        private IUnitOfWork _unitOfWork;

        public AssetMovementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateAssetMovementVM AssetMovementVM)
        {
            _unitOfWork.AssetMovementRepository.Add(AssetMovementVM);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var AssetMovementObj = _unitOfWork.AssetMovementRepository.GetById(id);
            _unitOfWork.AssetMovementRepository.Delete(AssetMovementObj.Id);
            _unitOfWork.CommitAsync();

            return AssetMovementObj.Id;
        }

        public IEnumerable<IndexAssetMovementVM.GetData> GetAll()
        {
            return _unitOfWork.AssetMovementRepository.GetAll();
        }

        public IEnumerable<AssetMovement> GetAllAssetMovements()
        {
            return _unitOfWork.AssetMovementRepository.GetAllAssetMovements();
        }

        public AssetMovement GetById(int id)
        {
            return _unitOfWork.AssetMovementRepository.GetById(id);
        }

        public IEnumerable<IndexAssetMovementVM.GetData> GetMovementByAssetDetailId(int assetId)
        {
            return _unitOfWork.AssetMovementRepository.GetMovementByAssetDetailId(assetId);
        }

        public int Update(EditAssetMovementVM AssetMovementVM)
        {
            _unitOfWork.AssetMovementRepository.Update(AssetMovementVM);
            _unitOfWork.CommitAsync();
            return AssetMovementVM.Id;
        }
    }
}
