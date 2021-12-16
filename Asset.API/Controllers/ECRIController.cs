using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ECRIVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECRIController : ControllerBase
    {

        private IECRIService _ECRIService;
        private IPagingService _pagingService;

        public ECRIController(IECRIService ECRIService, IPagingService pagingService)
        {
            _ECRIService = ECRIService;
            _pagingService = pagingService;
        }
        [HttpGet]
        [Route("ListECRIs")]
        public IEnumerable<IndexECRIVM.GetData> GetAll()
        {
           return _ECRIService.GetAll().ToList();

        }

        [HttpPut]
        [Route("GetECRISWithPaging")]
        public IEnumerable<IndexECRIVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstECRIS = _ECRIService.GetAll().ToList();
            return _pagingService.GetAll<IndexECRIVM.GetData>(pageInfo, lstECRIS);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<ECRI>();
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditECRIVM> GetById(int id)
        {
            return _ECRIService.GetById(id);
        }



        [HttpPost]
        [Route("SortECRI/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexECRIVM.GetData> SortMasterAssets(int pagenumber, int pagesize, SortECRIVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _ECRIService.sortECRI(sortObj).ToList();
            return _pagingService.GetAll<IndexECRIVM.GetData>(pageInfo, list);
        }




        [HttpPut]
        [Route("UpdateECRI")]
        public IActionResult Update(EditECRIVM ECRIVM)
        {
            try
            {
                int id = ECRIVM.Id;
                var lstCode = _ECRIService.GetAll().ToList().Where(a => a.Code == ECRIVM.Code && a.Id != id).ToList();
                if (lstCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "ECRI code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstNames = _ECRIService.GetAll().ToList().Where(a => a.Name == ECRIVM.Name && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "ECRI name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _ECRIService.GetAll().ToList().Where(a => a.NameAr == ECRIVM.NameAr && a.Id != id).ToList();
                if (lstArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "ECRI arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _ECRIService.Update(ECRIVM);
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
        [Route("AddECRI")]
        public ActionResult<ECRI> Add(CreateECRIVM ECRIVM)
        {
            var lstCode = _ECRIService.GetAll().ToList().Where(a => a.Code == ECRIVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "ECRI code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _ECRIService.GetAll().ToList().Where(a => a.Name == ECRIVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "ECRI name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _ECRIService.GetAll().ToList().Where(a => a.NameAr == ECRIVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "ECRI arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _ECRIService.Add(ECRIVM);
                return CreatedAtAction("GetById", new { id = savedId }, ECRIVM);
            }
        }

        [HttpDelete]
        [Route("DeleteECRI/{id}")]
        public ActionResult<ECRI> Delete(int id)
        {
            try
            {

                int deletedRow = _ECRIService.Delete(id);
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
