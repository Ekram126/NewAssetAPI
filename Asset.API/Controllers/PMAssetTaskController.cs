using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.MasterAssetVM;
using Asset.ViewModels.PMAssetTaskVM;
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
    public class PMAssetTaskController : ControllerBase
    {

        private IPMAssetTaskService _pmAssetTaskService;

        public PMAssetTaskController(IPMAssetTaskService pmAssetTaskService)
        {
            _pmAssetTaskService = pmAssetTaskService;
        }


        [HttpGet]
        [Route("ListPMAssetTasks")]
        public IEnumerable<PMAssetTask> GetAll()
        {
            return _pmAssetTaskService.GetAll();
        }


        [HttpGet]
        [Route("GetPMAssetTaskByMasterAssetId/{masterAssetId}")]
        public IEnumerable<PMAssetTask> GetPMAssetTaskByMasterAssetId(int masterAssetId)
        {
            return _pmAssetTaskService.GetPMAssetTaskByMasterAssetId(masterAssetId);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<PMAssetTask> GetById(int id)
        {
            return _pmAssetTaskService.GetById(id);
        }
        [HttpGet]
        [Route("GetPMAssetTaskByTaskIdAndMasterAssetId/{masterAssetId}/{taskId}")]
        public ActionResult<PMAssetTask> GetById(int masterAssetId,int taskId)
        {
            return _pmAssetTaskService.GetPMAssetTaskByTaskIdAndMasterAssetId(masterAssetId, taskId);
        }



        [HttpPut]
        [Route("UpdatePMAssetTask")]
        public IActionResult Update(PMAssetTask model)
        {
            try
            {
      
                    int updatedRow = _pmAssetTaskService.Update(model);
            
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddPMAssetTask")]
        public ActionResult<int> Add(CreatePMAssetTaskVM model)
        {

                var savedId = _pmAssetTaskService.Add(model);
                return CreatedAtAction("GetById", new { id = savedId }, model);
       
        }

        [HttpDelete]
        [Route("DeletePMAssetTask/{id}")]
        public ActionResult<PMAssetTask> Delete(int id)
        {
            try
            {
              int deletedRow = _pmAssetTaskService.Delete(id);
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
