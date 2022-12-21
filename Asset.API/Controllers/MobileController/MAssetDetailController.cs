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
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            return _assetDetailService.AutoCompleteAssetBarCode(barcode, hospitalId);
        }

        [HttpGet]
        [Route("AutoCompleteAssetSerial/{serial}/{hospitalId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            return _assetDetailService.AutoCompleteAssetSerial(serial, hospitalId);
        }


        [HttpGet]
        [Route("ListAssetDetailByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> ListAssetDetailByUserId(string userId)
        {
            return await _assetDetailService.GetAssetDetailsByUserId(userId);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public EditAssetDetailVM GetById(int id)
        {
            return _assetDetailService.GetById(id);
        }


        [HttpGet]
        [Route("GetAssetDetailById/{userId}/{assetId}")]
        public MobileAssetDetailVM GetAssetDetailById(string userId, int assetId)
        {
            return _assetDetailService.GetAssetDetailById(userId, assetId);
        }
    }
}
