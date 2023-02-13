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
        private IHospitalService _hospitalService;

        public MDepartmentController(IDepartmentService DepartmentService, IHospitalService hospitalService)
        {
            _DepartmentService = DepartmentService;
            _hospitalService = hospitalService;
        }

        [HttpGet]
        [Route("ListDepartments")]
        public ActionResult GetAll()
        {

            var list = _DepartmentService.GetAll();
            if (list.Count() == 0)
            {
                return Ok(new { data = list, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = list, msg = "Success", status = '1' });
        }


        [HttpGet]
        [Route("CountDepartments/{hospitalId}")]
        public ActionResult CountDepartments(int hospitalId)
        {
            var total= _hospitalService.CountDepartmentsByHospitalId(hospitalId);
            return Ok(new { data = total.ToString(), msg = "Success", status = '1' });
        }


    }
}
