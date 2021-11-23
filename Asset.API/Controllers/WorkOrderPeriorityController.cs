using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderPeriorityVM;
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
    public class WorkOrderPeriorityController : ControllerBase
    {
        private IWorkOrderPeriorityService _workOrderPeriorityService;

        public WorkOrderPeriorityController(IWorkOrderPeriorityService workOrderPeriorityService)
        {
            _workOrderPeriorityService = workOrderPeriorityService;
        }
        // GET: api/<WorkOrderPeriorityController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderPeriorityVM> Get()
        {
            return _workOrderPeriorityService.GetAllWorkOrderPeriorities();
        }

        // GET api/<WorkOrderPeriorityController>/5
        [HttpGet("{id}")]
        public IndexWorkOrderPeriorityVM Get(int id)
        {
            return _workOrderPeriorityService.GetWorkOrderPeriorityById(id);
        }

        // POST api/<WorkOrderPeriorityController>
        [HttpPost]
        public void Post(CreateWorkOrderPeriorityVM createWorkOrderPeriorityVM)
        {
            _workOrderPeriorityService.AddWorkOrderPeriority(createWorkOrderPeriorityVM);
        }

        // PUT api/<WorkOrderPeriorityController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderPeriorityVM editWorkOrderPeriorityVM)
        {
            _workOrderPeriorityService.UpdateWorkOrderPeriority(id, editWorkOrderPeriorityVM);
        }

        // DELETE api/<WorkOrderPeriorityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _workOrderPeriorityService.DeleteWorkOrderPeriority(id);
        }
    }
}
