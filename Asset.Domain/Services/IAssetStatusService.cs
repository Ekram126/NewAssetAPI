using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IAssetStatusService
    {

        IEnumerable<IndexAssetStatusVM.GetData> GetAll();
        EditAssetStatusVM GetById(int id);
        IEnumerable<AssetStatu> GetAllAssetStatus();
        IEnumerable<IndexAssetStatusVM.GetData> GetAssetStatusByName(string AssetStatusName);
        IEnumerable<IndexAssetStatusVM.GetData> SortAssetStatuses(SortAssetStatusVM sortObj);
 

        IEnumerable<IndexAssetStatusVM.GetData> GetAllAssetsGroupByStatusId(int statusId, string userId);

        int Add(CreateAssetStatusVM AssetStatusVM);
        int Update(EditAssetStatusVM AssetStatusVM);
        int Delete(int id);
    }
}
