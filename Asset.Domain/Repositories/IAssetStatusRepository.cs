using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface IAssetStatusRepository
    {

        IEnumerable<IndexAssetStatusVM.GetData> GetAll();

        IEnumerable<AssetStatu> GetAllAssetStatus();
        IEnumerable<IndexAssetStatusVM.GetData> GetAssetStatusByName(string AssetStatusName);
        IEnumerable<IndexAssetStatusVM.GetData> SortAssetStatuses(SortAssetStatusVM sortObj);

        IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusId(int statusId,string userId);

        EditAssetStatusVM GetById(int id);
        int Add(CreateAssetStatusVM AssetStatusVM);
        int Update(EditAssetStatusVM AssetStatusVM);
        int Delete(int id);
    }
}
