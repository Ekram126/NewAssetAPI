using Asset.API.Helpers;
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
        private IAssetDetailService _assetDetailService;
        private IDepartmentService _DepartmentService;
        private IPagingService _pagingService;


        public DepartmentController(IDepartmentService DepartmentService, IAssetDetailService assetDetailService, IPagingService pagingService)
        {
            _assetDetailService = assetDetailService;
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

        [HttpPost]
        [Route("SortDepartments/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexDepartmentVM.GetData> SortAssets(int pagenumber, int pagesize, SortDepartmentVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _DepartmentService.SortDepartments(sortObj);
            return _pagingService.GetAll<IndexDepartmentVM.GetData>(pageInfo, list.ToList());
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _DepartmentService.GetAll().ToList().Count;
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditDepartmentVM> GetById(int id)
        {
            return _DepartmentService.GetById(id);
        }


        [HttpGet]
        [Route("GetDepartmentsByHospitalId/{hospitalId}")]
        public List<Department> GetDepartmentsByHospitalId(int hospitalId)
        {
            return _DepartmentService.GetDepartmentsByHospitalId(hospitalId).ToList();
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
        public ActionResult Add(CreateDepartmentVM DepartmentVM)
        {
            if (DepartmentVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "Department code should not exceed 5 characters", MessageAr = "هذا الكود لا يزيد عن 5 حروف وأرقام " });

            }
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
                return Ok(savedId);// CreatedAtAction("GetById", new { id = savedId }, DepartmentVM);
            }
        }





        [HttpPost]
        [Route("AddDepartmentToHospital")]
        public ActionResult AddDepartmentToHospital(CreateDepartmentVM DepartmentVM)
        {
            if (DepartmentVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "Department code should not exceed 5 characters", MessageAr = "هذا الكود لا يزيد عن 5 حروف وأرقام " });

            }
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
                var savedId = _DepartmentService.AddDepartmentToHospital(DepartmentVM);
                return Ok(savedId);
            }
        }







        [HttpDelete]
        [Route("DeleteDepartment/{id}")]
        public ActionResult<Department> Delete(int id)
        {
            try
            {
                var lstHospitalAssets = _assetDetailService.GetAll().Where(a => a.DepartmentId == id).ToList();
                if (lstHospitalAssets.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "hostassets", Message = "There are assets in this departments ", MessageAr = "هناك أصول في هذا القسم" });
                }
                else
                {
                    int deletedRow = _DepartmentService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpGet("GenerateDepartmentCode")]
        public GenerateDepartmentCodeVM GenerateDepartmentCode()
        {
            return _DepartmentService.GenerateDepartmentCode();
        }
    }
}
