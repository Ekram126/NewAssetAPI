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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetDetailController : ControllerBase
    {

        private const int PageSize = 10;
        private IAssetDetailService _AssetDetailService;
        private IWorkOrderService _workOrderService;
        private IAssetOwnerService _assetOwnerService;
        private IAssetStatusTransactionService _assetStatusTransactionService;
        private IAssetMovementService _assetMovementService;
        private IRequestService _requestService;
        private IPMAssetTimeService _pMAssetTimeService;
        private IPagingService _pagingService;
        private QrController _qrController;
        IWebHostEnvironment _webHostingEnvironment;
        string strInsitute, strInsituteAr, strLogo = "";
        private readonly ISettingService _settingService;

        //[Obsolete]
        //IHostingEnvironment _webHostingEnvironment;
        // private object ComponentInfo;

        [Obsolete]
        public AssetDetailController(IAssetDetailService AssetDetailService, IAssetOwnerService assetOwnerService, IWorkOrderService workOrderService,
            IPMAssetTimeService pMAssetTimeService, IPagingService pagingService, IAssetMovementService assetMovementService, IAssetStatusTransactionService assetStatusTransactionService,
            QrController qrController, IRequestService requestService, IWebHostEnvironment webHostingEnvironment, ISettingService settingService)
        {
            _AssetDetailService = AssetDetailService;
            _assetMovementService = assetMovementService;
            _requestService = requestService;
            _webHostingEnvironment = webHostingEnvironment;
            _assetOwnerService = assetOwnerService;
            _pMAssetTimeService = pMAssetTimeService;
            _pagingService = pagingService;
            _qrController = qrController;
            _settingService = settingService;
            _workOrderService = workOrderService;
            _assetStatusTransactionService = assetStatusTransactionService;
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
        [Route("GetAssetHistoryById/{assetId}")]
        public ViewAssetDetailVM GetAssetHistoryById(int assetId)
        {
            return _AssetDetailService.GetAssetHistoryById(assetId);
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
        [Route("SearchHospitalAssetsByDepartmentId/{departmentId}/{userId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize)
        {
            var lstAssets = _AssetDetailService.SearchHospitalAssetsByDepartmentId(departmentId, userId, pageNumber, pageSize);
            return lstAssets;
        }


        [HttpPost]
        [Route("SearchAssetDetails/{pagenumber}/{pagesize}")]
        public IndexAssetDetailVM SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            var list = _AssetDetailService.SearchAssetInHospital(pagenumber, pagesize, searchObj);
            return list;// _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list);
        }
        [HttpPost]
        [Route("FilterDataByDepartmentBrandSupplierId")]
        public List<IndexAssetDetailVM.GetData> FilterDataByDepartmentBrandSupplierId(FilterHospitalAsset data)
        {
            var list = _AssetDetailService.FilterDataByDepartmentBrandSupplierId(data);
            return list;
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
        [Route("GenerateAssetDetailBarcode")]
        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            return _AssetDetailService.GenerateAssetDetailBarcode();
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
        [Route("GetListOfAssetDetailsByHospitalNotInContract2/{barcode}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract2(string barcode, int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContract(barcode, hospitalId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalNotInContractBySerialNumber/{serialNumber}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(serialNumber, hospitalId);
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

        [HttpPost]
        [Route("GetAssetsByUserIdAndPaging/{userId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM GetAssetsByUserIdAndPaging(string userId, int pageNumber, int pageSize)
        {
            return _AssetDetailService.GetAssetsByUserId(userId, pageNumber, pageSize);
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



        [HttpGet]
        [Route("GetAllPMAssetTaskScheduleByAssetId/{assetId}")]
        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId)
        {
            return _AssetDetailService.GetAllPMAssetTaskScheduleByAssetId(assetId);
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
                var lstWO = _workOrderService.GetLastRequestAndWorkOrderByAssetId(id).ToList();
                if (lstWO.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "wo", Message = "You cannot delete this asset it has workorders", MessageAr = "لا يمكن مسح هذا الأصل لأن له  أوامر شغل" });
                }
                else
                {

                    var lstOwners = _assetOwnerService.GetOwnersByAssetDetailId(id).ToList();
                    if (lstOwners.Count > 0)
                    {
                        foreach (var item in lstOwners)
                        {
                            _assetOwnerService.Delete(item.Id);
                        }
                    }

                    var lstAssetTransactions = _assetStatusTransactionService.GetAssetStatusByAssetDetailId(id).ToList();
                    if (lstAssetTransactions.Count > 0)
                    {
                        foreach (var item in lstAssetTransactions)
                        {
                            _assetStatusTransactionService.Delete(item.Id);
                        }
                    }


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
        [Route("Group/{assetId}")]
        public IEnumerable<PmDateGroupVM> GetEquimentswithgrouping(int assetId)
        {
            return _AssetDetailService.GetAllwithgrouping(assetId);
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
        [Route("GetAssetByDepartment")]
        public ActionResult<List<DepartmentGroupVM>> GetAssetByDepartment(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByDepartment(AssetModel);
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
        [Route("SortAssets2/{statusId}/{userId}")]
        public IndexAssetDetailVM SortAssets(Sort sortObj, int statusId, string userId)
        {
            var assetDetailData = _AssetDetailService.SortAssets(sortObj, statusId, userId);
            return assetDetailData;
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

        [HttpPost]
        [Route("CreateAssetDepartmentBrandSupplierPDF")]
        public void CreateAssetDepartmentBrandSupplierPDF(FilterHospitalAsset filterHospitalAssetObj)
        {

            var lstSettings = _settingService.GetAll().ToList();
            if (lstSettings.Count > 0)
            {
                foreach (var item in lstSettings)
                {
                    if (item.KeyName == "Institute")
                    {
                        strInsitute = item.KeyValue;
                        strInsituteAr = item.KeyValueAr;
                    }

                    if (item.KeyName == "Logo")
                        strLogo = item.KeyValue;
                }
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.NewPage();
            document.Open();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string adobearabic = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfUniCode = BaseFont.CreateFont(adobearabic, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfUniCode, 14);

            Phrase ph = new Phrase(" ", font);
            document.Add(ph);

            PdfPTable bodytable = AssetDepartmentBrandSupplier(filterHospitalAssetObj);
            int countnewpages = bodytable.Rows.Count / 25;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf", bytes);


            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + filterHospitalAssetObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
                }
                //Header
                for (int i = 1; i <= pages; i++)
                {
                    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/" + strLogo;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    jpg.ScaleAbsolute(70f, 50f);
                    PdfPTable headertable = new PdfPTable(2);
                    headertable.SetTotalWidth(new float[] { 250f, 50f });
                    headertable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    headertable.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new PdfPCell(jpg));
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 10;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);

                    if (filterHospitalAssetObj.Lang == "ar")
                    {
                        headertable.AddCell(new PdfPCell(new Phrase("\t\t\t\t " + strInsituteAr + "\n" + filterHospitalAssetObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    }
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(" " + strInsitute + "\n" + filterHospitalAssetObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    headertable.WriteSelectedRows(0, -1, 270, 830, stamper.GetOverContent(i));

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    string adobearabicheaderTitle = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                    BaseFont bfUniCodeheaderTitle = BaseFont.CreateFont(adobearabicheaderTitle, BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCodeheaderTitle, 13);
                    titlefont.SetStyle("bold");


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.SetTotalWidth(new float[] { 600f });
                    titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    titleTable.WidthPercentage = 100;
                    titleTable.AddCell(new PdfPCell(new Phrase("تقرير الأجهزة بالأقسام والموردين والماركات", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    if (filterHospitalAssetObj.Start == "")
                        filterHospitalAssetObj.Start = "01/01/1900";

                    var sDate = DateTime.Parse(filterHospitalAssetObj.Start);
                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (filterHospitalAssetObj.End == "")
                        filterHospitalAssetObj.End = DateTime.Today.Date.ToShortDateString();

                    var eDate = DateTime.Parse(filterHospitalAssetObj.End);
                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    titleTable.WriteSelectedRows(0, -1, 0, 760, stamper.GetOverContent(i));
                }
                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(8);
                    bodytable2.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;

                    bodytable2.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 7 });
                    int countRows = bodytable.Rows.Count;
                    if (countRows > 25)
                    {
                        countRows = 25;
                    }
                    bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count; k++)
                    {
                        bodytable.DeleteRow(1);
                    }
                    bodytable2.WriteSelectedRows(0, -1, 10, 700, stamper.GetUnderContent(i));
                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf", bytes);
            memoryStream.Close();
            document.Close();

        }
        public PdfPTable AssetDepartmentBrandSupplier(FilterHospitalAsset filterHospitalAssetObj)
        {

            var lstData = _AssetDetailService.FilterDataByDepartmentBrandSupplierId(filterHospitalAssetObj).ToList();
            PdfPTable table = new PdfPTable(8);
            table.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 7 });
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 10);


            if (filterHospitalAssetObj.selectedElement == "supplier" || filterHospitalAssetObj.selectedElement == "المورد")
            {
                var lstAssetsByBrand = _AssetDetailService.GetAssetBySupplier(lstData).ToList();
                foreach (var item in lstAssetsByBrand)
                {
                    // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

                    PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
                    c1.Colspan = 8;
                    table.AddCell(c1);



                    string[] col = { "المورد", "الماركة", "القسم", "الموديل", "السيريال", "الباركود", "الاسم", "م" };
                    string[] encol = { "No.", "Name", "Barcode", "Serial", "Model", "Department", "Brand", "Supplier" };
                    if (filterHospitalAssetObj.Lang == "ar")
                    {
                        for (int i = col.Length - 1; i >= 0; i--)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                            cell.PaddingBottom = 10;
                            table.AddCell(cell);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= encol.Length - 1; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(encol[i]));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                            cell.PaddingBottom = 10;
                            table.AddCell(cell);
                        }
                    }
                    if (item.AssetList.Count > 0)
                    {
                        int index = 0;
                        foreach (var groupItems in item.AssetList)
                        {
                            ++index;
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
                            if (groupItems.PurchaseDate != null)
                                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                            else
                                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }
                    }
                }
            }
            //if (filterHospitalAssetObj.selectedElement == "brand" || filterHospitalAssetObj.selectedElement == "الصانع")
            //{
            //    var lstAssetsByBrand = _AssetDetailService.GetAssetByBrands(lstData).ToList();
            //    foreach (var item in lstAssetsByBrand)
            //    {
            //        // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

            //        PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
            //        c1.Colspan = 8;
            //        table.AddCell(c1);

            //        foreach (var groupItems in item.AssetList)
            //        {
            //           // ++index;
            //           // table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (groupItems.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}
            //if (filterHospitalAssetObj.selectedElement == "Department" || filterHospitalAssetObj.selectedElement == "القسم")
            //{
            //    var lstAssetsByBrand = _AssetDetailService.GetAssetByDepartment(lstData).ToList();
            //    foreach (var item in lstAssetsByBrand)
            //    {
            //        // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

            //        PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
            //        c1.Colspan = 8;
            //        table.AddCell(c1);

            //        foreach (var groupItems in item.AssetList)
            //        {
            //            ++index;
            //            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (groupItems.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}
            //else
            //{

            //    foreach (var item in lstData)
            //    {
            //        //  table.AddCell(new PdfPCell(new Phrase("R3C1-4")) { Colspan = 8 });
            //        ++index;
            //        if (filterHospitalAssetObj.Lang == "ar")
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (item.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //        else
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.AssetName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.DepartmentName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.BrandName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SupplierName, font)) { PaddingBottom = 5 });
            //            if (item.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(DateTime.Parse(item.PurchaseDate.ToString()).ToString("g", new CultureInfo("en-US")), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}

            return table;
        }

        [HttpGet]
        [Route("DownloadAssetDepartmentBrandSupplierPDF")]
        public HttpResponseMessage DownloadFile()
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf";
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
                System.IO.Directory.CreateDirectory(file);
            //return new HttpResponseMessage(HttpStatusCode.Gone);
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
                response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(fStream)
                };
                response.Content.Headers.ContentDisposition =
                                            new ContentDispositionHeaderValue("attachment")
                                            {
                                                FileName = Path.GetFileName(fStream.Name)
                                            };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            return response;
        }






        [HttpPost]
        [Route("GenerateQrCodeForAllAssets")]
        public bool GenerateQrCodeForAllAssets(string domainName)
        {

            var path = HttpContext.Request.Path;
            var pathBase = HttpContext.Request.PathBase;
            domainName = "http://" + HttpContext.Request.Host.Value;
            return _AssetDetailService.GenerateQrCodeForAllAssets(domainName);
        }



        [Route("GenerateWordForQrCodeForAllAssets")]
        public ActionResult GenerateWordForQrCodeForAllAssets()
        {

            using (WordDocument document = new WordDocument())
            {
                //Opens the Word template document
                string strTemplateFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\CardTemplate.dotx";

                Stream docStream = System.IO.File.OpenRead(strTemplateFile);
                document.Open(docStream, FormatType.Docx);
                docStream.Dispose();


                var allAssets = ListAssets().ToList();
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField_InsertPageBreak);
                MailMergeDataTable dataTable = new MailMergeDataTable("Asset_QrCode", allAssets);
                document.MailMerge.ExecuteGroup(dataTable);


                //Saves the file in the given path
                string strExportFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\Cards.docx";
                docStream = System.IO.File.Create(strExportFile);
                document.Save(docStream, FormatType.Docx);
                docStream.Dispose();
                document.Close();


            }
            return Ok();
        }

        int i = 1;
        private void MergeField_InsertPageBreak(object sender, MergeFieldEventArgs args)
        {

            List<IndexAssetDetailVM.GetData> allAssets = ListAssets();

            if (args.FieldName == "BarCode" && i != allAssets.Count)
            {
                //Gets the owner paragraph 
                WParagraph paragraph = args.CurrentMergeField.OwnerParagraph;
                //Appends the page break 
                paragraph.AppendBreak(BreakType.PageBreak);
                i++;
            }

        }

        private List<IndexAssetDetailVM.GetData> ListAssets()
        {
            var allAssets = _AssetDetailService.GetAll().ToList();
            return allAssets;
        }



        private DataTable GetAssetsAsDataTable()
        {

            var allAssets = _AssetDetailService.GetAll().ToList();
            DataTable table = new DataTable("Asset_QrCode");
            // table.TableName = "QrCode";
            // Add fields to the Product_PriceList table.
            table.Columns.Add("AssetName");
            table.Columns.Add("BrandName");
            table.Columns.Add("SerialNumber");
            table.Columns.Add("Model");
            table.Columns.Add("BarCode");
            table.Columns.Add("QRFilePath");
            table.Columns.Add("HospitalNameAr");
            // DataRow row;

            // Inserting values to the tables.
            foreach (var item in allAssets)
            {
                DataRow row = table.NewRow();
                row["AssetName"] = item.AssetName;
                row["BrandName"] = item.BrandName;
                row["SerialNumber"] = item.SerialNumber;
                row["Model"] = item.Model;
                row["BarCode"] = item.BarCode;
                row["QRFilePath"] = item.QrFilePath;
                row["HospitalNameAr"] = item.QrFilePath;
                table.Rows.Add(row);

            }
            return table;
        }

    }
}
