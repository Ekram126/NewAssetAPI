using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.AssetPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class AssetPeriorityService : IAssetPeriorityService
    {
        private IUnitOfWork _unitOfWork;

        public AssetPeriorityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public int Add(CreateAssetPeriorityVM assetPeriorityObj)
        {
            _unitOfWork.AssetPeriorityRepository.Add(assetPeriorityObj);
            return _unitOfWork.CommitAsync();
        }

        public int Delete(int id)
        {
            var assetPeriorityObj = _unitOfWork.AssetPeriorityRepository.GetById(id);
            _unitOfWork.AssetPeriorityRepository.Delete(assetPeriorityObj.Id);
            _unitOfWork.CommitAsync();
            return assetPeriorityObj.Id;
        }

        public IEnumerable<IndexAssetPeriorityVM.GetData> GetAll()
        {
            return _unitOfWork.AssetPeriorityRepository.GetAll();
        }

        public EditAssetPeriorityVM GetById(int id)
        {
            return _unitOfWork.AssetPeriorityRepository.GetById(id);
        }

        public int Update(EditAssetPeriorityVM assetPeriorityObj)
        {
            _unitOfWork.AssetPeriorityRepository.Update(assetPeriorityObj);
            _unitOfWork.CommitAsync();
            return assetPeriorityObj.Id;
        }
    }
}
