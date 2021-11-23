using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalExecludeReasonVM;
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
    public class HospitalExecludeReasonController : ControllerBase
    {

        private IHospitalExecludeReasonService _hospitalExecludeReasonService;

        public HospitalExecludeReasonController(IHospitalExecludeReasonService hospitalExecludeReasonService)
        {
            _hospitalExecludeReasonService = hospitalExecludeReasonService;
        }


        [HttpGet]
        [Route("ListHospitalExcludeReasons")]
        public IEnumerable<IndexHospitalExecludeReasonVM.GetData> GetAll()
        {
            return _hospitalExecludeReasonService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalExecludeReasonVM> GetById(int id)
        {
            return _hospitalExecludeReasonService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateHospitalExecludeReason")]
        public IActionResult Update(EditHospitalExecludeReasonVM hospitalExecludeReasonVM)
        {
            try
            {
                int id = hospitalExecludeReasonVM.Id;
                int updatedRow = _hospitalExecludeReasonService.Update(hospitalExecludeReasonVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddHospitalExecludeReason")]
        public ActionResult<HospitalExecludeReason> Add(CreateHospitalExecludeReasonVM hospitalExecludeReasonVM)
        {

            var savedId = _hospitalExecludeReasonService.Add(hospitalExecludeReasonVM);
            return CreatedAtAction("GetById", new { id = savedId }, hospitalExecludeReasonVM);

        }

        [HttpDelete]
        [Route("DeleteHospitalExecludeReason/{id}")]
        public ActionResult<HospitalExecludeReason> Delete(int id)
        {
            try
            {
                var HospitalExecludeReasonObj = _hospitalExecludeReasonService.GetById(id);
                int deletedRow = _hospitalExecludeReasonService.Delete(id);
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
