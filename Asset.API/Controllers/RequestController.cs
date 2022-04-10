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
using Microsoft.EntityFrameworkCore;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IWorkOrderService _workOrderService;
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

        [HttpGet]
        [Route("CountRequestsByHospitalId/{hospitalId}/{userId}")]
        public int CountRequestsByHospitalId(int hospitalId, string userId)
        {
            return _requestService.CountRequestsByHospitalId(hospitalId, userId);
        }



        [HttpGet("GetRequestByWorkOrderId/{workOrderId}")]
        public ActionResult<IndexRequestsVM> GetRequestByWorkOrderId(int workOrderId)
        {
            var requestObj = _requestService.GetRequestByWorkOrderId(workOrderId);
            return requestObj;
        }
        [HttpPost]
        [Route("GetAllRequestsByAssetId/{assetId}/{hospitalId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId, PagingParameter pageInfo)
        {
            var lstRequests = _requestService.GetAllRequestsByAssetId(assetId, hospitalId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }
        [HttpPost]
        [Route("GetRequestsByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(int pagenumber, int pagesize, SearchRequestDateVM requestDateObj)
        {

            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstRequests = _requestService.GetRequestsByDate(requestDateObj).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }
        [HttpPost]
        [Route("CountGetRequestsByDate")]
        public int CountGetRequestsByDate(SearchRequestDateVM requestDateObj)
        {
            return _requestService.GetRequestsByDate(requestDateObj).ToList().Count;

        }
        [HttpGet("CountAllRequestsByAssetId/{assetId}/{hospitalId}")]
        public int CountAllRequestsByAssetId(int assetId, int hospitalId)
        {
            return _requestService.GetAllRequestsByAssetId(assetId, hospitalId).ToList().Count;

        }
        [HttpGet("GetTotalRequestForAssetInHospital/{assetDetailId}")]
        public int GetTotalRequestForAssetInHospital(int assetDetailId)
        {
            return _requestService.GetTotalRequestForAssetInHospital(assetDetailId);

        }
        [HttpGet("PrintServiceRequestById/{id}")]
        public ActionResult<PrintServiceRequestVM> PrintWorkOrderById(int id)
        {
            return _requestService.PrintServiceRequestById(id);
        }
        [HttpGet("GetByRequestCode/{code}")]
        public IndexRequestsVM GetByRequestCode(string code)
        {
            return _requestService.GetByRequestCode(code);
        }
        [HttpGet("GetAllRequestsByHospitalAssetId/{assetId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId)
        {
            return _requestService.GetAllRequestsByHospitalAssetId(assetId);
        }


        [HttpGet]
        [Route("GetTotalOpenRequest/{userId}")]
        public int GetTotalOpenReques(string userId)
        {
            return _requestService.GetTotalOpenRequest(userId);
        }

        // POST api/<RequestController>
        [HttpPost]
        public int PostRequestDTO(CreateRequestVM createRequestVM)
        {
            return _requestService.AddRequest(createRequestVM);
        }

        // PUT api/<RequestController>/5
        [HttpPut]
        [Route("UpdateRequest")]
        public IActionResult PutRequestDTO(EditRequestVM editRequestVM)
        {
            _requestService.UpdateRequest(editRequestVM);
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
        [Route("getcount/{userId}")]
        public int count(string userId)
        {
            var count = _requestService.GetAllRequestsWithTrackingByUserId(userId).ToList().Count;
            return count;
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
            int count = _requestService.SearchRequests(searchObj).Count();
            return count;
        }

        [HttpPut]
        [Route("GetAllRequestsWithTrackingByUserIdWithPagingAndStatusId/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId, PagingParameter pageInfo)
        {
            var Requests = _requestService.GetAllRequestsByStatusId(userId, statusId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, Requests);
        }
        [HttpGet]
        [Route("GetRequestsCountByStatusId/{userId}/{statusId}")]
        public int GetCountByStatusId(string userId, int statusId)
        {
            return _requestService.GetAllRequestsByStatusId(userId, statusId).ToList().Count;
        }




        [HttpPost]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetId/{userId}/{assetId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestId(string userId, int assetId, PagingParameter pageInfo)
        {
            var lstRequests = _requestService.GetRequestsByUserIdAssetId(userId, assetId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }
        [HttpGet]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetIdCount")]
        public int GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestIdCount(string userId, int assetId)
        {
            return _requestService.GetRequestsByUserIdAssetId(userId, assetId).ToList().Count;
        }

        [HttpPost]
        [Route("SortRequests/{pagenumber}/{pagesize}/{statusId}")]
        public async Task<IEnumerable<IndexRequestsVM>> SortRequests(int pagenumber, int pagesize, SortRequestVM sortObj, int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = await _requestService.SortRequests(sortObj, statusId);
            return _pagingService.GetAll<IndexRequestsVM>(pageInfo, list.ToList());
        }


        [HttpPost]
        [Route("SortRequestsByAssetId/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestsVM> SortRequestsByAssetId(int pagenumber, int pagesize, SortRequestVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.SortRequestsByAssetId(sortObj);
            return _pagingService.GetAll<IndexRequestsVM>(pageInfo, list.ToList());
        }

        //[HttpPost]
        //[Route("SortRequestsCount")]
        //public async Task<int> SortRequestsCount(SortRequestVM sortObj, int statusId)
        //{
        //    var count = await Task.FromResult( _requestService.SortRequests(sortObj, statusId));

        //    return Task.Run(() => {
        //        long total = 0;
        //        _requestService.SortRequests(sortObj, statusId);
        //        });
        //        return total;
        //    });

        //    return count;
        //}
    }
}
