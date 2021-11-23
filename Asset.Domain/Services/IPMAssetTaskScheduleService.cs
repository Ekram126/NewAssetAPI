using Asset.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IPMAssetTaskScheduleService
    {
        List<PMAssetTaskSchedule> GetAll();


        int Add(PMAssetTaskSchedule pmAssetTaskScheduleObj);
    }
}
