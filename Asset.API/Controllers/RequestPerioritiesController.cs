using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asset.Models;
using Asset.Domain.Services;
using Asset.ViewModels.RequestPeriorityVM;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestPerioritiesController : ControllerBase
    {
        private IRequestPeriorityService _requestPeriorityService;

        public RequestPerioritiesController(IRequestPeriorityService  requestPeriorityService)
        {
            _requestPeriorityService = requestPeriorityService;
        }

        // GET: api/RequestPeriorities
        [HttpGet]
        public IEnumerable<IndexRequestPeriority> GetRequestPeriority()
        {
            return _requestPeriorityService.GetAllRequestPeriority();
        }

        // GET: api/RequestPeriorities/5
        [HttpGet("{id}")]
        public ActionResult<IndexRequestPeriority> GetRequestPeriority(int id)
        {
            return _requestPeriorityService.GetRequestPeriorityById(id);
        }

     //[HttpGet("{id}")]
     //   public ActionResult<IndexRequestPeriority> GetByPeriorityName(string name)
     //   {
     //       return _requestPeriorityService.GetRequestPeriorityById(id);
     //   }





        // PUT: api/RequestPeriorities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutRequestPeriority(int id, EditRequestPeriority editRequestPeriority)
        {
            _requestPeriorityService.UpdateRequestPeriority(id, editRequestPeriority);
            return CreatedAtAction("GetRequestPeriority", new { id = editRequestPeriority.Id }, editRequestPeriority);

        }

        // POST: api/RequestPeriorities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<RequestPeriority> PostRequestPeriority(CreateRequestPeriority createRequestPeriority)
        {
            _requestPeriorityService.AddRequestPeriority(createRequestPeriority);
            return CreatedAtAction("GetRequestPeriority", new { id = createRequestPeriority.Id }, createRequestPeriority);
        }
        // DELETE: api/RequestPeriorities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRequestPeriority(int id)
        {
            _requestPeriorityService.DeleteRequestPeriority(id);
            return Ok();
        }
    }
}
