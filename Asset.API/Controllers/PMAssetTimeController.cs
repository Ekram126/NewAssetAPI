using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
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
    public class PMAssetTimeController : ControllerBase
    {

        private IPMAssetTimeService _pmAssetTimeService;

        public PMAssetTimeController(IPMAssetTimeService pmAssetTimeService)
        {
            _pmAssetTimeService = pmAssetTimeService;
        }


        [HttpGet]
        [Route("ListPMAssetTimes")]
        public IEnumerable<PMAssetTime> GetAll()
        {
            return _pmAssetTimeService.GetAll();
        }


        [HttpGet]
        [Route("GetDateByAssetDetailId/{assetId}")]
        public IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetId)
        {
            return _pmAssetTimeService.GetDateByAssetDetailId(assetId);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<PMAssetTime> GetById(int id)
        {
            return _pmAssetTimeService.GetById(id);
        }
        //[HttpGet]
        //[Route("GetPMAssetTimeByTaskIdAndMasterAssetId/{masterAssetId}/{taskId}")]
        //public ActionResult<PMAssetTime> GetById(int masterAssetId,int taskId)
        //{
        //    return _pmAssetTimeService.GetPMAssetTimeByTaskIdAndMasterAssetId(masterAssetId, taskId);
        //}



        [HttpPut]
        [Route("UpdatePMAssetTime")]
        public IActionResult Update(PMAssetTime model)
        {
            try
            {
                int updatedRow = _pmAssetTimeService.Update(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddPMAssetTime")]
        public ActionResult<int> Add(PMAssetTime model)
        {
        
              var   savedId = _pmAssetTimeService.Add(model);
            
            return CreatedAtAction("GetById", new { id = savedId }, model);

        }

        [HttpDelete]
        [Route("DeletePMAssetTime/{id}")]
        public ActionResult<PMAssetTime> Delete(int id)
        {
            try
            {
                int deletedRow = _pmAssetTimeService.Delete(id);
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
