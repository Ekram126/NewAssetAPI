using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.RequestVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Asset.ViewModels.RequestTrackingVM;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Asset.ViewModels.HospitalApplicationVM;
using Asset.ViewModels.SupplierExecludeAssetVM;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IHospitalApplicationService _hospitalApplicationService;
        private readonly ISupplierExecludeAssetService _supplierExecludeAssetService;
        private readonly IWorkOrderService _workOrderService;
        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;
        public int page_count_for_all = 0;


        string strInsitute, strInsituteAr, strLogo = "";
        bool isAgency, isScrap, isVisit, isExternalFix, isOpenRequest;
        private readonly ISettingService _settingService;

        public RequestController(IRequestService requestService, IWorkOrderService workOrderService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment, ISettingService settingService, IHospitalApplicationService hospitalApplicationService, ISupplierExecludeAssetService supplierExecludeAssetService)
        {
            _requestService = requestService;
            _workOrderService = workOrderService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
            _settingService = settingService;
            _hospitalApplicationService = hospitalApplicationService;
            _supplierExecludeAssetService = supplierExecludeAssetService;
        }

        // GET: api/<RequestController>
        [HttpGet]
        public IEnumerable<IndexRequestsVM> GetRequestDTO()
        {
            return _requestService.GetAllRequests();
        }
        [HttpGet]
        [Route("GetAllRequestsWithTrackingByUserId/{userId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId)
        {
            return _requestService.GetAllRequestsWithTrackingByUserId(userId);
        }
        [HttpGet("GetById/{id}")]
        public ActionResult<IndexRequestsVM> GetById(int id)
        {
            var requestDTO = _requestService.GetRequestById(id);
            return requestDTO;
        }

        [HttpGet("AlertOpenedRequestAssetsAndHighPeriority/{periorityId}/{hospitalId}")]
        public List<IndexRequestVM.GetData> AlertOpenedRequestAssetsAndHighPeriority(int periorityId, int hospitalId)
        {
            var lstRequestHighPeriority = _requestService.AlertOpenedRequestAssetsAndHighPeriority(periorityId, hospitalId);
            return lstRequestHighPeriority;
        }


        [HttpGet]
        [Route("GenerateRequestNumber")]
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            return _requestService.GenerateRequestNumber();
        }

        [HttpGet]
        [Route("ExportRequestsByStatusId/{hospitalId}/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> ExportRequestByStatusId(int hospitalId, string userId, int statusId)
        {
            return _requestService.ExportRequestByStatusId(hospitalId, userId, statusId);
        }



        [HttpGet]
        [Route("CountRequestsByHospitalId/{hospitalId}/{userId}")]
        public int CountRequestsByHospitalId(int hospitalId, string userId)
        {
            return _requestService.CountRequestsByHospitalId(hospitalId, userId);
        }

        [HttpGet("GetRequestByWorkOrderId/{workOrderId}")]
        public ActionResult<IndexRequestsVM> GetRequestByWorkOrderId(int workOrderId)
        {
            var requestObj = _requestService.GetRequestByWorkOrderId(workOrderId);
            return requestObj;
        }
        [HttpPost]
        [Route("GetAllRequestsByAssetId/{assetId}/{hospitalId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByAssetId(int assetId, int hospitalId, PagingParameter pageInfo)
        {
            var lstRequests = _requestService.GetAllRequestsByAssetId(assetId, hospitalId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }

        [HttpPost]
        [Route("GetAllRequestsByDate")]
        public IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(SearchRequestDateVM requestDateObj)
        {
            return _requestService.GetRequestsByDate(requestDateObj).ToList();
        }


        [HttpPost]
        [Route("GetRequestsByDateAndStatus/{pageNumber}/{pageSize}")]
        public IndexRequestVM GetRequestsByDateAndStatus(SearchRequestDateVM requestDateObj, int pageNumber, int pageSize)
        {
            return _requestService.GetRequestsByDateAndStatus(requestDateObj, pageNumber, pageSize);
        }

        [HttpPost]
        [Route("GetOpenRequestsByDate/{pageNumber}/{pageSize}")]
        public OpenRequestVM GetOpenRequestsByDate(SearchOpenRequestVM searchOpenRequestObj, int pageNumber, int pageSize)
        {
            return _requestService.ListOpenRequests(searchOpenRequestObj, pageNumber, pageSize);
        }

        [HttpPost]
        [Route("GetRequestsByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestVM.GetData> GetRequestsByDate(int pagenumber, int pagesize, SearchRequestDateVM requestDateObj)
        {

            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstRequests = _requestService.GetRequestsByDate(requestDateObj).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }
        [HttpPost]
        [Route("CountGetRequestsByDate")]
        public int CountGetRequestsByDate(SearchRequestDateVM requestDateObj)
        {
            return _requestService.GetRequestsByDate(requestDateObj).ToList().Count;

        }
        [HttpGet("CountAllRequestsByAssetId/{assetId}/{hospitalId}")]
        public int CountAllRequestsByAssetId(int assetId, int hospitalId)
        {
            return _requestService.GetAllRequestsByAssetId(assetId, hospitalId).ToList().Count;

        }
        [HttpGet("GetTotalRequestForAssetInHospital/{assetDetailId}")]
        public int GetTotalRequestForAssetInHospital(int assetDetailId)
        {
            return _requestService.GetTotalRequestForAssetInHospital(assetDetailId);

        }
        [HttpGet("PrintServiceRequestById/{id}")]
        public ActionResult<PrintServiceRequestVM> PrintWorkOrderById(int id)
        {
            return _requestService.PrintServiceRequestById(id);
        }
        [HttpGet("GetByRequestCode/{code}")]
        public IndexRequestsVM GetByRequestCode(string code)
        {
            return _requestService.GetByRequestCode(code);
        }
        [HttpGet("GetAllRequestsByHospitalAssetId/{assetId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByHospitalAssetId(int assetId)
        {
            return _requestService.GetAllRequestsByHospitalAssetId(assetId);
        }
        [HttpGet]
        [Route("GetTotalOpenRequest/{userId}")]
        public int GetTotalOpenReques(string userId)
        {
            return _requestService.GetTotalOpenRequest(userId);
        }


        [HttpGet]
        [Route("ListOpenRequests/{hospitalId}")]
        public List<Request> ListOpenRequests(int hospitalId)
        {
            return _requestService.ListOpenRequests(hospitalId);
        }



        [HttpGet]
        [Route("ListNewRequests/{hospitalId}")]
        public List<IndexRequestVM.GetData> ListNewRequests(int hospitalId)
        {
            return _requestService.ListNewRequests(hospitalId);
        }




        [HttpGet]
        [Route("UpdateOpenedRequest/{requestId}")]
        public int UpdateOpenedRequest(int requestId)
        {
            return _requestService.UpdateOpenedRequest(requestId);
        }



        [HttpGet]
        [Route("ListOpenRequestTracks/{hospitalId}")]
        public List<IndexRequestTracking> ListClosedRequestTracks(int hospitalId)
        {
            return _requestService.ListOpenRequestTracks(hospitalId);
        }


        [HttpGet]
        [Route("UpdateOpenedRequestTrack/{trackId}")]
        public int UpdateOpenedRequestTrack(int trackId)
        {
            return _requestService.UpdateOpenedRequestTrack(trackId);
        }



        [HttpPost]
        public int PostRequestDTO(CreateRequestVM createRequestVM)
        {
            return _requestService.AddRequest(createRequestVM);
        }

        // PUT api/<RequestController>/5
        [HttpPut]
        [Route("UpdateRequest")]
        public IActionResult PutRequestDTO(EditRequestVM editRequestVM)
        {
            _requestService.UpdateRequest(editRequestVM);
            return CreatedAtAction("GetRequestDTO", new { id = editRequestVM.Id }, editRequestVM);
        }

        // DELETE api/<RequestController>/5
        [HttpDelete]
        [Route("DeleteRequest/{id}")]
        public ActionResult DeleteRequestDTO(int id)
        {

            var lstWorkOrders = _workOrderService.GetAllWorkOrders().Where(a => a.RequestId == id).ToList();

            if (lstWorkOrders.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "req", Message = "You can't delete this request", MessageAr = "لا يمكنك مسح هذا الطلب" });
            }
            else
            {
                _requestService.DeleteRequest(id);
            }
            return Ok();
        }

        [HttpPut]
        [Route("GetAllRequestsWithTrackingByUserIdWithPaging/{userId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserId(string userId, PagingParameter pageInfo)
        {
            var Requests = _requestService.GetAllRequestsWithTrackingByUserId(userId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, Requests);
        }
        [HttpGet]
        [Route("getcount/{userId}")]
        public int count(string userId)
        {
            var count = _requestService.GetAllRequestsWithTrackingByUserId(userId).ToList().Count;
            return count;
        }



        [HttpPost]
        [Route("SearchInRequests2/{pagenumber}/{pagesize}")]
        public IndexRequestVM SearchInRequests2(int pagenumber, int pagesize, SearchRequestVM searchObj)
        {

            return _requestService.SearchInRequests(searchObj, pagenumber, pagesize);
        }














        [HttpPost]
        [Route("SearchInRequests/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestVM.GetData> SearchInRequests(int pagenumber, int pagesize, SearchRequestVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.SearchRequests(searchObj).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInRequestsCount")]
        public int SearchInRequestsCount(SearchRequestVM searchObj)
        {
            int count = _requestService.SearchRequests(searchObj).Count();
            return count;
        }

        [HttpPut]
        [Route("GetAllRequestsWithTrackingByUserIdWithPagingAndStatusId/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsByStatusId(string userId, int statusId, PagingParameter pageInfo)
        {
            var Requests = _requestService.GetAllRequestsByStatusId(userId, statusId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, Requests);
        }




        //[HttpPost]
        //[Route("GetRequestsByStatusIdAndPaging/{userId}/{statusId}/{pageNumber}/{pageSize}")]
        //public List<IndexRequestVM.GetData> GetRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize)
        //{
        //    var Requests = _requestService.GetRequestsByStatusIdAndPaging(userId, statusId, pageNumber, pageSize).ToList();
        //    return Requests;
        //}


        [HttpPost]
        [Route("GetRequestsByStatusIdAndPaging/{userId}/{statusId}/{pageNumber}/{pageSize}")]
        public IndexRequestVM GetRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize)
        {
            var Requests = _requestService.GetAllRequestsByStatusIdAndPaging(userId, statusId, pageNumber, pageSize);
            return Requests;
        }

        [HttpPut]
        [Route("ExportAllRequests/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> ExportAllRequests(string userId, int statusId)
        {
            return _requestService.ExportRequestsByStatusId(userId, statusId).ToList();

        }


        [HttpGet]
        [Route("GetRequestsCountByStatusIdAndPaging/{userId}/{statusId}")]
        public int GetRequestsCountByStatusIdAndPaging(string userId, int statusId)
        {
            return _requestService.GetRequestsCountByStatusIdAndPaging(userId, statusId);
        }


        [HttpGet]
        [Route("GetRequestsCountByStatusId/{userId}/{statusId}")]
        public int GetCountByStatusId(string userId, int statusId)
        {
            return _requestService.GetAllRequestsByStatusId(userId, statusId).ToList().Count;
        }


        [HttpPost]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetId/{userId}/{assetId}")]
        public IEnumerable<IndexRequestVM.GetData> GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestId(string userId, int assetId, PagingParameter pageInfo)
        {
            var lstRequests = _requestService.GetRequestsByUserIdAssetId(userId, assetId).ToList();
            return _pagingService.GetAll<IndexRequestVM.GetData>(pageInfo, lstRequests);
        }


        [HttpGet]
        [Route("GetRequestsByUserIdWithPagingAndStatusIdAndAssetIdCount")]
        public int GetAllRequestsWithTrackingByUserIdWithPagingAndStatusIdAndRequestIdCount(string userId, int assetId)
        {
            return _requestService.GetRequestsByUserIdAssetId(userId, assetId).ToList().Count;
        }

        [HttpPost]
        [Route("SortRequestsByPaging/{statusId}/{pageNumber}/{pageSize}")]
        public async Task<List<IndexRequestsVM>> SortRequestsByPaging(SortRequestVM sortObj, int statusId, int pageNumber, int pageSize)
        {
            var Requests = await _requestService.SortRequestsByPaging(sortObj, statusId, pageNumber, pageSize);
            return Requests;
        }



        [HttpPost]
        [Route("SortRequests/{pagenumber}/{pagesize}/{statusId}")]
        public async Task<IEnumerable<IndexRequestsVM>> SortRequests(int pagenumber, int pagesize, SortRequestVM sortObj, int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = await _requestService.SortRequests(sortObj, statusId);
            return _pagingService.GetAll<IndexRequestsVM>(pageInfo, list.ToList());
        }


        [HttpPost]
        [Route("SortRequestsByAssetId/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexRequestsVM> SortRequestsByAssetId(int pagenumber, int pagesize, SortRequestVM sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.SortRequestsByAssetId(sortObj);
            return _pagingService.GetAll<IndexRequestsVM>(pageInfo, list.ToList());
        }


        [HttpPost]
        [Route("CreateRequestAttachments")]
        public int CreateRequestAttachments(RequestDocument attachObj)
        {
            return _requestService.CreateRequestAttachments(attachObj);
        }

        [HttpPost]
        [Route("UploadRequestFiles")]
        public ActionResult UploadRequestFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/RequestDocuments";
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



        [HttpPost]
        [Route("GetLastRequestAttachment")]
        public int GetLastRequestAttachment(RequestDocument attachObj)
        {
            return _requestService.CreateRequestAttachments(attachObj);
        }

        //[HttpGet]
        //[Route("GetRequestEstimationById/{id}")]
        //public  List<ReportRequestVM> GetRequestEstimationById(int id)
        //{
        //    return _requestService.GetRequestEstimationById(id);
        //}


        [HttpPost]
        [Route("GetRequestEstimations/{pagenumber}/{pagesize}")]
        public IEnumerable<ReportRequestVM> GetRequestEstimations(int pagenumber, int pagesize, SearchRequestDateVM searchRequestDateObj)
        {
            //return _requestService.GetRequestEstimations(requestDateObj);
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _requestService.GetRequestEstimations(searchRequestDateObj).ToList();
            return _pagingService.GetAll<ReportRequestVM>(pageInfo, list);

        }

        [HttpPost]
        [Route("CountGetRequestEstimations")]
        public int CountGetRequestEstimations(SearchRequestDateVM searchRequestDateObj)
        {
            return _requestService.GetRequestEstimations(searchRequestDateObj).ToList().Count();
        }


        [HttpPost]
        [Route("GetAllRequestEstimations")]
        public IEnumerable<ReportRequestVM> GetAllRequestEstimations(SearchRequestDateVM searchRequestDateObj)
        {
            return _requestService.GetRequestEstimations(searchRequestDateObj).ToList();
        }






        [HttpGet]
        [Route("PrintReport")]
        public FileStreamResult GenerateReport(int id)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            document.NewPage();
            string fontLoc = _webHostingEnvironment.ContentRootPath + "/Images/ARIALUNI.TTF";

            //@"c:\windows\fonts\ARIALUNI.ttf"; // make sure to have the correct path to the font file
            BaseFont bf = BaseFont.CreateFont(fontLoc, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(bf, 12);

            //PdfPTable table = new PdfPTable(2);
            //table.DefaultCell.NoWrap = false;
            //table.WidthPercentage = 100;
            //table.AddCell(getCell("Testing", PdfPCell.ALIGN_LEFT, f));
            //table.AddCell(getCell("Text in the middle", PdfPCell.ALIGN_CENTER, f));
            //PdfPCell cell = new PdfPCell(new Phrase("مرحبا", f));
            //cell.NoWrap = false;
            //table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            //table.AddCell(cell);
            //document.Add(table);


            PdfPTable titletable = new PdfPTable(1);
            titletable.WidthPercentage = 100;
            titletable.SetWidths(new int[] { 1 });
            titletable.AddCell(new Phrase("وزارة الصحة والسكان", f));
            //titletable.AddCell(new Phrase(" ", f));


            titletable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            document.Add(titletable);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");

        }
        private PdfPCell getCell(string text, int alignment, Font f)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, f));
            cell.Padding = 0;
            cell.HorizontalAlignment = alignment;
            cell.Border = PdfPCell.NO_BORDER;
            return cell;
        }






        [HttpPost]
        [Route("CreatePDF")]
        public void CreatePDF(SearchRequestDateVM searchRequestDateObj)
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

                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);
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
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfUniCode, 12);

            Phrase ph = new Phrase(" ", font);
            document.Add(ph);

            PdfPTable bodytable = createFirstTable(searchRequestDateObj);
            int countnewpages = bodytable.Rows.Count / 12;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRWOReports/SRWOReport.pdf", bytes);


            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchRequestDateObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);



                    if (searchRequestDateObj.Lang == "ar")
                    {

                        headertable.AddCell(new PdfPCell(new Phrase("  \t\t\t\t\t\t " + strInsituteAr + "\n" + searchRequestDateObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    }
                    else
                        headertable.AddCell(new PdfPCell(new Phrase("  " + strInsitute + "\n" + searchRequestDateObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });

                    headertable.WriteSelectedRows(0, -1, 420, 580, stamper.GetOverContent(i));

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    string adobearabicheaderTitle = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                    BaseFont bfUniCodeheaderTitle = BaseFont.CreateFont(adobearabicheaderTitle, BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCodeheaderTitle, 13);
                    titlefont.SetStyle("bold");


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.SetTotalWidth(new float[] { 800f });
                    titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    titleTable.WidthPercentage = 100;
                    titleTable.AddCell(new PdfPCell(new Phrase("بلاغات الأعطال  /  أوامر الشغل", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    var sDate = DateTime.Parse(searchRequestDateObj.StrStartDate);
                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    var eDate = DateTime.Parse(searchRequestDateObj.StrEndDate);
                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    titleTable.WriteSelectedRows(0, -1, 5, 520, stamper.GetOverContent(i));
                }
                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(12);
                    bodytable2.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 65f, 65f, 65f, 65f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;

                    bodytable2.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 25, 25, 20, 20, 7 });
                    int countRows = bodytable.Rows.Count;
                    if (countRows > 12)
                    {
                        countRows = 12;
                    }
                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);
                    //  bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count - 1; k++)
                    {
                        bodytable.DeleteRow(1);
                    }
                    bodytable2.WriteSelectedRows(0, -1, 10, 460, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRWOReports/SRWOReport.pdf", bytes);
            memoryStream.Close();
            document.Close();

        }
        public PdfPTable createFirstTable(SearchRequestDateVM searchRequestDateObj)
        {
            var lstData = _requestService.GetRequestEstimations(searchRequestDateObj).ToList();
            PdfPTable table = new PdfPTable(12);

            table.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 65f, 65f, 65f, 65f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 25, 25, 20, 20, 7 });
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);


            string[] col = { " المدة بين إغلاق البلاغ وأمر الشغل", "ت. إغلاق أمر الشغل", "ت. إغلاق البلاغ", "المدة بين بداية ونهاية أمر الشغل", "ت. نهاية أمر الشغل", "ت. بداية أمر الشغل", " المدة من بداية البلاغ حتى تاريخ إنشاء أمر الشغل", "إنشاء أمر الشغل", "تاريخ بلاغ العطل", "رقم بلاغ العطل", "رقم امر الشغل", "م" };
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


                if (item.WorkOrderNumber != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.WorkOrderNumber), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.RequestNumber != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.RequestNumber), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.StartRequestDate != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.StartRequestDate).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.InitialWorkOrderDate != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.InitialWorkOrderDate).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.DurationBetweenStartRequestWorkOrder != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.DurationBetweenStartRequestWorkOrder), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.FirstStepInTrackWorkOrderInProgress != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.FirstStepInTrackWorkOrderInProgress).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.LastStepInTrackWorkOrderInProgress != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.LastStepInTrackWorkOrderInProgress).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.DurationBetweenWorkOrders != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.DurationBetweenWorkOrders), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.CloseRequestDate != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.CloseRequestDate).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.ClosedWorkOrderDate != null)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedWorkOrderDate).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.DurationTillCloseDate != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.DurationTillCloseDate), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            }

            return table;
        }







        [HttpPost]
        [Route("CreateSRReportWithinDatePDF")]
        public void CreateSRReportWithinDatePDF(SearchRequestDateVM searchRequestDateObj)
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

                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);
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

            PdfPTable bodytable = createSRReportWithinDateTable(searchRequestDateObj);
            int countnewpages = bodytable.Rows.Count / 12;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchRequestDateObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);
                    if (searchRequestDateObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchRequestDateObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchRequestDateObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


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
                    titleTable.AddCell(new PdfPCell(new Phrase("بلاغات الأعطال", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    titleTable.AddCell(new PdfPCell(new Phrase(searchRequestDateObj.StatusName, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    if (searchRequestDateObj.StrStartDate == "")
                        sDate = DateTime.Parse("01/01/1900");
                    else
                        sDate = DateTime.Parse(searchRequestDateObj.StrStartDate);

                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (searchRequestDateObj.StrEndDate == "")
                        eDate = DateTime.Today.Date;
                    else
                        eDate = DateTime.Parse(searchRequestDateObj.StrEndDate);


                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    if (sDate == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من بداية بلاغات الأعطال إلى  " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    titleTable.WriteSelectedRows(0, -1, 5, 520, stamper.GetOverContent(i));
                }








                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 90f, 80f, 100f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 12)
                    {
                        countRows = 12;
                    }
                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);
                    //  bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count - 1; k++)
                    {
                        bodytable.DeleteRow(1);
                    }
                    bodytable2.WriteSelectedRows(0, -1, 10, 445, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRReport.pdf", bytes);


            memoryStream.Close();
            document.Close();

        }
        public PdfPTable createSRReportWithinDateTable(SearchRequestDateVM searchRequestDateObj)
        {
            var lstData = _requestService.GetRequestsByDate(searchRequestDateObj).ToList();


            //  var lstData = _requestService.GetRequestsByDateAndStatus(searchRequestDateObj).ToList();
            PdfPTable table = new PdfPTable(10);
            table.SetTotalWidth(new float[] { 90f, 80f, 90f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);

            // string[] col = { "تاريخ الإغلاق", "حالة البلاغ", "الوصف", "السيريال", "الباركود", "اسم الأصل", "التاريخ", "رقم البلاغ", "الوقت", "م" };


            string[] col = { "تاريخ إغلاق بلاغ العطل", "حالة بلاغ العطل", "الوصف", "الوقت", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم بلاغ العطل", "م" };
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
                if (item.RequestCode != null)
                    table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.RequestDate.ToString() != "")
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.AssetNameAr != null)
                    table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.Barcode != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.SerialNumber != null)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    //table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
                }
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                TimeSpan diff = DateTime.Now - item.RequestDate;
                var days = diff.Days;
                var hours = diff.Hours;
                int minutes = diff.Minutes;
                int seconds = diff.Seconds;

                if (searchRequestDateObj.Lang == "en")
                {
                    var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                    item.ElapsedTime = elapsedTime;
                }
                else
                {
                    var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                    item.ElapsedTime = elapsedTime;
                }
                if (item.ElapsedTime != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.Description != null)
                    table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.StatusNameAr != null)
                    table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.StatusId == 2)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            }
            return table;
        }
        [HttpGet]
        [Route("DownloadCreateSRReportWithinDatePDF/{fileName}")]
        public HttpResponseMessage DownloadFile(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRReport.pdf";
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
                return new HttpResponseMessage(HttpStatusCode.Gone);
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
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


        [HttpPost]
        [Route("CreateSRReportWithinDateAndStatusPDF")]
        public void CreateSRReportWithinDateAndStatusPDF(SearchRequestDateVM searchRequestDateObj)
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

                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);
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

            PdfPTable bodytable = createSRReportWithinDateAndStatusDateTable(searchRequestDateObj);
            int countnewpages = bodytable.Rows.Count / 13;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRDateAndStatusReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchRequestDateObj.PrintedBy, font), 200f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);
                    if (searchRequestDateObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchRequestDateObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchRequestDateObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


                    headertable.WriteSelectedRows(0, -1, 420, 590, stamper.GetOverContent(i));

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    string adobearabicheaderTitle = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                    BaseFont bfUniCodeheaderTitle = BaseFont.CreateFont(adobearabicheaderTitle, BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCodeheaderTitle, 16);
                    titlefont.SetStyle("bold");


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.SetTotalWidth(new float[] { 800f });
                    titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    titleTable.WidthPercentage = 100;
                    titleTable.AddCell(new PdfPCell(new Phrase("بلاغات الأعطال", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    titleTable.AddCell(new PdfPCell(new Phrase(searchRequestDateObj.StatusName, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    if (searchRequestDateObj.StrStartDate == "")
                        sDate = DateTime.Parse("01/01/1900");
                    else
                        sDate = DateTime.Parse(searchRequestDateObj.StrStartDate);

                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (searchRequestDateObj.StrEndDate == "")
                        eDate = DateTime.Today.Date;
                    else
                        eDate = DateTime.Parse(searchRequestDateObj.StrEndDate);


                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    if (sDate == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من بداية بلاغات الأعطال إلى  " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    titleTable.WriteSelectedRows(0, -1, 5, 550, stamper.GetOverContent(i));
                }


                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 90f, 80f, 100f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 13)
                    {
                        countRows = 13;
                    }
                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);
                    //   bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count - 1; k++)
                    {
                        bodytable.DeleteRow(1);
                    }
                    bodytable2.WriteSelectedRows(0, -1, 10, 480, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRDateAndStatusReport.pdf", bytes);


            memoryStream.Close();
            document.Close();

        }
        public PdfPTable createSRReportWithinDateAndStatusDateTable(SearchRequestDateVM searchRequestDateObj)
        {
            var lstData = _requestService.GetRequestsByDateAndStatus(searchRequestDateObj).ToList();
            PdfPTable table = new PdfPTable(10);
            table.SetTotalWidth(new float[] { 90f, 80f, 90f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);

            // string[] col = { "تاريخ الإغلاق", "حالة البلاغ", "الوصف", "السيريال", "الباركود", "اسم الأصل", "التاريخ", "رقم البلاغ", "الوقت", "م" };

            if (searchRequestDateObj.Lang == "ar")
            {
                string[] col = { "تاريخ إغلاق بلاغ العطل", "حالة بلاغ العطل", "الوصف", "المدة", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم بلاغ العطل", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
            }

            else
            {
                string[] col = { "No.", "Request Code", "Date", "Asset Name", "Barcode", "Serial", "Total days", "Notes", "Status", "Closed Date" };
                for (int i = 0; i <= col.Length - 1; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
            }




            int index = 0;
            foreach (var item in lstData)
            {
                ++index;

                if (searchRequestDateObj.Lang == "ar")
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });



                if (item.RequestCode != null)
                    table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.RequestDate.ToString() != "")
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.AssetNameAr != null)
                    table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.Barcode != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.SerialNumber != null)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    //table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
                }
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.StatusId != 2)
                {
                    TimeSpan diff = DateTime.Now - item.RequestDate;
                    var days = diff.Days;
                    var hours = diff.Hours;
                    int minutes = diff.Minutes;
                    int seconds = diff.Seconds;


                    if (searchRequestDateObj.Lang == "en")
                    {
                        var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                        item.ElapsedTime = elapsedTime;
                    }
                    else
                    {
                        var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                        item.ElapsedTime = elapsedTime;
                    }

                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
                else
                {
                    TimeSpan diff2 = DateTime.Parse(item.DescriptionDate.ToString()) - item.RequestDate;
                    var days2 = diff2.Days;
                    var hours2 = diff2.Hours;
                    int minutes2 = diff2.Minutes;
                    int seconds2 = diff2.Seconds;


                    if (searchRequestDateObj.Lang == "en")
                    {
                        var elapsedTime = days2 + " days " + hours2 + " hours ";// + minutes + " minutes " + seconds + " seconds";
                        item.ElapsedTime = elapsedTime;
                    }
                    else
                    {
                        var elapsedTime = days2 + " يوم " + hours2 + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                        item.ElapsedTime = elapsedTime;
                    }

                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }

                if (item.Description != null)
                    table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.StatusNameAr != null)
                    table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.StatusId == 2)
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            }
            return table;
        }
        [HttpGet]
        [Route("DownloadCreateSRReportWithinDateAndStatusPDF/{fileName}")]
        public HttpResponseMessage DownloadSRReportWithinDateAndStatusPDF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
            {
                // return new HttpResponseMessage(HttpStatusCode.Gone);


                var folder = Directory.CreateDirectory(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/");
                var openFile = System.IO.File.Create(folder + fileName);
                openFile.Close();


                var file2 = folder + fileName;
                var fStream = new FileStream(file2, FileMode.Open, FileAccess.Read);
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
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
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


        [HttpPost]
        [Route("CreateServiceRequestPDF")]
        public void CreateServiceRequestPDF(SearchRequestVM searchRequestObj)
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

                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);
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

            PdfPTable bodytable = CreateServiceRequestTable(searchRequestObj);
            int countnewpages = bodytable.Rows.Count / 13;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/CreateSRReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchRequestObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 3;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);
                    if (searchRequestObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchRequestObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchRequestObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
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
                    titleTable.AddCell(new PdfPCell(new Phrase("بلاغات الأعطال", titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    titleTable.AddCell(new PdfPCell(new Phrase(searchRequestObj.StatusName, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    if (searchRequestObj.StrStartDate == "")
                        sDate = DateTime.Parse("01/01/1900");
                    else
                        sDate = DateTime.Parse(searchRequestObj.StrStartDate);

                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (searchRequestObj.StrEndDate == "")
                        eDate = DateTime.Today.Date;
                    else
                        eDate = DateTime.Parse(searchRequestObj.StrEndDate);


                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    if (sDate == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من بداية بلاغات الأعطال إلى  " + strEnd, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 3, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    titleTable.WriteSelectedRows(0, -1, 5, 530, stamper.GetOverContent(i));
                }


                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 90f, 80f, 100f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });



                    int countRows = bodytable.Rows.Count;
                    if (countRows > 13)
                    {
                        countRows = 13;
                    }

                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);


                    // bodytable2.Rows.Add(bodytable.Rows[0]);
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
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/CreateSRReport.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }
        public PdfPTable CreateServiceRequestTable(SearchRequestVM searchRequestObj)
        {
            var lstData = _requestService.SearchRequests(searchRequestObj).ToList();


            PdfPTable table = new PdfPTable(10);
            table.SetTotalWidth(new float[] { 90f, 80f, 90f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchRequestObj.Lang == "ar")
            {
                string[] col = { "تاريخ إغلاق بلاغ العطل", "حالة بلاغ العطل", "الوصف", "الوقت", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم بلاغ العطل", "م" };
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
                    if (item.RequestCode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.RequestDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        //table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    TimeSpan diff = DateTime.Now - item.RequestDate;
                    var days = diff.Days;
                    var hours = diff.Hours;
                    int minutes = diff.Minutes;
                    int seconds = diff.Seconds;

                    if (searchRequestObj.Lang == "en")
                    {
                        var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                        item.ElapsedTime = elapsedTime;
                    }
                    else
                    {
                        var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                        item.ElapsedTime = elapsedTime;
                    }
                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Description != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusId == 2)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
            }
            else
            {
                string[] col = { "No", "Request Number", "Date", "Asset Name", "BarCode", "Serial", "Time", "Notes", "Status", "Closed Date" };
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
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    if (item.RequestCode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.RequestDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    TimeSpan diff = DateTime.Now - item.RequestDate;
                    var days = diff.Days;
                    var hours = diff.Hours;
                    int minutes = diff.Minutes;
                    int seconds = diff.Seconds;

                    if (searchRequestObj.Lang == "en")
                    {
                        var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                        item.ElapsedTime = elapsedTime;
                    }
                    else
                    {
                        var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                        item.ElapsedTime = elapsedTime;
                    }
                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Description != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusId == 2)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
            }

            return table;
        }
        [HttpGet]
        [Route("DownloadCreateServiceRequestPDF/{fileName}")]
        public HttpResponseMessage DownloadCreateServiceRequestPDFF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/CreateSRReport.pdf";
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
                return new HttpResponseMessage(HttpStatusCode.Gone);
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
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


        [HttpPost]
        [Route("CreateServiceRequestGeneratePDF")]
        public void CreateServiceRequestGeneratePDF(List<ExportRequestVM> requests)
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

                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);



                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);
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

            PdfPTable bodytable = CreateServiceRequestGenerateTable(requests);
            int countnewpages = bodytable.Rows.Count / 12;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/CreateSRReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + requests[0].PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);
                    if (requests[0].Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + requests[0].HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + requests[0].HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


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
                    titleTable.AddCell(new PdfPCell(new Phrase("بلاغات الأعطال", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    //   titleTable.AddCell(new PdfPCell(new Phrase(requests[0].StatusName, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    //DateTime sDate = new DateTime();
                    //DateTime eDate = new DateTime();
                    //if (requests[0].StrStartDate == "")
                    //    sDate = DateTime.Parse("01/01/1900");
                    //else
                    //    sDate = DateTime.Parse(requests[0].StrStartDate);

                    //var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    //var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    //var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    //var strStart = sday + "/" + smonth + "/" + syear;

                    //if (requests[0].StrEndDate == "")
                    //    eDate = DateTime.Today.Date;
                    //else
                    //    eDate = DateTime.Parse(requests[0].StrEndDate);
                    //var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    //var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    //var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    //var strEnd = eday + "/" + emonth + "/" + eyear;

                    //if (sDate == DateTime.Parse("01/01/1900"))
                    //    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من بداية بلاغات الأعطال إلى  " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    //else
                    //    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    titleTable.WriteSelectedRows(0, -1, 5, 530, stamper.GetOverContent(i));
                }

                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 90f, 80f, 100f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 12)
                    {
                        countRows = 12;
                    }

                    bodytable2.Rows.Insert(0, bodytable.Rows[0]);

                    //  bodytable2.Rows.Add(bodytable.Rows[0]);
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
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/CreateSRCheckedReport.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }
        public PdfPTable CreateServiceRequestGenerateTable(List<ExportRequestVM> requests)
        {
            var lstData = _requestService.PrintListOfRequests(requests).ToList();


            PdfPTable table = new PdfPTable(10);
            table.SetTotalWidth(new float[] { 90f, 80f, 90f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (requests[0].Lang == "ar")
            {
                string[] col = { "تاريخ إغلاق بلاغ العطل", "حالة بلاغ العطل", "الوصف", "الوقت", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم بلاغ العطل", "م" };
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
                    if (item.RequestCode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.RequestDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        //table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    //TimeSpan diff = DateTime.Now - item.RequestDate;
                    //var days = diff.Days;
                    //var hours = diff.Hours;
                    //int minutes = diff.Minutes;
                    //int seconds = diff.Seconds;

                    //if (requests[0].Lang == "en")
                    //{
                    //    var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                    //    item.ElapsedTime = elapsedTime;
                    //}
                    //else
                    //{
                    //    var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                    //    item.ElapsedTime = elapsedTime;
                    //}

                    if (item.StatusId != 2)
                    {
                        TimeSpan diff = DateTime.Now - item.RequestDate;
                        var days = diff.Days;
                        var hours = diff.Hours;
                        int minutes = diff.Minutes;
                        int seconds = diff.Seconds;


                        if (requests[0].Lang == "en")
                        {
                            var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                            item.ElapsedTime = elapsedTime;
                        }
                        else
                        {
                            var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                            item.ElapsedTime = elapsedTime;
                        }

                        if (item.ElapsedTime != null)
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }
                    else
                    {
                        TimeSpan diff2 = DateTime.Parse(item.DescriptionDate.ToString()) - item.RequestDate;
                        var days2 = diff2.Days;
                        var hours2 = diff2.Hours;
                        int minutes2 = diff2.Minutes;
                        int seconds2 = diff2.Seconds;


                        if (requests[0].Lang == "en")
                        {
                            var elapsedTime = days2 + " days " + hours2 + " hours ";// + minutes + " minutes " + seconds + " seconds";
                            item.ElapsedTime = elapsedTime;
                        }
                        else
                        {
                            var elapsedTime = days2 + " يوم " + hours2 + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                            item.ElapsedTime = elapsedTime;
                        }

                        if (item.ElapsedTime != null)
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }






                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Description != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusId == 2)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
            }
            else
            {
                string[] col = { "No", "Request Number", "Date", "Asset Name", "BarCode", "Serial", "Time", "Notes", "Status", "Closed Date" };
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
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    if (item.RequestCode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.RequestCode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.RequestDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.Barcode), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    TimeSpan diff = DateTime.Now - item.RequestDate;
                    var days = diff.Days;
                    var hours = diff.Hours;
                    int minutes = diff.Minutes;
                    int seconds = diff.Seconds;

                    if (requests[0].Lang == "en")
                    {
                        var elapsedTime = days + " days " + hours + " hours ";// + minutes + " minutes " + seconds + " seconds";
                        item.ElapsedTime = elapsedTime;
                    }
                    else
                    {
                        var elapsedTime = days + " يوم " + hours + " ساعة ";// + minutes + " دقيقة " + seconds + " ثانية";
                        item.ElapsedTime = elapsedTime;
                    }
                    if (item.ElapsedTime != null)
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.ElapsedTime), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Description != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Description, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.StatusId == 2)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
            }

            return table;
        }
        [HttpGet]
        [Route("DownloadServiceRequestCheckBoxPDF/{fileName}")]
        public HttpResponseMessage DownloadServiceRequestCheckBoxPDF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
            {
                //  return new HttpResponseMessage(HttpStatusCode.Gone);



                var folder = Directory.CreateDirectory(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/");
                var openFile = System.IO.File.Create(folder + fileName);
                openFile.Close();


                var file2 = folder + fileName;
                var fStream = new FileStream(file2, FileMode.Open, FileAccess.Read);
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
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
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










































        [HttpPost]
        [Route("CreateOpenServiceRequestPDF")]
        public void CreateOpenServiceRequestPDF(SearchOpenRequestVM searchOpenRequestObj)
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



                    if (item.KeyName == "IsScrap")
                        isScrap = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsVisit")
                        isVisit = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsExternalFix")
                        isExternalFix = Convert.ToBoolean(item.KeyValue);


                    if (item.KeyName == "IsOpenRequest")
                        isOpenRequest = Convert.ToBoolean(item.KeyValue);

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






            DateTime sDate1 = new DateTime();
            DateTime eDate1 = new DateTime();
            if (searchOpenRequestObj.StrStartDate == "")
                sDate1 = DateTime.Parse("01/01/1900");
            else
                sDate1 = DateTime.Parse(searchOpenRequestObj.StrStartDate);

            var sday1 = ArabicNumeralHelper.toArabicNumber(sDate1.Day.ToString());
            var smonth1 = ArabicNumeralHelper.toArabicNumber(sDate1.Month.ToString());
            var syear1 = ArabicNumeralHelper.toArabicNumber(sDate1.Year.ToString());
            var strStart1 = sday1 + "/" + smonth1 + "/" + syear1;

            if (searchOpenRequestObj.StrEndDate == "")
                eDate1 = DateTime.Today.Date;
            else
                eDate1 = DateTime.Parse(searchOpenRequestObj.StrEndDate);
            var eday1 = ArabicNumeralHelper.toArabicNumber(eDate1.Day.ToString());
            var emonth1 = ArabicNumeralHelper.toArabicNumber(eDate1.Month.ToString());
            var eyear1 = ArabicNumeralHelper.toArabicNumber(eDate1.Year.ToString());
            var strEnd1 = eday1 + "/" + emonth1 + "/" + eyear1;



            PdfPTable bodytable = CreateOpenServiceRequestGenerateTable(searchOpenRequestObj);
            int countnewpages = bodytable.Rows.Count / 12;


            SearchHospitalApplicationVM searchExecludeObj = new SearchHospitalApplicationVM();
            searchExecludeObj.AppTypeId = 1;
            searchExecludeObj.StatusId = 2;
            searchExecludeObj.strStartDate = searchOpenRequestObj.StrStartDate;
            searchExecludeObj.strEndDate = searchOpenRequestObj.StrEndDate;
            searchExecludeObj.HospitalId = searchOpenRequestObj.HospitalId;
            searchExecludeObj.Lang = searchOpenRequestObj.Lang;
            searchExecludeObj.StartDate = sDate1;
            searchExecludeObj.EndDate = eDate1;
            PdfPTable execludebodytabledata = CreateHospitalExecludesGenerateTable(searchExecludeObj);
            int new_count_for_exlude_reporty = execludebodytabledata.Rows.Count / 12;



            SearchHospitalApplicationVM searchHospitalHoldObj = new SearchHospitalApplicationVM();
            searchHospitalHoldObj.AppTypeId = 2;
            searchHospitalHoldObj.StatusId = 2;
            searchHospitalHoldObj.strStartDate = searchOpenRequestObj.StrStartDate;
            searchHospitalHoldObj.strEndDate = searchOpenRequestObj.StrEndDate;
            searchHospitalHoldObj.HospitalId = searchOpenRequestObj.HospitalId;
            searchHospitalHoldObj.Lang = searchOpenRequestObj.Lang;
            searchHospitalHoldObj.StartDate = sDate1;
            searchHospitalHoldObj.EndDate = eDate1;


            PdfPTable execludebodytabledata2 = CreateHospitalHoldsGenerateTable(searchHospitalHoldObj);
            int new_count_for_exlude_reporty2 = execludebodytabledata2.Rows.Count / 12;


            SearchSupplierExecludeAssetVM searchSupplierExecludeObj = new SearchSupplierExecludeAssetVM();
            searchSupplierExecludeObj.AppTypeId = 1;
            searchSupplierExecludeObj.StatusId = 2;
            searchSupplierExecludeObj.strStartDate = searchOpenRequestObj.StrStartDate;
            searchSupplierExecludeObj.strEndDate = searchOpenRequestObj.StrEndDate;
            searchSupplierExecludeObj.HospitalId = searchOpenRequestObj.HospitalId;
            searchSupplierExecludeObj.Lang = searchOpenRequestObj.Lang;
            searchSupplierExecludeObj.StartDate = sDate1;
            searchSupplierExecludeObj.EndDate = eDate1;
            PdfPTable execludesupplierdata = CreateSupplierExecludesGenerateTable(searchSupplierExecludeObj);
            int new_count_for_exlude_supplier = execludesupplierdata.Rows.Count / 12;



            SearchSupplierExecludeAssetVM searchSupplierHoldObj = new SearchSupplierExecludeAssetVM();
            searchSupplierHoldObj.AppTypeId = 2;
            searchSupplierHoldObj.StatusId = 2;
            searchSupplierHoldObj.strStartDate = searchOpenRequestObj.StrStartDate;
            searchSupplierHoldObj.strEndDate = searchOpenRequestObj.StrEndDate;
            searchSupplierHoldObj.HospitalId = searchOpenRequestObj.HospitalId;
            searchSupplierHoldObj.Lang = searchOpenRequestObj.Lang;
            searchSupplierHoldObj.StartDate = sDate1;
            searchSupplierHoldObj.EndDate = eDate1;


            PdfPTable holdsupplierdata = CreateSupplierHoldsGenerateTable(searchSupplierHoldObj);
            int new_count_for_hold_supplier = holdsupplierdata.Rows.Count / 12;



            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }
            if (((float)bodytable.Rows.Count) % 12 != 0)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }


            if (countnewpages <= 0 && bodytable.Rows.Count != 0)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }
            else
            {
                if (((float)execludebodytabledata.Rows.Count % 12) != 0)
                {
                    for (int i = page_count_for_all; i <= (new_count_for_exlude_reporty + countnewpages); i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
                else
                {
                    for (int i = page_count_for_all; i <= new_count_for_exlude_reporty; i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }

            }


            if (new_count_for_exlude_reporty <= 0 && execludebodytabledata.Rows.Count != 0)
            {

                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;

            }
            else
            {
                if (((float)execludebodytabledata2.Rows.Count % 12) != 0)
                {
                    for (int i = page_count_for_all; i <= (new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1); i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
                else
                {
                    for (int i = page_count_for_all; i <= new_count_for_exlude_reporty2; i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }

            }






            if (new_count_for_exlude_reporty2 <= 0 && execludebodytabledata2.Rows.Count != 0)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }
            else
            {
                if (((float)execludesupplierdata.Rows.Count % 12) != 0)
                {
                    for (int i = page_count_for_all; i <= (new_count_for_exlude_supplier + new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1); i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
                else
                {
                    for (int i = page_count_for_all; i <= new_count_for_exlude_supplier; i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
            }





            if (new_count_for_exlude_supplier <= 0 && execludesupplierdata.Rows.Count != 0)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }
            else
            {
                if (((float)holdsupplierdata.Rows.Count % 12) != 0)
                {
                    for (int i = page_count_for_all; i <= (new_count_for_hold_supplier + new_count_for_exlude_supplier + new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1); i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
                else
                {
                    for (int i = page_count_for_all; i <= new_count_for_hold_supplier; i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
            }








            if (new_count_for_hold_supplier <= 0 && holdsupplierdata.Rows.Count != 0)
            {
                document.NewPage();
                writer.PageEmpty = false;
                page_count_for_all++;
            }
            else
            {
                if (((float)holdsupplierdata.Rows.Count % 12) != 0)
                {
                    for (int i = page_count_for_all; i <= (new_count_for_hold_supplier + new_count_for_exlude_supplier + new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1); i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
                else
                {
                    for (int i = page_count_for_all; i <= new_count_for_hold_supplier; i++)
                    {
                        document.NewPage();
                        writer.PageEmpty = false;
                        page_count_for_all++;
                    }
                }
            }



            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/AllOpenRequestReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchOpenRequestObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    headertable.AddCell(cell);
                    if (searchOpenRequestObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchOpenRequestObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchOpenRequestObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


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
                    titleTable.AddCell(new PdfPCell(new Phrase("نموذج مبدئي للشهادة", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    if (sDate1 == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من البداية إلى  " + strEnd1, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart1 + " إلى " + strEnd1, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    titleTable.WriteSelectedRows(0, -1, 5, 520, stamper.GetOverContent(i));
                }


                if (countnewpages == 0 && bodytable.Rows.Count != 0)
                    countnewpages++;
                if (((float)bodytable.Rows.Count / 12) != 0)
                    countnewpages++;
                for (int i = 1; i <= countnewpages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(11);
                    bodytable2.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_CENTER;
                    bodytable2.WidthPercentage = 200;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 30 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 12)
                    {
                        countRows = 12;
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
                    bodytable2.WriteSelectedRows(0, -1, 10, 460, stamper.GetUnderContent(i));
                }



                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                string adobearabicheaderTitle2 = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                BaseFont bfUniCodeheaderTitle2 = BaseFont.CreateFont(adobearabicheaderTitle2, BaseFont.IDENTITY_H, true);
                iTextSharp.text.Font titlefont2 = new iTextSharp.text.Font(bfUniCodeheaderTitle2, 16);
                titlefont2.SetStyle("bold");

                if (new_count_for_exlude_reporty == 0 && execludebodytabledata.Rows.Count != 0)
                    new_count_for_exlude_reporty++;
                if (((float)execludebodytabledata.Rows.Count % 12) != 0)
                {
                    new_count_for_exlude_reporty++;
                }
                int tt = countnewpages + new_count_for_exlude_reporty;
                for (int i = new_count_for_exlude_reporty + 1; i <= tt; i++)
                {

                    PdfPTable execludetitleTable = new PdfPTable(1);
                    execludetitleTable.SetTotalWidth(new float[] { 800f });
                    execludetitleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludetitleTable.WidthPercentage = 100;
                    execludetitleTable.AddCell(new PdfPCell(new Phrase("أجهزة مستبعدة من خلال المستشفى", titlefont2)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                    execludetitleTable.WriteSelectedRows(0, -1, 10, 480, stamper.GetUnderContent(i));


                    PdfPTable execludebodytable = new PdfPTable(11);
                    execludebodytable.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
                    execludebodytable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludebodytable.HorizontalAlignment = Element.ALIGN_CENTER;
                    execludebodytable.WidthPercentage = 200;
                    execludebodytable.PaddingTop = 200;
                    execludebodytable.HeaderRows = 1;
                    execludebodytable.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 25 });

                    int countRows = execludebodytabledata.Rows.Count;
                    if (countRows > 0)
                    {
                        if (countRows > 12)
                        {
                            countRows = 12;
                        }
                        execludebodytable.Rows.Insert(0, execludebodytabledata.Rows[0]);
                        for (int j = 1; j <= countRows - 1; j++)
                        {
                            execludebodytable.Rows.Add(execludebodytabledata.Rows[j]);
                        }
                        for (int k = 1; k <= execludebodytable.Rows.Count - 1; k++)
                        {
                            execludebodytabledata.DeleteRow(1);
                        }
                        execludebodytable.WriteSelectedRows(0, -1, 10, 450, stamper.GetUnderContent(i));

                    }
                }







                if (((float)execludebodytabledata2.Rows.Count % 12) != 0)
                {
                    new_count_for_exlude_reporty2++;
                }
                int ttt = tt + new_count_for_exlude_reporty2;
                for (int i = new_count_for_exlude_reporty + countnewpages + 1; i <= ttt; i++)
                {

                    PdfPTable execludetitleTable2 = new PdfPTable(1);
                    execludetitleTable2.SetTotalWidth(new float[] { 800f });
                    execludetitleTable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludetitleTable2.WidthPercentage = 100;
                    execludetitleTable2.AddCell(new PdfPCell(new Phrase("أجهزة متوقفة مؤقتاً من خلال المستشفى", titlefont2)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                    execludetitleTable2.WriteSelectedRows(0, -1, 0, 480, stamper.GetUnderContent(i));


                    PdfPTable execludebodytable2 = new PdfPTable(11);
                    execludebodytable2.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
                    execludebodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludebodytable2.HorizontalAlignment = Element.ALIGN_CENTER;
                    execludebodytable2.WidthPercentage = 200;
                    execludebodytable2.PaddingTop = 200;
                    execludebodytable2.HeaderRows = 1;
                    execludebodytable2.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 25 });

                    int countRows = execludebodytabledata2.Rows.Count;
                    if (countRows > 0)
                    {
                        if (countRows > 12)
                        {
                            countRows = 12;
                        }
                        execludebodytable2.Rows.Insert(0, execludebodytabledata2.Rows[0]);
                        for (int j = 1; j <= countRows - 1; j++)
                        {
                            execludebodytable2.Rows.Add(execludebodytabledata2.Rows[j]);
                        }
                        for (int k = 1; k <= execludebodytable2.Rows.Count - 1; k++)
                        {
                            execludebodytabledata2.DeleteRow(1);
                        }
                        execludebodytable2.WriteSelectedRows(0, -1, 10, 450, stamper.GetUnderContent(i));
                    }
                }







                ////if (new_count_for_exlude_supplier == 0 && execludesupplierdata.Rows.Count != 0)
                ////    new_count_for_exlude_supplier++;
                if (((float)execludesupplierdata.Rows.Count % 12) != 0)
                {
                    new_count_for_exlude_supplier++;
                }
                int ttt2 = ttt + new_count_for_exlude_supplier;
                int totalPages = (new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1);
                for (int i = totalPages; i <= ttt2; i++)
                {
                    PdfPTable execludetitleTable2 = new PdfPTable(1);
                    execludetitleTable2.SetTotalWidth(new float[] { 800f });
                    execludetitleTable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludetitleTable2.WidthPercentage = 100;
                    execludetitleTable2.AddCell(new PdfPCell(new Phrase("أجهزة مستبعدة من خلال وادي النيل", titlefont2)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                    execludetitleTable2.WriteSelectedRows(0, -1, 0, 480, stamper.GetUnderContent(i));

                    PdfPTable execludebodytable3 = new PdfPTable(11);
                    execludebodytable3.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
                    execludebodytable3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    execludebodytable3.HorizontalAlignment = Element.ALIGN_CENTER;
                    execludebodytable3.WidthPercentage = 200;
                    execludebodytable3.PaddingTop = 200;
                    execludebodytable3.HeaderRows = 1;
                    execludebodytable3.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 25 });
                    int countRows = execludesupplierdata.Rows.Count;
                    if (countRows > 0)
                    {
                        if (countRows > 12)
                        {
                            countRows = 12;
                        }
                        execludebodytable3.Rows.Insert(0, execludesupplierdata.Rows[0]);
                        for (int j = 1; j <= countRows - 1; j++)
                        {
                            execludebodytable3.Rows.Add(execludesupplierdata.Rows[j]);
                        }
                        for (int k = 1; k <= execludebodytable3.Rows.Count - 1; k++)
                        {
                            execludesupplierdata.DeleteRow(1);
                        }
                        execludebodytable3.WriteSelectedRows(0, -1, 10, 450, stamper.GetUnderContent(i));
                    }
                }










                if (((float)holdsupplierdata.Rows.Count % 12) != 0)
                {
                    new_count_for_hold_supplier++;
                }
                int newPages = ttt2 + new_count_for_hold_supplier;
                int totalPages2 = (new_count_for_exlude_supplier + new_count_for_exlude_reporty2 + new_count_for_exlude_reporty + countnewpages + 1);
                for (int i = totalPages2; i <= newPages; i++)
                {
                    PdfPTable holdtitleTable = new PdfPTable(1);
                    holdtitleTable.SetTotalWidth(new float[] { 800f });
                    holdtitleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    holdtitleTable.WidthPercentage = 100;
                    holdtitleTable.AddCell(new PdfPCell(new Phrase("أجهزة متوقفة مؤقتاً من خلال وادي النيل", titlefont2)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                    holdtitleTable.WriteSelectedRows(0, -1, 0, 480, stamper.GetUnderContent(i));

                    PdfPTable holdbodytable3 = new PdfPTable(11);
                    holdbodytable3.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
                    holdbodytable3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    holdbodytable3.HorizontalAlignment = Element.ALIGN_CENTER;
                    holdbodytable3.WidthPercentage = 200;
                    holdbodytable3.PaddingTop = 200;
                    holdbodytable3.HeaderRows = 1;
                    holdbodytable3.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 25 });

                    int countRows = holdsupplierdata.Rows.Count;
                    if (countRows > 0)
                    {
                        if (countRows > 12)
                        {
                            countRows = 12;
                        }
                        holdbodytable3.Rows.Insert(0, holdsupplierdata.Rows[0]);
                        for (int j = 1; j <= countRows - 1; j++)
                        {
                            holdbodytable3.Rows.Add(holdsupplierdata.Rows[j]);
                        }
                        for (int k = 1; k <= holdbodytable3.Rows.Count - 1; k++)
                        {
                            holdsupplierdata.DeleteRow(1);
                        }
                        holdbodytable3.WriteSelectedRows(0, -1, 10, 450, stamper.GetUnderContent(i));
                    }
                }



            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/AllOpenRequestReport.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }





        public PdfPTable CreateOpenServiceRequestGenerateTable(SearchOpenRequestVM searchOpenRequestObj)
        {
            var lstData = _requestService.ListOpenRequestsPDF(searchOpenRequestObj).ToList();
            PdfPTable table = new PdfPTable(11);
            table.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.WidthPercentage = 200;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 80 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchOpenRequestObj.Lang == "ar")
            {
                //  string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "تاريخ الإنتهاء من الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };

                string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };
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



                    table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });


                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.BrandNameAr != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ModelNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.Barcode != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.RequestDate.ToString() != "")
                    {
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.RequestDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.FixCost != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.FixCost.ToString()), font)) { PaddingBottom = 5 });

                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.CostPerDay != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.CostPerDay.ToString()), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.AllDays != null)
                    {

                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.AllDays.ToString()), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                    if (item.TotalCost != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.TotalCost.ToString()), font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                }
            }
            else
            {
                string[] col = { "No", "Asset Name", "Brand", "Model", "BarCode", "Serial", "Date", "Last fix date", "Fix cost", "Fix cost / Day", "Total Days", "TOtal Cost" };
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


                    if (item.BGColor == "#f7c2c2")
                    {
                        PdfPCell indexCell = new PdfPCell(new Phrase(index.ToString(), font));
                        indexCell.PaddingBottom = 5;
                        indexCell.BackgroundColor = new iTextSharp.text.BaseColor(247, 194, 194);
                        table.AddCell(indexCell);
                    }
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });
                    }

                    if (item.AssetName != null)
                    {
                        PdfPCell nameCell = new PdfPCell(new Phrase(new Phrase(item.AssetName, font)));
                        nameCell.PaddingBottom = 5;


                        table.AddCell(nameCell);
                    }
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }


                    if (item.BrandNameAr != null)
                    {
                        PdfPCell brandCell = new PdfPCell(new Phrase(new Phrase(item.BrandName, font)));
                        brandCell.PaddingBottom = 5;

                        table.AddCell(brandCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ModelNumber != null)
                    {

                        PdfPCell modelCell = new PdfPCell(new Phrase(new Phrase(item.ModelNumber, font)));
                        modelCell.PaddingBottom = 5;

                        table.AddCell(modelCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.Barcode != null)
                    {
                        PdfPCell barcodeCell = new PdfPCell(new Phrase(new Phrase(item.Barcode, font)));
                        barcodeCell.PaddingBottom = 5;

                        table.AddCell(barcodeCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        PdfPCell serialCell = new PdfPCell(new Phrase(new Phrase(item.SerialNumber, font)));
                        serialCell.PaddingBottom = 5;

                        table.AddCell(serialCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.RequestDate.ToString() != "")
                    {

                        PdfPCell requestDateCell = new PdfPCell(new Phrase(item.RequestDate.ToString(), font));
                        requestDateCell.PaddingBottom = 5;

                        table.AddCell(requestDateCell);

                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.FixCost != null)
                    {

                        PdfPCell fixCostCell = new PdfPCell(new Phrase(new Phrase(item.FixCost.ToString(), font)));
                        fixCostCell.PaddingBottom = 5;

                        table.AddCell(fixCostCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.CostPerDay != null)
                    {

                        PdfPCell costPerDayCell = new PdfPCell(new Phrase(new Phrase(item.CostPerDay.ToString(), font)));
                        costPerDayCell.PaddingBottom = 5;

                        table.AddCell(costPerDayCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.AllDays != null)
                    {

                        PdfPCell allDaysCell = new PdfPCell(new Phrase(new Phrase(item.AllDays.ToString(), font)));
                        allDaysCell.PaddingBottom = 5;

                        table.AddCell(allDaysCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                    if (item.TotalCost != null)
                    {
                        PdfPCell totalCostCell = new PdfPCell(new Phrase(new Phrase(item.TotalCost.ToString(), font)));
                        totalCostCell.PaddingBottom = 5;

                        table.AddCell(totalCostCell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
            }

            return table;
        }

        public PdfPTable CreateHospitalExecludesGenerateTable(SearchHospitalApplicationVM searchOpenRequestObj)
        {
            PdfPTable table = new PdfPTable(11);

            table.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.WidthPercentage = 200;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 80 });

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchOpenRequestObj.Lang == "ar")
            {

                string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                var lstData = _hospitalApplicationService.GetAllHospitalExecludes(searchOpenRequestObj).Results.ToList();
                if (lstData.Count > 0)
                {
                    foreach (var item in lstData)
                    {
                        ++index;


                        PdfPCell indexCell = new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font));
                        indexCell.PaddingBottom = 5;
                        table.AddCell(indexCell);


                        if (item.AssetNameAr != null)
                        {
                            PdfPCell nameCell = new PdfPCell(new Phrase(item.AssetNameAr, font));
                            nameCell.PaddingBottom = 5;
                            table.AddCell(nameCell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }

                        if (item.BrandNameAr != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.ModelNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.BarCode != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BarCode, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.SerialNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.Date.ToString() != "")
                        {
                            table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.Date.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.FixCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.FixCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.CostPerDay != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.CostPerDay.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.AllDays != null)
                        {

                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.AllDays.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                        if (item.TotalCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.TotalCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    }



                    return table;
                }


            }
            return table;

        }

        public PdfPTable CreateHospitalHoldsGenerateTable(SearchHospitalApplicationVM searchOpenRequestObj)
        {
            PdfPTable table = new PdfPTable(11);

            table.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.WidthPercentage = 200;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 80 });

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchOpenRequestObj.Lang == "ar")
            {

                string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                var lstData = _hospitalApplicationService.GetAllHospitalHolds(searchOpenRequestObj).Results.ToList();
                if (lstData.Count > 0)
                {
                    foreach (var item in lstData)
                    {
                        ++index;


                        PdfPCell indexCell = new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font));
                        indexCell.PaddingBottom = 5;
                        table.AddCell(indexCell);


                        if (item.AssetNameAr != null)
                        {
                            PdfPCell nameCell = new PdfPCell(new Phrase(item.AssetNameAr, font));
                            nameCell.PaddingBottom = 5;
                            table.AddCell(nameCell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }

                        if (item.BrandNameAr != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.ModelNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.BarCode != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BarCode, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.SerialNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.Date.ToString() != "")
                        {
                            table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.Date.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.FixCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.FixCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.CostPerDay != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.CostPerDay.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.AllDays != null)
                        {

                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.AllDays.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                        if (item.TotalCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.TotalCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }
                    return table;
                }
            }
            return table;

        }

        public PdfPTable CreateSupplierExecludesGenerateTable(SearchSupplierExecludeAssetVM searchObj)
        {
            PdfPTable table = new PdfPTable(11);

            table.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.WidthPercentage = 200;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 80 });

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchObj.Lang == "ar")
            {

                string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                var lstData = _supplierExecludeAssetService.GetAllSupplierExecludes(searchObj).Results.ToList();
                if (lstData.Count > 0)
                {
                    foreach (var item in lstData)
                    {
                        ++index;


                        PdfPCell indexCell = new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font));
                        indexCell.PaddingBottom = 5;
                        table.AddCell(indexCell);


                        if (item.AssetNameAr != null)
                        {
                            PdfPCell nameCell = new PdfPCell(new Phrase(item.AssetNameAr, font));
                            nameCell.PaddingBottom = 5;
                            table.AddCell(nameCell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }

                        if (item.BrandNameAr != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.ModelNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.BarCode != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BarCode, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.SerialNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.Date.ToString() != "")
                        {
                            table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.Date.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.FixCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.FixCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.CostPerDay != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.CostPerDay.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.AllDays != null)
                        {

                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.AllDays.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                        if (item.TotalCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.TotalCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    }
                    return table;
                }
            }
            return table;

        }

        public PdfPTable CreateSupplierHoldsGenerateTable(SearchSupplierExecludeAssetVM searchObj)
        {
            PdfPTable table = new PdfPTable(11);

            table.SetTotalWidth(new float[] { 10f, 10f, 40f, 50f, 50f, 60f, 70f, 120f, 120f, 200f, 90f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.WidthPercentage = 200;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 200, 200, 200, 200, 200, 200, 200, 200, 200, 300, 80 });

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            if (searchObj.Lang == "ar")
            {

                string[] col = { "الإجمالي", "عدد الأيام", "قيمة الإصلاح / اليوم", "قيمة الإصلاح", "التاريخ", "السيريال", "الباركود", "الموديل", "الماركة", "اسم الأصل", "م" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
                int index = 0;
                var lstData = _supplierExecludeAssetService.GetAllSupplierHoldes(searchObj).Results.ToList();
                if (lstData.Count > 0)
                {
                    foreach (var item in lstData)
                    {
                        ++index;

                        PdfPCell indexCell = new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font));
                        indexCell.PaddingBottom = 5;
                        table.AddCell(indexCell);

                        if (item.AssetNameAr != null)
                        {
                            PdfPCell nameCell = new PdfPCell(new Phrase(item.AssetNameAr, font));
                            nameCell.PaddingBottom = 5;
                            table.AddCell(nameCell);
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }

                        if (item.BrandNameAr != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.ModelNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.ModelNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.BarCode != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BarCode, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.SerialNumber != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.Date.ToString() != "")
                        {
                            table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.Date.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.FixCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.FixCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                        if (item.CostPerDay != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.CostPerDay.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                        if (item.AllDays != null)
                        {

                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.AllDays.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                        if (item.TotalCost != null)
                        {
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.TotalCost.ToString()), font)) { PaddingBottom = 5 });
                        }
                        else
                            table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                    }
                    return table;
                }
            }
            return table;

        }











        [HttpGet]
        [Route("DownloadOpenServiceRequestPDF/{fileName}")]
        public HttpResponseMessage DownloadOpenServiceRequestPDF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
            {
                // return new HttpResponseMessage(HttpStatusCode.Gone);


                var folder = Directory.CreateDirectory(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/");
                var openFile = System.IO.File.Create(folder + fileName);
                openFile.Close();


                var file2 = folder + fileName;
                var fStream = new FileStream(file2, FileMode.Open, FileAccess.Read);
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
            else
            {
                //if file present than read file 
                var fStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //compose response and include file as content in it
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
