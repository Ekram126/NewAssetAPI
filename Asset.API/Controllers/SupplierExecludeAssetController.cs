using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierExecludeAssetVM;
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
    public class SupplierExecludeAssetController : ControllerBase
    {

        private ISupplierExecludeAssetService _supplierExecludeAssetService;
        IWebHostEnvironment _webHostingEnvironment;
        private IPagingService _pagingService;
        public SupplierExecludeAssetController(ISupplierExecludeAssetService supplierExecludeAssetService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _supplierExecludeAssetService = supplierExecludeAssetService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;

        }


        [HttpGet]
        [Route("ListSupplierExcludeAssets")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll()
        {
            return _supplierExecludeAssetService.GetAll();
        }



        [HttpPut]
        [Route("GetAllByStatusId/{statusId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusId(PagingParameter pageInfo,int statusId)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByStatusId(statusId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }

        [HttpGet]
        [Route("GetCountAfterFilterStatusId/{statusId}")]
        public int GetCountAfterFilterStatusId(int statusId)
        {
           return _supplierExecludeAssetService.GetAllByStatusId(statusId).ToList().Count;
        }


        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPaging")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAll().ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }



        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPagingAndTypeId/{appTypeId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> ListSupplierExcludeAssetsWithPagingAndTypeId(int appTypeId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByAppTypeId(appTypeId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _supplierExecludeAssetService.GetAll().ToList().Count;
        }


        [HttpGet]
        [Route("GetcountByAppTypeId/{appTypeId}")]
        public int GetcountByAppTypeId(int appTypeId)
        {
            return _supplierExecludeAssetService.GetAllByAppTypeId(appTypeId).ToList().Count;
        }






        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId/{statusId}/{appTypeId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> ListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId(int statusId,int appTypeId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByStatusIdAndAppTypeId(statusId, appTypeId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("CountListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId/{statusId}/{appTypeId}")]
        public int CountListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId(int statusId, int appTypeId)
        {
            return _supplierExecludeAssetService.GetAllByStatusIdAndAppTypeId(statusId, appTypeId).ToList().Count;
        }

        [HttpPost]
        [Route("SortSuplierApp/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> SortSuplierApp(int pagenumber, int pagesize, SortSupplierExecludeAssetVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list =  _supplierExecludeAssetService.SortSuplierApp(sortObj);
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, list.ToList());
        }







        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierExecludeAssetVM> GetById(int id)
        {
            return _supplierExecludeAssetService.GetById(id);
        }



        [HttpGet]
        [Route("GetSupplierExecludeAssetDetailById/{id}")]
        public ActionResult<ViewSupplierExecludeAssetVM> GetSupplierExecludeAssetDetailById(int id)
        {
            return _supplierExecludeAssetService.GetSupplierExecludeAssetDetailById(id);
        }





        [HttpPut]
        [Route("UpdateSupplierExecludeAsset")]
        public IActionResult Update(EditSupplierExecludeAssetVM SupplierExecludeAssetVM)
        {
            try
            {

                int updatedRow = _supplierExecludeAssetService.Update(SupplierExecludeAssetVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }

        [HttpPut]
        [Route("UpdateExcludedDate")]
        public IActionResult UpdateExcludedDate(EditSupplierExecludeAssetVM model)
        {
            try
            {

                int updatedRow = _supplierExecludeAssetService.UpdateExcludedDate(model);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }



        [HttpPost]
        [Route("AddSupplierExecludeAsset")]
        public int Add(CreateSupplierExecludeAssetVM supplierExecludeAssetObj)
        {

            var savedId = _supplierExecludeAssetService.Add(supplierExecludeAssetObj);
            return savedId;
        }

        [HttpDelete]
        [Route("DeleteSupplierExecludeAsset/{id}")]
        public ActionResult<SupplierExecludeAsset> Delete(int id)
        {
            try
            {
                var SupplierExecludeAssetObj = _supplierExecludeAssetService.GetById(id);

                int deletedRow = _supplierExecludeAssetService.Delete(id);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }



        [HttpPost]
        [Route("CreateSupplierExecludeAssetAttachments")]
        public int CreateSupplierExecludeAssetAttachments(SupplierExecludeAttachment attachObj)
        {
            return _supplierExecludeAssetService.CreateSupplierExecludAttachments(attachObj);
        }
        [HttpPost]
        [Route("UploadSupplierExecludeAssetFiles")]
        //  [Obsolete]
        public ActionResult UploadSupplierExecludeAssetFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludeAssets/" + file.FileName;
            Stream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpGet]
        [Route("GetAttachmentBySupplierExecludeAssetId/{supplierExecludeAssetId}")]
        public IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            return _supplierExecludeAssetService.GetAttachmentBySupplierExecludeAssetId(supplierExecludeAssetId);
        }

        [HttpDelete]
        [Route("DeleteSupplierExecludeAssetAttachment/{id}")]
        public int DeleteSupplierExecludeAssetAttachment(int id)
        {
            return _supplierExecludeAssetService.DeleteSupplierExecludeAttachment(id);
        }
    }
}
