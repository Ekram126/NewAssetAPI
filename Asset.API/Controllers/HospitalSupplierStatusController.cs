using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestStatusVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalSupplierStatusController : ControllerBase
    {
        private IHospitalSupplierStatusService  _hospitalSupplierStatusService;
      

        public HospitalSupplierStatusController(IHospitalSupplierStatusService hospitalSupplierStatusService)
        {
            _hospitalSupplierStatusService = hospitalSupplierStatusService;
        }
       
     
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<HospitalSupplierStatus> GetAll()
        {
            return _hospitalSupplierStatusService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<HospitalSupplierStatus> GetById(int id)
        {
            return _hospitalSupplierStatusService.GetById(id);
        }


    }
}
