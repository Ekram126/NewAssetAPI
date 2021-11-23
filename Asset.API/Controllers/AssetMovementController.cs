using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
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
    public class AssetMovementController : ControllerBase
    {

        private IAssetMovementService _assetMovementService;

        public AssetMovementController(IAssetMovementService assetMovementService)
        {
            _assetMovementService = assetMovementService;
        }


        [HttpGet]
        [Route("ListAssetMovements")]
        public IEnumerable<IndexAssetMovementVM.GetData> GetAll()
        {
            return _assetMovementService.GetAll();
        }




        [HttpGet]
        [Route("GetMovementByAssetDetailId/{assetId}")]
        public IEnumerable<IndexAssetMovementVM.GetData> GetMovementByAssetDetailId(int assetId)
        {
            return _assetMovementService.GetMovementByAssetDetailId(assetId);
        }





        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<AssetMovement> GetById(int id)
        {
            return _assetMovementService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateAssetMovement")]
        public IActionResult Update(EditAssetMovementVM AssetMovementVM)
        {
            try
            {

                int updatedRow = _assetMovementService.Update(AssetMovementVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddAssetMovement")]
        public ActionResult<AssetMovement> Add(CreateAssetMovementVM AssetMovementVM)
        {
  
                var savedId = _assetMovementService.Add(AssetMovementVM);
                return CreatedAtAction("GetById", new { id = savedId }, AssetMovementVM);
            
        }

        [HttpDelete]
        [Route("DeleteAssetMovement/{id}")]
        public ActionResult<AssetMovement> Delete(int id)
        {
            try
            {
                int deletedRow = _assetMovementService.Delete(id);
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
