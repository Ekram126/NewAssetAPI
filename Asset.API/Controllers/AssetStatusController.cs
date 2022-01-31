using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetStatusVM;
using Asset.ViewModels.PagingParameter;
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
    public class AssetStatusController : ControllerBase
    {

        private IAssetStatusService _assetStatusService;
        private IPagingService _pagingService;

        public AssetStatusController(IAssetStatusService assetStatusService, IPagingService pagingService)
        {
            _assetStatusService = assetStatusService;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListAssetStatus")]
        public IEnumerable<IndexAssetStatusVM.GetData> GetAll()
        {
            return _assetStatusService.GetAll();
        }


        [HttpPut]
        [Route("GetAssetStatusWithPaging")]
        public IEnumerable<IndexAssetStatusVM.GetData> GetAssetStatusWithPaging(PagingParameter pageInfo)
        {
            var HospitalAssets = _assetStatusService.GetAll().ToList();
            return _pagingService.GetAll<IndexAssetStatusVM.GetData>(pageInfo, HospitalAssets);
        }


        [HttpGet]
        [Route("GetAssetStatusCount")]
        public int GetAssetStatusCount()
        {
            return _assetStatusService.GetAll().ToList().Count;// _AssetDetailService.GetAll().ToList().Count();
        }


        [HttpPost]
        [Route("SortAssetStatuses/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetStatusVM.GetData> SortAssetStatuses(int pagenumber, int pagesize, SortAssetStatusVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _assetStatusService.SortAssetStatuses(sortObj);
            return _pagingService.GetAll<IndexAssetStatusVM.GetData>(pageInfo, list.ToList());
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditAssetStatusVM> GetById(int id)
        {
            return _assetStatusService.GetById(id);
        }
        [HttpPut]
        [Route("UpdateAssetStatus")]
        public IActionResult Update(EditAssetStatusVM AssetStatusVM)
        {
            try
            {
                int id = AssetStatusVM.Id;
                var lstCode = _assetStatusService.GetAll().ToList().Where(a => a.Code == AssetStatusVM.Code && a.Id != id).ToList();
                if (lstCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "AssetStatus code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstNames = _assetStatusService.GetAll().ToList().Where(a => a.Name == AssetStatusVM.Name && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _assetStatusService.GetAll().ToList().Where(a => a.NameAr == AssetStatusVM.NameAr && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _assetStatusService.Update(AssetStatusVM);
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
        [Route("AddAssetStatus")]
        public ActionResult<AssetStatu> Add(CreateAssetStatusVM AssetStatusVM)
        {
            var lstCode = _assetStatusService.GetAll().ToList().Where(a => a.Code == AssetStatusVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "AssetStatus code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _assetStatusService.GetAll().ToList().Where(a => a.Name == AssetStatusVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _assetStatusService.GetAll().ToList().Where(a => a.NameAr == AssetStatusVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "AssetStatus arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _assetStatusService.Add(AssetStatusVM);
                return CreatedAtAction("GetById", new { id = savedId }, AssetStatusVM);
            }
        }

        [HttpDelete]
        [Route("DeleteAssetStatus/{id}")]
        public ActionResult<AssetStatu> Delete(int id)
        {
            try
            {

                int deletedRow = _assetStatusService.Delete(id);
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
