using Asset.Models;
using Asset.ViewModels.OriginVM;
using Asset.ViewModels.WNPMAssetTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IWNPMAssetTimeRepository
    {
        // IndexWNPMAssetTimesVM GetAll(int yearQuarter,int pageNumber, int pageSize, string userId);
        List<WNPMAssetTime> GetAllWNPMAssetTime();

        IndexWNPMAssetTimesVM GetAll(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId);
        List<CalendarWNPMAssetTimeVM> GetAll(int hospitalId, string userId);
        IEnumerable<WNPMAssetTime> GetDateByAssetDetailId(int assetDetailId);
        IndexWNPMAssetTimesVM SearchAssetTimes(SearchAssetTimeVM searchObj, int pageNumber, int pageSize, string userId);
        IndexWNPMAssetTimesVM SortAssetTimes(SortWNPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId);
        IndexWNPMAssetTimesVM GetAllAssetTimesIsDone(bool? isDone, int pageNumber, int pageSize, string userId);
        ViewWNPMAssetTimeVM GetAssetTimeById(int id);
        WNPMAssetTime GetById(int id);
        int Add(WNPMAssetTime timeObj);
        int Update(WNPMAssetTime timeObj);
        int Delete(int id);

        public int CreateAssetTimes(int year, int hospitalId);
        int CreateWNPMAssetTimeAttachment(WNPMAssetTimeAttachment attachObj);

        List<WNPMAssetTimeAttachment> GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(int WNPMAssetTimeId);
        public int CreateAssetFiscalTimes(int year, int hospitalId);

        public IndexWNPMAssetTimesVM GetAllWithDate(WNPDateVM filterObj, int pageNumber, int pageSize, string userId);
    }
}
