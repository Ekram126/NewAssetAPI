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
        public IEnumerable<IndexHospitalVM.GetData> GetAll()
        {
            return _HospitalService.GetAll().ToList();
        }

        [HttpGet]
        [Route("GetHospitalsByCityId/{cityId}")]
        public IEnumerable<Hospital> GetHospitalsByCityId(int cityId)
        {
            return _HospitalService.GetHospitalsByCityId(cityId);
        }
    }
}
