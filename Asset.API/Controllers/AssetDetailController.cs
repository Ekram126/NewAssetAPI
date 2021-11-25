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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetDetailController : ControllerBase
    {
        private IAssetDetailService _AssetDetailService;

        private IAssetOwnerService _assetOwnerService;

        private IPMAssetTimeService _pMAssetTimeService;
        private IPagingService _pagingService;

        [Obsolete]
        IHostingEnvironment _webHostingEnvironment;

        [Obsolete]
        public AssetDetailController(IAssetDetailService AssetDetailService, IAssetOwnerService assetOwnerService,
            IPMAssetTimeService pMAssetTimeService, IPagingService pagingService,
            IHostingEnvironment webHostingEnvironment)
        {
            _AssetDetailService = AssetDetailService;
            _webHostingEnvironment = webHostingEnvironment;
            _assetOwnerService = assetOwnerService;
            _pMAssetTimeService = pMAssetTimeService;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListAssetDetails")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            return _AssetDetailService.GetAll();
        }
        [HttpPut]
        [Route("ListAssetDetailsWithPaging")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var HospitalAssets = _AssetDetailService.GetAll().ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, HospitalAssets);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<AssetDetail>();
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
        [Route("SearchAssetDetails")]
        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetInHospital(SearchMasterAssetVM model)
        {
            return _AssetDetailService.SearchAssetInHospital(model);
        }


        [HttpPost]
        [Route("SearchAssetDetailsByHospitalId")]
        public IEnumerable<IndexAssetDetailVM.GetData> SearchAssetDetailsByHospitalId(SearchMasterAssetVM model)
        {
            return _AssetDetailService.SearchAssetInHospitalByHospitalId(model);
        }



        [HttpPost]
        [Route("SearchAssetDetails/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetDetailVM.GetData> SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _AssetDetailService.SearchAssetInHospital(searchObj).ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchAssetDetailsCount")]
        public int SearchInMasterAssetsCount(SearchMasterAssetVM searchObj)
        {
            int count = _AssetDetailService.SearchAssetInHospital(searchObj).ToList().Count();
            return count;
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
        public ActionResult<EditAssetDetailVM> GetById(int id)
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
        [Route("ViewAllAssetDetailByMasterId/{MasterAssetId}")]
        public IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId)
        {
            return _AssetDetailService.ViewAllAssetDetailByMasterId(MasterAssetId);
        }



        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalId(hospitalId);
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

                int updatedRow = _AssetDetailService.Update(AssetDetailVM);

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
            var savedId = _AssetDetailService.Add(AssetDetailVM);
            return Ok(new { assetId = savedId });

        }

        [HttpDelete]
        [Route("DeleteAssetDetail/{id}")]
        public ActionResult<AssetDetail> Delete(int id)
        {
            try
            {

                int deletedRow = _AssetDetailService.Delete(id);
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
            Stream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
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
            var list = _AssetDetailService.SortAssets(sortObj).ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list);
        }
    }
}
