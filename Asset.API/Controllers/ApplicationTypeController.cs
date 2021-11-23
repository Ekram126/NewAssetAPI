using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ApplicationTypeVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationTypeController : ControllerBase
    {

        private IApplicationTypeService _applicationTypeService;

        public ApplicationTypeController(IApplicationTypeService applicationTypeService)
        {
            _applicationTypeService = applicationTypeService;
        }


        [HttpGet]
        [Route("ListApplicationTypes")]
        public IEnumerable<IndexApplicationTypeVM.GetData> GetAll()
        {
            return _applicationTypeService.GetAll();
        }


        //[HttpGet]
        //[Route("GetById/{id}")]
        //public ActionResult<EditApplicationTypeVM> GetById(int id)
        //{
        //    return _ApplicationTypeService.GetById(id);
        //}



        //[HttpPut]
        //[Route("UpdateApplicationType")]
        //public IActionResult Update(EditApplicationTypeVM ApplicationTypeVM)
        //{
        //    try
        //    {
        //        int id = ApplicationTypeVM.Id;
        //        int updatedRow = _ApplicationTypeService.Update(ApplicationTypeVM);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        string msg = ex.Message;
        //        return BadRequest("Error in update");
        //    }
        //    return Ok();
        //}


        //[HttpPost]
        //[Route("AddApplicationType")]
        //public ActionResult<ApplicationType> Add(CreateApplicationTypeVM ApplicationTypeVM)
        //{
        //    var savedId = _ApplicationTypeService.Add(ApplicationTypeVM);
        //    return CreatedAtAction("GetById", new { id = savedId }, ApplicationTypeVM);
        //}

        //[HttpDelete]
        //[Route("DeleteApplicationType/{id}")]
        //public ActionResult<ApplicationType> Delete(int id)
        //{
        //    try
        //    {
        //        var ApplicationTypeObj = _ApplicationTypeService.GetById(id);
        //        int deletedRow = _ApplicationTypeService.Delete(id);
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        string msg = ex.Message;
        //        return BadRequest("Error in delete");
        //    }
        //    return Ok();
        //}

    }
}
