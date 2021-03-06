using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface IAssetMovementRepository
    {
        IEnumerable<AssetMovement> GetAllAssetMovements();
        IEnumerable<IndexAssetMovementVM.GetData> GetAll();
        IEnumerable<IndexAssetMovementVM.GetData> GetMovementByAssetDetailId(int assetId);
        AssetMovement GetById(int id);
        int Add(CreateAssetMovementVM movementObj);
        int Update(EditAssetMovementVM movementObj);
        int Delete(int id);
    }
}
