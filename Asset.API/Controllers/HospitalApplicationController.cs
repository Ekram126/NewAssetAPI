using Asset.API.Helpers;
using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.HospitalApplicationVM;
using Asset.ViewModels.HospitalExecludeReasonVM;
using Asset.ViewModels.HospitalHoldReasonVM;
using Asset.ViewModels.PagingParameter;
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
    public class HospitalApplicationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IHospitalApplicationService _hospitalApplicationService;
        private IHospitalReasonTransactionService _hospitalReasonTransactionService;

        private IHospitalExecludeReasonService _hospitalExecludeReasonService;
        private IHospitalHoldReasonService _hospitalHoldReasonService;

        private IEmployeeService _employeeService;
        private IMasterAssetService _masterAssetService;
        private IAssetDetailService _assetDetailService;
        // private ApplicationUser applicationUser;

        private readonly IEmailSender _emailSender;

        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;


        public HospitalApplicationController(UserManager<ApplicationUser> userManager, IHospitalApplicationService hospitalApplicationService, IWebHostEnvironment webHostingEnvironment,
            IPagingService pagingService, IEmailSender emailSender, IAssetDetailService assetDetailService, IEmployeeService employeeService,
            IHospitalExecludeReasonService hospitalExecludeReasonService, IHospitalHoldReasonService hospitalHoldReasonService,
            IMasterAssetService masterAssetService, IHospitalReasonTransactionService hospitalReasonTransactionService)
        {
            _userManager = userManager;
            _hospitalApplicationService = hospitalApplicationService;
            _webHostingEnvironment = webHostingEnvironment;
            _emailSender = emailSender;
            _assetDetailService = assetDetailService;
            _pagingService = pagingService;
            _masterAssetService = masterAssetService;
            _hospitalReasonTransactionService = hospitalReasonTransactionService;
            _hospitalExecludeReasonService = hospitalExecludeReasonService;
            _hospitalHoldReasonService = hospitalHoldReasonService;
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("ListHospitalApplications")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAll()
        {
            return _hospitalApplicationService.GetAll();
        }

        [HttpPut]
        [Route("ListHospitalApplicationsWithPaging")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAll().ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }
        [HttpGet]
        [Route("GenerateHospitalApplicationNumber")]
        public GeneratedHospitalApplicationNumberVM GenerateHospitalApplicationNumber()
        {
            return _hospitalApplicationService.GenerateHospitalApplicationNumber();
        }

        [HttpPut]
        [Route("ListHospitalApplicationsWithPaging/{hospitalId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllWithPaging(int? hospitalId, PagingParameter pageInfo)
        {
            List<IndexHospitalApplicationVM.GetData> list = new List<IndexHospitalApplicationVM.GetData>();

            if (hospitalId != 0)

                list = _hospitalApplicationService.GetAllByHospitalId(int.Parse(hospitalId.ToString())).ToList();
            else
                list = _hospitalApplicationService.GetAll().ToList();


            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, list);
        }


        [HttpPost]
        [Route("GetHospitalApplicationByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetHospitalApplicationByDate(int pagenumber, int pagesize, SearchHospitalApplicationVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstRequests = _hospitalApplicationService.GetHospitalApplicationByDate(searchObj).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstRequests);
        }


        [HttpPost]
        [Route("CountGetHospitalApplicationByDate")]
        public int CountGetHospitalApplicationByDate(SearchHospitalApplicationVM searchObj)
        {
            return _hospitalApplicationService.GetHospitalApplicationByDate(searchObj).ToList().Count;
        }


        [HttpPost]
        [Route("GetAllHospitalExecludes/{statusId}/{appTypeId}/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            var lstExcludes = _hospitalApplicationService.GetAllHospitalExecludes(searchObj,statusId, appTypeId, hospitalId, pageNumber, pageSize);
            return lstExcludes;
        }

        [HttpPost]
        [Route("GetAllHospitalHolds/{statusId}/{appTypeId}/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexHospitalApplicationVM GetAllHospitalHolds(SearchHospitalApplicationVM searchObj,int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            var lstHolds = _hospitalApplicationService.GetAllHospitalHolds(searchObj,statusId, appTypeId, hospitalId, pageNumber, pageSize);
            return lstHolds;
        }



        [HttpPost]
        [Route("PrintAllHospitalExecludes")]
        public IndexHospitalApplicationVM GetAllHospitalExecludes(SearchHospitalApplicationVM searchObj)
        {
            var lstExcludes = _hospitalApplicationService.GetAllHospitalExecludes(searchObj);
            return lstExcludes;
        }

        [HttpPost]
        [Route("PrintAllHospitalHolds")]
        public IndexHospitalApplicationVM PrintAllHospitalHolds(SearchHospitalApplicationVM searchObj)
        {
            var lstHolds = _hospitalApplicationService.GetAllHospitalHolds(searchObj);
            return lstHolds;
        }







        [HttpGet]
        [Route("getcount/{hospitalId}")]
        public int count(int? hospitalId)
        {
            int listCount = 0;
            if (hospitalId != 0)

                listCount = _hospitalApplicationService.GetAllByHospitalId(int.Parse(hospitalId.ToString())).ToList().Count;
            else
                listCount = _hospitalApplicationService.GetAll().ToList().Count;


            return listCount; //_pagingService.Count<HospitalApplication>();
        }

        [HttpPut]
        [Route("GetAllHospitalsByStatusId/{statusId}/{hospitalId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> GetAllByStatusId(PagingParameter pageInfo, int statusId, int hospitalId)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAllByStatusId(statusId, hospitalId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }

        [HttpGet]
        [Route("GetHospitalCountAfterFilterStatusId/{statusId}/{hospitalId}")]
        public int GetCountAfterFilterStatusId(int statusId, int hospitalId)
        {
            return _hospitalApplicationService.GetAllByStatusId(statusId, hospitalId).ToList().Count;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalApplicationVM> GetById(int id)
        {
            return _hospitalApplicationService.GetById(id);
        }

        [HttpGet]
        [Route("GetHospitalApplicationById/{id}")]
        public ActionResult<ViewHospitalApplicationVM> GetHospitalApplicationById(int id)
        {
            return _hospitalApplicationService.GetHospitalApplicationById(id);
        }

        [HttpGet]
        [Route("GetAssetHospitalId/{assetId}")]
        public ActionResult<int> GetAssetHospitalId(int assetId)
        {
            return _hospitalApplicationService.GetAssetHospitalId(assetId);
        }

        [HttpPut]
        [Route("ListHospitalApplicationsWithPagingAndTypeId/{appTypeId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> ListHospitalApplicationsWithPagingAndTypeId(int appTypeId, PagingParameter pageInfo)
        {
            var lstHospitalApplications = _hospitalApplicationService.GetAllByAppTypeId(appTypeId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstHospitalApplications);
        }
        [HttpGet]
        [Route("GetCountByAppTypeId/{appTypeId}")]
        public int GetcountByAppTypeId(int appTypeId)
        {
            return _hospitalApplicationService.GetAllByAppTypeId(appTypeId).ToList().Count;
        }





        [HttpPut]
        [Route("ListHospitalApplicationsByTypeIdAndStatusId/{statusId}/{appTypeId}/{hospitalId}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> ListHospitalApplicationsByTypeIdAndStatusId(int statusId, int appTypeId, int hospitalId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _hospitalApplicationService.GetAllByAppTypeIdAndStatusId(statusId, appTypeId, hospitalId).ToList();
            return _pagingService.GetAll<IndexHospitalApplicationVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("GetcountByAppTypeIdAndStatusId/{statusId}/{appTypeId}/{hospitalId}")]
        public int GetcountByAppTypeIdAndStatusId(int statusId, int appTypeId, int hospitalId)
        {
            var total = _hospitalApplicationService.GetAllByAppTypeIdAndStatusId(statusId, appTypeId, hospitalId).ToList().Count;
            return total;
        }




        [HttpPut]
        [Route("UpdateHospitalApplication")]
        public IActionResult Update(EditHospitalApplicationVM hospitalApplicationVM)
        {
            try
            {
                int id = hospitalApplicationVM.Id;
                int updatedRow = _hospitalApplicationService.Update(hospitalApplicationVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
            return Ok();
        }


        [HttpPut]
        [Route("UpdateExcludedDate")]
        public IActionResult UpdateExcludedDate(EditHospitalApplicationVM model)
        {
            try
            {

                int updatedRow = _hospitalApplicationService.UpdateExcludedDate(model);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddHospitalApplication")]
        public int AddAsync(CreateHospitalApplicationVM hospitalApplicationVM)
        {
            var savedId = _hospitalApplicationService.Add(hospitalApplicationVM);
            return savedId;
        }

        [HttpDelete]
        [Route("DeleteHospitalApplication/{id}")]
        public ActionResult<HospitalApplication> Delete(int id)
        {
            try
            {
                var HospitalApplicationObj = _hospitalApplicationService.GetById(id);
                int deletedRow = _hospitalApplicationService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("CreateHospitalApplicationAttachments")]
        public int CreateHospitalApplicationAttachments(HospitalApplicationAttachment attachObj)
        {
            return _hospitalApplicationService.CreateHospitalApplicationAttachments(attachObj);
        }

        [HttpGet]
        [Route("SendHospitalExcludeEmail/{hospitalApplicationId}")]
        public async Task<int> SendHospitalExcludeEmail(int hospitalApplicationId)
        {
            string strExcludes = "";
            string strHolds = "";
           // string phone = "";
            string exchold = "";
            List<string> execludeNames = new List<string>();
            List<string> holdNames = new List<string>();
            List<IndexHospitalExecludeReasonVM.GetData> lstExcludes = new List<IndexHospitalExecludeReasonVM.GetData>();
            List<IndexHospitalHoldReasonVM.GetData> lstHolds = new List<IndexHospitalHoldReasonVM.GetData>();
            //var userObj = await _userManager.FindByNameAsync("MemberUser");
            //var lstEmployees = _employeeService.GetAll().Where(a => a.Email == userObj.Email).ToList();
            //if (lstEmployees.Count > 0)
            //{
            //    phone = lstEmployees[0].Phone;
            //}
            //if (lstEmployees.Count == 0)
            //{
            //    phone = userObj.PhoneNumber;
            //}
            var transObj = _hospitalReasonTransactionService.GetById(hospitalApplicationId);
            var applicationObj = _hospitalApplicationService.GetById(int.Parse(transObj.HospitalApplicationId.ToString()));

            var assetObj = _assetDetailService.GetById(int.Parse(applicationObj.AssetId.ToString()));
            var masterObj = _masterAssetService.GetById(int.Parse(assetObj.MasterAssetId.ToString()));
            var lstReasons = _hospitalReasonTransactionService.GetAll().Where(a => a.HospitalApplicationId == applicationObj.Id).ToList();
            if (lstReasons.Count > 0)
            {
                if (applicationObj.AppTypeId == 1)
                {
                    exchold = "Exclude";
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
                    exchold = "Hold";
                    foreach (var item in lstReasons)
                    {
                        lstHolds.Add(_hospitalHoldReasonService.GetAll().Where(a => a.Id == item.ReasonId).FirstOrDefault());
                    }

                    foreach (var reason in lstHolds)
                    {
                        holdNames.Add(reason.NameAr);
                    }
                    strHolds = string.Join(",", holdNames);
                }
            }



            var memberUsers = await _userManager.GetUsersInRoleAsync("Member");
            if (memberUsers.ToList().Count > 0)
            {
                foreach (var usr in memberUsers)
                {
                    if (!string.IsNullOrEmpty(usr.PhoneNumber))
                    {
                        StringBuilder strBuild = new StringBuilder();
                        strBuild.Append($"Dear {usr.UserName}\r\n");
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




                        string from = "almostakbaltechnology.dev@gmail.com";
                        //string to = "pineapple_126@hotmail.com ";
                        string subject = "Exclude-Hold Asset";
                        string body = strBuild.ToString();
                        string appSpecificPassword = "fajtjigwpcnxyyuv";

                        //var mailMessage = new MailMessage(from, to, subject, body);
                        var mailMessage2 = new MailMessage(from, usr.Email, subject, body);
                      //  mailMessage.IsBodyHtml = true;
                        mailMessage2.IsBodyHtml = true;
                        using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                        {

                            smtpClient.EnableSsl = true;
                            smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                           // smtpClient.Send(mailMessage);
                            smtpClient.Send(mailMessage2);
                        }

                        var SMSobj = new SendSMS();
                        SMSobj.Language = 1;
                        SMSobj.Mobile = usr.PhoneNumber;
                        SMSobj.Environment = 1;
                        SMSobj.Message = $"This Asset {masterObj.NameAr} with barcode:{assetObj.Barcode} requested to be {exchold}";
                        var json = JsonConvert.SerializeObject(SMSobj);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        // var UrlSMS = "https://smsmisr.com/api/v2";
                        var UrlSMS = "https://smsmisr.com/api/SMS";

                        using var client = new HttpClient();
                        var response = await client.PostAsync(UrlSMS, data);
                        string resultS = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(resultS);

                    }
                }
            }
            return 1;
        }





        [HttpPost]
        [Route("UploadHospitalApplicationFiles")]
        public ActionResult UploadHospitalApplicationFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/HospitalApplications";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {

            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("GetAttachmentByHospitalApplicationId/{hospitalApplicationId}")]
        public IEnumerable<HospitalApplicationAttachment> GetAttachmentByHospitalApplicationId(int hospitalApplicationId)
        {
            return _hospitalApplicationService.GetAttachmentByHospitalApplicationId(hospitalApplicationId);
        }


        [HttpDelete]
        [Route("DeleteHospitalApplicationAttachment/{id}")]
        public int DeleteHospitalApplicationAttachment(int id)
        {
            return _hospitalApplicationService.DeleteHospitalApplicationAttachment(id);
        }

        [HttpPost]
        [Route("SortHospitalApp/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexHospitalApplicationVM.GetData> SortHospitalApp(int pagenumber, int pagesize, SortHospitalApplication sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _hospitalApplicationService.SortHospitalApp(sortObj).ToList();
            return _pagingService.GetAll(pageInfo, list);
        }
    }
}
