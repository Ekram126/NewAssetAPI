using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OriginVM;
using Asset.ViewModels.WNPMAssetTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Core.Services
{
    public class WNPMAssetTimeService : IWNPMAssetTimeService
    {
        private IUnitOfWork _unitOfWork;

        public WNPMAssetTimeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(WNPMAssetTime timeObj)
        {
            return _unitOfWork.WNPMAssetTimeRepository.Add(timeObj);
        }

        public int Delete(int id)
        {
            return _unitOfWork.WNPMAssetTimeRepository.Delete(id);
        }

        public IndexWNPMAssetTimesVM GetAll(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAll(filterObj,pageNumber, pageSize, userId);
        }

        public List<CalendarWNPMAssetTimeVM> GetAll(int hospitalId, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAll(hospitalId, userId);
        }

        public IndexWNPMAssetTimesVM GetAllAssetTimesIsDone(bool? isDone, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAllAssetTimesIsDone(isDone, pageNumber, pageSize, userId);
        }

        public ViewWNPMAssetTimeVM GetAssetTimeById(int id)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAssetTimeById(id);
        }

        public WNPMAssetTime GetById(int id)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetById(id);
        }

        public IEnumerable<WNPMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetDateByAssetDetailId(assetDetailId);
        }

        public IndexWNPMAssetTimesVM SearchAssetTimes(SearchAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.SearchAssetTimes(searchObj, pageNumber, pageSize, userId);
        }
        public IndexWNPMAssetTimesVM SortAssetTimes(SortWNPMAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.SortAssetTimes(searchObj, pageNumber, pageSize, userId);
        }
        public int Update(WNPMAssetTime timeObj)
        {
            return _unitOfWork.WNPMAssetTimeRepository.Update(timeObj);
        }


        public int CreateAssetTimes(int year, int hospitalId)
        {
           

            return _unitOfWork.WNPMAssetTimeRepository.CreateAssetTimes(year, hospitalId);
        }

        public List<WNPMAssetTime> GetAllWNPMAssetTime()
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAllWNPMAssetTime();
           
        }

        public int CreateWNPMAssetTimeAttachment(WNPMAssetTimeAttachment attachObj)
        {
            return _unitOfWork.WNPMAssetTimeRepository.CreateWNPMAssetTimeAttachment(attachObj);
        }

        public List<WNPMAssetTimeAttachment> GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(int WNPMAssetTimeId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(WNPMAssetTimeId);
           
        }

        public int CreateAssetFiscalTimes(int year, int hospitalId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.CreateAssetFiscalTimes(year, hospitalId);
        }

        public IndexWNPMAssetTimesVM GetAllWithDate(WNPDateVM filterObj, int pageNumber, int pageSize, string userId)
        {
            return _unitOfWork.WNPMAssetTimeRepository.GetAllWithDate(filterObj, pageNumber, pageSize, userId);
        }
    }
}
