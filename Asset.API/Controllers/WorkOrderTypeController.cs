using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderTypeVM;
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
    public class WorkOrderTypeController : ControllerBase
    {
        private IWorkOrderTypeService _workOrderTypeService;

        public WorkOrderTypeController(IWorkOrderTypeService workOrderTypeService)
        {
            _workOrderTypeService = workOrderTypeService;
        }
        // GET: api/<WorkOrderTypeController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderTypeVM> Get()
        {
            return _workOrderTypeService.GetAllWorkOrderTypes();
        }

        // GET api/<WorkOrderTypeController>/5
        [HttpGet("{id}")]
        public IndexWorkOrderTypeVM Get(int id)
        {
            return _workOrderTypeService.GetWorkOrderTypeById(id);
        }

        // POST api/<WorkOrderTypeController>
        [HttpPost]
        public IActionResult Post(CreateWorkOrderTypeVM createWorkOrderTypeVM)
        {
            var lstcodes = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.Code == createWorkOrderTypeVM.Code).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "code already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.Name == createWorkOrderTypeVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.NameAr == createWorkOrderTypeVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _workOrderTypeService.AddWorkOrderType(createWorkOrderTypeVM);
                return Ok();
            }
        }

        // PUT api/<WorkOrderTypeController>/5
        [HttpPut]
        public IActionResult Put(int id, EditWorkOrderTypeVM editWorkOrderTypeVM)
        {
            var lstcodes = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.Code == editWorkOrderTypeVM.Code && a.Id != editWorkOrderTypeVM.Id).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.Name == editWorkOrderTypeVM.Name && a.Id != editWorkOrderTypeVM.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _workOrderTypeService.GetAllWorkOrderTypes().ToList().Where(a => a.NameAr == editWorkOrderTypeVM.NameAr && a.Id != editWorkOrderTypeVM.Id).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _workOrderTypeService.UpdateWorkOrderType(id, editWorkOrderTypeVM);
                return Ok();
            }
        }

        // DELETE api/<WorkOrderTypeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _workOrderTypeService.DeleteWorkOrderType(id);
        }
    }
}
