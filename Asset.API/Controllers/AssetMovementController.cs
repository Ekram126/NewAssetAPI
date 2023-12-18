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
        [Route("ListAssetMovements/{pageNumber}/{pageSize}")]
        public IndexAssetMovementVM GetAll(int pageNumber, int pageSize)
        {
            return _assetMovementService.GetAll(pageNumber, pageSize);
        }


        [HttpGet]
        [Route("GetMovementByAssetDetailId/{assetId}")]
        public IEnumerable<IndexAssetMovementVM.GetData> GetMovementByAssetDetailId(int assetId)
        {
            return _assetMovementService.GetMovementByAssetDetailId(assetId);
        }




        [HttpPost]
        [Route("SearchAssetMovement/{pageNumber}/{pageSize}")]
        public IndexAssetMovementVM GetMovementByAssetDetailId(SearchAssetMovementVM searchObj, int pageNumber, int pageSize)
        {
            return _assetMovementService.SearchAssetMovement(searchObj, pageNumber, pageSize);
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
            var oldMovement = _assetMovementService.GetAllAssetMovements()
                .Where(a => a.BuildingId == AssetMovementVM.BuildingId && a.FloorId == AssetMovementVM.FloorId && a.RoomId == AssetMovementVM.RoomId && a.AssetDetailId == AssetMovementVM.AssetDetailId).OrderByDescending(a => a.MovementDate).ToList();
            if (oldMovement.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "same", Message = "Cannot move asset to same place", MessageAr = "لا يمكن نقل نفس الأصل في ذات المكان" });

            }
            else
            {
                var savedId = _assetMovementService.Add(AssetMovementVM);
                return CreatedAtAction("GetById", new { id = savedId }, AssetMovementVM);
            }

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
