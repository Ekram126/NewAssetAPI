using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asset.Domain.Services;
using Asset.ViewModels.ManufacturerPMAssetVM;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using Asset.Core.Services;
using Asset.ViewModels.WNPMAssetTimes;
using Asset.Models;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerPMAssetController : ControllerBase
    {
        private IManufacturerPMAssetService _manufacturerPMAssetService;
        public ManufacturerPMAssetController(IManufacturerPMAssetService manufacturerPMAssetService)
        {
            _manufacturerPMAssetService = manufacturerPMAssetService;
        }


        //[HttpPost]
        //[Route("CreateAssetTimes/{year}/{hospitalId}")]
        //public IActionResult CreateAssetTimes(int year, int hospitalId)
        //{
        //    var lstAssetTimes = _wNPMAssetTimeService.GetAllWNPMAssetTime().GroupBy(a => a.PMDate.Value.Date.Year).ToList();
        //    if (lstAssetTimes.Count > 0)
        //    {
        //        foreach (var item in lstAssetTimes)
        //        {
        //            if (item.Key == year)
        //            {
        //                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "year", Message = "This Year aleardy exist", MessageAr = "هذه الأصول موجودة مسبقاً" });
        //            }
        //            else
        //            {
        //                var added = _wNPMAssetTimeService.CreateAssetTimes(year, hospitalId);
        //                return Ok();
        //            }
        //        }
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "year", Message = "This Year aleardy exist", MessageAr = "هذه الأصول موجودة مسبقاً" });
        //    }
        //    else
        //    {
        //        var added = _wNPMAssetTimeService.CreateAssetTimes(year, hospitalId);
        //        return Ok();
        //    }
        //    //return Ok();
        //}

        [HttpGet]
        [Route("CreateManfacturerAssetTimes/{pageNumber}/{pageSize}")]
        public IndexUnScheduledManfacturerPMAssetVM CreateManfacturerAssetTimes(int pageNumber, int pageSize)
        {

            return _manufacturerPMAssetService.CreateManfacturerAssetTimes(pageNumber, pageSize);

        }
        [HttpGet]
        [Route("GetAllForCheck")]
        public List<ForCheckManfacturerPMAssetsVM> GetAllForCheck()
        {
            return _manufacturerPMAssetService.GetAllForCheck();
        }
        [HttpGet]
        [Route("GetAll/{pageNumber}/{pageSize}/{userId}")]
        public IndexManfacturerPMAssetVM GetAll(int pageNumber, int pageSize, string userId)
        {
            return _manufacturerPMAssetService.GetAll(pageNumber, pageSize, userId);
        }


        [HttpPost]
        [Route("SearchManfacturerAssetTimes/{pageNumber}/{pageSize}/{userId}")]
        public IndexManfacturerPMAssetVM SearchManfacturerAssetTimes(SearchManfacturerAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _manufacturerPMAssetService.SearchAssetTimes(searchObj, pageNumber, pageSize, userId);
        }


        //[HttpGet]
        //[Route("GetAllForCalendar/{hospitalId}/{userId}")]
        //public List<CalendarWNPMAssetTimeVM> GetAllForCalendar(int hospitalId, string userId)
        //{
        //    return _wNPMAssetTimeService.GetAll(hospitalId, userId);
        //}


        [HttpGet]
        [Route("GetAllForManfacturerCalendar/{hospitalId}/{userId}")]
        public List<CalendarManfacturerPMAssetTimeVM> GetAllForManfacturerCalendar(int hospitalId, string userId)
        {
            return _manufacturerPMAssetService.GetAll(hospitalId, userId);
        }

        [HttpPost]
        [Route("SortManfacturerAssetTimes/{pageNumber}/{pageSize}/{userId}")]
        public IndexManfacturerPMAssetVM SortManfacturerAssetTimes(SortManfacturerPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId)
        {
            return _manufacturerPMAssetService.SortManfacturerAssetTimes(sortObj, pageNumber, pageSize, userId);
        }

        [HttpGet]
        [Route("GetManfacturerAssetById/{id}")]
        public ViewManfacturerPMAssetTimeVM GetManfacturerAssetById(int id)
        {
            return _manufacturerPMAssetService.GetAssetTimeById(id);
        }





        [HttpPut]
        [Route("UpdateManfacturerAssetTime")]
        public int UpdateManfacturerAssetTime(ManufacturerPMAsset model)
        {
            return _manufacturerPMAssetService.Update(model);
        }


        [HttpGet]
        [Route("GetManfacturerAssetModelById/{id}")]
        public ManufacturerPMAsset GetManfacturerAssetModelById(int id)
        {
            var obj = _manufacturerPMAssetService.GetById(id);
            return obj;
        }



        [HttpPost]
        [Route("GetAllManfacturerAssetsTimes2/{pageNumber}/{pageSize}/{userId}")]
        public IndexManfacturerPMAssetVM GetAllManfacturerAssetsTimes2(FilterManfacturerTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {
            return _manufacturerPMAssetService.GetAll(filterObj, pageNumber, pageSize, userId);
        }




    }
}



