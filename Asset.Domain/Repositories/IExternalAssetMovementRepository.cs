using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using System.Collections.Generic;

namespace Asset.Domain.Repositories
{
    public interface IExternalAssetMovementRepository
    {
        IEnumerable<ExternalAssetMovement> GetExternalAssetMovements();
        IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId);
        ExternalAssetMovement GetById(int id);
        int Add(ExternalAssetMovement movementObj);
        int Update(ExternalAssetMovement movementObj);
        int Delete(int id);
        int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj);
        IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId);

    }
}
