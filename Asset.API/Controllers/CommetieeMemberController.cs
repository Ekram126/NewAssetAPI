using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.CommetieeMemberVM;
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
    public class CommetieeMemberController : ControllerBase
    {

        private ICommetieeMemberService _commetieeMemberService;

        public CommetieeMemberController(ICommetieeMemberService commetieeMemberService)
        {
            _commetieeMemberService = commetieeMemberService;
        }


        [HttpGet]
        [Route("ListCommetieeMembers")]
        public IEnumerable<IndexCommetieeMemberVM.GetData> GetAll()
        {
            return _commetieeMemberService.GetAll();
        }



        [HttpGet]
        [Route("CountCommetieeMembers")]
        public int CountCommetieeMembers()
        {
            return _commetieeMemberService.CountCommetieeMembers();
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditCommetieeMemberVM> GetById(int id)
        {
            return _commetieeMemberService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateCommetieeMember")]
        public IActionResult Update(EditCommetieeMemberVM CommetieeMemberVM)
        {
            try
            {
                //int id = CommetieeMemberVM.Id;
                //var lstCitiesCode = _commetieeMemberService.GetAllCities().ToList().Where(a => a.Code == CommetieeMemberVM.Code && a.Id != id).ToList();
                //if (lstCitiesCode.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "CommetieeMember code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                //}
                //var lstCitiesNames = _commetieeMemberService.GetAllCities().ToList().Where(a => a.Name == CommetieeMemberVM.Name && a.Id != id).ToList();
                //if (lstCitiesNames.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "CommetieeMember name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                //}
                //var lstCitiesArNames = _commetieeMemberService.GetAllCities().ToList().Where(a => a.NameAr == CommetieeMemberVM.NameAr && a.Id != id).ToList();
                //if (lstCitiesArNames.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "CommetieeMember arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                //}

                //else
                //{
                    int updatedRow = _commetieeMemberService.Update(CommetieeMemberVM);
              //  }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddCommetieeMember")]
        public ActionResult<CommetieeMember> Add(CreateCommetieeMemberVM CommetieeMemberVM)
        {
            //var lstOrgCode = _commetieeMemberService.GetAllCities().ToList().Where(a => a.Code == CommetieeMemberVM.Code).ToList();
            //if (lstOrgCode.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "CommetieeMember code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            //}
            //var lstOrgNames = _commetieeMemberService.GetAllCities().ToList().Where(a => a.Name == CommetieeMemberVM.Name).ToList();
            //if (lstOrgNames.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "CommetieeMember name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            //}
            //var lstCitiesArNames = _commetieeMemberService.GetAllCities().ToList().Where(a => a.NameAr == CommetieeMemberVM.NameAr).ToList();
            //if (lstCitiesArNames.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "CommetieeMember arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            //}
            //else
            //{
                var savedId = _commetieeMemberService.Add(CommetieeMemberVM);
                return CreatedAtAction("GetById", new { id = savedId }, CommetieeMemberVM);
           // }
        }

        [HttpDelete]
        [Route("DeleteCommetieeMember/{id}")]
        public ActionResult<CommetieeMember> Delete(int id)
        {
            try
            {

                int deletedRow = _commetieeMemberService.Delete(id);
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
