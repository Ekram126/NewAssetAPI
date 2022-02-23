using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
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
    public class HospitalApplicationController : ControllerBase
    {

        private IHospitalApplicationService _hospitalApplicationService;
        private IPagingService _pagingService;
        //  [Obsolete]
        IWebHostEnvironment _webHostingEnvironment;
        public HospitalApplicationController(IHospitalApplicationService hospitalApplicationService, IWebHostEnvironment webHostingEnvironment, IPagingService pagingService)
        {
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;

            _pagingService = pagingService;
        }

        [HttpGet]
        [Route("ListHospitalApplications")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAll()
        {
            return _hospitalApplicationService.GetAll();
        }

        [HttpPut]
        [Route("ListHospitalApplicationsWithPaging")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAll().ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }



        [HttpPut]
        [Route("ListHospitalApplicationsWithPaging/{hospitalId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllWithPaging(int? hospitalId,PagingParameter pageInfo)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();

            if (hospitalId != 0)

                list = _hospitalApplicationService.GetAllByHospitalId(int.Parse(hospitalId.ToString())).ToList();
            else
                list = _hospitalApplicationService.GetAll().ToList();


            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, list);
        }


        [HttpGet]
        [Route("getcount/{hospitalId}")]
        public int count(int? hospitalId)
        {
                   int listCount = 0;
            if (hospitalId != 0)

                listCount = _hospitalApplicationService.GetAllByHospitalId(int.Parse(hospitalId.ToString())).ToList().Count;
            else
                listCount = _hospitalApplicationService.GetAll().ToList().Count;


            return listCount; //_pagingService.Count<HospitalApplication>();
        }

        [HttpPut]
        [Route("GetAllHospitalsByStatusId/{statusId}/{hospitalId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(PagingParameter pageInfo, int statusId, int hospitalId)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAllByStatusId(statusId,hospitalId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }

        [HttpGet]
        [Route("GetHospitalCountAfterFilterStatusId/{statusId}/{hospitalId}")]
        public int GetCountAfterFilterStatusId(int statusId,int hospitalId)
        {
            return _hospitalApplicationService.GetAllByStatusId(statusId,hospitalId).ToList().Count;
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalApplicationVM> GetById(int id)
        {
            return _hospitalApplicationService.GetById(id);
        }

        [HttpGet]
        [Route("GetHospitalApplicationById/{id}")]
        public ActionResult<ViewHospitalApplicationVM> GetHospitalApplicationById(int id)
        {
            return _hospitalApplicationService.GetHospitalApplicationById(id);
        }

        [HttpGet]
        [Route("GetAssetHospitalId/{assetId}")]
        public ActionResult<int> GetAssetHospitalId(int assetId)
        {
            return _hospitalApplicationService.GetAssetHospitalId(assetId);
        }

        [HttpPut]
        [Route("ListHospitalApplicationsWithPagingAndTypeId/{appTypeId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> ListHospitalApplicationsWithPagingAndTypeId(int appTypeId, PagingParameter pageInfo)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAllByAppTypeId(appTypeId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }
        [HttpGet]
        [Route("GetCountByAppTypeId/{appTypeId}")]
        public int GetcountByAppTypeId(int appTypeId)
        {
            return _hospitalApplicationService.GetAllByAppTypeId(appTypeId).ToList().Count;
        }





        [HttpPut]
        [Route("ListHospitalApplicationsByTypeIdAndStatusId/{hospitalId}/{appTypeId}/{statusId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> ListHospitalApplicationsByTypeIdAndStatusId(int hospitalId,int appTypeId,int statusId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _hospitalApplicationService.GetAllByAppTypeIdAndStatusId(hospitalId, appTypeId, statusId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("GetcountByAppTypeIdAndStatusId/{hospitalId}/{appTypeId}/{statusId}")]
        public int GetcountByAppTypeIdAndStatusId(int hospitalId,int appTypeId, int statusId)
        {
            return _hospitalApplicationService.GetAllByAppTypeIdAndStatusId(hospitalId, appTypeId,statusId).ToList().Count;
        }




        [HttpPut]
        [Route("UpdateHospitalApplication")]
        public IActionResult Update(EditHospitalApplicationVM hospitalApplicationVM)
        {
            try
            {
                int id = hospitalApplicationVM.Id;
                int updatedRow = _hospitalApplicationService.Update(hospitalApplicationVM);
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
        public IActionResult UpdateExcludedDate(EditHospitalApplicationVM model)
        {
            try
            {

                int updatedRow = _hospitalApplicationService.UpdateExcludedDate(model);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddHospitalApplication")]
        public int Add(CreateHospitalApplicationVM hospitalApplicationVM)
        {

            var savedId = _hospitalApplicationService.Add(hospitalApplicationVM);
            return savedId;

            //CreatedAtAction("GetById", new { id = savedId }, hospitalApplicationVM);

        }

        [HttpDelete]
        [Route("DeleteHospitalApplication/{id}")]
        public ActionResult<HospitalApplication> Delete(int id)
        {
            try
            {
                var HospitalApplicationObj = _hospitalApplicationService.GetById(id);
                int deletedRow = _hospitalApplicationService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("CreateHospitalApplicationAttachments")]
        public int CreateHospitalApplicationAttachments(HospitalApplicationAttachment attachObj)
        {
            return _hospitalApplicationService.CreateHospitalApplicationAttachments(attachObj);
        }
        [HttpPost]
        [Route("UploadHospitalApplicationFiles")]
        public ActionResult UploadHospitalApplicationFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/HospitalApplications/" + file.FileName;
            Stream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("GetAttachmentByHospitalApplicationId/{hospitalApplicationId}")]
        public IEnumerable<HospitalApplicationAttachment> GetAttachmentByHospitalApplicationId(int hospitalApplicationId)
        {
            return _hospitalApplicationService.GetAttachmentByHospitalApplicationId(hospitalApplicationId);
        }

        [HttpDelete]
        [Route("DeleteHospitalApplicationAttachment/{id}")]
        public int DeleteHospitalApplicationAttachment(int id)
        {
            return _hospitalApplicationService.DeleteHospitalApplicationAttachment(id);
        }

        [HttpPost]
        [Route("SortHospitalApp/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> SortHospitalApp(int pagenumber, int pagesize, SortHospitalApplication sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _hospitalApplicationService.SortHospitalApp(sortObj).ToList();
            return _pagingService.GetAll(pageInfo, list);
        }
    }
}
