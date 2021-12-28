using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.ViewModels.SubProblemVM;
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
    public class SubProblemController : ControllerBase
    {
        private ISubProblemService _subProblemService;

        public SubProblemController(ISubProblemService subProblemService)
        {
            _subProblemService = subProblemService;
        }
        [Route("GetAllSubProblemsByProblemId/{ProblemId}")]
        public IEnumerable<IndexSubProblemVM> GetAllSubProblemsByProblemId(int ProblemId)
        {
            return _subProblemService.GetAllSubProblemsByProblemId(ProblemId);
        }




        // GET: api/<SubProblemController>
        [HttpGet]
        public IEnumerable<IndexSubProblemVM> Get()
        {
            return _subProblemService.GetAllSubProblems();
        }

        // GET api/<SubProblemController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexSubProblemVM> Get(int id)
        {
            return _subProblemService.GetSubProblemById(id);
        }

        // POST api/<SubProblemController>
        [HttpPost]
        public IActionResult Post(CreateSubProblemVM createSubProblemVM)
        {
            var lstCode = _subProblemService.GetAllSubProblems().ToList().Where(a => a.Code == createSubProblemVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = " code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _subProblemService.GetAllSubProblems().ToList().Where(a => a.Name == createSubProblemVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _subProblemService.GetAllSubProblems().ToList().Where(a => a.NameAr == createSubProblemVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _subProblemService.AddSubProblem(createSubProblemVM);
                return Ok();
            }
        }

        // PUT api/<SubProblemController>/5
        [HttpPut]
        public IActionResult Put(EditSubProblemVM editSubProblemVM)
        {
            int id = editSubProblemVM.Id;
            var lstCode = _subProblemService.GetAllSubProblems().ToList().Where(a => a.Code == editSubProblemVM.Code && a.Id != id).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = " code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _subProblemService.GetAllSubProblems().ToList().Where(a => a.Name == editSubProblemVM.Name && a.Id != id).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = " name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _subProblemService.GetAllSubProblems().ToList().Where(a => a.NameAr == editSubProblemVM.NameAr && a.Id != id).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = " arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _subProblemService.UpdateSubProblem( editSubProblemVM);
                return Ok();
            }

        }

        // DELETE api/<SubProblemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _subProblemService.DeleteSubProblem(id);
        }
    }
}
