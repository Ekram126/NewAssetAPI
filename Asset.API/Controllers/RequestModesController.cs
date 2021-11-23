using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asset.Models;
using Asset.Domain.Services;
using Asset.ViewModels.RequestModeVM;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestModesController : ControllerBase
    {
        private IRequestModeService _requestModeService;

        public RequestModesController(IRequestModeService requestModeService)
        {
            _requestModeService = requestModeService;
        }

        // GET: api/RequestModes
        [HttpGet]
        public IEnumerable<IndexRequestMode> GetRequestMode()
        {
            return  _requestModeService.GetAllRequestMode();
        }

        // GET: api/RequestModes/5
        [HttpGet("{id}")]
        public ActionResult<IndexRequestMode> GetRequestMode(int id)
        {
            return _requestModeService.GetRequestModeById(id);
        }

        // PUT: api/RequestModes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutRequestMode(int id, EditRequestMode editRequestMode)
        {
            _requestModeService.UpdateRequestMode(id, editRequestMode);
            return CreatedAtAction("GetRequestMode", new { id = editRequestMode.Id }, editRequestMode);

        }

        // POST: api/RequestModes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<RequestMode> PostRequestMode(CreateRequestMode createRequestMode)
        {
            _requestModeService.AddRequestMode(createRequestMode);
            return CreatedAtAction("GetRequestMode", new { id = createRequestMode.Id }, createRequestMode);
        }

        // DELETE: api/RequestModes/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRequestMode(int id)
        {
            _requestModeService.DeleteRequestMode(id);
            return Ok();
        }
    }
}
