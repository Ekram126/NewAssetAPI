using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asset.ViewModels.PagingParameter;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MVisitTypeController : ControllerBase
    {
        IVisitTypeService _visitTypeService;
  
        public MVisitTypeController(IVisitTypeService visitTypeService)
        {
            _visitTypeService = visitTypeService;
        }

        [HttpPost]
        [Route("GetVisitTypes")]
        public ActionResult GetVisitTypes()
        {
            var lstVisitTypes = _visitTypeService.GetAll();
            if (lstVisitTypes.Count == 0)
            {
                return Ok(new { data = "", msg = "No Data", status = '0' });
            }
            else
                return Ok(new { data = lstVisitTypes, msg = "Success", status = '1' });
        }
    }
}
