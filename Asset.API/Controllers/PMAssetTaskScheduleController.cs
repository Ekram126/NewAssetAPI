using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMAssetTaskScheduleController : ControllerBase
    {
       
            private IPMAssetTaskScheduleService _pMAssetTaskScheduleService;

         

            [Obsolete]
            IHostingEnvironment _webHostingEnvironment;

            [Obsolete]
            public PMAssetTaskScheduleController(IPMAssetTaskScheduleService pMAssetTaskScheduleService,
                IHostingEnvironment webHostingEnvironment)
            {
                _pMAssetTaskScheduleService = pMAssetTaskScheduleService;
                _webHostingEnvironment = webHostingEnvironment;
             
            }

        [HttpPost]
      
        public IActionResult CreatePMAssetTaskSchedule(PMAssetTaskSchedule PMAssetTaskScheduleObj)
        {
            var lstAssetSchedules = _pMAssetTaskScheduleService.GetAll().ToList().Where(a => a.PMAssetTaskId == PMAssetTaskScheduleObj.PMAssetTaskId && a.PMAssetTimeId == PMAssetTaskScheduleObj.PMAssetTimeId).ToList();
            if (lstAssetSchedules.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "pmtime", Message = "This schedule is already exist", MessageAr = "هذا العنصر مسجل قبل ذلك" });
            }
            else
            {
                var id = _pMAssetTaskScheduleService.Add(PMAssetTaskScheduleObj);
            }

            return Ok();
        }
    }
}
