using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SubCategoryVM;
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
    public class SubCategoryController : ControllerBase
    {

        private ISubCategoryService _SubCategoryService;

        public SubCategoryController(ISubCategoryService SubCategoryService)
        {
            _SubCategoryService = SubCategoryService;
        }


        [HttpGet]
        [Route("ListSubCategories")]
        public IEnumerable<IndexSubCategoryVM.GetData> GetAll()
        {
            return _SubCategoryService.GetAll();
        }






        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSubCategoryVM> GetById(int id)
        {
            return _SubCategoryService.GetById(id);
        }


        [HttpGet]
        [Route("GetSubCategoryByCategoryId/{categoryId}")]
        public ActionResult<IEnumerable<SubCategory>> GetSubCategoryByCategoryId(int categoryId)
        {
            return _SubCategoryService.GetSubCategoryByCategoryId(categoryId).ToList();
        }




        [HttpPut]
        [Route("UpdateSubCategory")]
        public IActionResult Update(EditSubCategoryVM SubCategoryVM)
        {
            try
            {
                int id = SubCategoryVM.Id;
                var lstsubCategoriesCode = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.Code == SubCategoryVM.Code && a.Id != id).ToList();
                if (lstsubCategoriesCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "SubCategory code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstsubCategoriesNames = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.Name == SubCategoryVM.Name && a.Id != id).ToList();
                if (lstsubCategoriesNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "SubCategory name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstsubCategoriesArNames = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.NameAr == SubCategoryVM.NameAr && a.Id != id).ToList();
                if (lstsubCategoriesArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "SubCategory arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _SubCategoryService.Update(SubCategoryVM);
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
        [Route("AddSubCategory")]
        public ActionResult<SubCategory> Add(CreateSubCategoryVM SubCategoryVM)
        {
            var lstOrgCode = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.Code == SubCategoryVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "SubCategory code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.Name == SubCategoryVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "SubCategory name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstsubCategoriesArNames = _SubCategoryService.GetAllSubCategories().ToList().Where(a => a.NameAr == SubCategoryVM.NameAr).ToList();
            if (lstsubCategoriesArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "SubCategory arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
                {
                var savedId = _SubCategoryService.Add(SubCategoryVM);
                return CreatedAtAction("GetById", new { id = savedId }, SubCategoryVM);
            }
        }

        [HttpDelete]
        [Route("DeleteSubCategory/{id}")]
        public ActionResult<SubCategory> Delete(int id)
        {
            try
            {

                int deletedRow = _SubCategoryService.Delete(id);
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
