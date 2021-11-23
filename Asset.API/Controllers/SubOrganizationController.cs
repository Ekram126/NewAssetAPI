using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SubOrganizationVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Asset.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SubOrganizationController : ControllerBase
    {
        private ISubOrganizationService _SubOrganizationService;
        private IHospitalService _hospitalService;

        public SubOrganizationController(ISubOrganizationService SubOrganizationService,     IHospitalService hospitalService)
        {
            _SubOrganizationService = SubOrganizationService;
            _hospitalService = hospitalService;
        }


        [HttpGet] 
        [Route("ListSubOrganizations")]
        public IEnumerable<IndexSubOrganizationVM.GetData> GetAll()
        {
            return _SubOrganizationService.GetAll();
        }


        [HttpGet]
        [Route("GetSubOrganizationByOrgId/{orgId}")]
        public IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgId(int orgId)
        {
            return _SubOrganizationService.GetSubOrganizationByOrgId(orgId);
        }






        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<SubOrganization> GetById(int id)
        {
            return _SubOrganizationService.GetById(id);
        }



        [HttpGet]
        [Route("GetOrganizationBySubId/{subId}")]
        public ActionResult<Organization> GetOrganizationBySubId(int subId)
        {
            return _SubOrganizationService.GetOrganizationBySubId(subId);
        }


        [HttpPut]
        [Route("UpdateSubOrganization")]
        public IActionResult Update(EditSubOrganizationVM subOrganizationVM)
        {
            try
            {
                int id = subOrganizationVM.Id;
                var lstOrgCode = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.Code == subOrganizationVM.Code && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Sub Organization code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstOrgNames = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.Name == subOrganizationVM.Name && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Sub Organization name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }



                var lstOrgArNames = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.NameAr == subOrganizationVM.NameAr && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Sub Organization arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _SubOrganizationService.Update(subOrganizationVM);
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
        [Route("AddSubOrganization")]
        public ActionResult<SubOrganization> Add(CreateSubOrganizationVM subOrganizationVM)
        {
            var lstOrgCode = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.Code == subOrganizationVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Sub Organization code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.Name == subOrganizationVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Sub Organization name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }

            var lstOrgArNames = _SubOrganizationService.GetAllSubOrganizations().ToList().Where(a => a.NameAr == subOrganizationVM.NameAr).ToList();
            if (lstOrgArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Sub Organization arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _SubOrganizationService.Add(subOrganizationVM);
                return CreatedAtAction("GetById", new { id = savedId }, subOrganizationVM);
            }
        }

        [HttpDelete]
        [Route("DeleteSubOrganization/{id}")]
        public ActionResult<SubOrganization> Delete(int id)
        {
            try
            {
                var lstHospitals = _hospitalService.GetAllHospitals().Where(a => a.SubOrganizationId == id).ToList();
                if (lstHospitals.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "hospital", Message = "You cannot delete this sub organization it has related hospitals", MessageAr = "لا يمكنك مسح الهيئة الفرعية وذلك لارتباط مستشفيات  بها" });
                }
                else
                {
                    int deletedRow = _SubOrganizationService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

    }
}
