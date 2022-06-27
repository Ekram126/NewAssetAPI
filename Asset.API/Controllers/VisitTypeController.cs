using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitTypeController : ControllerBase
    {
        IVisitTypeService _visitTypeService;

        public VisitTypeController(IVisitTypeService visitTypeService)
        {
            _visitTypeService = visitTypeService;
        }



        // GET: api/<VisitTypeController>
        [HttpGet]
        //[Route("GetAllVisitTypes/{hospitalId}")]
        //public List<IndexVisitVM.GetData> Get(int hospitalId)
        //{
        //    return _visitTypeService.GetAll(hospitalId).ToList();
        //}
        [Route("GetAllVisitTypes")]
        public List<VisitType> Get()
        {
            return _visitTypeService.GetAll().ToList();
        }

        // GET api/<VisitTypeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

      
        // DELETE api/<VisitController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
