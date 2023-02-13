using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.BrandVM;
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
    public class BrandController : ControllerBase
    {

        private IBrandService _BrandService;
        private IPagingService _pagingService;

        public BrandController(IBrandService BrandService, IPagingService pagingService)
        {
            _BrandService = BrandService;
            _pagingService = pagingService;

        }


        [HttpGet]
        [Route("ListBrands")]
        public IEnumerable<IndexBrandVM.GetData> GetAll()
        {
            return _BrandService.GetAll();
        }


        [HttpGet]
        [Route("GetTop10Brands/{hospitalId}")]
        public IEnumerable<IndexBrandVM.GetData> GetTop10Brands(int hospitalId)
        {
            return _BrandService.GetTop10Brands(hospitalId);
        }
        [HttpGet]
        [Route("GetTop10BrandsCount/{hospitalId}")]
        public int GetTop10BrandsCount(int hospitalId)
        {
            return _BrandService.GetTop10Brands(hospitalId).ToList().Count;
        }

        [HttpPost]
        [Route("SortBrands/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexBrandVM.GetData> SortBrands(int pagenumber, int pagesize, SortBrandVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list =  _BrandService.SortBrands(sortObj);
            return _pagingService.GetAll<IndexBrandVM.GetData>(pageInfo, list.ToList());
        }






        [HttpPut]
        [Route("GetBrandsWithPaging")]
        public IEnumerable<IndexBrandVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstBrands = _BrandService.GetAll().ToList();
            return _pagingService.GetAll<IndexBrandVM.GetData>(pageInfo, lstBrands);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Brand>();
        }


        [HttpGet]
        [Route("CountBrands")]
        public int CountBrands()
        {
            return _BrandService.CountBrands();
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditBrandVM> GetById(int id)
        {
            return _BrandService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateBrand")]
        public IActionResult Update(EditBrandVM BrandVM)
        {
            try
            {
                int id = BrandVM.Id;
                var lstbrandsCode = _BrandService.GetAllBrands().ToList().Where(a => a.Code == BrandVM.Code && a.Id != id).ToList();
                if (lstbrandsCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Brand code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstbrandsNames = _BrandService.GetAllBrands().ToList().Where(a => a.Name == BrandVM.Name && a.Id != id).ToList();
                if (lstbrandsNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Brand name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstbrandsArNames = _BrandService.GetAllBrands().ToList().Where(a => a.NameAr == BrandVM.NameAr && a.Id != id).ToList();
                if (lstbrandsArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Brand arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _BrandService.Update(BrandVM);
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
        [Route("AddBrand")]
        public ActionResult Add(CreateBrandVM BrandVM)
        {
            if (BrandVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "codelen", Message = "code must not be over 99999", MessageAr = "هذا الكود  لابد ألا يزيد عن خمس حروف أو أرقام" });

            }
            var lstbrandsCode = _BrandService.GetAllBrands().ToList().Where(a => a.Code == BrandVM.Code).ToList();
            if (lstbrandsCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Brand code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstbrandsNames = _BrandService.GetAllBrands().ToList().Where(a => a.Name == BrandVM.Name).ToList();
            if (lstbrandsNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Brand name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstbrandsArNames = _BrandService.GetAllBrands().ToList().Where(a => a.NameAr == BrandVM.NameAr).ToList();
            if (lstbrandsArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Brand arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _BrandService.Add(BrandVM);
                var brandObj = _BrandService.GetById(savedId);
                return Ok(savedId);
            }
        }

        [HttpDelete]
        [Route("DeleteBrand/{id}")]
        public ActionResult<Brand> Delete(int id)
        {
            try
            {

                int deletedRow = _BrandService.Delete(id);
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
