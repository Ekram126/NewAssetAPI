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

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingScheduleController : ControllerBase
    {

        private IStockTakingScheduleService _stockTakingScheduleService;
        IWebHostEnvironment _webHostingEnvironment;
        public StockTakingScheduleController(IStockTakingScheduleService stockTakingScheduleService,IWebHostEnvironment webHostingEnvironment)
        {
            _stockTakingScheduleService = stockTakingScheduleService;
 
            _webHostingEnvironment = webHostingEnvironment;

        }




        //[HttpGet]
        //[Route("ListStockTakingSchedules")]
        //public IEnumerable<IndexStockTakingScheduleVM.GetData> GetAll()
        //{
        //    return _stockTakingScheduleService.GetAll();
        //}
        [HttpGet]
        [Route("GetAllWithPaging/{pageNumber}/{pageSize}")]
        public IndexStockTakingScheduleVM GetAllWithPaging( int pageNumber, int pageSize)
        {
            return _stockTakingScheduleService.GetAllWithPaging( pageNumber, pageSize);
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



        //[HttpPost]
        //[Route("CreateExternalFixFile")]
        //public int CreateExternalFixFiles(CreateExternalFixFileVM attachObj)
        //{
        //    return _ExternalFixService.AddExternalFixFile(attachObj);
        //}
        [HttpGet]
        [Route("GetStockTakingScheduleById/{id}")]
        public IndexStockTakingScheduleVM.GetData GetStockTakingScheduleById(int id)
        {
           return _stockTakingScheduleService.GetById(id);  

        }




    }
}
