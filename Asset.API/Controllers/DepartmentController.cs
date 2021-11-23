﻿using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;
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
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _DepartmentService;
        private IPagingService _pagingService;


        public DepartmentController(IDepartmentService DepartmentService, IPagingService pagingService)
        {
            _DepartmentService = DepartmentService;
            _pagingService = pagingService;
        }


        [HttpGet]
        [Route("ListDepartments")]
        public IEnumerable<IndexDepartmentVM.GetData> GetAll()
        {
            return _DepartmentService.GetAll();
        }

        [HttpPut]
        [Route("GetDepartmentsWithPaging")]
        public IEnumerable<IndexDepartmentVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstdepts = _DepartmentService.GetAll().ToList();
            return _pagingService.GetAll<IndexDepartmentVM.GetData>(pageInfo, lstdepts);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Department>();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditDepartmentVM> GetById(int id)
        {
            return _DepartmentService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateDepartment/{id}")]
        public IActionResult Update(EditDepartmentVM DepartmentVM)
        { 
            try
            {
                int id = DepartmentVM.Id;
                var lstDepartmentCode = _DepartmentService.GetAllDepartments().ToList().Where(a => a.Code == DepartmentVM.Code && a.Id != id).ToList();
                if (lstDepartmentCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Department code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstDepartmentNames = _DepartmentService.GetAllDepartments().ToList().Where(a => a.Name == DepartmentVM.Name && a.Id != id).ToList();
                if (lstDepartmentNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Department name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _DepartmentService.Update(DepartmentVM);
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
        [Route("AddDepartment")]
        public ActionResult<Department> Add(CreateDepartmentVM DepartmentVM)
        {
            var lstDepartmentCode = _DepartmentService.GetAllDepartments().ToList().Where(a => a.Code == DepartmentVM.Code).ToList();
            if (lstDepartmentCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Department code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstDepartmentNames = _DepartmentService.GetAllDepartments().ToList().Where(a => a.Name == DepartmentVM.Name).ToList();
            if (lstDepartmentNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Department name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _DepartmentService.Add(DepartmentVM);
                return CreatedAtAction("GetById", new { id = savedId }, DepartmentVM);
            }
        }

        [HttpDelete]
        [Route("DeleteDepartment/{id}")]
        public ActionResult<Department> Delete(int id)
        {
            try
            {

                int deletedRow = _DepartmentService.Delete(id);
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
