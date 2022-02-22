using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierHoldReasonVM;
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
    public class SupplierHoldReasonController : ControllerBase
    {

        private ISupplierHoldReasonService _supplierHoldReasonService;

        public SupplierHoldReasonController(ISupplierHoldReasonService supplierHoldReasonService)
        {
            _supplierHoldReasonService = supplierHoldReasonService;

        }


        [HttpGet]
        [Route("ListSupplierHoldReasons")]
        public IEnumerable<IndexSupplierHoldReasonVM.GetData> GetAll()
        {
            return _supplierHoldReasonService.GetAll();
        }





        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierHoldReasonVM> GetById(int id)
        {
            return _supplierHoldReasonService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateSupplierHoldReason")]
        public IActionResult Update(EditSupplierHoldReasonVM SupplierHoldReasonVM)
        {
            try
            {
                int id = SupplierHoldReasonVM.Id;



                int updatedRow = _supplierHoldReasonService.Update(SupplierHoldReasonVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddSupplierHoldReason")]
        public ActionResult<SupplierHoldReason> Add(CreateSupplierHoldReasonVM SupplierHoldReasonVM)
        {
  
                var savedId = _supplierHoldReasonService.Add(SupplierHoldReasonVM);
                return CreatedAtAction("GetById", new { id = savedId }, SupplierHoldReasonVM);
            
        }

        [HttpDelete]
        [Route("DeleteSupplierHoldReason/{id}")]
        public ActionResult<SupplierHoldReason> Delete(int id)
        {
            try
            {
                var SupplierHoldReasonObj = _supplierHoldReasonService.GetById(id);
            
                    int deletedRow = _supplierHoldReasonService.Delete(id);
                
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
