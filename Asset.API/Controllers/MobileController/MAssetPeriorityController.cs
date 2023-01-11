using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetPeriorityVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MAssetPeriorityController : ControllerBase
    {

        private IAssetPeriorityService _AssetPeriorityService;

        public MAssetPeriorityController(IAssetPeriorityService AssetPeriorityService)
        {
            _AssetPeriorityService = AssetPeriorityService;
        }


        [HttpGet]
        [Route("ListAssetPeriorities")]

        public ActionResult<IEnumerable<IndexAssetPeriorityVM.GetData>> GetAll()
        {
            var ListAssetPeriorities = _AssetPeriorityService.GetAll();
            if (ListAssetPeriorities.Count() == 0)
            {
                return Ok(new { data = ListAssetPeriorities, msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = ListAssetPeriorities, msg = "Success", status = '1' });
        }
    }
}

