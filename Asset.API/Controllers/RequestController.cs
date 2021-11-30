using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.RequestVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IWorkOrderService  _workOrderService;
        private IPagingService _pagingService;
        public RequestController(IRequestService requestService, IWorkOrderService workOrderService, IPagingService pagingService)
        {
            _requestService = requestService;
            _workOrderService = workOrderService;
            _pagingService = pagingService;
        }

        // GET: api/<RequestController>
        [HttpGet]
        public IEnumerable<IndexRequestsVM> GetRequestDTO()
        {
            return _requestService.GetAllRequests();
        }

        [HttpGet]
        [Route("GetAllRequestsWithTrackingByUserId/{userId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId)
        {
            return _requestService.GetAllRequestsWithTrackingByUserId(userId);
        }


        // GET api/<RequestController>/5
        [HttpGet("GetById/{id}")]
        public ActionResult<IndexRequestsVM> GetById(int id)
        {
            var requestDTO = _requestService.GetRequestById(id);
            return requestDTO;
        }

        [HttpGet]
        [Route("GenerateRequestNumber")]
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            return _requestService.GenerateRequestNumber();
        }


        [HttpGet("GetRequestByWorkOrderId/{workOrderId}")]
        public ActionResult<IndexRequestsVM> GetRequestByWorkOrderId(int workOrderId)
        {
            var requestObj = _requestService.GetRequestByWorkOrderId(workOrderId);
            return requestObj;
        }


        [HttpGet("GetTotalRequestForAssetInHospital/{assetDetailId}")]
        public int GetTotalRequestForAssetInHospital(int assetDetailId)
        {
            return _requestService.GetTotalRequestForAssetInHospital(assetDetailId);
            
        }




        // POST api/<RequestController>
        [HttpPost]
        public int PostRequestDTO(CreateRequestVM createRequestVM)
        {
            return _requestService.AddRequest(createRequestVM);
            //return CreatedAtAction("GetRequestDTO", new { id = requestDTO.Id }, requestDTO);
        }

        // PUT api/<RequestController>/5
        [HttpPut("{id}")]
        public IActionResult PutRequestDTO(int id, EditRequestVM editRequestVM)
        {
            _requestService.UpdateRequest(id, editRequestVM);
            return CreatedAtAction("GetRequestDTO", new { id = editRequestVM.Id }, editRequestVM);
        }

        // DELETE api/<RequestController>/5
        [HttpDelete]
        [Route("DeleteRequest/{id}")]
        public ActionResult DeleteRequestDTO(int id)
        {

           var lstWorkOrders = _workOrderService.GetAllWorkOrders().Where(a => a.RequestId == id).ToList();

            if (lstWorkOrders.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "req", Message = "You can't delete this request", MessageAr = "لا يمكنك مسح هذا الطلب" });
            }
            else
            {
                _requestService.DeleteRequest(id);
            }
            return Ok();
        }

        [HttpPut]
        [Route("GetAllRequestsWithTrackingByUserIdWithPaging/{userId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId, PagingParameter pageInfo)
        {
            var Requests = _requestService.GetAllRequestsWithTrackingByUserId(userId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, Requests);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Request>();
        }



        [HttpPost]
        [Route("SearchInRequests/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestVM.GetData> SearchInRequests(int pagenumber, int pagesize, SearchRequestVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.SearchRequests(searchObj).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInRequestsCount")]
        public int SearchInRequestsCount(SearchRequestVM searchObj)
        {
            int count = _requestService.SearchRequests(searchObj).ToList().Count();
            return count;
        }




        [HttpPut]
        [Route("GetAllRequestsWithTrackingByUserIdWithPagingAndStatusId/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId,int statusId, PagingParameter pageInfo)
        {
            var Requests = _requestService.GetAllRequestsByStatusId(userId,statusId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, Requests);
        }
        [HttpGet]
        [Route("GetRequestsCountByStatusId")]
        public int GetCountByStatusId(string userId, int statusId)
        {
            return _requestService.GetAllRequestsByStatusId(userId, statusId).ToList().Count;
        }




        [HttpPost]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetId/{userId}/{assetId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestId(string userId,int assetId, PagingParameter pageInfo)
        {
            var lstRequests = _requestService.GetRequestsByUserIdAssetId(userId, assetId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }
        [HttpGet]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetIdCount")]
        public int GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestIdCount(string userId, int assetId)
        {
            return _requestService.GetRequestsByUserIdAssetId(userId,  assetId).ToList().Count;
        }

        [HttpPost]
        [Route("SortRequests/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestsVM> SortRequests(int pagenumber, int pagesize, SortRequestVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.SortRequests(sortObj).ToList();
            return _pagingService.GetAll<IndexRequestsVM>(pageInfo, list);
        }

    }
}
