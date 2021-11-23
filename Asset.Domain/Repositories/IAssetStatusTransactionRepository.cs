using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;


namespace Asset.Domain.Repositories
{
   public interface IAssetStatusTransactionRepository
    {
        IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAll();
        AssetStatusTransaction GetById(int id);
        IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAssetStatusByAssetDetailId(int assetId);
        int Add(AssetStatusTransaction assetStatusTransactionObj);
        int Update(AssetStatusTransaction assetStatusTransactionObj);
        int Delete(int id);
    }
}
