using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.GovernorateVM;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.PMAssetTaskScheduleVM;
using Asset.ViewModels.PmAssetTimeVM;
using Asset.ViewModels.SupplierVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Asset.API.Helpers;
using System.Data.Entity;



namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetDetailController : ControllerBase
    {

        private const int PageSize = 10;
        private IAssetDetailService _AssetDetailService;

        private IAssetOwnerService _assetOwnerService;
        private IAssetMovementService _assetMovementService;
        private IRequestService _requestService;
        private IPMAssetTimeService _pMAssetTimeService;
        private IPagingService _pagingService;
        private QrController _qrController;

        [Obsolete]
        IHostingEnvironment _webHostingEnvironment;
        // private object ComponentInfo;

        [Obsolete]
        public AssetDetailController(IAssetDetailService AssetDetailService, IAssetOwnerService assetOwnerService,
            IPMAssetTimeService pMAssetTimeService, IPagingService pagingService, IAssetMovementService assetMovementService,
            QrController qrController, IRequestService requestService,
            IHostingEnvironment webHostingEnvironment)
        {
            _AssetDetailService = AssetDetailService;
            _assetMovementService = assetMovementService;
            _requestService = requestService;
            _webHostingEnvironment = webHostingEnvironment;
            _assetOwnerService = assetOwnerService;
            _pMAssetTimeService = pMAssetTimeService;
            _pagingService = pagingService;
            _qrController = qrController;
        }
        [HttpGet]
        [Route("ListAssetDetails")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            return _AssetDetailService.GetAll();
        }





        [HttpPost]
        [Route("GetHospitalAssets/{hospitalId}/{statusId}/{userId}/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            return _AssetDetailService.GetHospitalAssets(hospitalId, statusId, userId, page, pageSize, sortObj);
        }

        [HttpPost]
        [Route("CountHospitalAssets/{hospitalId}/{statusId}/{userId}/{pagenumber}/{pagesize}")]
        public int CountHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            return _AssetDetailService.GetHospitalAssets(hospitalId, statusId, userId, page, pageSize, sortObj).Count();
        }






        [HttpPut]
        [Route("ListAssetDetailsWithPaging")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var HospitalAssets = _AssetDetailService.GetAll().ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, HospitalAssets);
        }
        [HttpGet]
        [Route("ListAssetDetailCarouselByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> ListAssetDetailCarouselByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetDetailsByUserId(userId);
        }
        [HttpGet]
        [Route("AutoCompleteAssetBarCode/{barcode}/{hospitalId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            return _AssetDetailService.AutoCompleteAssetBarCode(barcode, hospitalId);
        }

        [HttpGet]
        [Route("AutoCompleteAssetSerial/{serial}/{hospitalId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            return _AssetDetailService.AutoCompleteAssetSerial(serial, hospitalId);
        }

        [HttpGet]
        [Route("GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId/{barcode}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId)
        {
            return _AssetDetailService.GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(barcode, hospitalId);
        }






        [HttpGet]
        [Route("getcount/{userId}")]
        public int count(string userId)
        {
            return _AssetDetailService.GetAssetDetailsByUserId(userId).Result.ToList().Count;// _AssetDetailService.GetAll().ToList().Count();
        }
        [HttpGet]
        [Route("GetAllSerialsByMasterAssetIdAndHospitalId/{masterAssetId}/{hospitalId}")]
        public IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId)
        {
            return _AssetDetailService.GetAllSerialsByMasterAssetIdAndHospitalId(masterAssetId, hospitalId);
        }
        [HttpGet]
        [Route("GetAllAssetDetailsByHospitalId/{hospitalId}")]
        public IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetAllAssetDetailsByHospitalId(hospitalId);
        }
        [HttpPost]
        [Route("GetAllAssetsByStatusId/{statusId}/{userId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAllRequestsByStatusId(int statusId, string userId, PagingParameter pageInfo)
        {
            var lstAssets = _AssetDetailService.GetAllAssetsByStatusId(statusId, userId).ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, lstAssets);
        }
        [HttpPost]
        [Route("GetAllAssetsCountByStatusId/{statusId}/{userId}")]
        public int GetCountByStatusId(int statusId, string userId)
        {
            return _AssetDetailService.GetAllAssetsByStatusId(statusId, userId).ToList().Count;
        }



        [HttpPost]
        [Route("GetAllAssetsByStatusId2/{pagenumber}/{pagesize}/{statusId}/{userId}")]
        public IndexAssetDetailVM GetAllRequestsByStatusId(int pageNumber, int pageSize, int statusId, string userId)
        {
            var lstAssets = _AssetDetailService.GetAllAssetsByStatusId(pageNumber, pageSize, statusId, userId);
            return lstAssets;
        }



        [HttpPost]
        [Route("SearchAssetDetails/{pagenumber}/{pagesize}")]
        public IndexAssetDetailVM SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            var list = _AssetDetailService.SearchAssetInHospital(pagenumber, pagesize, searchObj);
            return list;// _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list);
        }



        [HttpGet]
        [Route("GetAssetDetailsByAssetId/{assetId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId)
        {
            return _AssetDetailService.GetAssetDetailsByAssetId(assetId);
        }
        [HttpGet]
        [Route("GetDateByAssetDetailId/{assetDetailId}")]
        public IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _pMAssetTimeService.GetDateByAssetDetailId(assetDetailId);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public EditAssetDetailVM GetById(int id)
        {
            return _AssetDetailService.GetById(id);
        }


        [HttpGet]
        [Route("ViewAssetDetailByMasterId/{masterId}")]
        public ActionResult<ViewAssetDetailVM> ViewAssetDetailByMasterId(int masterId)
        {
            return _AssetDetailService.ViewAssetDetailByMasterId(masterId);
        }




        [HttpGet]
        [Route("AlertAssetsBefore3Monthes")]
        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes()
        {
            return _AssetDetailService.AlertAssetsBefore3Monthes();
        }


        [HttpGet]
        [Route("AlertAssetsBefore3MonthesWithDuration/{duration}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration)
        {
            return _AssetDetailService.AlertAssetsBefore3Monthes(duration);
        }




        [HttpGet]
        [Route("ViewAllAssetDetailByMasterId/{MasterAssetId}")]
        public IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId)
        {
            return _AssetDetailService.ViewAllAssetDetailByMasterId(MasterAssetId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalNotInContract/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContract(hospitalId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetNoneExcludedAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetNoneExcludedAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetSupplierNoneExcludedAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetSupplierNoneExcludedAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetAssetDetailsByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetDetailsByUserId(userId);
        }
        [HttpGet]
        [Route("GetAssetsByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetsByUserId(userId);
        }
        [HttpPut]
        [Route("GetAssetDetailsByUserIdWithPaging/{userId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByUserId(string userId, PagingParameter pageInfo)
        {
            var AssetDetail = _AssetDetailService.GetAssetDetailsByUserId(userId).Result.ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, AssetDetail);
        }



        [HttpPost]
        [Route("GetAssetDetailsByUserIdWithPaging2/{pagenumber}/{pagesize}/{userId}")]
        public async Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId)
        {
            var lstAssetDetails = await _AssetDetailService.GetAssetDetailsByUserId2(pageNumber, pageSize, userId);
            return lstAssetDetails;
        }



        [HttpGet]
        [Route("GetAllPMAssetTaskSchedules/{hospitalId}")]
        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId)
        {
            return _AssetDetailService.GetAllPMAssetTaskSchedules(hospitalId);
        }
        [HttpPut]
        [Route("UpdateAssetDetail")]
        public IActionResult Update(EditAssetDetailVM AssetDetailVM)
        {
            try
            {
                //a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber
                int id = AssetDetailVM.Id;
                if (!string.IsNullOrEmpty(AssetDetailVM.Code))
                {
                    var lstCode = _AssetDetailService.GetAll().Where(a => a.Code == AssetDetailVM.Code && a.Id != id).ToList();
                    if (lstCode.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                    }
                }
                var lstNames = _AssetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "serial", Message = "Asset serial already exist", MessageAr = "هذا السيريال مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _AssetDetailService.Update(AssetDetailVM);
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
        [Route("AddAssetDetail")]
        public ActionResult<AssetDetail> Add(CreateAssetDetailVM AssetDetailVM)
        {
            var lstCode = _AssetDetailService.GetAll().ToList().Where(a => a.Code == AssetDetailVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _AssetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Asset already exist with this data", MessageAr = "هذا الجهاز مسجل سابقاً" });
            }
            else
            {
                var savedId = _AssetDetailService.Add(AssetDetailVM);
                _qrController.Index(AssetDetailVM.Id);
                CreateAssetDetailAttachmentVM qrAttach = new CreateAssetDetailAttachmentVM();
                qrAttach.AssetDetailId = AssetDetailVM.Id;
                qrAttach.FileName = "asset-" + AssetDetailVM.Id + ".png";
                CreateAssetDetailAttachments(qrAttach);
                return Ok(new { assetId = savedId });
            }


            // return Ok(new { assetId = AssetDetailVM.Id });

        }

        [HttpDelete]
        [Route("DeleteAssetDetail/{id}")]
        public ActionResult<AssetDetail> Delete(int id)
        {
            try
            {
                var assetObj = _AssetDetailService.GetById(id);
                var lstMovements = _assetMovementService.GetMovementByAssetDetailId(id).ToList();
                if (lstMovements.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "move", Message = "You cannot delete this asset it has movement", MessageAr = "لا يمكن مسح هذا الأصل لأن له حركات في المستشفى" });
                }
                var lstRequests = _requestService.GetAllRequestsByAssetId(id, int.Parse(assetObj.HospitalId.ToString())).ToList();
                if (lstRequests.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "request", Message = "You cannot delete this asset it has requests", MessageAr = "لا يمكن مسح هذا الأصل لأن له بلاغات صيانة " });
                }
                else
                {
                    int deletedRow = _AssetDetailService.Delete(id);
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
        [Route("CreateAssetDetailAttachments")]
        public int CreateAssetDetailAttachments(CreateAssetDetailAttachmentVM attachObj)
        {
            return _AssetDetailService.CreateAssetDetailDocuments(attachObj);
        }
        [HttpPost]
        [Route("UploadAssetDetailFiles")]
        [Obsolete]
        public ActionResult UploadInFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/" + file.FileName;
            if (!System.IO.File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("GetOwnersByAssetDetailId/{assetDetailId}")]
        public List<AssetOwner> GetOwnersByAssetDetailId(int assetDetailId)
        {
            return _assetOwnerService.GetOwnersByAssetDetailId(assetDetailId).ToList();
        }

        [HttpGet]
        [Route("GetAttachmentByAssetDetailId/{assetId}")]
        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _AssetDetailService.GetAttachmentByAssetDetailId(assetId);
        }

        [HttpDelete]
        [Route("DeleteAssetDetailAttachment/{id}")]
        public int DeleteAssetDetailAttachment(int id)
        {
            return _AssetDetailService.DeleteAssetDetailAttachment(id);
        }
        [HttpGet]
        [Route("CountAssetsByHospital")]
        public IEnumerable<CountAssetVM> CountAssetsByHospital()
        {
            return _AssetDetailService.CountAssetsByHospital();
        }

        [HttpGet]
        [Route("ListTopAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.ListTopAssetsByHospitalId(hospitalId);
        }



        [HttpGet]
        [Route("ListAssetsByGovernorateIds")]
        public IEnumerable<CountAssetVM> ListAssetsByGovernorateIds()
        {
            return _AssetDetailService.ListAssetsByGovernorateIds();
        }

        [HttpGet]
        [Route("ListAssetsByCityIds")]
        public IEnumerable<CountAssetVM> ListAssetsByCityIds()
        {
            return _AssetDetailService.ListAssetsByCityIds();
        }




        [HttpGet]
        [Route("CountAssetsInHospitalByHospitalId/{hospitalId}")]
        public IEnumerable<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId)
        {
            return _AssetDetailService.CountAssetsInHospitalByHospitalId(hospitalId);
        }









        [HttpGet]
        [Route("CountAssetsByHospitalId/{hospitalId}")]
        public int CountAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.CountAssetsByHospitalId(hospitalId);
        }

        [HttpGet]
        [Route("Group/{masterId}")]
        public IEnumerable<PmDateGroupVM> GetEquimentswithgrouping(int masterId)
        {
            return _AssetDetailService.GetAllwithgrouping(masterId);
        }

        [HttpGet]
        [Route("MonthDiff/{d1}/{d2}")]
        public int MonthDiff(DateTime d1, DateTime d2)
        {
            int m1;
            int m2;
            if (d1 < d2)
            {
                m1 = (d2.Month - d1.Month);//for years
                m2 = (d2.Year - d1.Year) * 12; //for months
            }
            else
            {
                m1 = (d1.Month - d2.Month);//for years
                m2 = (d1.Year - d2.Year) * 12; //for months
            }

            return m1 + m2;
        }

        [Route("FilterAsset")]
        [HttpPost]
        public ActionResult<List<IndexAssetDetailVM.GetData>> FilterAsset(filterDto data)
        {
            return _AssetDetailService.FilterAsset(data);
        }
        [HttpPost]
        [Route("GetAssetByBrands")]
        public ActionResult<List<BrandGroupVM>> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByBrands(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByHospital")]
        public ActionResult<List<GroupHospitalVM>> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByHospital(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByGovernorate")]
        public ActionResult<List<GroupGovernorateVM>> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByGovernorate(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByCity")]
        public ActionResult<List<GroupCityVM>> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByCity(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetBySupplier")]
        public ActionResult<List<GroupSupplierVM>> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetBySupplier(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByOrganization")]
        public ActionResult<List<GroupOrganizationVM>> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByOrganization(AssetModel);
        }
        [HttpPost]
        [Route("SortAssets/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetDetailVM.GetData> SortAssets(int pagenumber, int pagesize, Sort sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _AssetDetailService.SortAssets(sortObj);
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list.ToList());
        }


        [HttpPost]
        [Route("SortAssetsCount")]
        public int SortAssets(Sort sortObj)
        {
            var list = _AssetDetailService.SortAssets(sortObj);
            // var list = await Task.Run(() => _AssetDetailService.SortAssets(sortObj));

            var count = list.Count();
            return count;
        }

        [HttpGet]
        [Route("GetAssetsByAgeGroup/{hospitalId}")]
        public List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId)
        {
            var list = _AssetDetailService.GetAssetsByAgeGroup(hospitalId);
            return list;
        }
        [HttpPost]
        [Route("GetGeneralAssetsByAgeGroup")]
        public List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge model)
        {
            var list = _AssetDetailService.GetGeneralAssetsByAgeGroup(model);
            return list;
        }




        [Route("GetLastDocumentForAssetDetailId/{assetDetailId}")]
        public AssetDetailAttachment GetLastDocumentForWorkOrderTrackingId(int assetDetailId)
        {
            return _AssetDetailService.GetLastDocumentForAssetDetailId(assetDetailId);
        }
    }
}
