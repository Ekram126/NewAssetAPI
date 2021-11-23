using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
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
    public class AssetStatusController : ControllerBase
    {

        private IAssetStatusService _AssetStatusService;

        public AssetStatusController(IAssetStatusService AssetStatusService)
        {
            _AssetStatusService = AssetStatusService;
        }


        [HttpGet]
        [Route("ListAssetStatus")]
        public IEnumerable<IndexAssetStatusVM.GetData> GetAll()
        {
            return _AssetStatusService.GetAll();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditAssetStatusVM> GetById(int id)
        {
            return _AssetStatusService.GetById(id);
        }
        [HttpPut]
        [Route("UpdateAssetStatus")]
        public IActionResult Update(EditAssetStatusVM AssetStatusVM)
        {
            try
            {
                int id = AssetStatusVM.Id;
                var lstCode = _AssetStatusService.GetAll().ToList().Where(a => a.Code == AssetStatusVM.Code && a.Id != id).ToList();
                if (lstCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "AssetStatus code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstNames = _AssetStatusService.GetAll().ToList().Where(a => a.Name == AssetStatusVM.Name && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _AssetStatusService.GetAll().ToList().Where(a => a.NameAr == AssetStatusVM.NameAr && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _AssetStatusService.Update(AssetStatusVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddAssetStatus")]
        public ActionResult<AssetStatu> Add(CreateAssetStatusVM AssetStatusVM)
        {
            var lstCode = _AssetStatusService.GetAll().ToList().Where(a => a.Code == AssetStatusVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "AssetStatus code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _AssetStatusService.GetAll().ToList().Where(a => a.Name == AssetStatusVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _AssetStatusService.GetAll().ToList().Where(a => a.NameAr == AssetStatusVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _AssetStatusService.Add(AssetStatusVM);
                return CreatedAtAction("GetById", new { id = savedId }, AssetStatusVM);
            }
        }

        [HttpDelete]
        [Route("DeleteAssetStatus/{id}")]
        public ActionResult<AssetStatu> Delete(int id)
        {
            try
            {

                int deletedRow = _AssetStatusService.Delete(id);
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
