using Asset.Domain;
using Asset.Domain.Services;
using Asset.ViewModels.AssetWorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class AssetWorkOrderTaskService : IAssetWorkOrderTaskService
    {
        private IUnitOfWork _unitOfWork;

        public AssetWorkOrderTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddAssetWorkOrderTask(CreateAssetWorkOrderTaskVM createAssetWorkOrderTaskVM)
        {
            _unitOfWork.AssetWorkOrderTask.Add(createAssetWorkOrderTaskVM);
        }

        public void DeleteAssetWorkOrderTask(int id)
        {
            _unitOfWork.AssetWorkOrderTask.Delete(id);
        }

        public IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasks()
        {
            return _unitOfWork.AssetWorkOrderTask.GetAll();
        }

        public IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasksByMasterAssetId(int MasterAssetId)
        {
            return _unitOfWork.AssetWorkOrderTask.GetAllAssetWorkOrderTasksByMasterAssetId(MasterAssetId);
        }

        public IndexAssetWorkOrderTaskVM GetAssetWorkOrderTaskById(int id)
        {
            return _unitOfWork.AssetWorkOrderTask.GetById(id);
        }

        public void UpdateAssetWorkOrderTask(int id, EditAssetWorkOrderTaskVM editAssetWorkOrderTaskVM)
        {
            _unitOfWork.AssetWorkOrderTask.Update(id, editAssetWorkOrderTaskVM);
        }
    }
}
