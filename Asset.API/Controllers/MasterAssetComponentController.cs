using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.MasterAssetAttachmentVM;
using Asset.ViewModels.MasterAssetComponentVM;
using Asset.ViewModels.MasterAssetVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterAssetComponentController : ControllerBase
    {
        private IPagingService _pagingService;
        private IMasterAssetComponentService _masterAssetComponentService;
     
 
        public MasterAssetComponentController(
            IMasterAssetComponentService masterAssetComponentService,
            IPagingService pagingService)
        {
            _masterAssetComponentService = masterAssetComponentService;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListMasterAssetComponents")]
        public IEnumerable<IndexMasterAssetComponentVM.GetData> GetAll()
        {
            return _masterAssetComponentService.GetAll();
        }
       
       

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditMasterAssetComponentVM> GetById(int id)
        {
            return _masterAssetComponentService.GetById(id);
        }


        [HttpGet]
        [Route("ViewMasterAssetComponent/{id}")]
        public ActionResult<ViewMasterAssetComponentVM> ViewMasterAssetComponent(int id)
        {
            return _masterAssetComponentService.ViewMasterAssetComponent(id);
        }



        [HttpGet]
        [Route("GetMasterAssetComponentByMasterAssetId/{masterAssetId}")]
        public ActionResult<IEnumerable<IndexMasterAssetComponentVM.GetData>> GetMasterAssetComponentByMasterAssetId(int masterAssetId)
        {
            return _masterAssetComponentService.GetMasterAssetComponentByMasterAssetId(masterAssetId).ToList();
        }

        [HttpPut]
        [Route("UpdateMasterAssetComponent")]
        public IActionResult Update(EditMasterAssetComponentVM MasterAssetVM)
        {
            try
            {
                int id = MasterAssetVM.Id;
                var lstCode = _masterAssetComponentService.GetAll().Where(a => a.Code == MasterAssetVM.Code && a.Id != id).ToList();
                if (lstCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstNames = _masterAssetComponentService.GetAll().ToList().Where(a => a.Name == MasterAssetVM.Name && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _masterAssetComponentService.GetAll().ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _masterAssetComponentService.Update(MasterAssetVM);
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
        [Route("AddMasterAssetComponent")]
        public ActionResult<MasterAssetComponent> Add(CreateMasterAssetComponentVM model)
        {
           
            if (model.CompCode.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Master asset component code should not exceed 5 characters", MessageAr = "هذا الكود لابد ألا يزيد عن 5 أحرف أو أرقام" });
            }
            else
            {
                var savedId = _masterAssetComponentService.Add(model);
                return Ok(new { componentId = savedId });
            }
   
        }

        [HttpDelete]
        [Route("DeleteMasterAssetComponent/{id}")]
        public ActionResult<MasterAssetComponent> Delete(int id)
        {
            try
            {

                int deletedRow = _masterAssetComponentService.Delete(id);
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
