using Asset.Core.Services;
using Asset.Domain.Services;
using Asset.ViewModels.StockTakingScheduleVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingHospitalController : ControllerBase
    {
        private IStockTakingHospitalService _stockTakingHospitalService;

        public StockTakingHospitalController(IStockTakingHospitalService stockTakingHospitalService)
        {
            _stockTakingHospitalService=stockTakingHospitalService;
        }
        [HttpGet]
        [HttpGet("GetHospitalsByScheduleId/{scheduleId}")]
        public IEnumerable<RelatedHospital> GetHospitalsByScheduleId(int scheduleId)
        {
            return _stockTakingHospitalService.GetHospitalsByScheduleId(scheduleId);
        }
    }
}
