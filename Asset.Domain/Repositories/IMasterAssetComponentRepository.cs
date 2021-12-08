
using Asset.ViewModels.MasterAssetComponentVM;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
    public interface IMasterAssetComponentRepository
    {
        IEnumerable<IndexMasterAssetComponentVM.GetData> GetAll();
        IEnumerable<IndexMasterAssetComponentVM.GetData> GetMasterAssetComponentByMasterAssetId(int masterAssetId);
        EditMasterAssetComponentVM GetById(int id);
        ViewMasterAssetComponentVM ViewMasterAssetComponent(int id);
        int Add(CreateMasterAssetComponentVM masterAssetObj);
        int Update(EditMasterAssetComponentVM masterAssetObj);
        int Delete(int id);

    }
}
