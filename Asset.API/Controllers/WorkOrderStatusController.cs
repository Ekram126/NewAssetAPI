using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.WorkOrderStatusVM;
using Asset.ViewModels.WorkOrderVM;
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
    public class WorkOrderStatusController : ControllerBase
    {
        private IWorkOrderTrackingService _workOrderTrackingService;
        private IWorkOrderStatusService _workOrderStatusService;
        private IPagingService _pagingService;


        public WorkOrderStatusController(IWorkOrderStatusService workOrderStatusService, IWorkOrderTrackingService workOrderTrackingService, IPagingService pagingService)
        {
            _workOrderTrackingService = workOrderTrackingService;
            _workOrderStatusService = workOrderStatusService;
            _pagingService = pagingService;
        }

        [HttpGet]
        public IEnumerable<IndexWorkOrderStatusVM> Get()
        {
            return _workOrderStatusService.GetAllWorkOrderStatuses();
        }

        [HttpGet("{id}")]
        public IndexWorkOrderStatusVM Get(int id)
        {
            return _workOrderStatusService.GetWorkOrderStatusById(id);
        }

        [HttpGet]
        [Route("GetAll/{userId}")]
        public IndexWorkOrderStatusVM GetAll(string userId)
        {
            return _workOrderStatusService.GetAll(userId);
        }

        [HttpPost]
        [Route("GetAllWOForReportByDate")]
        public IndexWorkOrderStatusVM GetAllForReportByDate(SearchWorkOrderByDateVM requestDateObj)
        {
             return _workOrderStatusService.GetAllForReport(requestDateObj);
        }



        [HttpPost]
        [Route("SortWOStatuses/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexWorkOrderStatusVM> SortWOStatuses(int pagenumber, int pagesize, SortWorkOrderStatusVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _workOrderStatusService.SortWOStatuses(sortObj);
            return _pagingService.GetAll<IndexWorkOrderStatusVM>(pageInfo, list.ToList());
        }
        [HttpPut]
        [Route("GetWOStatusWithPaging")]
        public IEnumerable<IndexWorkOrderStatusVM> GetAll(PagingParameter pageInfo)
        {
            var lstWOStatus = _workOrderStatusService.GetAllWorkOrderStatuses().ToList();
            return _pagingService.GetAll<IndexWorkOrderStatusVM>(pageInfo, lstWOStatus);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Count;
        }



        [HttpPost]
        public IActionResult Post(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            var lstcodes = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Code == createWorkOrderStatusVM.Code).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Name == createWorkOrderStatusVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.NameAr == createWorkOrderStatusVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _workOrderStatusService.AddWorkOrderStatus(createWorkOrderStatusVM);
                return Ok();
            }
        }

        // PUT api/<WorkOrderStatusController>/5
        [HttpPut("{id}")]
        [Route("UpdateWorkOrderStatus")]
        public IActionResult Put(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            
            var lstcodes = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Code == editWorkOrderStatusVM.Code && a.Id != editWorkOrderStatusVM.Id).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Name == editWorkOrderStatusVM.Name && a.Id != editWorkOrderStatusVM.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.NameAr == editWorkOrderStatusVM.NameAr && a.Id != editWorkOrderStatusVM.Id).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _workOrderStatusService.UpdateWorkOrderStatus(id, editWorkOrderStatusVM);
                return Ok();
            }
        }

    
        [HttpDelete("{id}")]
        public ActionResult<WorkOrderStatus> Delete(int id)
        {
            try
            {
                var lstStatus = _workOrderTrackingService.GetAll().ToList().Where(a => a.WorkOrderStatusId == id).ToList();
                if (lstStatus.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "wostatus", Message = "This status already in use", MessageAr = "هذه الحالة مستخدمة" });
                }
                else
                {
                    _workOrderStatusService.DeleteWorkOrderStatus(id);
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
