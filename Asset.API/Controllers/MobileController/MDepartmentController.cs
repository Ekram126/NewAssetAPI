using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.DepartmentVM;
using Asset.ViewModels.HospitalVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MDepartmentController : ControllerBase
    {
        private IDepartmentService _DepartmentService;

        public MDepartmentController(IDepartmentService DepartmentService)
        {
           _DepartmentService = DepartmentService;
           
        }

        [HttpGet]
        [Route("ListDepartments")]
        public IEnumerable<IndexDepartmentVM.GetData> GetAll()
        {
            return _DepartmentService.GetAll();
        }

    }
}
