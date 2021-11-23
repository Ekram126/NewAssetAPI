using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.OrganizationVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Asset.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private IOrganizationService _organizationService;
        private IHospitalService _hospitalService;
        private ISubOrganizationService _subOrganizationService;
        public OrganizationController(IOrganizationService organizationService, IHospitalService hospitalService, ISubOrganizationService subOrganizationService)
        {
            _organizationService = organizationService;
            _hospitalService = hospitalService;
            _subOrganizationService = subOrganizationService;
        }


        [HttpGet]
        [Route("ListOrganizations")]
        public IEnumerable<IndexOrganizationVM.GetData> GetAll()
        {
            return _organizationService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditOrganizationVM> GetById(int id)
        {
            return _organizationService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateOrganization")]
        public IActionResult Update(EditOrganizationVM organizationVM)
        { 
            try
            {
                int id = organizationVM.Id;
                var lstOrgCode = _organizationService.GetAllOrganizations().ToList().Where(a => a.Code == organizationVM.Code && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Organization code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstOrgNames = _organizationService.GetAllOrganizations().ToList().Where(a => a.Name == organizationVM.Name && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Organization name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                var lstOrgArNames = _organizationService.GetAllOrganizations().ToList().Where(a => a.NameAr == organizationVM.NameAr && a.Id != id).ToList();
                if (lstOrgArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Organization arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _organizationService.Update(organizationVM);
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
        [Route("AddOrganization")]
        public ActionResult<Organization> Add(CreateOrganizationVM organizationVM)
        {
            var lstOrgCode = _organizationService.GetAllOrganizations().ToList().Where(a => a.Code == organizationVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Organization code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _organizationService.GetAllOrganizations().ToList().Where(a => a.Name == organizationVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Organization name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }

            var lstOrgArNames = _organizationService.GetAllOrganizations().ToList().Where(a => a.NameAr == organizationVM.NameAr).ToList();
            if (lstOrgArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "nameAr", Message = "Organization arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _organizationService.Add(organizationVM);
                return CreatedAtAction("GetById", new { id = savedId }, organizationVM);
            }
        }

        [HttpDelete]
        [Route("DeleteOrganization/{id}")]
        public ActionResult<Organization> Delete(int id)
        {
            try
            {

                var lstSubOrgs = _subOrganizationService.GetSubOrganizationByOrgId(id).ToList();
                if (lstSubOrgs.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "sub", Message = "You cannot delete this organization it has related sub organizations", MessageAr = "لا يمكنك مسح الهيئة وذلك لارتباط هيئات فرعية بها" });
                }


                var lstHospitals = _hospitalService.GetAllHospitals().Where(a=>a.OrganizationId == id).ToList();
                if (lstHospitals.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "hospital", Message = "You cannot delete this organization it has related hospitals", MessageAr = "لا يمكنك مسح الهيئة وذلك لارتباط مستشفيات  بها" });
                }
              
                else
                {
                    int deletedRow = _organizationService.Delete(id);
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
