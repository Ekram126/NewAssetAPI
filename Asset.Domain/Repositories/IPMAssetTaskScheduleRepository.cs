using Asset.Models;
using Asset.ViewModels.PMAssetTaskScheduleVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
   public interface IPMAssetTaskScheduleRepository
    {
        List<PMAssetTaskSchedule> GetAll();
        int Add(PMAssetTaskSchedule pmAssetTaskScheduleObj);
    }
}
