using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.ViewModels.RequestTypeVM;
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
    public class RequestTypeController : ControllerBase
    {
        private IRequestTypeService _requestTypeService;

        public RequestTypeController(IRequestTypeService requestTypeService)
        {
            _requestTypeService = requestTypeService;
        }
        // GET: api/<RequestTypeController>
        [HttpGet]
        public IEnumerable<IndexRequestTypeVM> Get()
        {
            return _requestTypeService.GetAllRequestTypes();
        }

        // GET api/<RequestTypeController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexRequestTypeVM> Get(int id)
        {
            return _requestTypeService.GetRequestTypeById(id);
        }

        // POST api/<RequestTypeController>
        [HttpPost]
        public IActionResult Post(CreateRequestTypeVM createRequestTypeVM)
        {
            var lstcodes = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.Code == createRequestTypeVM.Code).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Status code already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.Name == createRequestTypeVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Status name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.NameAr == createRequestTypeVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Status arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _requestTypeService.AddRequestType(createRequestTypeVM);
                return Ok();
            }
        }

        // PUT api/<RequestTypeController>/5
        [HttpPut]
        [Route("UpdateRequestType")]
        public IActionResult Put(EditRequestTypeVM editRequestTypeVM)
        {
            var lstcodes = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.Code == editRequestTypeVM.Code && a.Id  != editRequestTypeVM.Id).ToList();
            if (lstcodes.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Status code already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstNames = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.Name == editRequestTypeVM.Name && a.Id != editRequestTypeVM.Id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Status name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _requestTypeService.GetAllRequestTypes().ToList().Where(a => a.NameAr == editRequestTypeVM.NameAr && a.Id != editRequestTypeVM.Id).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Status arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _requestTypeService.UpdateRequestType(editRequestTypeVM);
                return Ok();
            }
        }

        // DELETE api/<RequestTypeController>/5
        [HttpDelete]
        [Route("DeleteRequestType/{id}")]
        public void Delete(int id)
        {
            _requestTypeService.DeleteRequestType(id);
        }
    }
}
