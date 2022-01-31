using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ClassificationVM;
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
    public class ClassificationController : ControllerBase
    {

        private IClassificationService _ClassificationService;
        private IPagingService _pagingService;

        public ClassificationController(IClassificationService ClassificationService,
            IPagingService pagingService)
        {
            _ClassificationService = ClassificationService;
            _pagingService = pagingService;

        }


        [HttpGet]
        [Route("ListClassifications")]
        public IEnumerable<Classification> GetAll()
        {
            return _ClassificationService.GetAll();
        }

        [HttpPut]
        [Route("GetClassificationsWithPaging")]
        public IEnumerable<Classification> GetAll(PagingParameter pageInfo)
        {
            var lstcategories = _ClassificationService.GetAll().ToList();
            return _pagingService.GetAll<Classification>(pageInfo, lstcategories);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Classification>();
        }

        [HttpPost]
        [Route("SortClassifications/{pagenumber}/{pagesize}")]
        public IEnumerable<Classification> SortClassifications(int pagenumber, int pagesize, SortClassificationVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _ClassificationService.SortClassification(sortObj);
            return _pagingService.GetAll<Classification>(pageInfo, list.ToList());
        }





        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<Classification> GetById(int id)
        {
            return _ClassificationService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateClassification")]
        public IActionResult Update(Classification ClassificationVM)
        {
            try
            {
                int id = ClassificationVM.Id;
                var lstClassificationCode = _ClassificationService.GetAll().ToList().Where(a => a.Code == ClassificationVM.Code && a.Id != id).ToList();
                if (lstClassificationCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Classification code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstClassificationNames = _ClassificationService.GetAll().ToList().Where(a => a.Name == ClassificationVM.Name && a.Id != id).ToList();
                if (lstClassificationNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Classification name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstClassificationArNames = _ClassificationService.GetAll().ToList().Where(a => a.NameAr == ClassificationVM.NameAr && a.Id != id).ToList();
                if (lstClassificationArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Classification arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _ClassificationService.Update(ClassificationVM);
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
        [Route("AddClassification")]
        public ActionResult<Classification> Add(Classification ClassificationVM)
        {
            var lstClassificationCode = _ClassificationService.GetAll().ToList().Where(a => a.Code == ClassificationVM.Code).ToList();
            if (lstClassificationCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Classification code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstClassificationNames = _ClassificationService.GetAll().ToList().Where(a => a.Name == ClassificationVM.Name).ToList();
            if (lstClassificationNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Classification name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstClassificationArNames = _ClassificationService.GetAll().ToList().Where(a => a.NameAr == ClassificationVM.NameAr).ToList();
            if (lstClassificationArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Classification arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _ClassificationService.Add(ClassificationVM);
                return CreatedAtAction("GetById", new { id = savedId }, ClassificationVM);
            }
        }

        [HttpDelete]
        [Route("DeleteClassification/{id}")]
        public ActionResult<Classification> Delete(int id)
        {
            try
            {

                int deletedRow = _ClassificationService.Delete(id);
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
