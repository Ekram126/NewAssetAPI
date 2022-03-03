using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalReasonTransactionVM;
using Asset.ViewModels.PagingParameter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalApplicationTransactionController : ControllerBase
    {

        private IHospitalReasonTransactionService _hospitalReasonTransactionService;
        private IHospitalApplicationService _hospitalApplicationService;
        private IPagingService _pagingService;
        //  [Obsolete]
        IWebHostEnvironment _webHostingEnvironment;
        public HospitalApplicationTransactionController(IHospitalReasonTransactionService hospitalReasonTransactionService, IHospitalApplicationService hospitalApplicationService, IWebHostEnvironment webHostingEnvironment, IPagingService pagingService)
        {
            _hospitalReasonTransactionService = hospitalReasonTransactionService;
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;
        }

        [HttpGet]
        [Route("ListHospitalReasonTransaction")]
        public IEnumerable<HospitalReasonTransaction> GetAll()
        {
            return _hospitalReasonTransactionService.GetAll();
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<HospitalReasonTransaction> GetById(int id)
        {
            return _hospitalReasonTransactionService.GetById(id);
        }


        [HttpPut]
        [Route("UpdateHospitalReasonTransaction")]
        public IActionResult Update(HospitalReasonTransaction hospitalApplicationVM)
        {
            try
            {
                int id = hospitalApplicationVM.Id;
                int updatedRow = _hospitalReasonTransactionService.Update(hospitalApplicationVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
            return Ok();
        }


  
        [HttpPost]
        [Route("AddHospitalReasonTransaction")]
        public int Add(CreateHospitalReasonTransactionVM transObj)
        {
            var savedId = _hospitalReasonTransactionService.Add(transObj);
            return savedId;
        }

        [HttpDelete]
        [Route("DeleteHospitalReasonTransaction/{id}")]
        public ActionResult<HospitalApplication> Delete(int id)
        {
            try
            {
                var HospitalApplicationObj = _hospitalReasonTransactionService.GetById(id);
                int deletedRow = _hospitalReasonTransactionService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpGet]
        [Route("GetAttachments/{appId}")]
        public IEnumerable<IndexHospitalReasonTransactionVM.GetData> GetAttachmentByHospitalApplicationId(int appId)
        {
            return _hospitalReasonTransactionService.GetAttachmentByHospitalApplicationId(appId);
        }


    }
}
