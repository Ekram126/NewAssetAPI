using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalExecludeReasonVM;
using Asset.ViewModels.HospitalHoldReasonVM;
using Asset.ViewModels.HospitalReasonTransactionVM;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.UserVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalApplicationTransactionController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IHospitalApplicationService _hospitalApplicationService;
        private readonly EmailConfigurationVM _emailConfig;
        private IHospitalExecludeReasonService _hospitalExecludeReasonService;
        private IHospitalHoldReasonService _hospitalHoldReasonService;
        private IMasterAssetService _masterAssetService;
        private IAssetDetailService _assetDetailService;


        private readonly IEmailSender _emailSender;
        private IHospitalReasonTransactionService _hospitalReasonTransactionService;


        private IPagingService _pagingService;
        //  [Obsolete]
        IWebHostEnvironment _webHostingEnvironment;
        public HospitalApplicationTransactionController(UserManager<ApplicationUser> userManager, IHospitalReasonTransactionService hospitalReasonTransactionService,
            IHospitalApplicationService hospitalApplicationService, IWebHostEnvironment webHostingEnvironment, IPagingService pagingService,
            IEmailSender emailSender, IAssetDetailService assetDetailService, EmailConfigurationVM emailConfig,
            IHospitalExecludeReasonService hospitalExecludeReasonService, IHospitalHoldReasonService hospitalHoldReasonService,
            IMasterAssetService masterAssetService)
        {
            _hospitalReasonTransactionService = hospitalReasonTransactionService;
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;
            _emailConfig = emailConfig;


            _userManager = userManager;
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;
            _emailSender = emailSender;
            _assetDetailService = assetDetailService;
            _pagingService = pagingService;
            _masterAssetService = masterAssetService;
            _hospitalExecludeReasonService = hospitalExecludeReasonService;
            _hospitalHoldReasonService = hospitalHoldReasonService;
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

            return  _hospitalReasonTransactionService.Add(transObj);           
        }


     
        [HttpDelete]
        [Route("DeleteHospitalReasonTransaction/{id}")]
        public ActionResult<HospitalApplication> Delete(int id)
        {
            try
            {
                //var HospitalApplicationObj = _hospitalReasonTransactionService.GetById(id);
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
