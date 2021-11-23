using Asset.ViewModels.AssetWorkOrderTaskVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
    public interface IAssetWorkOrderTaskRepository
    {
        IEnumerable<IndexAssetWorkOrderTaskVM> GetAll();
        IEnumerable<IndexAssetWorkOrderTaskVM> GetAllAssetWorkOrderTasksByMasterAssetId(int MasterAssetId);
        IndexAssetWorkOrderTaskVM GetById(int id);
        void Add(CreateAssetWorkOrderTaskVM createAssetWorkOrderTaskVM);
        void Update(int id, EditAssetWorkOrderTaskVM editAssetWorkOrderTaskVM);
        void Delete(int id);
    }
}
