using Asset.API.Helpers;
using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.SupplierExecludeReasonVM;
using Asset.ViewModels.SupplierHoldReasonVM;
using Asset.ViewModels.UserVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierExecludeController : ControllerBase
    {
        private ISupplierService _supplierService;
        private ISupplierExecludeService _supplierExecludeService;
        private ISupplierExecludeReasonService _supplierExecludeReasonService;
        private ISupplierHoldReasonService _supplierHoldReasonService;
        private ISupplierExecludeAssetService _supplierExecludeAssetService;
        private IMasterAssetService _masterAssetService;
        private IAssetDetailService _assetDetailService;
        private readonly UserManager<ApplicationUser> _userManager;


        private readonly IEmailSender _emailSender;

        public SupplierExecludeController(UserManager<ApplicationUser> userManager, ISupplierExecludeService supplierExecludeService,
            IEmailSender emailSender, IAssetDetailService assetDetailService, ISupplierExecludeAssetService supplierExecludeAssetService,
            ISupplierExecludeReasonService supplierExecludeReasonService, ISupplierHoldReasonService supplierHoldReasonService,
            IMasterAssetService masterAssetService, ISupplierService supplierService)
        {
            _supplierExecludeService = supplierExecludeService;


            _userManager = userManager;
            _emailSender = emailSender;
            _assetDetailService = assetDetailService;
            _masterAssetService = masterAssetService;
            _supplierHoldReasonService = supplierHoldReasonService;
            _supplierExecludeReasonService = supplierExecludeReasonService;
            _supplierExecludeAssetService = supplierExecludeAssetService;
            _supplierService = supplierService;

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

            return _supplierExecludeService.Add(supplierExecludeObj);
        }


        [HttpGet]
        [Route("SendSupplierExcludeEmail/{supplierExecludeAssetId}")]
        public async Task<int> SendSupplierExcludeEmail(int supplierExecludeAssetId)
        {
            string strExcludes = "";
            string strHolds = "";
            string phone = "";
            string userName = "";
            string exchold = "";
            List<string> execludeNames = new List<string>();
            List<IndexSupplierExcludeReasonVM.GetData> lstExcludes = new List<IndexSupplierExcludeReasonVM.GetData>();
            List<IndexSupplierHoldReasonVM.GetData> lstHolds = new List<IndexSupplierHoldReasonVM.GetData>();


            var applicationObj = _supplierExecludeService.GetById(supplierExecludeAssetId);
            var hospitalAssetObj = _supplierExecludeAssetService.GetById(supplierExecludeAssetId);
            var userObj = await _userManager.FindByNameAsync("MemberUser");

            var lstSuppliers = _supplierService.GetAllSuppliers().Where(a => a.EMail == userObj.Email).ToList();
            if (lstSuppliers.Count > 0)
            {
                phone = lstSuppliers[0].Mobile;
                userName = lstSuppliers[0].Name;
            }
            if (lstSuppliers.Count == 0)
            {
                phone = userObj.PhoneNumber;
                userName = userObj.UserName;
            }
            var assetObj = _assetDetailService.GetById(int.Parse(hospitalAssetObj.AssetId.ToString()));
            var masterObj = _masterAssetService.GetById(int.Parse(assetObj.MasterAssetId.ToString()));

            if (hospitalAssetObj.AppTypeId == 1)
            {
                exchold = "Exclude";
                var lstReasons = _supplierExecludeService.GetAll().Where(a => a.SupplierExecludeAssetId == supplierExecludeAssetId).ToList();
                if (lstReasons.Count > 0)
                {

                    foreach (var item in lstReasons)
                    {
                        lstExcludes.Add(_supplierExecludeReasonService.GetAll().Where(a => a.Id == item.ReasonId).FirstOrDefault());
                    }
                    foreach (var reason in lstExcludes)
                    {
                        execludeNames.Add(reason.NameAr);
                    }
                    strExcludes = string.Join(",", execludeNames);
                }
            }



            if (hospitalAssetObj.AppTypeId == 2)
            {
                exchold = "Hold";
                var lstReasons = _supplierExecludeService.GetAll().Where(a => a.SupplierExecludeAssetId == supplierExecludeAssetId).ToList();
                foreach (var item in lstReasons)
                {
                    var itemObj = _supplierHoldReasonService.GetAll().Where(a => a.Id == item.ReasonId).FirstOrDefault();

                    lstHolds.Add(itemObj);
                }

                foreach (var reason in lstHolds)
                {
                    execludeNames.Add(reason.NameAr);
                }
                strHolds = string.Join(",", execludeNames);
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
            if (hospitalAssetObj.AppTypeId == 1)
            {
                strBuild.Append("<tr>");
                strBuild.Append("<td> Reasons");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + strExcludes);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");
            }
            if (hospitalAssetObj.AppTypeId == 2)
            {
                strBuild.Append("<tr>");
                strBuild.Append("<td> Reasons");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + strHolds);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");
            }
            strBuild.Append("</table>");



            string from = "almostakbaltechnology.dev@gmail.com";
            string to = "pineapple_126@hotmail.com ";
            string subject = "Exclude-Hold Asset";
            string body = strBuild.ToString();
            string appSpecificPassword = "fajtjigwpcnxyyuv";

            var mailMessage = new MailMessage(from, to, subject, body);
            var mailMessage2 = new MailMessage(from, userObj.Email, subject, body);
            mailMessage.IsBodyHtml = true;
            mailMessage2.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {

                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                smtpClient.Send(mailMessage);
                smtpClient.Send(mailMessage2);
            }



            var SMSobj = new SendSMS();
            SMSobj.Language = 1;
            SMSobj.Mobile = phone;// "01021162629";
            SMSobj.Environment = 1;
            //SMSobj.Username = userName;
            SMSobj.Message = $"This Asset {masterObj.NameAr} with barcode:{assetObj.Barcode} requested to be {exchold}";
            var json = JsonConvert.SerializeObject(SMSobj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //  var UrlSMS = "https://smsmisr.com/api/v2";
            var UrlSMS = "https://smsmisr.com/api/SMS/?";


            using var client = new HttpClient();
            var response = await client.PostAsync(UrlSMS, data);
            string resultS = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(resultS);



            return 1;

        }
    }
}
