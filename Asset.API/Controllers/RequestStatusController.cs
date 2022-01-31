using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
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
        private IRequestTrackingService _requestTrackingService;
        private IPagingService _pagingService;

        public RequestStatusController(IRequestStatusService requestStatusService, IRequestTrackingService requestTrackingService, IPagingService pagingService)
        {
            _requestStatusService = requestStatusService;
            _requestTrackingService = requestTrackingService;
            _pagingService = pagingService;
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




        [HttpPut]
        [Route("GetRequestStatusesWithPaging")]
        public IEnumerable<IndexRequestStatusVM.GetData> GetAll(PagingParameter pageInfo)
        {
            var lstBrands = _requestStatusService.GetAllRequestStatus().ToList();
            return _pagingService.GetAll<IndexRequestStatusVM.GetData>(pageInfo, lstBrands);
        }

        [HttpGet]
        [Route("GetCount")]
        public int count()
        {
            return _requestStatusService.GetAllRequestStatus().ToList().Count;
        }


        [HttpPost]
        [Route("SortRequestStatuses/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestStatusVM.GetData> SortWorkOrderTypes(int pagenumber, int pagesize, SortRequestStatusVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestStatusService.SortRequestStatuses(sortObj);
            return _pagingService.GetAll<IndexRequestStatusVM.GetData>(pageInfo, list.ToList());
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Request status name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.NameAr == createRequestStatusVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Request status arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _requestStatusService.Add(createRequestStatusVM);
                return Ok();
            }

        }

        [HttpPut]
        [Route("UpdateRequestStatus")]
        public IActionResult PutRequestStatus(RequestStatus editRequestStatus)
        {
           
            var lstNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.Name == editRequestStatus.Name && a.Id != editRequestStatus.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestStatusService.GetAllRequestStatus().ToList().Where(a => a.NameAr == editRequestStatus.NameAr && a.Id != editRequestStatus.Id).ToList();
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
                var lstRequestTracking = _requestTrackingService.GetAll().Where(a => a.RequestStatusId == id).ToList();
                if (lstRequestTracking.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "reqStatus", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                   int deletedRow = _requestStatusService.Delete(id);
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
