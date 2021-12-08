using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetComponentVM;
using Asset.ViewModels.MasterAssetVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
  public  class MasterAssetComponentService : IMasterAssetComponentService
    {
        private IUnitOfWork _unitOfWork;

        public MasterAssetComponentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreateMasterAssetComponentVM masterAssetObj)
        {
            return _unitOfWork.MasterAssetComponentRepository.Add(masterAssetObj);
        }

      
        public int Delete(int id)
        {
            var masterAssetObj = _unitOfWork.MasterAssetComponentRepository.GetById(id);
            _unitOfWork.MasterAssetComponentRepository.Delete(masterAssetObj.Id);
            _unitOfWork.CommitAsync();
            return masterAssetObj.Id;
        }

   
        public IEnumerable<IndexMasterAssetComponentVM.GetData> GetAll()
        {
            return _unitOfWork.MasterAssetComponentRepository.GetAll();
        }

     
        public EditMasterAssetComponentVM GetById(int id)
        {
            return _unitOfWork.MasterAssetComponentRepository.GetById(id);
        }

        public IEnumerable<IndexMasterAssetComponentVM.GetData> GetMasterAssetComponentByMasterAssetId(int masterAssetId)
        {
            return _unitOfWork.MasterAssetComponentRepository.GetMasterAssetComponentByMasterAssetId(masterAssetId);
        }

        public int Update(EditMasterAssetComponentVM masterAssetObj)
        {
            _unitOfWork.MasterAssetComponentRepository.Update(masterAssetObj);
            _unitOfWork.CommitAsync();
            return masterAssetObj.Id;
        }

        public ViewMasterAssetComponentVM ViewMasterAsset(int id)
        {
            return _unitOfWork.MasterAssetComponentRepository.ViewMasterAssetComponent(id);
        }

        public ViewMasterAssetComponentVM ViewMasterAssetComponent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
