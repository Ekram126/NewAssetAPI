using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OriginVM;
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
    public class PMTimeController : ControllerBase
    {

        private IPMTimeService _pMTimeService;

        public PMTimeController(IPMTimeService pMTimeService)
        {
            _pMTimeService = pMTimeService;
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
        [Route("ListPMTimes")]
        public IEnumerable<PMTime> GetAll()
        {
            return _pMTimeService.GetAll();
        }



        //[HttpGet]
        //[Route("GetById/{id}")]
        //public ActionResult<EditOriginVM> GetById(int id)
        //{
        //    return _OriginService.GetById(id);
        //}



        //[HttpPut]
        //[Route("UpdateOrigin")]
        //public IActionResult Update(EditOriginVM OriginVM)
        //{
        //    try
        //    {
        //        //int id = OriginVM.Id;
        //        //var lstCitiesCode = _OriginService.GetAllCities().ToList().Where(a => a.Code == OriginVM.Code && a.Id != id).ToList();
        //        //if (lstCitiesCode.Count > 0)
        //        //{
        //        //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
        //        //}
        //        //var lstCitiesNames = _OriginService.GetAllCities().ToList().Where(a => a.Name == OriginVM.Name && a.Id != id).ToList();
        //        //if (lstCitiesNames.Count > 0)
        //        //{
        //        //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
        //        //}
        //        //var lstCitiesArNames = _OriginService.GetAllCities().ToList().Where(a => a.NameAr == OriginVM.NameAr && a.Id != id).ToList();
        //        //if (lstCitiesArNames.Count > 0)
        //        //{
        //        //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
        //        //}

        //        //else
        //        //{
        //            int updatedRow = _OriginService.Update(OriginVM);
        //       // }
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        string msg = ex.Message;
        //        return BadRequest("Error in update");
        //    }

        //    return Ok();
        //}


        //[HttpPost]
        //[Route("AddOrigin")]
        //public ActionResult<Origin> Add(CreateOriginVM OriginVM)
        //{
        //    //var lstOrgCode = _OriginService.GetAllCities().ToList().Where(a => a.Code == OriginVM.Code).ToList();
        //    //if (lstOrgCode.Count > 0)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
        //    //}
        //    //var lstOrgNames = _OriginService.GetAllCities().ToList().Where(a => a.Name == OriginVM.Name).ToList();
        //    //if (lstOrgNames.Count > 0)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
        //    //}
        //    //var lstCitiesArNames = _OriginService.GetAllCities().ToList().Where(a => a.NameAr == OriginVM.NameAr).ToList();
        //    //if (lstCitiesArNames.Count > 0)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
        //    //}
        //    //else
        //    //{
        //        var savedId = _OriginService.Add(OriginVM);
        //        return CreatedAtAction("GetById", new { id = savedId }, OriginVM);
        //   // }
        //}

        //[HttpDelete]
        //[Route("DeleteOrigin/{id}")]
        //public ActionResult<Origin> Delete(int id)
        //{
        //    try
        //    {

        //        int deletedRow = _OriginService.Delete(id);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        string msg = ex.Message;
        //        return BadRequest("Error in delete");
        //    }

        //    return Ok();
        //}
    }
}
