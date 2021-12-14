using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.RoleCategoryVM;
using System.Collections.Generic;


namespace Asset.Core.Services
{
    public class AssetStatusTransactionService : IAssetStatusTransactionService
    {

        private IUnitOfWork _unitOfWork;

        public AssetStatusTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public int Add(CreateAssetStatusTransactionVM AssetStatusTransactionVM)
        {
          return  _unitOfWork.AssetStatusTransactionRepository.Add(AssetStatusTransactionVM);
            // _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var AssetStatusTransactionObj = _unitOfWork.AssetStatusTransactionRepository.GetById(id);
            _unitOfWork.AssetStatusTransactionRepository.Delete(AssetStatusTransactionObj.Id);
            _unitOfWork.CommitAsync();

            return AssetStatusTransactionObj.Id;
        }

        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAll()
        {
            return _unitOfWork.AssetStatusTransactionRepository.GetAll();
        }

        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAssetStatusByAssetDetailId(int assetId)
        {
            return _unitOfWork.AssetStatusTransactionRepository.GetAssetStatusByAssetDetailId(assetId);
        }

        public AssetStatusTransaction GetById(int id)
        {
            return _unitOfWork.AssetStatusTransactionRepository.GetById(id);
        }

        public List<AssetStatusTransaction> GetLastTransactionByAssetId(int assetId)
        {
            return _unitOfWork.AssetStatusTransactionRepository.GetLastTransactionByAssetId(assetId);
        }

        public int Update(AssetStatusTransaction AssetStatusTransactionVM)
        {
            _unitOfWork.AssetStatusTransactionRepository.Update(AssetStatusTransactionVM);
            _unitOfWork.CommitAsync();
            return AssetStatusTransactionVM.Id;
        }
    }
}
