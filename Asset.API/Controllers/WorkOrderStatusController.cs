using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderStatusVM;
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
    public class WorkOrderStatusController : ControllerBase
    {
        private IWorkOrderStatusService _workOrderStatusService;

        public WorkOrderStatusController(IWorkOrderStatusService workOrderStatusService)
        {
            _workOrderStatusService = workOrderStatusService;
        }
        // GET: api/<WorkOrderStatusController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderStatusVM> Get()
        {
            return _workOrderStatusService.GetAllWorkOrderStatuses();
        }

        // GET api/<WorkOrderStatusController>/5
        [HttpGet("{id}")]
        public IndexWorkOrderStatusVM Get(int id)
        {
            return _workOrderStatusService.GetWorkOrderStatusById(id);
        }

        // POST api/<WorkOrderStatusController>
        [HttpPost]
        public IActionResult Post(CreateWorkOrderStatusVM createWorkOrderStatusVM)
        {
            var lstcodes = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Name == createWorkOrderStatusVM.Name).ToList();
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _workOrderStatusService.AddWorkOrderStatus(createWorkOrderStatusVM);
                return Ok();
            }
        }

        // PUT api/<WorkOrderStatusController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, EditWorkOrderStatusVM editWorkOrderStatusVM)
        {
            var lstcodes = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Code == editWorkOrderStatusVM.Code && a.Id == editWorkOrderStatusVM.Id).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.Name == editWorkOrderStatusVM.Name && a.Id == editWorkOrderStatusVM.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _workOrderStatusService.GetAllWorkOrderStatuses().ToList().Where(a => a.NameAr == editWorkOrderStatusVM.NameAr && a.Id == editWorkOrderStatusVM.Id).ToList();
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

        // DELETE api/<WorkOrderStatusController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _workOrderStatusService.DeleteWorkOrderStatus(id);
        }
    }
}
