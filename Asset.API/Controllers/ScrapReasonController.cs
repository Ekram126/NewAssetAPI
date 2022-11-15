using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
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
    public class ScrapReasonController : ControllerBase
    {
        IScrapReasonService _ScrapReasonService;

        public ScrapReasonController(IScrapReasonService ScrapReasonService)
        {
            _ScrapReasonService = ScrapReasonService;
        }

        // GET: api/<ScrapReasonController>
        [HttpGet]
       
        [Route("GetAllScrapReasons")]
        public List<ScrapReason> Get()
        {
            return _ScrapReasonService.GetAll().ToList();
        }

        // GET api/<ScrapReasonController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

      
    }
}
