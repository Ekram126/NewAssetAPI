using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OriginVM;
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
    public class OriginController : ControllerBase
    {

        private IOriginService _OriginService;
        private IPagingService _pagingService;

        public OriginController(IOriginService OriginService, IPagingService pagingService)
        {
            _OriginService = OriginService;
            _pagingService = pagingService;
        }


        //private int NumberOfWorkDays(DateTime start, int numberOfDays)
        //{
        //    int workDays = 0;

        //    DateTime end = start.AddDays(numberOfDays);

        //    while (start != end)
        //    {
        //        if (start.DayOfWeek != DayOfWeek.Friday && start.DayOfWeek != DayOfWeek.Saturday)
        //        {
        //            workDays++;
        //        }

        //        start = start.AddDays(1);
        //    }

        //    return workDays;
        //}


        [HttpGet]
        [Route("ListOrigins")]
        public IEnumerable<IndexOriginVM.GetData> GetAll()
        {
         //   NumberOfWorkDays(DateTime.Today.Date, 30);

            return _OriginService.GetAll();
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditOriginVM> GetById(int id)
        {
            return _OriginService.GetById(id);
        }


        [HttpPut]
        [Route("GetOriginsWithPaging")]
        public IEnumerable<IndexOriginVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstECRIS = _OriginService.GetAll().ToList();
            return _pagingService.GetAll<IndexOriginVM.GetData>(pageInfo, lstECRIS);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Origin>();
        }



        [HttpPut]
        [Route("UpdateOrigin")]
        public IActionResult Update(EditOriginVM OriginVM)
        {
            try
            {
                int id = OriginVM.Id;
                var lstoriginCode = _OriginService.GetAllOrigins().ToList().Where(a => a.Code == OriginVM.Code && a.Id != id).ToList();
                if (lstoriginCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstoriginNames = _OriginService.GetAllOrigins().ToList().Where(a => a.Name == OriginVM.Name && a.Id != id).ToList();
                if (lstoriginNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstoriginArNames = _OriginService.GetAllOrigins().ToList().Where(a => a.NameAr == OriginVM.NameAr && a.Id != id).ToList();
                if (lstoriginArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _OriginService.Update(OriginVM);
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
        [Route("AddOrigin")]
        public ActionResult<Origin> Add(CreateOriginVM OriginVM)
        {
            var lstoriginCode = _OriginService.GetAllOrigins().ToList().Where(a => a.Code == OriginVM.Code).ToList();
            if (lstoriginCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstoriginNames = _OriginService.GetAllOrigins().ToList().Where(a => a.Name == OriginVM.Name).ToList();
            if (lstoriginNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstoriginArNames = _OriginService.GetAllOrigins().ToList().Where(a => a.NameAr == OriginVM.NameAr).ToList();
            if (lstoriginArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _OriginService.Add(OriginVM);
                return CreatedAtAction("GetById", new { id = savedId }, OriginVM);
            }
        }

        [HttpDelete]
        [Route("DeleteOrigin/{id}")]
        public ActionResult<Origin> Delete(int id)
        {
            try
            {

                int deletedRow = _OriginService.Delete(id);
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
