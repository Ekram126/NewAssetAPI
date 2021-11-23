using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalHoldReasonVM;
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
    public class HospitalHoldReasonController : ControllerBase
    {

        private IHospitalHoldReasonService _hospitalHoldReasonService;

        public HospitalHoldReasonController(IHospitalHoldReasonService hospitalHoldReasonService)
        {
            _hospitalHoldReasonService = hospitalHoldReasonService;
        }


        [HttpGet]
        [Route("ListHospitalHoldReasons")]
        public IEnumerable<IndexHospitalHoldReasonVM.GetData> GetAll()
        {
            return _hospitalHoldReasonService.GetAll();
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalHoldReasonVM> GetById(int id)
        {
            return _hospitalHoldReasonService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateHospitalHoldReason")]
        public IActionResult Update(EditHospitalHoldReasonVM hospitalHoldReasonVM)
        {
            try
            {
                int id = hospitalHoldReasonVM.Id;
                int updatedRow = _hospitalHoldReasonService.Update(hospitalHoldReasonVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
            return Ok();
        }


        [HttpPost]
        [Route("AddHospitalHoldReason")]
        public ActionResult<HospitalHoldReason> Add(CreateHospitalHoldReasonVM hospitalHoldReasonVM)
        {

            var savedId = _hospitalHoldReasonService.Add(hospitalHoldReasonVM);
            return CreatedAtAction("GetById", new { id = savedId }, hospitalHoldReasonVM);

        }

        [HttpDelete]
        [Route("DeleteHospitalHoldReason/{id}")]
        public ActionResult<HospitalHoldReason> Delete(int id)
        {
            try
            {
                var HospitalHoldReasonObj = _hospitalHoldReasonService.GetById(id);
                int deletedRow = _hospitalHoldReasonService.Delete(id);
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
