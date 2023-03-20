using Asset.Domain.Services;
using Asset.Models;
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
    public class MHospitalController : ControllerBase
    {
        private IHospitalService _HospitalService;
        public MHospitalController(IHospitalService HospitalService)
        {
            _HospitalService = HospitalService;
        }


        [HttpGet]
        [Route("ListHospitals")]
        public ActionResult<IEnumerable<IndexHospitalVM.GetData>> GetAll()
        {
            var ListHospitals = _HospitalService.GetAll();
            if (ListHospitals.Count() == 0)
            {
                return Ok(new { data = ListHospitals, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = ListHospitals, msg = "Success", status = '1' });
        }

        [HttpGet]

        [Route("GetHospitalsByCityId/{cityId}")]

        public ActionResult GetHospitalsByCityId(int cityId)
        {
            var lstHospitals = _HospitalService.GetHospitalsByCityId(cityId);
            if (lstHospitals.Count() == 0)
            {
                return Ok(new { data = lstHospitals, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstHospitals, msg = "Success", status = '1' });
        }





        [HttpGet]
        [Route("GetHospitalDepartmentByHospitalId2/{hospitalId}")]
        public ActionResult GetHospitalDepartmentByHospitalId2(int hospitalId)
        {
            var lstHospitalDepartments = _HospitalService.GetHospitalDepartmentByHospitalId2(hospitalId);
            if (lstHospitalDepartments.Count() == 0)
            {
                return Ok(new { data = lstHospitalDepartments, msg = "No Data Found", status = '0' });
            }
            else
                return Ok(new { data = lstHospitalDepartments, msg = "Success", status = '1' });
        }
    }
}
