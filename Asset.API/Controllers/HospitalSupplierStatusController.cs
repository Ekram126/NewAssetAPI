using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalSupplierStatusVM;
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
        private IHospitalSupplierStatusService _hospitalSupplierStatusService;


        public HospitalSupplierStatusController(IHospitalSupplierStatusService hospitalSupplierStatusService)
        {
            _hospitalSupplierStatusService = hospitalSupplierStatusService;
        }


        [HttpGet]
        [Route("GetAll/{appTypeId}/{hospitalId}")]
        public IndexHospitalSupplierStatusVM GetAll(int statusId, int appTypeId, int? hospitalId)
        {
            return _hospitalSupplierStatusService.GetAll(statusId, appTypeId, hospitalId);
        }


        [HttpGet]
        [Route("GetAllByAtatusAppTypeId/{statusId}/{appTypeId}/{hospitalId}")]
        public IndexHospitalSupplierStatusVM GetAllByAtatusAppTypeId(int statusId, int appTypeId, int? hospitalId)
        {
            return _hospitalSupplierStatusService.GetAll(statusId, appTypeId, hospitalId);
        }

        [HttpGet]
        [Route("GetAllByHospitals")]
        public IndexHospitalSupplierStatusVM GetAllByHospitals()
        {
            return _hospitalSupplierStatusService.GetAllByHospitals();
        }

        //(int statusId, int appTypeId, int? hospitalId);
        [HttpGet]
        [Route("GetHospitalByStatusAppTypeId/{statusId}/{appTypeId}/{hospitalId}")]
        public IndexHospitalSupplierStatusVM GetAllByHospitals(int statusId, int appTypeId, int? hospitalId)
        {
            return _hospitalSupplierStatusService.GetAllByHospitals(statusId, appTypeId, hospitalId);
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<HospitalSupplierStatus> GetById(int id)
        {
            return _hospitalSupplierStatusService.GetById(id);
        }


    }
}
