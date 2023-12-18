using System.Collections.Generic;
using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.ExternalAssetMovementVM;

namespace Asset.Domain.Services
{
  public  interface IExternalAssetMovementService
    {
        IndexExternalAssetMovementVM GetExternalAssetMovements(int pageNumber,int pageSize);
        IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId);
        EditExternalAssetMovementVM GetById(int id);
        int Add(ExternalAssetMovement movementObj);
        int Update(ExternalAssetMovement movementObj);
        int Delete(int id);

        int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj);
        IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId);
        IndexExternalAssetMovementVM SearchExternalAssetMovement(SearchExternalAssetMovementVM searchObj, int pageNumber, int pageSize);
    }
}
