using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierExecludeController : ControllerBase
    {

        private ISupplierExecludeService _supplierExecludeService;
      
    
        public SupplierExecludeController(ISupplierExecludeService supplierExecludeService)
        {
            _supplierExecludeService = supplierExecludeService;
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<SupplierExeclude> GetById(int id)
        {
            return _supplierExecludeService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateSupplierExeclude")]
        public IActionResult Update(SupplierExeclude SupplierExecludeVM)
        {
            try
            {

                int updatedRow = _supplierExecludeService.Update(SupplierExecludeVM);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }



        [HttpPost]
        [Route("AddSupplierExeclude")]
        public int Add(SupplierExeclude supplierExecludeObj)
        {

            var savedId = _supplierExecludeService.Add(supplierExecludeObj);
            return savedId;
        }

     
    }
}
