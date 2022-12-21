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
        public IEnumerable<IndexGovernorateVM.GetData> GetAll()
        {
            return _governorateService.GetAll();
        }



    }
}
