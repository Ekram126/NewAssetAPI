using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IAssetStatusTransactionService
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
