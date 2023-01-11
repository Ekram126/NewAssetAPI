using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
   public interface IAssetOwnerService
    {


        IEnumerable<AssetOwner> GetAll();
        List<AssetOwner> GetOwnersByAssetDetailId(int assetDetailId);

        int Delete(int id);
    }
}
