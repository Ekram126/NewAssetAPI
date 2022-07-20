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

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IWorkOrderService _workOrderService;
        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;

        public RequestController(IRequestService requestService, IWorkOrderService workOrderService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _requestService = requestService;
            _workOrderService = workOrderService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
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




        [HttpGet]
        [Route("GenerateRequestNumber")]
        public GeneratedRequestNumberVM GenerateRequestNumber()
        {
            return _requestService.GenerateRequestNumber();
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




        [HttpPost]
        [Route("GetRequestsByStatusIdAndPaging/{userId}/{statusId}/{pageNumber}/{pageSize}")]
        public List<IndexRequestVM.GetData> GetRequestsByStatusIdAndPaging(string userId, int statusId, int pageNumber, int pageSize)
        {
            var Requests = _requestService.GetRequestsByStatusIdAndPaging(userId, statusId,pageNumber,pageSize).ToList();
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
            //  titletable.AddCell(createTextCell("ffffff"));
            // titletable.AddCell(createImageCell(imageURL));

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
            int countnewpages = bodytable.Rows.Count / 20;
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
                    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/MHP.png";
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
                        headertable.AddCell(new PdfPCell(new Phrase("  \t\t\t\t\t\tوزارة الصحة والسكان " + "\n" + searchRequestDateObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase("  Ministry of Health and Population" + "\n" + searchRequestDateObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });

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
                    if (countRows > 20)
                    {
                        countRows = 20;
                    }
                    bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count; k++)
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

            //var processing = new Process();
            //processing.StartInfo = new ProcessStartInfo(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRWOReport.pdf")
            //{
            //    UseShellExecute = true,
            //    Verb = "runas"
            //};
            //processing.Start();



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
            int countnewpages = bodytable.Rows.Count / 20;
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
                    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/MHP.png";
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
                        headertable.AddCell(new PdfPCell(new Phrase("وزارة الصحة والسكان" + "\n" + searchRequestDateObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase("Ministry of Health and Population" + "\n" + searchRequestDateObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });

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
                    PdfPTable bodytable2 = new PdfPTable(10);
                    bodytable2.SetTotalWidth(new float[] { 90f, 80f, 100f, 90f, 90f, 90f, 100f, 90f, 90f, 15f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 20, 20, 48, 20, 20, 20, 35, 25, 20, 7 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 20)
                    {
                        countRows = 20;
                    }
                    bodytable2.Rows.Add(bodytable.Rows[0]);
                    for (int j = 1; j <= countRows - 1; j++)
                    {
                        bodytable2.Rows.Add(bodytable.Rows[j]);
                    }
                    for (int k = 1; k <= bodytable2.Rows.Count; k++)
                    {
                        bodytable.DeleteRow(1);
                    }
                    bodytable2.WriteSelectedRows(0, -1, 10, 460, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRReport.pdf", bytes);


            memoryStream.Close();
            document.Close();
            //var processing = new Process();
            //processing.StartInfo = new ProcessStartInfo(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SRReports/SRReport.pdf")
            //{
            //    UseShellExecute = true,
            //    Verb = "runas"
            //};
            //processing.Start();
        }
        public PdfPTable createSRReportWithinDateTable(SearchRequestDateVM searchRequestDateObj)
        {
            var lstData = _requestService.GetRequestsByDate(searchRequestDateObj).ToList();

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
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
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
        [Route("DownloadCreateSRReportWithinDatePDF")]
        public HttpResponseMessage DownloadFile()
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
    }
}
