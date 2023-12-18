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
        private IMasterAssetService _masterAssetService;
        private IWorkOrderService _workOrderService;
        private readonly IRequestService _requestService;


        public MAssetDetailController(IAssetDetailService assetDetailService, IWorkOrderService workOrderService, IMasterAssetService masterAssetService, IRequestService requestService)
        {
            _assetDetailService = assetDetailService;
            _workOrderService = workOrderService;
            _masterAssetService = masterAssetService;
            _requestService = requestService;
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
        [Route("AutoCompleteAssetName/{name}")]
        public ActionResult<IEnumerable<IndexAssetDetailVM.GetData>> AutoCompleteMasterAssetName2(string name)
        {
            var lstAutoCompleteAssetName = _masterAssetService.AutoCompleteMasterAssetName2(name);
            if (lstAutoCompleteAssetName.Count() == 0)
            {
                return Ok(new { data = lstAutoCompleteAssetName, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstAutoCompleteAssetName, msg = "Success", status = '1' });
        }




        [HttpGet]
        [Route("GetAssetDetailById/{userId}/{assetId}")]
        public ActionResult GetAssetDetailById(string userId, int assetId)
        {

            var lstAssetDetail = _assetDetailService.GetAssetDetailById(userId, assetId);
            if (lstAssetDetail == null)
            {
                return Ok(new { data = lstAssetDetail, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstAssetDetail, msg = "Success", status = '1' });
        }


        [HttpGet]
        [Route("GetAssetDetailByIdOnly/{userId}/{assetId}")]
        public ActionResult GetAssetDetailByIdOnly(string userId, int assetId)
        {

            var lstAssetDetail = _assetDetailService.GetAssetDetailByIdOnly(userId, assetId);
            if (lstAssetDetail == null)
            {
                return Ok(new { data = lstAssetDetail, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstAssetDetail, msg = "Success", status = '1' });
        }




        [HttpGet]
        [Route("GetWorkOrderByRequestId/{requestId}/{userId}")]
        public ActionResult GetMobileWorkOrderByRequestUserId(int requestId, string userId)
        {

            var lstAssetDetail = _workOrderService.GetMobileWorkOrderByRequestUserId(requestId, userId);
            if (lstAssetDetail != null)
            {
                return Ok(new { data = lstAssetDetail, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = lstAssetDetail, msg = "No Data Found", status = '0' });
        }


        [HttpPost]
        [Route("SearchAssetDetails/{pageNumber}/{pageSize}")]
        public ActionResult SearchInMasterAssets2(SearchMasterAssetVM searchObj, int pageNumber, int pageSize)
        {
            var list = _assetDetailService.MobSearchAssetInHospital(searchObj, pageNumber, pageSize);
            if (list != null)
            {
                return Ok(new { data = list, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = list, msg = "No Data Found", status = '0' });
        }

        [HttpGet]
        [Route("CountAssetsByHospitalId/{hospitalId}")]
        public ActionResult CountAssetsByHospitalId(int hospitalId)
        {
            var total = _assetDetailService.CountAssetsByHospitalId(hospitalId);
            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }



        [HttpGet]
        [Route("CountWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public ActionResult CountWorkOrdersByHospitalId(int hospitalId, string userId)
        {
            var total = _workOrderService.CountWorkOrdersByHospitalId(hospitalId, userId);
            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }



        [HttpGet]
        [Route("CountRequestsByHospitalId/{hospitalId}/{userId}")]
        public ActionResult CountRequestsByHospitalId(int hospitalId, string userId)
        {
            var total = _requestService.CountRequestsByHospitalId(hospitalId, userId);
            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }
    }
}
