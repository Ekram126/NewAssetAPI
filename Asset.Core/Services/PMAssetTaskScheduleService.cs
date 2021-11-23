using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
  public class PMAssetTaskScheduleService: IPMAssetTaskScheduleService
    {
        private IUnitOfWork _unitOfWork;

        public PMAssetTaskScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public int Add(PMAssetTaskSchedule PMAssetTaskScheduleObj)
        {
            _unitOfWork.pMAssetTaskScheduleRepository.Add(PMAssetTaskScheduleObj);
            return _unitOfWork.CommitAsync();
        }

        public List<PMAssetTaskSchedule> GetAll()
        {
            return _unitOfWork.pMAssetTaskScheduleRepository.GetAll();
        }
    }
}
