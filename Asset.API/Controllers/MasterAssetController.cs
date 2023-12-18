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
        private IAssetDetailService _assetDetailService;
        private IMasterAssetComponentService _masterAssetComponentService;
        IWebHostEnvironment _webHostEnvironment;
        [Obsolete]
        IHostingEnvironment _webHostingEnvironment;

        [Obsolete]
        public MasterAssetController(IMasterAssetService MasterAssetService,
            IMasterAssetComponentService masterAssetComponentService,
             IAssetDetailService assetDetailService,
            IHostingEnvironment webHostingEnvironment,
             IWebHostEnvironment webHostEnvironment,
            IPagingService pagingService)
        {
            _MasterAssetService = MasterAssetService;
            _assetDetailService = assetDetailService;
            _masterAssetComponentService = masterAssetComponentService;
            _webHostingEnvironment = webHostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListMasterAssets")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetAll()
        {
            return _MasterAssetService.GetAll();
        }

        [HttpGet]
        [Route("GetTop10MasterAsset/{hospitalId}")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetTop10MasterAsset(int hospitalId)
        {
            return _MasterAssetService.GetTop10MasterAsset(hospitalId);
        }

        [HttpGet]
        [Route("GetTop10MasterAssetCount/{hospitalId}")]
        public int GetTop10MasterAssetCount(int hospitalId)
        {
            var total = _MasterAssetService.GetTop10MasterAsset(hospitalId).ToList().Count();
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
        [Route("GetListMasterAsset")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetListMasterAsset()
        {
            return _MasterAssetService.GetListMasterAsset();
        }



        [HttpGet]
        [Route("AutoCompleteMasterAssetName/{name}")]
        public IEnumerable<MasterAsset> AutoCompleteMasterAssetName(string name)
        {
            return _MasterAssetService.AutoCompleteMasterAssetName(name);
        }


        [HttpGet]
        [Route("AutoCompleteMasterAssetName2/{name}")]
        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName2(string name)
        {
            return _MasterAssetService.AutoCompleteMasterAssetName2(name);
        }





        [HttpGet]
        [Route("DistinctAutoCompleteMasterAssetName/{name}")]
        public IEnumerable<MasterAsset> DistinctAutoCompleteMasterAssetName(string name)
        {
            return _MasterAssetService.DistinctAutoCompleteMasterAssetName(name);
        }




        [HttpGet]
        [Route("AutoCompleteMasterAssetName3/{name}/{hospitalId}")]
        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName3(string name, int hospitalId)
        {
            return _MasterAssetService.AutoCompleteMasterAssetName3(name, hospitalId);
        }


        [HttpGet]
        [Route("AutoCompleteMasterAssetName4/{name}/{hospitalId}")]
        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName4(string name, int hospitalId)
        {
            return _MasterAssetService.AutoCompleteMasterAssetName4(name, hospitalId);
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



        [Route("GetLastDocumentForMsterAssetId/{masterId}")]
        public MasterAssetAttachment GetLastDocumentForMsterAssetId(int masterId)
        {
            return _MasterAssetService.GetLastDocumentForMsterAssetId(masterId);
        }



        [HttpPut]
        [Route("UpdateMasterAsset")]
        public IActionResult Update(EditMasterAssetVM MasterAssetVM)
        {
            try
            {
                int id = MasterAssetVM.Id;
                if (MasterAssetVM.Code != null)
                {
                    var lstCode = _MasterAssetService.GetAllMasterAssets().Where(a => (a.Code == MasterAssetVM.Code && a.Code != null) && a.Id != id).ToList();
                    if (lstCode.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                    }
                }
                var lstNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Name == MasterAssetVM.Name && a.ModelNumber == MasterAssetVM.ModelNumber && (a.VersionNumber == MasterAssetVM.VersionNumber && a.VersionNumber != null) && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.ModelNumber == MasterAssetVM.ModelNumber && (a.VersionNumber == MasterAssetVM.VersionNumber && a.VersionNumber != null)  && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "MasterAsset arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
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

            return Ok(MasterAssetVM.Id);
        }


        [HttpPut]
        [Route("UpdateMasterAssetImageAfterInsert")]
        public IActionResult Update(CreateMasterAssetVM masterAssetObj)
        {
            try
            {
                int updatedRow = _MasterAssetService.UpdateMasterAssetImageAfterInsert(masterAssetObj);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok(masterAssetObj.Id);
        }


        [HttpPost]
        [Route("AddMasterAsset")]
        public ActionResult Add(CreateMasterAssetVM MasterAssetVM)
        {
            if(MasterAssetVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "code must be maximum 5  charchters", MessageAr = "هذا الكود اقصى حد  له 5 حروف وأرقام " });
            }
            if (MasterAssetVM.BrandId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "brnd", Message = "You shold select Brand", MessageAr = "لابد من اختيار الماركة" });
            }
            var lstCode = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Code == MasterAssetVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.Name == MasterAssetVM.Name && a.ModelNumber == MasterAssetVM.ModelNumber && a.VersionNumber == MasterAssetVM.VersionNumber).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "MasterAsset already exist with this data", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _MasterAssetService.GetAllMasterAssets().ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.ModelNumber == MasterAssetVM.ModelNumber && a.VersionNumber == MasterAssetVM.VersionNumber).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "MasterAsset arabic already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _MasterAssetService.Add(MasterAssetVM);
                return Ok(savedId);
            }
        }

        [HttpDelete]
        [Route("DeleteMasterAsset/{id}")]
        public ActionResult<MasterAsset> Delete(int id)
        {
            try
            {
                var assetObj = _MasterAssetService.GetById(id);
                var lstHospitalAssets = _assetDetailService.ViewAllAssetDetailByMasterId(id).ToList();
                if (lstHospitalAssets.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "MasterAsset cannot be deleted", MessageAr = "لا يمكن مسح الأصل الرئيسي" });
                }
                else
                {
                    int deletedRow = _MasterAssetService.Delete(id);
                }
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
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets";
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


        [HttpPost]
        [Route("UploadMasterAssetImage")]
        [Obsolete]
        public ActionResult UploadMasterAssetImage(IFormFile file)
        {

            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }

            return StatusCode(StatusCodes.Status201Created);
        }
        //public void ReplaceFile(string fileToMoveAndDelete, string fileToReplace, string backupOfFileToReplace)
        //{
        //    // Create a new FileInfo object.    
        //    FileInfo fInfo = new FileInfo(fileToMoveAndDelete);

        //    // replace the file.    
        //    fInfo.Replace(fileToReplace, backupOfFileToReplace, true);
        //}

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
        [Route("CountMasterAssetsByBrand/{hospitalId}")]
        public List<CountMasterAssetBrands> CountMasterAssetsByBrand(int hospitalId)
        {
            return _MasterAssetService.CountMasterAssetsByBrand(hospitalId);
        }

        [HttpGet]
        [Route("CountMasterAssetsBySupplier/{hospitalId}")]
        public List<CountMasterAssetSuppliers> CountMasterAssetsBySupplier(int hospitalId)
        {
            return _MasterAssetService.CountMasterAssetsBySupplier(hospitalId);
        }


        [HttpDelete]
        [Route("DeleteMasterAssetImage/{id}")]
        public ActionResult DeleteMasterAssetImage(int id)
        {
            var masterAssetObj = _MasterAssetService.GetById(id);
            var folderPath = _webHostEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage/" + masterAssetObj.AssetImg;
            bool exists = System.IO.File.Exists(folderPath);
            if (exists)
            {
                System.IO.File.Delete(folderPath);
                masterAssetObj.AssetImg = "";
                _MasterAssetService.Update(masterAssetObj);
            }
            return Ok();
        }






        [HttpGet]
        [Route("GenerateMasterAssetcode")]
        public GeneratedMasterAssetCodeVM GenerateAssetDetailBarcode()
        {
            return _MasterAssetService.GenerateAssetDetailBarcode();
        }



    }
}
