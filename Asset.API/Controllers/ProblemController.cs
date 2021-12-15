﻿using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.ViewModels.ProblemVM;
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
    public class ProblemController : ControllerBase
    {
        private IProblemService _problemService;

        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }
        // GET: api/<ProblemController>
        [HttpGet]
        public IEnumerable<IndexProblemVM> Get()
        {
            return _problemService.GetAllProblems();
        }

        // GET api/<ProblemController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexProblemVM> Get(int id)
        {
            return _problemService.GetProblemById(id);
        }

        // POST api/<ProblemController>
        [HttpPost]
        [Route("AddProblem")]
        public ActionResult<CreateProblemVM> Post(CreateProblemVM createProblemVM)
        {
            var lstCode = _problemService.GetAllProblems().ToList().Where(a => a.Code == createProblemVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Problem code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _problemService.GetAllProblems().ToList().Where(a => a.Name == createProblemVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Problem name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _problemService.GetAllProblems().ToList().Where(a => a.NameAr == createProblemVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Problem arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
               _problemService.AddProblem(createProblemVM);
                return Ok();
            }
           
        }

        // PUT api/<ProblemController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, EditProblemVM editProblemVM)
        {
            _problemService.UpdateProblem(id, editProblemVM);
            var lstCode = _problemService.GetAllProblems().ToList().Where(a => a.Code != editProblemVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Problem code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _problemService.GetAllProblems().ToList().Where(a => a.Name != editProblemVM.Name).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Problem name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _problemService.GetAllProblems().ToList().Where(a => a.NameAr != editProblemVM.NameAr).ToList();
            if (lstArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Problem arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                _problemService.UpdateProblem(id, editProblemVM);
                return Ok();
            }
        }

        // DELETE api/<ProblemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _problemService.DeleteProblem(id);
        }
    }
}
