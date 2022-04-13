using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStatusTransactionVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetStatusTransactionController : ControllerBase
    {

        private IAssetStatusTransactionService _assetStatusTransactionService;

        public AssetStatusTransactionController(IAssetStatusTransactionService assetStatusTransactionService)
        {
            _assetStatusTransactionService = assetStatusTransactionService;
        }


        [HttpGet]
        [Route("ListAssetStatusTransactions")]
        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetAll()
        {
            return _assetStatusTransactionService.GetAll();
        }


        [HttpGet]
        [Route("GetLastTransactionByAssetId/{assetId}")]
        public List<AssetStatusTransaction> GetLastTransactionByAssetId(int assetId)
        {
            return _assetStatusTransactionService.GetLastTransactionByAssetId(assetId);
        }


        [HttpGet]
        [Route("GetStatusTransactionByAssetDetailId/{assetId}")]
        public IEnumerable<IndexAssetStatusTransactionVM.GetData> GetStatusTransactionByAssetDetailId(int assetId)
        {
            return _assetStatusTransactionService.GetAssetStatusByAssetDetailId(assetId);
        }





        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<AssetStatusTransaction> GetById(int id)
        {
            return _assetStatusTransactionService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateAssetStatusTransaction")]
        public IActionResult Update(AssetStatusTransaction AssetStatusTransactionVM)
        {
            try
            {

                int updatedRow = _assetStatusTransactionService.Update(AssetStatusTransactionVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddAssetStatusTransaction")]
        public int Add(CreateAssetStatusTransactionVM AssetStatusTransactionVM)
        {

            var savedId = _assetStatusTransactionService.Add(AssetStatusTransactionVM);
            return savedId;

        }

        [HttpDelete]
        [Route("DeleteAssetStatusTransaction/{id}")]
        public ActionResult<AssetStatusTransaction> Delete(int id)
        {
            try
            {
                int deletedRow = _assetStatusTransactionService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }
    }
}
