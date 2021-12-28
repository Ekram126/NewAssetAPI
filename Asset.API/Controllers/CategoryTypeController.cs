using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CategoryTypeVM;
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
    public class CategoryTypeController : ControllerBase
    {

        private ICategoryTypeService _categoryTypeService;
        private ICategoryService _categoryService;
        private ISubCategoryService _subCategoryService;
        

        public CategoryTypeController(ICategoryTypeService categoryTypeService, ICategoryService categoryService,
            ISubCategoryService subCategoryService)
        {
            _categoryTypeService = categoryTypeService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;

        }


        [HttpGet]
        [Route("ListCategoryTypes")]
        public IEnumerable<IndexCategoryTypeVM.GetData> GetAll()
        {
            return _categoryTypeService.GetAll();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditCategoryTypeVM> GetById(int id)
        {
            return _categoryTypeService.GetById(id);
        }

        [HttpPut]
        [Route("UpdateCategoryType")]
        public IActionResult Update(EditCategoryTypeVM CategoryVM)
        {
            try
            {
                int id = CategoryVM.Id;
                var lstCategoryCode = _categoryTypeService.GetAll().ToList().Where(a => a.Code == CategoryVM.Code && a.Id != id).ToList();
                if (lstCategoryCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCategoryNames = _categoryTypeService.GetAll().ToList().Where(a => a.Name == CategoryVM.Name && a.Id != id).ToList();
                if (lstCategoryNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCategoryArNames = _categoryTypeService.GetAll().ToList().Where(a => a.NameAr == CategoryVM.NameAr && a.Id != id).ToList();
                if (lstCategoryArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _categoryTypeService.Update(CategoryVM);
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
        [Route("AddCategoryType")]
        public ActionResult<Category> Add(CreateCategoryTypeVM CategoryVM)
        {
            var lstCategoryCode = _categoryTypeService.GetAll().ToList().Where(a => a.Code == CategoryVM.Code).ToList();
            if (lstCategoryCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstCategoryNames = _categoryTypeService.GetAll().ToList().Where(a => a.Name == CategoryVM.Name).ToList();
            if (lstCategoryNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCategoryArNames = _categoryTypeService.GetAll().ToList().Where(a => a.NameAr == CategoryVM.NameAr).ToList();
            if (lstCategoryArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _categoryTypeService.Add(CategoryVM);
                return CreatedAtAction("GetById", new { id = savedId }, CategoryVM);
           }
        }

        [HttpDelete]
        [Route("DeleteCategoryType/{id}")]
        public ActionResult<Category> Delete(int id)
        {
            try
            {
                var lstCategories = _categoryService.GetAll().Where(a => a.CategoryTypeId == id).ToList();
                if (lstCategories.Count > 0)
                {
                    foreach (var item in lstCategories)
                    {
                        var lstSubCategories = _subCategoryService.GetAll().Where(a => a.CategoryId == item.Id).ToList();
                        if(lstSubCategories.Count > 0)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "categories", Message = "This Category has Sub Categories", MessageAr = "هذا التصنيف يحتوي على تصنيفات فرعية" });
                        }
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "categories", Message = "This Category Type has Categories", MessageAr = "هذا النوع من التصنيف يحتوي على تصنيفات " });
                }
                else
                {
                    int deletedRow = _categoryTypeService.Delete(id);
                }
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
