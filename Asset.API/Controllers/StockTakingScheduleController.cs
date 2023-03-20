using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.StockTakingScheduleVM;

using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Asset.ViewModels.AssetStockTakingVM;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingScheduleController : ControllerBase
    {

        private IStockTakingScheduleService _stockTakingScheduleService;
        IWebHostEnvironment _webHostingEnvironment;
        private IAssetStockTakingService _assetStockTakingService;
        public StockTakingScheduleController(IStockTakingScheduleService stockTakingScheduleService,
            IWebHostEnvironment webHostingEnvironment, IAssetStockTakingService assetStockTakingService)
        {
            _stockTakingScheduleService = stockTakingScheduleService;
            _assetStockTakingService = assetStockTakingService;
            _webHostingEnvironment = webHostingEnvironment;
        }

        [HttpGet]
        [Route("GetAllWithPaging/{pageNumber}/{pageSize}")]
        public IndexStockTakingScheduleVM GetAllWithPaging(int pageNumber, int pageSize)
        {
            return _stockTakingScheduleService.GetAllWithPaging(pageNumber, pageSize);
        }

        [HttpDelete]
        [Route("DeleteStockTakingSchedule/{id}")]
        public ActionResult<ExternalFix> Delete(int id)
        {
            try
            {

                int deletedRow = _stockTakingScheduleService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();

        }
        [HttpPost]
        [Route("AddStockTakingSchedule")]
        public int Add(CreateStockTakingScheduleVM stockTakingScheduleVM)
        {

            var savedId = _stockTakingScheduleService.Add(stockTakingScheduleVM);
            return savedId;

        }


        [HttpGet]
        [Route("GenerateStockScheduleTakingNumberVM")]
        public GenerateStockScheduleTakingNumberVM GenerateStockScheduleTakingNumber()
        {
            return _stockTakingScheduleService.GenerateStockScheduleTakingNumber();
        }


        [HttpGet]
        [Route("GetStockTakingScheduleById/{id}")]
        public IndexStockTakingScheduleVM.GetData GetStockTakingScheduleById(int id)
        {
            return _stockTakingScheduleService.GetById(id);

        }



        [HttpPost]
        [Route("SearchStockTackingSchedule/{pageNumber}/{pageSize}")]
        public IndexStockTakingScheduleVM SearchStockTakingSchedule(SearchStockTakingScheduleVM searchObj, int pageNumber, int pageSize)
        {
            return _stockTakingScheduleService.SearchStockTakingSchedule(searchObj, pageNumber, pageSize);
        }


        [HttpPost]
        [Route("SortStockTakingSchedule/{pageNumber}/{pageSize}")]
        public IndexStockTakingScheduleVM SortStockTakingSchedule(int pageNumber, int pageSize,SortStockTakingScheduleVM sortObj)
        {
            return _stockTakingScheduleService.SortStockTakingSchedule(pageNumber, pageSize,sortObj);
        }
    }
}
