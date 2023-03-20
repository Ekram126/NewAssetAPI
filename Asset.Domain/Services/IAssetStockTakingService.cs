using Asset.Models;
using Asset.ViewModels.AssetStockTakingVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Services
{
    public interface IAssetStockTakingService
    {
        int Add(CreateAssetStockTakingVM createAssetStockTakingVM);
        IEnumerable<IndexAssetStockTakingVM.GetData> GetAll();
        IndexAssetStockTakingVM GetAllWithPaging(int page, int pageSize);
        IndexAssetStockTakingVM SearchInAssetStockTakings(SearchAssetStockTakingVM searchObj, int page, int pageSize);
        IndexAssetStockTakingVM ListAssetStockTakings(int pageNumber, int pageSize);
    }
}
