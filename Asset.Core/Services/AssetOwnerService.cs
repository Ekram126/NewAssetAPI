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
    public class AssetOwnerService : IAssetOwnerService
    {
        private IUnitOfWork _unitOfWork;

        public AssetOwnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     
        public int Delete(int id)
        {
            //var AssetStatusObj = _unitOfWork.AssetOwnerRepository.GetById(id);
            //_unitOfWork.AssetOwnerRepository.Delete(AssetStatusObj.Id);
            //_unitOfWork.CommitAsync();
            return 0;
        }
        public IEnumerable<AssetOwner> GetAll()
        {
            return _unitOfWork.AssetOwnerRepository.GetAll();
        }
        public List<AssetOwner> GetOwnersByAssetDetailId(int assetDetailId)
        {
            return _unitOfWork.AssetOwnerRepository.GetOwnersByAssetDetailId(assetDetailId);
        }
    }
}
