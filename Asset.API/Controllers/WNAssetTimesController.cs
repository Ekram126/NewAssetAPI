using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WNPMAssetTimes;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        IWebHostEnvironment _webHostingEnvironment;

        public WNAssetTimesController(IWNPMAssetTimeService wNPMAssetTimeService, IAssetDetailService assetDetailService, IWebHostEnvironment webHostingEnvironment)
        {
            _wNPMAssetTimeService = wNPMAssetTimeService;
            _assetDetailService = assetDetailService;
            _webHostingEnvironment = webHostingEnvironment;
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



        [HttpPost]
        [Route("GetAllWithDate/{pageNumber}/{pageSize}/{userId}")]
        public IndexWNPMAssetTimesVM GetAllWithDate(WNPDateVM searchObj, int pageNumber, int pageSize, string userId)
        {
            return _wNPMAssetTimeService.GetAllWithDate(searchObj, pageNumber, pageSize, userId);
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


        [HttpGet]
        [Route("GetWNPMAssetTimeAttachmentByWNPMAssetTimeId/{wnpmAssetTimeId}")]
        public List<WNPMAssetTimeAttachment> GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(int wnpmAssetTimeId)
        {
            return _wNPMAssetTimeService.GetWNPMAssetTimeAttachmentByWNPMAssetTimeId(wnpmAssetTimeId);
        }




        [HttpPost]
        [Route("CreateWNPMAssetTimeAttachment")]
        public int CreateWNPMAssetTimeAttachment(WNPMAssetTimeAttachment attachObj)
        {
            return _wNPMAssetTimeService.CreateWNPMAssetTimeAttachment(attachObj);
        }

        [HttpPost]
        [Route("UploadWNPMAssetTimeFiles")]
        public ActionResult UploadWNPMAssetTimeFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/WNPMAssetTime";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {
                //System.IO.File.Delete(filePath);
                //Stream stream = new FileStream(filePath, FileMode.Create);
                //file.CopyTo(stream);
                //stream.Close();
            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }



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




        [HttpGet]
        [Route("GetFiscalYearQuarters")]
        public List<Quarter> GetFiscalYearQuarters()
        {
            Year year = new Year(new FiscalTimeCalendar());
            ITimePeriodCollection quarters = year.GetQuarters();
            List<Quarter> list = new List<Quarter>();
            foreach (Quarter quarter in quarters)
            {
                list.Add(quarter);
            }
            return list;
        }


        [HttpGet]
        [Route("GetFiscalYearCurrentQuarter")]
        public YearQuarter GetFiscalYearCurrentQuarter()
        {
            FiscalTimeCalendar calendar = new FiscalTimeCalendar(); // use fiscal periods
            DateTime today = new DateTime(DateTime.Today.Date.Year, DateTime.Today.Date.Month, DateTime.Today.Date.Day);
            return new Quarter(today, calendar).YearQuarter;
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
            //return Ok();
        }



        public IActionResult CreateAssetFiscalTimes(int year, int hospitalId)
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
                        var added = _wNPMAssetTimeService.CreateAssetFiscalTimes(year, hospitalId);
                        return Ok();
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "year", Message = "This Year aleardy exist", MessageAr = "هذه الأصول موجودة مسبقاً" });
            }
            else
            {
                var added = _wNPMAssetTimeService.CreateAssetFiscalTimes(year, hospitalId);
                return Ok();
            }

        }
    }
}
