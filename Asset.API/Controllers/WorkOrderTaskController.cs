using Asset.Domain.Services;
using Asset.ViewModels.WorkOrderTaskVM;
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
    public class WorkOrderTaskController : ControllerBase
    {
        private IWorkOrderTaskService _workOrderTaskService;

        public WorkOrderTaskController(IWorkOrderTaskService workOrderTaskService)
        {
            _workOrderTaskService = workOrderTaskService;
        }
        // GET: api/<WorkOrderTaskController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderTaskVM> Get()
        {
            return _workOrderTaskService.GetAllWorkOrderTask();
        }
        [Route("GetAllWorkOrderTaskByWorkOrderId/{WorkOrderId}")]
        public IEnumerable<IndexWorkOrderTaskVM> GetAllWorkOrderTaskByWorkOrderId(int WorkOrderId)
        {
            return _workOrderTaskService.GetAllWorkOrderTaskByWorkOrderId(WorkOrderId);
        }
        // GET api/<WorkOrderTaskController>/5
        [HttpGet("{id}")]
        public IndexWorkOrderTaskVM Get(int id)
        {
            return _workOrderTaskService.GetWorkOrderTaskById(id);
        }

        // POST api/<WorkOrderTaskController>
        [HttpPost]
        public void Post(CreateWorkOrderTaskVM createWorkOrderTask)
        {
            _workOrderTaskService.AddWorkOrderTask(createWorkOrderTask);
        }

        // PUT api/<WorkOrderTaskController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderTaskVM editWorkOrderTask)
        {
            _workOrderTaskService.UpdateWorkOrderTask(id, editWorkOrderTask);
        }

        // DELETE api/<WorkOrderTaskController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _workOrderTaskService.DeleteWorkOrderTask(id);
        }
    }
}
