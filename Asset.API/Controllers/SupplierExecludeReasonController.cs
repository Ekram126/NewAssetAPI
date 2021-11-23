using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.SupplierExecludeReasonVM;
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
    public class SupplierExecludeReasonController : ControllerBase
    {

        private ISupplierExecludeReasonService _supplierExecludeReasonService;

        public SupplierExecludeReasonController(ISupplierExecludeReasonService supplierExecludeReasonService)
        {
            _supplierExecludeReasonService = supplierExecludeReasonService;

        }


        [HttpGet]
        [Route("ListSupplierExcludeReasons")]
        public IEnumerable<IndexSupplierExcludeReasonVM.GetData> GetAll()
        {
            return _supplierExecludeReasonService.GetAll();
        }





        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierExcludeReasonVM> GetById(int id)
        {
            return _supplierExecludeReasonService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateSupplierExecludeReason")]
        public IActionResult Update(EditSupplierExcludeReasonVM SupplierExecludeReasonVM)
        {
            try
            {
                int id = SupplierExecludeReasonVM.Id;



                int updatedRow = _supplierExecludeReasonService.Update(SupplierExecludeReasonVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddSupplierExecludeReason")]
        public ActionResult<SupplierExecludeReason> Add(CreateSupplierExcludeReasonVM SupplierExecludeReasonVM)
        {
  
                var savedId = _supplierExecludeReasonService.Add(SupplierExecludeReasonVM);
                return CreatedAtAction("GetById", new { id = savedId }, SupplierExecludeReasonVM);
            
        }

        [HttpDelete]
        [Route("DeleteSupplierExecludeReason/{id}")]
        public ActionResult<SupplierExecludeReason> Delete(int id)
        {
            try
            {
                var SupplierExecludeReasonObj = _supplierExecludeReasonService.GetById(id);
            
                    int deletedRow = _supplierExecludeReasonService.Delete(id);
                
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
