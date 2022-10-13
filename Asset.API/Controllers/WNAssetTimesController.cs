using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WNPMAssetTimes;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WNAssetTimesController : ControllerBase
    {
        private readonly IWNPMAssetTimeService _wNPMAssetTimeService;
        private readonly IAssetDetailService _assetDetailService;

        public WNAssetTimesController(IWNPMAssetTimeService wNPMAssetTimeService, IAssetDetailService assetDetailService)
        {
            _wNPMAssetTimeService = wNPMAssetTimeService;
            _assetDetailService = assetDetailService;

        }

        [HttpPost]
        [Route("GetAllAssetsTimes/{pageNumber}/{pageSize}/{userId}")]
        public IndexWNPMAssetTimesVM GetAllAssetsTimes(FilterAssetTimeVM filterObj, int pageNumber, int pageSize, string userId)
        {
            return _wNPMAssetTimeService.GetAll(filterObj, pageNumber, pageSize, userId);
        }



        [HttpGet]
        [Route("GetAllForCalendar/{hospitalId}/{userId}")]
        public List<CalendarWNPMAssetTimeVM> GetAllForCalendar(int hospitalId, string userId)
        {
            return _wNPMAssetTimeService.GetAll(hospitalId, userId);
        }


        [HttpPost]
        [Route("AddWNAssetTime")]
        public int AddWNAssetTime(WNPMAssetTime model)
        {
            return _wNPMAssetTimeService.Add(model);
        }



        [HttpPut]
        [Route("UpdateWNAssetTime")]
        public int UpdateWNAssetTime(WNPMAssetTime model)
        {
            return _wNPMAssetTimeService.Update(model);
        }


        [HttpGet]
        [Route("GetWNAssetTimeById/{id}")]
        public WNPMAssetTime GetWNAssetTimeById(int id)
        {
            var obj = _wNPMAssetTimeService.GetById(id);
            return obj;
        }



        [HttpPost]
        [Route("SearchAssetTimes/{pageNumber}/{pageSize}/{userId}")]
        public IndexWNPMAssetTimesVM SearchAssetTimes(SearchAssetTimeVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _wNPMAssetTimeService.SearchAssetTimes(searchObj, pageNumber, pageSize, userId);
        }


        [HttpPost]
        [Route("SortAssetTimes/{pageNumber}/{pageSize}/{userId}")]
        public IndexWNPMAssetTimesVM SortAssetTimes(SortWNPMAssetTimeVM sortObj, int pageNumber, int pageSize, string userId)
        {
            return _wNPMAssetTimeService.SortAssetTimes(sortObj, pageNumber, pageSize, userId);
        }





        [HttpPost]
        [Route("GetAllAssetTimesIsDone/{isDone}/{pageNumber}/{pageSize}/{userId}")]
        public IndexWNPMAssetTimesVM GetAllAssetTimesIsDone(bool? isDone, int pageNumber, int pageSize, string userId)
        {
            return _wNPMAssetTimeService.GetAllAssetTimesIsDone(isDone, pageNumber, pageSize, userId);
        }

        [HttpGet]
        [Route("GetAssetTimeById/{id}")]
        public ViewWNPMAssetTimeVM GetAssetTimeById(int id)
        {
            return _wNPMAssetTimeService.GetAssetTimeById(id);
        }
        //[HttpGet]
        //[Route("GetYearQuarters")]
        //public ITimePeriodCollection GetYearQuarters()
        //{
        //    Year year = new Year(DateTime.Today.Year);
        //    ITimePeriodCollection quarters = year.GetQuarters();
        //    foreach (Quarter quarter in quarters)
        //    {
        //        Console.WriteLine("Quarter: {0}", quarter.YearQuarter);
        //    }
        //    return quarters;
        //}



        [HttpGet]
        [Route("GetYearQuarters")]
        public List<Quarter> GetYearQuarters()
        {
            Year year = new Year(DateTime.Today.Year);
            ITimePeriodCollection quarters = year.GetQuarters();
            List<Quarter> list = new List<Quarter>();
            foreach (Quarter quarter in quarters)
            {
                list.Add(quarter);
            }
            return list;
        }

        [HttpPost]
        [Route("CreateAssetTimes/{year}/{hospitalId}")]
        public IActionResult CreateAssetTimes(int year, int hospitalId)
        {
            var lstAssetTimes = _wNPMAssetTimeService.GetAllWNPMAssetTime().GroupBy(a => a.PMDate.Value.Date.Year).ToList();
            if (lstAssetTimes.Count > 0)
            {
                foreach (var item in lstAssetTimes)
                {
                    if (item.Key == year)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "year", Message = "This Year aleardy exist", MessageAr = "هذه الأصول موجودة مسبقاً" });
                    }
                    else
                    {
                        var added = _wNPMAssetTimeService.CreateAssetTimes(year, hospitalId);
                        return Ok();
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "year", Message = "This Year aleardy exist", MessageAr = "هذه الأصول موجودة مسبقاً" });
            }
            else
            {
                var added = _wNPMAssetTimeService.CreateAssetTimes(year, hospitalId);
                return Ok();
            }
            return Ok();
        }
    }
}
