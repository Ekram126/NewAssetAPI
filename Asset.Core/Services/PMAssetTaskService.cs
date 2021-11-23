using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.MasterAssetVM;
using Asset.ViewModels.OriginVM;
using Asset.ViewModels.PMAssetTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
   public class PMAssetTaskService : IPMAssetTaskService
    {
        private IUnitOfWork _unitOfWork;

        public PMAssetTaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(CreatePMAssetTaskVM taskObj)
        {
          return  _unitOfWork.PMAssetTaskRepository.Add(taskObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.PMAssetTaskRepository.Delete(id);
        }

        public IEnumerable<PMAssetTask> GetAll()
        {
            return _unitOfWork.PMAssetTaskRepository.GetAll();
        }

        public PMAssetTask GetById(int id)
        {
            return _unitOfWork.PMAssetTaskRepository.GetById(id);
        }

        public IEnumerable<PMAssetTask> GetPMAssetTaskByMasterAssetId(int masterAssetId)
        {
            return _unitOfWork.PMAssetTaskRepository.GetPMAssetTaskByMasterAssetId(masterAssetId);
        }

        public PMAssetTask GetPMAssetTaskByTaskIdAndMasterAssetId(int masterAssetId, int taskId)
        {
            return _unitOfWork.PMAssetTaskRepository.GetPMAssetTaskByTaskIdAndMasterAssetId(masterAssetId,taskId);
        }

        public int Update(PMAssetTask taskObj)
        {
            return _unitOfWork.PMAssetTaskRepository.Update(taskObj);
        }
    }
}
