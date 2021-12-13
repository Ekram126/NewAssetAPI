using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestStatusController : ControllerBase
    {
        private IRequestStatusService _requestStatusService;

        public RequestStatusController(IRequestStatusService requestStatusService)
        {
            _requestStatusService = requestStatusService;
        }
        // GET: api/<RequestStatusController>
        [HttpGet]
        public IEnumerable<IndexRequestStatusVM.GetData> Get()
        {
            return _requestStatusService.GetAllRequestStatus();
        }

        [HttpGet]
        [Route("GetAll/{userId}")]
        public IEnumerable<IndexRequestStatusVM.GetData> GetAll(string userId)
        {
            return _requestStatusService.GetAll(userId);
        }







        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<RequestStatus> GetById(int id)
        {
            return _requestStatusService.GetById(id);
        }

        // POST api/<RequestStatusController>
        public ActionResult<IndexRequestStatusVM> Post(RequestStatus createRequestStatusVM)
        {
           
            var lstNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.Name == createRequestStatusVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "ECRI name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.NameAr == createRequestStatusVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "ECRI arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _requestStatusService.Add(createRequestStatusVM);
                return Ok();
            }

        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult PutRequestStatus(RequestStatus editRequestStatus)
        {
           
            var lstNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.Name == editRequestStatus.Name && a.Id == editRequestStatus.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.NameAr == editRequestStatus.NameAr && a.Id == editRequestStatus.Id).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _requestStatusService.Update(editRequestStatus);
                return Ok();
            }
        }

        // DELETE api/<RequestStatusController>/5

        [HttpDelete]
        [Route("DeleteReqStatus/{id}")]
        public ActionResult<IndexRequestStatusVM> Delete(int id)
        {
            try
            {

                int deletedRow = _requestStatusService.Delete(id);
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
