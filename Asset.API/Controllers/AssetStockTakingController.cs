using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ApplicationTypeVM;
using Asset.ViewModels.AssetStockTakingVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetStockTakingController : ControllerBase
    {

        private IAssetStockTakingService _assetStockTakingService;

        public AssetStockTakingController(IAssetStockTakingService assetStockTakingService)
        {
            _assetStockTakingService = assetStockTakingService;
        }


        [HttpGet]
        [Route("ListAssetStockTakings/{pageNumber}/{pageSize}")]
        public IndexAssetStockTakingVM ListAssetStockTakings(int pageNumber, int pageSize)
        {
            return _assetStockTakingService.ListAssetStockTakings(pageNumber, pageSize);
        }



      [HttpPost]
        [Route("SearchAssetStockTakings/{pageNumber}/{pageSize}")]
        public IndexAssetStockTakingVM SearchAssetStockTakings(SearchAssetStockTakingVM searchObj, int pageNumber, int pageSize)
        {
            return _assetStockTakingService.SearchInAssetStockTakings(searchObj,pageNumber, pageSize);
        }


    }
}
