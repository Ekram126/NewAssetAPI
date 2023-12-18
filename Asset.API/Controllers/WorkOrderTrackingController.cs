using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.WorkOrderTrackingVM;
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
    public class WorkOrderTrackingController : ControllerBase
    {
        private IWorkOrderTrackingService _workOrderTrackingService;

        public WorkOrderTrackingController(IWorkOrderTrackingService workOrderTrackingService)
        {
            _workOrderTrackingService = workOrderTrackingService;
        }
        // GET: api/<WorkOrderTrackingController>
        [Route("GetAllWorkOrderFromTrackingByServiceRequestId/{ServiceRequestId}")]
        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestId(int ServiceRequestId, string userId)
        {
            return _workOrderTrackingService.GetAllWorkOrderFromTrackingByServiceRequestId(ServiceRequestId, userId);
        }


        [Route("GetAllWorkOrderFromTrackingByUserId/{userId}")]
        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByUserId(string userId)
        {
            return _workOrderTrackingService.GetAllWorkOrderFromTrackingByUserId(userId);
        }

        [Route("GetAllWorkOrderFromTrackingByServiceRequestUserId/{ServiceRequestId}/{userId}")]
        public IEnumerable<LstWorkOrderFromTracking> GetAllWorkOrderFromTrackingByServiceRequestUserId(int ServiceRequestId, string userId)
        {
            return _workOrderTrackingService.GetAllWorkOrderFromTrackingByServiceRequestUserId(ServiceRequestId, userId);
        }



        [HttpGet]
        [Route("GetAttachmentsByWorkOrderId/{id}")]
        public IEnumerable<WorkOrderAttachment> GetAttachmentsByWorkOrderId(int id)
        {
            return _workOrderTrackingService.GetAttachmentsByWorkOrderId(id);
        }

        [HttpGet]
        [Route("GetEngManagerWhoFirstAssignedWO/{woId}")]
        public LstWorkOrderFromTracking GetEngManagerWhoFirstAssignedWO(int woId)
        {
            return _workOrderTrackingService.GetEngManagerWhoFirstAssignedWO(woId);
        }



        [HttpGet]
        [Route("GetFirstTrackForWorkOrderByWorkOrderId/{woId}")]
        public WorkOrderTracking GetFirstTrackForWorkOrderByWorkOrderId(int woId)
        {
            return _workOrderTrackingService.GetFirstTrackForWorkOrderByWorkOrderId(woId);
        }


        [HttpGet]
        [Route("GetTrackOfWorkOrderByWorkOrderId/{workOrderId}")]
        public List<IndexWorkOrderTrackingVM> GetTrackOfWorkOrderByWorkOrderId(int workOrderId)
        {
            return _workOrderTrackingService.GetTrackOfWorkOrderByWorkOrderId(workOrderId);
        }



        // GET api/<WorkOrderTrackingController>/5

        [HttpGet("{id}")]
        public IndexWorkOrderTrackingVM Get(int id)
        {
            return _workOrderTrackingService.GetWorkOrderTrackingById(id);
        }

        [HttpGet("GetWorkOrderTrackingById/{id}")]
        public IndexWorkOrderTrackingVM GetWorkOrderTrackingById(int id)
        {
            return _workOrderTrackingService.GetWorkOrderTrackingById(id);
        }


        [Route("GetAllWorkOrderByWorkOrderId/{WorkOrderId}")]
        public WorkOrderDetails GetAllWorkOrderByWorkOrderId(int WorkOrderId)
        {
            return _workOrderTrackingService.GetAllWorkOrderByWorkOrderId(WorkOrderId);
        }

        [Route("GetAllWorkOrderTrackingByWorkOrderId/{workOrderId}")]
        public List<IndexWorkOrderTrackingVM> GetAllWorkOrderTrackingByWorkOrderId(int workOrderId)
        {
            return _workOrderTrackingService.GetAllWorkOrderTrackingByWorkOrderId(workOrderId);
        }




        // POST api/<WorkOrderTrackingController>
        [HttpPost]
        [Route("AddWorkOrderTracking")]
        public IActionResult Post(CreateWorkOrderTrackingVM createWorkOrderObj)
        {

            var lstWOTrackings = _workOrderTrackingService.GetAll().Where(a => a.WorkOrderId == createWorkOrderObj.WorkOrderId).OrderByDescending(a => a.CreationDate).ToList();
            if (lstWOTrackings.Count > 0)
            {
                var lastDate = lstWOTrackings[0].CreationDate;
                if (createWorkOrderObj.CreationDate < lastDate)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "sr", Message = "Work Order Date should be greater than the last one", MessageAr = "تاريخ أمر الشغل لابد أن يكون متسلسل" });
                }
                else
                {
                    return Ok(_workOrderTrackingService.AddWorkOrderTracking(createWorkOrderObj));
                }
            }
            else
            {
                return Ok(_workOrderTrackingService.AddWorkOrderTracking(createWorkOrderObj));
            }
            //   return Ok();

        }

        // PUT api/<WorkOrderTrackingController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            _workOrderTrackingService.UpdateWorkOrderTracking(id, editWorkOrderTrackingVM);
        }



        [HttpPut]
        [Route("UpdateWorkOrderTracking")]
        public void UpdateWorkOrderTracking(EditWorkOrderTrackingVM editWorkOrderTrackingVM)
        {
            _workOrderTrackingService.UpdateWorkOrderTracking(editWorkOrderTrackingVM);
        }



        // DELETE api/<WorkOrderTrackingController>/5
        [HttpDelete]
        [Route("DeleteWorkOrderTracking/{id}")]
        public void Delete(int id)
        {
            _workOrderTrackingService.DeleteWorkOrderTracking(id);
        }
    }
}
