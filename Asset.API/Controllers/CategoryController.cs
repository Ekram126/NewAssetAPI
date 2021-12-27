﻿using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CategoryVM;
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
    public class CategoryController : ControllerBase
    {

        private ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }


        [HttpGet]
        [Route("ListCategories")]
        public IEnumerable<IndexCategoryVM.GetData> GetAll()
        {
            return _CategoryService.GetAll();
        }


        [HttpGet]
        [Route("GetCategoryByCategoryTypeId/{typeId}")]
        public IEnumerable<IndexCategoryVM.GetData> GetCategoryByCategoryTypeId(int typeId)
        {
            return _CategoryService.GetCategoryByCategoryTypeId(typeId);
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditCategoryVM> GetById(int id)
        {
            return _CategoryService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateCategory")]
        public IActionResult Update(EditCategoryVM CategoryVM)
        {
            try
            {
                int id = CategoryVM.Id;
                var lstCategoryCode = _CategoryService.GetAllCategories().ToList().Where(a => a.Code == CategoryVM.Code && a.Id != id).ToList();
                if (lstCategoryCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCategoryNames = _CategoryService.GetAllCategories().ToList().Where(a => a.Name == CategoryVM.Name && a.Id != id).ToList();
                if (lstCategoryNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCategoryArNames = _CategoryService.GetAllCategories().ToList().Where(a => a.NameAr == CategoryVM.NameAr && a.Id != id).ToList();
                if (lstCategoryArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _CategoryService.Update(CategoryVM);
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
        [Route("AddCategory")]
        public ActionResult<Category> Add(CreateCategoryVM CategoryVM)
        {
            var lstCategoryCode = _CategoryService.GetAllCategories().ToList().Where(a => a.Code == CategoryVM.Code).ToList();
            if (lstCategoryCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstCategoryNames = _CategoryService.GetAllCategories().ToList().Where(a => a.Name == CategoryVM.Name).ToList();
            if (lstCategoryNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCategoryArNames = _CategoryService.GetAllCategories().ToList().Where(a => a.NameAr == CategoryVM.NameAr).ToList();
            if (lstCategoryArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _CategoryService.Add(CategoryVM);
                return CreatedAtAction("GetById", new { id = savedId }, CategoryVM);
           }
        }

        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public ActionResult<Category> Delete(int id)
        {
            try
            {

                int deletedRow = _CategoryService.Delete(id);
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
