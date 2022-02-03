using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class AssetStatusService : IAssetStatusService
    {
        private IUnitOfWork _unitOfWork;

        public AssetStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateAssetStatusVM AssetStatusObj)
        {
            return _unitOfWork.AssetStatusRepository.Add(AssetStatusObj);
            //return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var AssetStatusObj = _unitOfWork.AssetStatusRepository.GetById(id);
            _unitOfWork.AssetStatusRepository.Delete(AssetStatusObj.Id);
            _unitOfWork.CommitAsync();
            return AssetStatusObj.Id;
        }

        public IEnumerable<IndexAssetStatusVM.GetData> GetAll()
        {
            return _unitOfWork.AssetStatusRepository.GetAll();
        }

        public IEnumerable<AssetStatu> GetAllAssetStatus()
        {
            return _unitOfWork.AssetStatusRepository.GetAllAssetStatus();
        }

        public EditAssetStatusVM GetById(int id)
        {
            return _unitOfWork.AssetStatusRepository.GetById(id);
        }

        public IEnumerable<IndexAssetStatusVM.GetData> GetAssetStatusByName(string AssetStatusName)
        {
            return _unitOfWork.AssetStatusRepository.GetAssetStatusByName(AssetStatusName);
        }

        public int Update(EditAssetStatusVM AssetStatusObj)
        {
            _unitOfWork.AssetStatusRepository.Update(AssetStatusObj);
            _unitOfWork.CommitAsync();
            return AssetStatusObj.Id;
        }

        public IEnumerable<IndexAssetStatusVM.GetData> SortAssetStatuses(SortAssetStatusVM sortObj)
        {
            return _unitOfWork.AssetStatusRepository.SortAssetStatuses(sortObj);
        }

        public IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusCount()
        {
            return _unitOfWork.AssetStatusRepository.GetAllAssetsGroupByStatusCount();
        }
    }
}
