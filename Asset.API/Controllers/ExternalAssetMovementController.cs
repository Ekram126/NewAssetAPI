using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetMovementVM;
using Asset.ViewModels.ExternalAssetMovementVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAssetMovementController : ControllerBase
    {
        IWebHostEnvironment _webHostingEnvironment;

        private IExternalAssetMovementService _externalAssetMovementService;

        public ExternalAssetMovementController(IExternalAssetMovementService externalAssetMovementService, IWebHostEnvironment webHostingEnvironment)
        {
            _externalAssetMovementService = externalAssetMovementService;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpGet]
        [Route("ListExternalAssetMovements/{pageNumber}/{pageSize}")]
        public IndexExternalAssetMovementVM GetExternalAssetMovements(int pageNumber, int pageSize)
        {
            return _externalAssetMovementService.GetExternalAssetMovements(pageNumber, pageSize);
        }



        [HttpPost]
        [Route("SearchExternalAssetMovement/{pageNumber}/{pageSize}")]
        public IndexExternalAssetMovementVM SearchExternalAssetMovement(SearchExternalAssetMovementVM searchObj, int pageNumber, int pageSize)
        {
            return _externalAssetMovementService.SearchExternalAssetMovement(searchObj, pageNumber, pageSize);
        }




        [HttpGet]
        [Route("GetExternalMovementsByAssetDetailId/{assetId}")]
        public IEnumerable<ExternalAssetMovement> GetExternalMovementsByAssetDetailId(int assetId)
        {
            return _externalAssetMovementService.GetExternalMovementsByAssetDetailId(assetId);
        }


        [HttpGet]
        [Route("GetExternalAssetMovementById/{id}")]
        public ActionResult<EditExternalAssetMovementVM> GetById(int id)
        {
            return _externalAssetMovementService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateExternalAssetMovement")]
        public IActionResult Update(ExternalAssetMovement movementObj)
        {
            try
            {

                int updatedRow = _externalAssetMovementService.Update(movementObj);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddExternalAssetMovement")]
        public int Add(ExternalAssetMovement AssetMovementVM)
        {
            //var oldMovement = _externalAssetMovementService.GetAllAssetMovements()
            //    .Where(a => a.BuildingId == AssetMovementVM.BuildingId && a.FloorId == AssetMovementVM.FloorId && a.RoomId == AssetMovementVM.RoomId && a.AssetDetailId == AssetMovementVM.AssetDetailId).OrderByDescending(a=>a.MovementDate).ToList();
            //if (oldMovement.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "same", Message = "Cannot move asset to same place", MessageAr = "لا يمكن نقل نفس الأصل في ذات المكان" });
            //}
            //else
            //{
            var savedId = _externalAssetMovementService.Add(AssetMovementVM);
            return savedId;
            // }

        }

        [HttpDelete]
        [Route("DeleteExternalAssetMovement/{id}")]
        public ActionResult<ExternalAssetMovement> Delete(int id)
        {
            try
            {
                int deletedRow = _externalAssetMovementService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("CreateExternalAssetMovementAttachments")]
        public int CreateExternalAssetMovementAttachments(ExternalAssetMovementAttachment attachObj)
        {
            return _externalAssetMovementService.CreateExternalAssetMovementAttachments(attachObj);
        }


        [HttpGet]
        [Route("GetExternalMovementAttachmentByExternalAssetMovementId/{externalAssetMovementId}")]
        public IEnumerable<ExternalAssetMovementAttachment> GetExternalMovementAttachmentByExternalAssetMovementId(int externalAssetMovementId)
        {
            return _externalAssetMovementService.GetExternalMovementAttachmentByExternalAssetMovementId(externalAssetMovementId);
        }

        [HttpPost]
        [Route("UploadExternalAssetMovementFiles")]
        public ActionResult UploadRequestFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ExternalAssetMovements";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {

            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
