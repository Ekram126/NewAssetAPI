using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asset.ViewModels.PagingParameter;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Asset.ViewModels.AssetDetailVM;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MDashBoardController : ControllerBase
    {

        private IMasterAssetService _masterAssetService;
        private IAssetDetailService _assetDetailService;

        public MDashBoardController(IMasterAssetService masterAssetService, IAssetDetailService assetDetailService)
        {
            _masterAssetService = masterAssetService;
            _assetDetailService = assetDetailService;
        }

        [HttpGet]
        [Route("CountMasterAssetsByBrand/{hospitalId}")]
        public ActionResult CountMasterAssetsByBrand(int hospitalId)
        {
            var lstMasterAssetBrands = _masterAssetService.CountMasterAssetsByBrand(hospitalId);
            if (lstMasterAssetBrands.Count() == 0)
            {
                return Ok(new { data = lstMasterAssetBrands, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstMasterAssetBrands, msg = "Success", status = '1' });
        }

        [HttpGet]
        [Route("ListTopAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId)
        {
            return _assetDetailService.ListTopAssetsByHospitalId(hospitalId);
        }

    }
}
