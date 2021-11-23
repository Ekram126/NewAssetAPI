using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.WorkOrderVM;
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
    public class WorkOrderController : ControllerBase
    {
        private IWorkOrderService _workOrderService;
        private IPagingService _pagingService;

        private IWorkOrderTrackingService _workOrderTackingService;

        public WorkOrderController(IWorkOrderService workOrderService, IWorkOrderTrackingService workOrderTackingService,IPagingService pagingService)
        {
            _workOrderService = workOrderService;
            _workOrderTackingService = workOrderTackingService;
            _pagingService = pagingService;
        }
        // GET: api/<WorkOrderController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderVM> Get()
        {
            return _workOrderService.GetAllWorkOrders();
        }


        [HttpGet]
        [Route("GetworkOrderByUserId/{requestId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId)
        {
            return _workOrderService.GetworkOrderByUserId(requestId, userId);
        }


        [HttpGet]
        [Route("GenerateWorOrderNumber")]
        public GeneratedWorkOrderNumberVM GenerateWorOrderNumber()
        {
            return _workOrderService.GenerateWorOrderNumber();
        }


        [HttpGet]
        [Route("GetworkOrder/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId)
        {
            return _workOrderService.GetworkOrder(userId);
        }

        [HttpGet("GetTotalWorkOrdersForAssetInHospital/{assetDetailId}")]
        public int GetTotalWorkOrdersForAssetInHospital(int assetDetailId)
        {
            return _workOrderService.GetTotalWorkOrdersForAssetInHospital(assetDetailId);

        }

        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId)
        {
            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<WorkOrder>();
        }

        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalStatusId/{hospitalId}/{userId}/{statusId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(PagingParameter pageInfo,int? hospitalId, string userId, int statusId)
        {
            var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId,statusId).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders);
        }


        [HttpGet]
        [Route("GetCountByStatus")]
        public int GetCountByStatus(int? hospitalId, string userId, int statusId)
        {
            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId).Count();
        }




        // GET api/<WorkOrderController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexWorkOrderVM> Get(int id)
        {
            return _workOrderService.GetWorkOrderById(id);
        }


        [HttpGet("GetWorkOrderByRequestId/{requestId}")]
        public ActionResult<IndexWorkOrderVM> GetWorkOrderByRequestId(int requestId)
        {
            return _workOrderService.GetWorkOrderByRequestId(requestId);
        }



        [HttpGet("PrintWorkOrderById/{id}")]
        public ActionResult<PrintWorkOrderVM> PrintWorkOrderById(int id)
        {
            return _workOrderService.PrintWorkOrderById(id);
        }






        // POST api/<WorkOrderController>
        [HttpPost]
        public int Post(CreateWorkOrderVM createWorkOrderVM)
        {
            return _workOrderService.AddWorkOrder(createWorkOrderVM);
        }

        // PUT api/<WorkOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderVM editWorkOrderVM)
        {
            _workOrderService.UpdateWorkOrder(id, editWorkOrderVM);
        }

        // DELETE api/<WorkOrderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var lstTracks = _workOrderTackingService.GetAllWorkOrderTrackingByWorkOrderId(id).ToList();
            if (lstTracks.Count > 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "workorder", Message = "Work Order has many tracks you cannot delete it", MessageAr = "لا يمكنك مسح هذا الطلب لوجود العديد من الأوامر" });
            }
            else
            {
                _workOrderService.DeleteWorkOrder(id);
            }

            return Ok();
        }
    }
}
