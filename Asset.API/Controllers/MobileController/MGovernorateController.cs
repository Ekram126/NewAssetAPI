using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MGovernorateController : ControllerBase
    {
        private IGovernorateService _governorateService;

        public MGovernorateController(IGovernorateService governorateService)
        {
            _governorateService = governorateService;
        }


        [HttpGet]
        [Route("ListGovernorates")]
        public ActionResult GetAll()
        {
            IEnumerable<IndexGovernorateVM.GetData> list = new List<IndexGovernorateVM.GetData>();

            list = _governorateService.GetAll();
            if (list.Count() == 0)
            {
                return Ok(new { data = list, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = list, msg = "Success", status = '1' });
        }



        [HttpGet]
        [Route("GetGovernorateWithHospitals")]
        public ActionResult GetGovernorateWithHospitals()
        {
            var lstGovhospitals= _governorateService.GetGovernorateWithHospitals();
            if (lstGovhospitals.Count() == 0)
            {
                return Ok(new { data = lstGovhospitals, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = lstGovhospitals, msg = "Success", status = '1' });
        }

    }



}

