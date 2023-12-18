using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.SupplierExecludeAssetVM;
using Asset.ViewModels.SupplierExecludeReasonVM;
using Asset.ViewModels.SupplierExecludeVM;
using Asset.ViewModels.SupplierHoldReasonVM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierExecludeAssetController : ControllerBase
    {
        IWebHostEnvironment _webHostingEnvironment;
        private IPagingService _pagingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private ISettingService _settingService;  
        private ISupplierService _supplierService;
        private ISupplierExecludeAssetService _supplierExecludeAssetService;
        private ISupplierExecludeService _supplierExecludeService;  
        private ISupplierExecludeReasonService _supplierExecludeReasonService;
        private ISupplierHoldReasonService _supplierHoldReasonService;

        private IHospitalSupplierStatusService _hospitalSupplierStatusService;




        private IAssetDetailService _assetDetailService;
        private IAssetStatusTransactionService _assetStatusTransactionService;
        private IAssetStatusService _assetStatusService;

        string strInsitute, strInsituteAr, strLogo = "";

        bool isAgency;

        public SupplierExecludeAssetController(ISupplierService supplierService, UserManager<ApplicationUser> userManager, IAssetDetailService assetDetailService, IAssetStatusService assetStatusService,
               ISupplierExecludeReasonService supplierExecludeReasonService, ISupplierHoldReasonService supplierHoldReasonService, IAssetStatusTransactionService assetStatusTransactionService, IHospitalSupplierStatusService hospitalSupplierStatusService,
            ISupplierExecludeAssetService supplierExecludeAssetService, ISettingService settingService, ISupplierExecludeService supplierExecludeService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _userManager = userManager;
            _supplierExecludeAssetService = supplierExecludeAssetService;
            _supplierExecludeService = supplierExecludeService;
            _webHostingEnvironment = webHostingEnvironment;
            _pagingService = pagingService;
            _settingService = settingService;
            _supplierService = supplierService;
            _supplierHoldReasonService = supplierHoldReasonService;
            _supplierExecludeReasonService = supplierExecludeReasonService;
            _assetDetailService = assetDetailService;
            _assetStatusTransactionService = assetStatusTransactionService;
            _hospitalSupplierStatusService = hospitalSupplierStatusService;
            _assetStatusService = assetStatusService;
        }


        [HttpGet]
        [Route("ListSupplierExcludeAssets")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAll()
        {
            return _supplierExecludeAssetService.GetAll();
        }

        [HttpPost]
        [Route("SearchSupplierExecludes/{pageNumber}/{pageSize}")]
        public IndexSupplierExecludeAssetVM SearchSupplierExecludes(SearchSupplierExecludeAssetVM searchObj, int pageNumber, int pageSize)
        {
            return _supplierExecludeAssetService.SearchSupplierExecludes(searchObj, pageNumber, pageSize);
        }

        [HttpPut]
        [Route("GetAllByStatusId/{statusId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllByStatusId(PagingParameter pageInfo, int statusId)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByStatusId(statusId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }

        [HttpGet]
        [Route("GetCountAfterFilterStatusId/{statusId}")]
        public int GetCountAfterFilterStatusId(int statusId)
        {
            return _supplierExecludeAssetService.GetAllByStatusId(statusId).ToList().Count;
        }


        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPaging")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAll().ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }


        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPagingAndTypeId/{appTypeId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> ListSupplierExcludeAssetsWithPagingAndTypeId(int appTypeId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByAppTypeId(appTypeId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _supplierExecludeAssetService.GetAll().ToList().Count;
        }


        [HttpGet]
        [Route("GetcountByAppTypeId/{appTypeId}")]
        public int GetcountByAppTypeId(int appTypeId)
        {
            return _supplierExecludeAssetService.GetAllByAppTypeId(appTypeId).ToList().Count;
        }




        [HttpPost]
        [Route("GetAllSupplierExecludes/{statusId}/{appTypeId}/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexSupplierExecludeAssetVM GetAllSupplierExecludes(SearchSupplierExecludeAssetVM searchObj, int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            var lstExcludes = _supplierExecludeAssetService.GetAllSupplierExecludes(searchObj, statusId, appTypeId, hospitalId, pageNumber, pageSize);
            return lstExcludes;
        }



        [HttpPost]
        [Route("GetAllSupplierHolds/{statusId}/{appTypeId}/{hospitalId}/{pageNumber}/{pageSize}")]
        public IndexSupplierExecludeAssetVM GetAllSupplierHolds(SearchSupplierExecludeAssetVM searchObj, int statusId, int appTypeId, int hospitalId, int pageNumber, int pageSize)
        {
            var lstHolds = _supplierExecludeAssetService.GetAllSupplierHoldes(searchObj, statusId, appTypeId, hospitalId, pageNumber, pageSize);
            return lstHolds;
        }


        [HttpPost]
        [Route("SearchSupplierExecludeAssetByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> GetSupplierExecludeAssetByDate(int pagenumber, int pagesize, SearchSupplierExecludeAssetVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstSupplierExcludes = _supplierExecludeAssetService.GetSupplierExecludeAssetByDate(searchObj).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExcludes);
        }


        [HttpPost]
        [Route("CountSearchSupplierExecludeAssetByDate")]
        public int CountGetHospitalApplicationByDate(SearchSupplierExecludeAssetVM searchObj)
        {
            return _supplierExecludeAssetService.GetSupplierExecludeAssetByDate(searchObj).ToList().Count;
        }


        [HttpPut]
        [Route("ListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId/{statusId}/{appTypeId}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> ListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId(int statusId, int appTypeId, PagingParameter pageInfo)
        {
            var lstSupplierExecludeAssets = _supplierExecludeAssetService.GetAllByStatusIdAndAppTypeId(statusId, appTypeId).ToList();
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, lstSupplierExecludeAssets);
        }
        [HttpGet]
        [Route("CountListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId/{statusId}/{appTypeId}")]
        public int CountListSupplierExcludeAssetsWithPagingWithStatusIdAndTypeId(int statusId, int appTypeId)
        {
            return _supplierExecludeAssetService.GetAllByStatusIdAndAppTypeId(statusId, appTypeId).ToList().Count;
        }

        [HttpPost]
        [Route("SortSuplierApp/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexSupplierExecludeAssetVM.GetData> SortSuplierApp(int pagenumber, int pagesize, SortSupplierExecludeAssetVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _supplierExecludeAssetService.SortSuplierApp(sortObj);
            return _pagingService.GetAll<IndexSupplierExecludeAssetVM.GetData>(pageInfo, list.ToList());
        }
        [HttpGet]
        [Route("GenerateSupplierExecludeAssetNumber")]
        public GenerateSupplierExecludeAssetNumberVM GenerateSupplierExecludeAssetNumber()
        {
            return _supplierExecludeAssetService.GenerateSupplierExecludeAssetNumber();
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierExecludeAssetVM> GetById(int id)
        {
            return _supplierExecludeAssetService.GetById(id);
        }
        [HttpGet]
        [Route("GetSupplierExecludeAssetDetailById/{id}")]
        public ActionResult<ViewSupplierExecludeAssetVM> GetSupplierExecludeAssetDetailById(int id)
        {
            return _supplierExecludeAssetService.GetSupplierExecludeAssetDetailById(id);
        }
        [HttpPut]
        [Route("UpdateSupplierExecludeAsset")]
        public IActionResult Update(EditSupplierExecludeAssetVM SupplierExecludeAssetVM)
        {
            try
            {
                int updatedRow = _supplierExecludeAssetService.Update(SupplierExecludeAssetVM);
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
        public async Task<IActionResult> UpdateExcludedDate(EditSupplierExecludeAssetVM model)
        {
            try
            {
                List<IndexSupplierExcludeReasonVM.GetData> lstExcludes = new List<IndexSupplierExcludeReasonVM.GetData>();
                List<IndexSupplierHoldReasonVM.GetData> lstHolds = new List<IndexSupplierHoldReasonVM.GetData>();
                int updatedRow = _supplierExecludeAssetService.UpdateExcludedDate(model);

                string strExcludes = "";
                string strHolds = "";
                string phone = "";
                string userName = "";
                string exchold = "";
                List<string> execludeNames = new List<string>();

                var excludeAssetObj = _supplierExecludeAssetService.GetById(model.Id);
                var userObj = await _userManager.FindByIdAsync(excludeAssetObj.UserId);
                var lstSuppliers = _supplierService.GetAllSuppliers().Where(a => a.EMail == userObj.Email).ToList();

                if (lstSuppliers.Count > 0)
                {
                    phone = lstSuppliers[0].Mobile;
                    userName = lstSuppliers[0].Name;
                }
                if (lstSuppliers.Count == 0)
                {
                    var usersupplierObj = await _userManager.FindByIdAsync(model.UserId);
                    phone = usersupplierObj.PhoneNumber;
                    userName = usersupplierObj.UserName;
                }

                var supplierExecludeAssetObj = _supplierExecludeAssetService.GetById(int.Parse(model.Id.ToString()));
                var assetObj = _assetDetailService.GetById(int.Parse(supplierExecludeAssetObj.AssetId.ToString()));

                var lstStatus = _assetStatusTransactionService.GetLastTransactionByAssetId(assetObj.Id);

                if (supplierExecludeAssetObj.AppTypeId == 1)
                {
                    exchold = "Exclude";
                    var lstReasons = _supplierExecludeService.GetAll().Where(a => a.SupplierExecludeAssetId == supplierExecludeAssetObj.Id).ToList();
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
                if (supplierExecludeAssetObj.AppTypeId == 2)
                {
                    exchold = "Hold";
                    var lstReasons = _supplierExecludeService.GetAll().Where(a => a.SupplierExecludeAssetId == supplierExecludeAssetObj.Id).ToList();
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
                strBuild.Append("<td>" + assetObj.AssetName);
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
                strBuild.Append("<td>" + assetObj.BarCode);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");
                if (supplierExecludeAssetObj.AppTypeId == 1)
                {
                    strBuild.Append("<tr>");
                    strBuild.Append("<td> Reasons");
                    strBuild.Append("</td>");
                    strBuild.Append("<td>" + strExcludes);
                    strBuild.Append("</td>");
                    strBuild.Append("</tr>");
                }
                if (supplierExecludeAssetObj.AppTypeId == 2)
                {
                    strBuild.Append("<tr>");
                    strBuild.Append("<td> Reasons");
                    strBuild.Append("</td>");
                    strBuild.Append("<td>" + strHolds);
                    strBuild.Append("</td>");
                    strBuild.Append("</tr>");
                }

                var assetStatusObj = _assetStatusService.GetById(int.Parse(lstStatus[0].AssetStatusId.ToString()));
                strBuild.Append("<tr>");
                strBuild.Append("<td> Member Exclude-Hold Result");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + assetStatusObj.Name);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");


                var hospitalSupplierStatusObj = _hospitalSupplierStatusService.GetById(int.Parse(supplierExecludeAssetObj.StatusId.ToString()));
                strBuild.Append("<tr>");
                strBuild.Append("<td> Member Exclude-Hold Result");
                strBuild.Append("</td>");
                strBuild.Append("<td>" + hospitalSupplierStatusObj.Name);
                strBuild.Append("</td>");
                strBuild.Append("</tr>");

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
                SMSobj.Mobile = phone;
                SMSobj.Environment = 1;
                SMSobj.Message = $"This Asset {assetObj.AssetName} with barcode:{assetObj.BarCode} is accepted to be {exchold}";
                var json = JsonConvert.SerializeObject(SMSobj);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var UrlSMS = "https://smsmisr.com/api/SMS";

                using var client = new HttpClient();
                var response = await client.PostAsync(UrlSMS, data);
                string resultS = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(resultS);
                return Ok();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
        }




        [HttpPost]
        [Route("AddSupplierExecludeAsset")]
        public int Add(CreateSupplierExecludeAssetVM supplierExecludeAssetObj)
        {
            var savedId = _supplierExecludeAssetService.Add(supplierExecludeAssetObj);
            return savedId;
        }
        [HttpDelete]
        [Route("DeleteSupplierExecludeAsset/{id}")]
        public ActionResult<SupplierExecludeAsset> Delete(int id)
        {
            try
            {
                var SupplierExecludeAssetObj = _supplierExecludeAssetService.GetById(id);

                int deletedRow = _supplierExecludeAssetService.Delete(id);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }
        [HttpPost]
        [Route("CreateSupplierExecludeAssetAttachments")]
        public int CreateSupplierExecludeAssetAttachments(SupplierExecludeAttachment attachObj)
        {
            return _supplierExecludeAssetService.CreateSupplierExecludAttachments(attachObj);
        }
        [HttpPost]
        [Route("UploadSupplierExecludeAssetFiles")]
        //  [Obsolete]
        public ActionResult UploadSupplierExecludeAssetFiles(IFormFile file)
        {

            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludeAssets";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            //  string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludeAssets/" + file.FileName;
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
        [Route("GetAttachmentBySupplierExecludeAssetId/{supplierExecludeAssetId}")]
        public IEnumerable<SupplierExecludeAttachment> GetAttachmentBySupplierExecludeAssetId(int supplierExecludeAssetId)
        {
            return _supplierExecludeAssetService.GetAttachmentBySupplierExecludeAssetId(supplierExecludeAssetId);
        }
        [HttpDelete]
        [Route("DeleteSupplierExecludeAssetAttachment/{id}")]
        public int DeleteSupplierExecludeAssetAttachment(int id)
        {
            return _supplierExecludeAssetService.DeleteSupplierExecludeAttachment(id);
        }
        [HttpGet]
        [Route("GetAttachmentBySupplierExcludeAssetId/{supplierExecludeAssetId}")]
        public IEnumerable<IndexSupplierExecludeVM.GetData> GetAttachmentBySupplierExcludeAssetId(int supplierExecludeAssetId)
        {
            return _supplierExecludeService.GetAttachmentBySupplierExecludeAssetId(supplierExecludeAssetId);
        }




        [HttpPost]
        [Route("CreateSupplierExecludePDF")]
        public void CreateSupplierExecludePDF(SearchSupplierExecludeAssetVM searchSupplierExecludeAssetObj)
        {
            var lstSettings = _settingService.GetAll().ToList();
            if (lstSettings.Count > 0)
            {
                foreach (var item in lstSettings)
                {
                    if (item.KeyName == "Institute")
                    {
                        strInsitute = item.KeyValue;
                        strInsituteAr = item.KeyValueAr;
                    }

                    if (item.KeyName == "Logo")
                        strLogo = item.KeyValue;


                    if (item.KeyName == "PMAgency")
                        isAgency = Convert.ToBoolean(item.KeyValue);
                }
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Document document = new Document(PageSize.A4.Rotate(), 20f, 20f, 30f, 20f);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.NewPage();
            document.Open();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string adobearabic = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfUniCode = BaseFont.CreateFont(adobearabic, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfUniCode, 14);

            Phrase ph = new Phrase(" ", font);
            document.Add(ph);

            PdfPTable bodytable = CreateServiceRequestTable(searchSupplierExecludeAssetObj);
            int countnewpages = bodytable.Rows.Count / 13;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludePDF/CreateSupplierExecludePDF.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchSupplierExecludeAssetObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
                }
                //Header
                for (int i = 1; i <= pages; i++)
                {
                    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/" + strLogo;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    jpg.ScaleAbsolute(70f, 50f);
                    PdfPTable headertable = new PdfPTable(2);
                    headertable.SetTotalWidth(new float[] { 350f, 50f });
                    headertable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    headertable.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new PdfPCell(jpg));
                    cell.PaddingTop = 3;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    headertable.AddCell(cell);
                    if (searchSupplierExecludeAssetObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchSupplierExecludeAssetObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchSupplierExecludeAssetObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


                    headertable.WriteSelectedRows(0, -1, 420, 580, stamper.GetOverContent(i));




                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    string adobearabicheaderTitle = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                    BaseFont bfUniCodeheaderTitle = BaseFont.CreateFont(adobearabicheaderTitle, BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCodeheaderTitle, 16);
                    titlefont.SetStyle("bold");


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.SetTotalWidth(new float[] { 800f });
                    titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    titleTable.WidthPercentage = 100;
                    titleTable.AddCell(new PdfPCell(new Phrase("استبعاد - إيقاف (مورد)", titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    titleTable.AddCell(new PdfPCell(new Phrase(searchSupplierExecludeAssetObj.StatusName, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    if (searchSupplierExecludeAssetObj.strStartDate == "")
                        sDate = DateTime.Parse("01/01/1900");
                    else
                        sDate = DateTime.Parse(searchSupplierExecludeAssetObj.strStartDate);

                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (searchSupplierExecludeAssetObj.strEndDate == "")
                        eDate = DateTime.Today.Date;
                    else
                        eDate = DateTime.Parse(searchSupplierExecludeAssetObj.strEndDate);


                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    if (sDate == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من البداية إلى  " + strEnd, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    titleTable.WriteSelectedRows(0, -1, 5, 530, stamper.GetOverContent(i));
                }


                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 120f, 100f, 100f, 100f, 80f, 100f, 100f, 50f, 50f, 20f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 120, 90, 90, 90, 80, 90, 90, 50, 50, 20 });



                    int countRows = bodytable.Rows.Count;
                    if (countRows > 13)
                    {
                        countRows = 13;
                    }

                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count - 1; k++)
                    {
                        bodytable.DeleteRow(1);
                    }


                    bodytable2.WriteSelectedRows(0, -1, 10, 453, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludePDF/CreateSupplierExecludePDF.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }
        public PdfPTable CreateServiceRequestTable(SearchSupplierExecludeAssetVM searchSupplierExecludeAssetObj)
        {
            var lstData = _supplierExecludeAssetService.PrintSearchSupplierExecludes(searchSupplierExecludeAssetObj).ToList();

            PdfPTable table = new PdfPTable(10);
            table.SetTotalWidth(new float[] { 120f, 100f, 100f, 100f, 80f, 100f, 100f, 50f, 50f, 20f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 120, 90, 90, 90, 80, 90, 90, 50, 50, 20 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchSupplierExecludeAssetObj.Lang == "ar")
            {
                string[] col = { "الأسباب", "الموديل", "الماركة", "الرقم المسلسل", "الباركود", "اسم الأصل", "تاريخ طلب الاستبعاد", "الحالة", "رقم الاستبعاد", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                foreach (var item in lstData)
                {
                    ++index;
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });

                    table.AddCell(new PdfPCell(new Phrase(item.ExNumber, font)) { PaddingBottom = 5 });



                    if (item.StatusNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                    if (item.DemandDate != null)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.DemandDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.BarCode != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.BarCode), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ModelNumber != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.BrandNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.AppTypeId == 1)
                    {
                        if (item.ReasonExTitlesAr != null)
                            table.AddCell(new PdfPCell(new Phrase(item.ReasonExTitlesAr, font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    }
                    else if (item.AppTypeId == 2)
                    {
                        if (item.ReasonHoldTitlesAr != null)
                            table.AddCell(new PdfPCell(new Phrase(item.ReasonHoldTitlesAr, font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }
                }
            }
            else
            {
                string[] col = { "No", "Date", "Status", "Ex Number", "Asset Name", "BarCode", "Serial", "Model", "Brand", "Reasons" };
                for (int i = 0; i <= col.Length - 1; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                foreach (var item in lstData)
                {
                    ++index;
                    table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });

                    table.AddCell(new PdfPCell(new Phrase(item.ExNumber, font)) { PaddingBottom = 5 });

                    if (item.Date != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Date, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusName != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusName)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetName != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetName)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.BarCode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.BarCode)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ModelNumber != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.BrandName != null)
                        table.AddCell(new PdfPCell(new Phrase(item.BrandName, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.AppTypeId == 1)
                    {
                        if (item.ReasonExTitles != null)
                            table.AddCell(new PdfPCell(new Phrase(item.ReasonExTitles, font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    }
                    else if (item.AppTypeId == 2)
                    {
                        if (item.ReasonHoldTitles != null)
                            table.AddCell(new PdfPCell(new Phrase(item.ReasonHoldTitles, font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }
                }
            }

            return table;
        }
        [HttpGet]
        [Route("DownloadCreateSupplierExecludePDF/{fileName}")]
        public HttpResponseMessage DownloadCreateServiceRequestPDFF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierExecludePDF/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
                return new HttpResponseMessage(HttpStatusCode.Gone);
            else
            {
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(fStream)
                };
                response.Content.Headers.ContentDisposition =
                                            new ContentDispositionHeaderValue("attachment")
                                            {
                                                FileName = Path.GetFileName(fStream.Name)
                                            };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            return response;
        }

    }
}
