using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestTrackingVM;
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
    public class RequestTrackingController : ControllerBase
    {
        private IRequestTrackingService _requestTrackingService;

        public RequestTrackingController(IRequestTrackingService requestTrackingService)
        {
            _requestTrackingService = requestTrackingService;
        }
        // GET: api/<RequestTrackingController>
        [Route("GetAllRequestFromTrackingByuserId/{UserId}/{assetdetailId}")]
        public IEnumerable<IndexRequestTracking> Get(string UserId, int assetDetailId)
        {
            return _requestTrackingService.GetAllRequestTracking(UserId, assetDetailId);
        }

        // GET api/<RequestTrackingController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexRequestTracking> Get(int id)
        {
            return _requestTrackingService.GetRequestTrackingById(id);
        }
        [HttpGet]
        [Route("GetAllTrackingsByRequestId/{requestId}")]
        public RequestDetails GetAllTrackingsByRequestId(int requestId)
        {
            return _requestTrackingService.GetAllTrackingsByRequestId(requestId);
        }



        [HttpGet]
        [Route("GetFirstTrackForRequestByRequestId/{requestId}")]
        public RequestTracking GetFirstTrackForRequestByRequestId(int requestId)
        {
            return _requestTrackingService.GetFirstTrackForRequestByRequestId(requestId);
        }





        [HttpGet]
        [Route("GetAllTracksByRequestId/{requestId}")]
        public List<RequestTrackingView> GetAllTracksByRequestId(int requestId)
        {
            return _requestTrackingService.GetRequestTracksByRequestId(requestId);
        }


        [HttpGet]
        [Route("CountRequestTracksByRequestId/{requestId}")]
        public int CountRequestTracksByRequestId(int requestId)
        {
            return _requestTrackingService.CountRequestTracksByRequestId(requestId);
        }




        // POST api/<RequestTrackingController>
        [HttpPost]
        [Route("AddRequestTracking")]
        public int Post(CreateRequestTracking createRequestTracking)
        {
            return _requestTrackingService.AddRequestTracking(createRequestTracking);
        }

        // PUT api/<RequestTrackingController>/5
        [HttpPut]
        [Route("UpdateRequestTracking")]
        public void Put(EditRequestTracking editRequestTrackingObj)
        {
            _requestTrackingService.UpdateRequestTracking(editRequestTrackingObj);
        }

        // DELETE api/<RequestTrackingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _requestTrackingService.DeleteRequestTracking(id);
        }
    }
}
