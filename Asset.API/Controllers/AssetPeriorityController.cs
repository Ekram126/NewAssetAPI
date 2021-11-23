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
    [Route("api/[controller]")]
    [ApiController]
    public class AssetPeriorityController : ControllerBase
    {

        private IAssetPeriorityService _AssetPeriorityService;

        public AssetPeriorityController(IAssetPeriorityService AssetPeriorityService)
        {
            _AssetPeriorityService = AssetPeriorityService;
        }


        [HttpGet]
        [Route("ListAssetPeriorities")]
        public IEnumerable<IndexAssetPeriorityVM.GetData> GetAll()
        {
            return _AssetPeriorityService.GetAll();
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditAssetPeriorityVM> GetById(int id)
        {
            return _AssetPeriorityService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateAssetPeriority")]
        public IActionResult Update(EditAssetPeriorityVM AssetPeriorityVM)
        {
            try
            {
         
                    int updatedRow = _AssetPeriorityService.Update(AssetPeriorityVM);
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddAssetPeriority")]
        public ActionResult<AssetPeriority> Add(CreateAssetPeriorityVM AssetPeriorityVM)
        {
 
                var savedId = _AssetPeriorityService.Add(AssetPeriorityVM);
                return CreatedAtAction("GetById", new { id = savedId }, AssetPeriorityVM);
            
        }

        [HttpDelete]
        [Route("DeleteAssetPeriority/{id}")]
        public ActionResult<AssetPeriority> Delete(int id)
        {
            try
            {

                int deletedRow = _AssetPeriorityService.Delete(id);
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
