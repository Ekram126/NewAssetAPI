using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.GovernorateVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {

        private IGovernorateService _governorateService;
        private ICityService _cityService;
        private IHospitalService _hospitalService;

        public GovernorateController(IGovernorateService governorateService, ICityService cityService, IHospitalService hospitalService)
        {
            _governorateService = governorateService;
            _cityService = cityService;
            _hospitalService = hospitalService;
        }


        [HttpGet]
        [Route("ListGovernorates")]
        public IEnumerable<IndexGovernorateVM.GetData> GetAll()
       {
            return _governorateService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditGovernorateVM> GetById(int id)
        {
            return _governorateService.GetById(id);
        }




        [HttpGet]
        [Route("GetGovernorateByName/{govName}")]
        public ActionResult<EditGovernorateVM> GetGovernorateByName(string govName)
        {
            return _governorateService.GetGovernorateByName(govName);
        }


        [HttpPut]
        [Route("UpdateGovernorate")]
        public IActionResult Update(EditGovernorateVM GovernorateVM)
        {
            try
            {
                int id = GovernorateVM.Id;
                var lstOrgCode = _governorateService.GetAllGovernorates().ToList().Where(a => a.Code == GovernorateVM.Code && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Governorate code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstOrgNames = _governorateService.GetAllGovernorates().ToList().Where(a => a.Name == GovernorateVM.Name && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Governorate name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                var lstOrgArNames = _governorateService.GetAllGovernorates().ToList().Where(a => a.NameAr == GovernorateVM.NameAr && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Governorate arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _governorateService.Update(GovernorateVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddGovernorate")]
        public ActionResult<Governorate> Add(CreateGovernorateVM GovernorateVM)
        {
            var lstOrgCode = _governorateService.GetAllGovernorates().ToList().Where(a => a.Code == GovernorateVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Governorate code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _governorateService.GetAllGovernorates().ToList().Where(a => a.Name == GovernorateVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Governorate name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstOrgArNames = _governorateService.GetAllGovernorates().ToList().Where(a => a.NameAr == GovernorateVM.NameAr).ToList();
            if (lstOrgArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Governorate arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }

            else
            {
                var savedId = _governorateService.Add(GovernorateVM);
                return CreatedAtAction("GetById", new { id = savedId }, GovernorateVM);
            }
        }

        [HttpDelete]
        [Route("DeleteGovernorate/{id}")]
        public ActionResult<Governorate> Delete(int id)
        {
            try
            {
                var cityObj = _cityService.GetById(id);
                
                var lstCities = _cityService.GetCitiesByGovernorateId(id).ToList();
                if (lstCities.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "city", Message = "You cannot delete this governorate it has related cities", MessageAr = "لا يمكنك مسح المحافظة وذلك لارتباط مدن بها" });
                }
                var lstHospitals = _hospitalService.GetAllHospitals().Where(a => a.GovernorateId == id).ToList();
                if (lstHospitals.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "hospital", Message = "You cannot delete this governorate it has related hospitals", MessageAr = "لا يمكنك مسح المحافظة وذلك لارتباط مستشفيات بها" });
                }

                
                else
                {
                    int deletedRow = _governorateService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }
        [HttpGet("GetGovernorateWithHospitals")]
        public IEnumerable<GovernorateWithHospitalsVM> GetGovernorateWithHospitals()
        {
            return _governorateService.GetGovernorateWithHospitals();
        }
    }
}
