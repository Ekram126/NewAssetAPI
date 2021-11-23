using Asset.ViewModels.AssetPeriorityVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
  public  interface IAssetPeriorityRepository
    {
        IEnumerable<IndexAssetPeriorityVM.GetData> GetAll();
        EditAssetPeriorityVM GetById(int id);
        int Add(CreateAssetPeriorityVM assetPeriorityObj);
        int Update(EditAssetPeriorityVM assetPeriorityObj);
        int Delete(int id);
    }
}
