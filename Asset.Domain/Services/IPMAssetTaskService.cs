using Asset.Models;
using Asset.ViewModels.PMAssetTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IPMAssetTaskService
    {
        IEnumerable<PMAssetTask> GetAll();

        IEnumerable<PMAssetTask> GetPMAssetTaskByMasterAssetId(int masterAssetId);
        PMAssetTask GetPMAssetTaskByTaskIdAndMasterAssetId(int masterAssetId, int taskId);

        PMAssetTask GetById(int id);
        int Add(CreatePMAssetTaskVM taskObj);
        int Update(PMAssetTask taskObj);
        int Delete(int id);
    }
}
