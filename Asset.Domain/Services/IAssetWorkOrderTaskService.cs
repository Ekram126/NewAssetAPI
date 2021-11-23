using Asset.ViewModels.AssetWorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IAssetWorkOrderTaskService
    {
        IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasks();
        IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasksByMasterAssetId(int MasterAssetId);
        IndexAssetWorkOrderTaskVM GetAssetWorkOrderTaskById(int id);
        void AddAssetWorkOrderTask(CreateAssetWorkOrderTaskVM createAssetWorkOrderTaskVM);
        void UpdateAssetWorkOrderTask(int id, EditAssetWorkOrderTaskVM editAssetWorkOrderTaskVM);
        void DeleteAssetWorkOrderTask(int id);
    }
}
