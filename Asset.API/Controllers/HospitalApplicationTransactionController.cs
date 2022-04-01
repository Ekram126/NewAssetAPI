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
            IEmailSender emailSender, IAssetDetailService assetDetailService,
            IHospitalExecludeReasonService hospitalExecludeReasonService, IHospitalHoldReasonService hospitalHoldReasonService,
            IMasterAssetService masterAssetService)
        {
            _hospitalReasonTransactionService = hospitalReasonTransactionService;
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;


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
        public async Task<int> Add(CreateHospitalReasonTransactionVM transObj)
        {
            string strExcludes = "";
            string  strHolds = "";
            List<string> execludeNames = new List<string>();
            List<IndexHospitalExecludeReasonVM.GetData> lstExcludes = new List<IndexHospitalExecludeReasonVM.GetData>();
            List<IndexHospitalHoldReasonVM.GetData> lstHolds = new List<IndexHospitalHoldReasonVM.GetData>();


            var savedId = _hospitalReasonTransactionService.Add(transObj);
            var applicationObj = _hospitalApplicationService.GetById(int.Parse(transObj.HospitalApplicationId.ToString()));
            var userObj = await _userManager.FindByNameAsync("MemberUser");
            var assetObj = _assetDetailService.GetById(int.Parse(applicationObj.AssetId.ToString()));
            var masterObj = _masterAssetService.GetById(int.Parse(assetObj.MasterAssetId.ToString()));


            var lstReasons = _hospitalReasonTransactionService.GetAll().Where(a => a.HospitalApplicationId == transObj.HospitalApplicationId).ToList();


            if (lstReasons.Count > 0)
            {
                if (applicationObj.AppTypeId == 1)
                {
                    foreach (var item in lstReasons)
                    {
                        lstExcludes.Add(_hospitalExecludeReasonService.GetAll().Where(a => a.Id == item.ReasonId).FirstOrDefault());


                    }
                    foreach (var reason in lstExcludes)
                    {
                        execludeNames.Add(reason.NameAr);
                    }
                    strExcludes = string.Join(",", execludeNames);
                }



                if (applicationObj.AppTypeId == 2)
                {
                    foreach (var item in lstReasons)
                    {
                        lstHolds.Add(_hospitalHoldReasonService.GetAll().Where(a => a.Id == item.ReasonId).FirstOrDefault());
                    }

                    foreach (var reason in lstHolds)
                    {
                        execludeNames.Add(reason.NameAr);
                    }
                    strHolds = string.Join(",", execludeNames);
                }
            }

            StringBuilder strBuild = new StringBuilder();
            strBuild.Append($"Dear {userObj.UserName}\r\n");
            strBuild.Append("<table>");
            strBuild.Append("<tr>");
            strBuild.Append("<td> Asset Name");
            strBuild.Append("</td>");
            strBuild.Append("<td>" + masterObj.NameAr);
            strBuild.Append("</td>");
            strBuild.Append("</tr>");
            strBuild.Append("<tr>");
            strBuild.Append("<td> Serial");
            strBuild.Append("</td>");
            strBuild.Append("<td>" + assetObj.SerialNumber);
            strBuild.Append("</td>");
            strBuild.Append("</tr>");
            strBuild.Append("<tr>");
            strBuild.Append("<td> BarCode");
            strBuild.Append("</td>");
            strBuild.Append("<td>" + assetObj.Barcode);
            strBuild.Append("</td>");
            strBuild.Append("</tr>");
            if (applicationObj.AppTypeId == 1)
            {
                strBuild.Append("<tr>");
                strBuild.Append("<td> Reasons");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + strExcludes);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");
            }
            if (applicationObj.AppTypeId == 2)
            {
                strBuild.Append("<tr>");
                strBuild.Append("<td> Reasons");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + strHolds);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");
            }
            strBuild.Append("</table>");


            var message = new MessageVM(new string[] { userObj.Email }, "Exclude-Hold Asset", strBuild.ToString());
            var message2 = new MessageVM(new string[] { "pineapple_126@hotmail.com" }, "Exclude-Hold Asset", strBuild.ToString());
   
            _emailSender.SendEmail(message);
            _emailSender.SendEmail(message2);


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
