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
    public class MasterAssetController : ControllerBase
    {
        private IPagingService _pagingService;
        private IMasterAssetService _MasterAssetService;
        private IMasterAssetComponentService _masterAssetComponentService;
        [Obsolete]
        IHostingEnvironment _webHostingEnvironment;

        [Obsolete]
        public MasterAssetController(IMasterAssetService MasterAssetService,
            IMasterAssetComponentService masterAssetComponentService,
            IHostingEnvironment webHostingEnvironment,
            IPagingService pagingService)
        {
            _MasterAssetService = MasterAssetService;
            _masterAssetComponentService = masterAssetComponentService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListMasterAssets")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetAll()
        {
            return _MasterAssetService.GetAll();
        }

        [HttpGet]
        [Route("GetTop10MasterAsset")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetTop10MasterAsset()
        {
            return _MasterAssetService.GetTop10MasterAsset();
        }

        [HttpGet]
        [Route("GetTop10MasterAssetCount")]
        public int GetTop10MasterAssetCount()
        {
            var total = _MasterAssetService.GetTop10MasterAsset().ToList().Count();
            return total;
        }


        [HttpPut]
        [Route("GetMasterAssetsWithPaging")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var lstMasterAssets = _MasterAssetService.GetAll().ToList();
            return _pagingService.GetAll<IndexMasterAssetVM.GetData>(pageInfo, lstMasterAssets);
        }



        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            var total = _MasterAssetService.GetAll().ToList().Count();
            return total;// _pagingService.Count<MasterAsset>();
        }

        [HttpGet]
        [Route("ListMasterAssetsByHospitalUserId/{hospitalId}/{userId}")]
        public IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalUserId(int hospitalId, string userId)
        {
            return _MasterAssetService.GetAllMasterAssetsByHospitalId(hospitalId, userId);
        }

        [HttpGet]
        [Route("AutoCompleteMasterAssetName/{name}")]
        public IEnumerable<MasterAsset> AutoCompleteMasterAssetName(string name)
        {
            return _MasterAssetService.AutoCompleteMasterAssetName(name);
        }


        [HttpPost]
        [Route("SearchInMasterAssets/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexMasterAssetVM.GetData> SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _MasterAssetService.SearchInMasterAssets(searchObj).ToList();
            return _pagingService.GetAll<IndexMasterAssetVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInMasterAssetsCount")]
        public int SearchInMasterAssetsCount(SearchMasterAssetVM searchObj)
        {
            int c = _MasterAssetService.SearchInMasterAssets(searchObj).ToList().Count();
            return c;
        }

        [HttpPost]
        [Route("SortMasterAssets/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexMasterAssetVM.GetData> SortMasterAssets(int pagenumber, int pagesize, SortMasterAssetVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _MasterAssetService.sortMasterAssets(sortObj).ToList();
            return _pagingService.GetAll<IndexMasterAssetVM.GetData>(pageInfo, list);
        }



        [HttpGet]
        [Route("ListMasterAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId)
        {
            return _MasterAssetService.GetAllMasterAssetsByHospitalId(hospitalId);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditMasterAssetVM> GetById(int id)
        {
            return _MasterAssetService.GetById(id);
        }


        [HttpGet]
        [Route("ViewMasterAsset/{id}")]
        public ActionResult<ViewMasterAssetVM> ViewMasterAsset(int id)
        {
            return _MasterAssetService.ViewMasterAsset(id);
        }




        [HttpPut]
        [Route("UpdateMasterAsset")]
        public IActionResult Update(EditMasterAssetVM MasterAssetVM)
        {
            try
            {
                int id = MasterAssetVM.Id;
                var lstCode = _MasterAssetService.GetAllMasterAssets().Where(a => a.Code == MasterAssetVM.Code && a.Id != id).ToList();
                if (lstCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Name == MasterAssetVM.Name && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _MasterAssetService.Update(MasterAssetVM);
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
        [Route("AddMasterAsset")]
        public ActionResult<MasterAsset> Add(CreateMasterAssetVM MasterAssetVM)
        {
            var lstCode = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Code == MasterAssetVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Name == MasterAssetVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.NameAr == MasterAssetVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "MasterAsset arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _MasterAssetService.Add(MasterAssetVM);
                return Ok(new { MasterAssetId = savedId });
            }
        }

        [HttpDelete]
        [Route("DeleteMasterAsset/{id}")]
        public ActionResult<MasterAsset> Delete(int id)
        {
            try
            {

                int deletedRow = _MasterAssetService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("CreateMasterAssetAttachments")]
        public int CreateMasterAssetAttachments(CreateMasterAssetAttachmentVM attachObj)
        {
            return _MasterAssetService.CreateMasterAssetDocuments(attachObj);
        }
        [HttpPost]
        [Route("UploadMasterAssetFiles")]
        [Obsolete]
        public ActionResult UploadInFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/" + file.FileName;
            Stream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpPost]
        [Route("UploadMasterAssetImage")]
        [Obsolete]
        public ActionResult UploadMasterAssetImage(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage/" + file.FileName;
            Stream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpGet]
        [Route("GetAttachmentByMasterAssetId/{assetId}")]
        public IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId)
        {
            return _MasterAssetService.GetAttachmentByMasterAssetId(assetId);
        }



        [HttpDelete]
        [Route("DeleteMasterAssetAttachment/{id}")]
        public int DeleteMasterAssetAttachment(int id)
        {
            return _MasterAssetService.DeleteMasterAssetAttachment(id);
        }



        [HttpGet]
        [Route("CountMasterAssets")]
        public int CountMasterAssets()
        {
            return _MasterAssetService.CountMasterAssets();
        }

        [HttpGet]
        [Route("CountMasterAssetsByBrand")]
        public List<CountMasterAssetBrands> CountMasterAssetsByBrand()
        {
            return _MasterAssetService.CountMasterAssetsByBrand();
        }

        [HttpGet]
        [Route("CountMasterAssetsBySupplier")]
        public List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier()
        {
            return _MasterAssetService.CountMasterAssetsBySupplier();
        }

    }
}
