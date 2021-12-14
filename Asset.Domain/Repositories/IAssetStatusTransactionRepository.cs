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
        List<AssetStatusTransaction> GetLastTransactionByAssetId(int assetId);
        IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAssetStatusByAssetDetailId(int assetId);
        int Add(CreateAssetStatusTransactionVM assetStatusTransactionObj);
        int Update(AssetStatusTransaction assetStatusTransactionObj);
        int Delete(int id);
    }
}
