using Asset.Domain.Services;
using Asset.Models;
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
    public class WorkOrderAssignController : ControllerBase
    {
        private IWorkOrderAssignService _workOrderAssignService;

        public WorkOrderAssignController(IWorkOrderAssignService workOrderAssignService)
        {
            _workOrderAssignService = workOrderAssignService;
        }
    

        [Route("GetAllWorkOrderAssignsByWorkOrederTrackId/{woTrackId}")]
        public IEnumerable<WorkOrderAssign> GetAllWorkOrderTaskByWorkOrderId(int woTrackId)
        {
            return _workOrderAssignService.GetAllWorkOrderAssignsByWorkOrederTrackId(woTrackId);
        }
        
        [HttpGet("{id}")]
        public WorkOrderAssign Get(int id)
        {
            return _workOrderAssignService.GetById(id);
        }

        
        [HttpPost]
        public int Post(WorkOrderAssign createWorkOrderTask)
        {
       return     _workOrderAssignService.Add(createWorkOrderTask);
        }

       
        [HttpPut("{id}")]
        public int Put(WorkOrderAssign editWorkOrderTask)
        {
          return _workOrderAssignService.Update(editWorkOrderTask);
        }

        // DELETE api/<WorkOrderTaskController>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
           return _workOrderAssignService.Delete(id);
        }
    }
}
