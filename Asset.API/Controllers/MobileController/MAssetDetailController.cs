using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailVM;
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
    public class MAssetDetailController : ControllerBase
    {
        private IAssetDetailService _assetDetailService;

        public MAssetDetailController(IAssetDetailService assetDetailService)
        {
            _assetDetailService = assetDetailService;
        }


        [HttpGet]
        [Route("AutoCompleteAssetBarCode/{barcode}/{hospitalId}")]
        public ActionResult<IEnumerable<IndexAssetDetailVM.GetData>> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            var lstAutoCompleteAssetBarCode = _assetDetailService.AutoCompleteAssetBarCode(barcode, hospitalId);
            if (lstAutoCompleteAssetBarCode.Count() == 0)
            {
                return Ok(new { data = "", msg = "No Data Fount", status = '0' });
            }
            else
                return Ok(new { data = lstAutoCompleteAssetBarCode, msg = "Success", status = '1' });
        }

        [HttpGet]
        [Route("AutoCompleteAssetSerial/{serial}/{hospitalId}")]
        public ActionResult<IEnumerable<IndexAssetDetailVM.GetData>> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            var lstAutoCompleteAssetSerial = _assetDetailService.AutoCompleteAssetSerial(serial, hospitalId);
            if (lstAutoCompleteAssetSerial.Count() == 0)
            {
                return Ok(new { data = lstAutoCompleteAssetSerial, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstAutoCompleteAssetSerial, msg = "Success", status = '1' });
        }


        [HttpGet]
        [Route("ListAssetDetailByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<IndexAssetDetailVM.GetData>>> ListAssetDetailByUserId(string userId)
        {
            var lstAssetDetailByUserId = await _assetDetailService.GetAssetDetailsByUserId(userId);
            if (lstAssetDetailByUserId != null)
            {
                return Ok(new { data = lstAssetDetailByUserId, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = "", msg = "No Data Found", status = '0' });

        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult GetById(int id)
        {
            var assetDetailObj = _assetDetailService.GetById(id);
            if (assetDetailObj != null)
            {
                return Ok(new { data = assetDetailObj, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = "", msg = "No Data Found", status = '0' });
        }


        [HttpGet]
        [Route("GetAssetDetailById/{userId}/{assetId}")]
        public ActionResult GetAssetDetailById(string userId, int assetId)
        {

            var lstAssetDetail = _assetDetailService.GetAssetDetailById(userId, assetId);
            if (lstAssetDetail != null)
            {
                return Ok(new { data = lstAssetDetail, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstAssetDetail, msg = "Success", status = '1' });
        }



        [HttpPost]
        [Route("SearchAssetDetails/{pagenumber}/{pagesize}")]
        public ActionResult SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            var list = _assetDetailService.SearchAssetInHospital(pagenumber, pagesize, searchObj);
            if (list != null)
            {
                return Ok(new { data = list, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = list, msg = "Success", status = '1' });
        }



    }
}
