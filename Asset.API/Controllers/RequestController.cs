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

        [HttpPut]
        [Route("ExportAllRequests/{userId}/{statusId}")]
        public IEnumerable<IndexRequestVM.GetData> ExportAllRequests(string userId, int statusId)
        {
            return _requestService.GetAllRequestsByStatusId(userId, statusId).ToList();

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


        //[HttpPost]
        //[Route("CreatePDF")]
        //public void CreatePDF(SearchRequestDateVM searchRequestDateObj)
        //{
        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
        //    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        //    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

        //    document.Open();

        //    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/MHP.png";
        //    //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
        //    //jpg.ScaleToFit(120f, 100f);
        //    //jpg.SpacingBefore = 10f;
        //    //jpg.SpacingAfter = 1f;
        //    //jpg.Alignment = Element.ALIGN_RIGHT;
        //    //document.Add(jpg);


        //    //Full path to the Unicode Arial file
        //    // string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Images/arialuni.ttf";
        //    //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
        //    //BaseFont bf = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //    //  BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //    // Font f = new Font(bf, 12);

        //    Font x = FontFactory.GetFont(@"C:\Windows\Fonts\Tahoma.ttf");
        //    var p = new Paragraph("إكرام لمعي عزيز", x);
        //    PdfPTable titletable = new PdfPTable(1);
        //    PdfPCell cell = new PdfPCell();
        //    cell.AddElement(p);
        //    titletable.AddCell(cell);


        //      document.Add(titletable);







        //    //PdfPTable titletable = new PdfPTable(2);
        //    //titletable.WidthPercentage = 100;
        //    //titletable.SetWidths(new int[] { 1, 2 });
        //    //titletable.AddCell(createTextCell("وزارة الصحة والسكان"));
        //    //titletable.AddCell(createTextCell("ffffff"));
        //    //// titletable.AddCell(createImageCell(imageURL));
        //    //document.Add(titletable);





        //    //document.Add(new Paragraph(50, "\u00a0"));
        //    //document.Add(createFirstTable(searchRequestDateObj));


        //    document.Close();
        //    byte[] bytes = memoryStream.ToArray();
        //    System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/" + DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf", bytes);
        //    memoryStream.Close();

        //}


        //public PdfPTable createFirstTable(SearchRequestDateVM searchRequestDateObj)
        //{
        //    var lstData = _requestService.GetRequestEstimations(searchRequestDateObj).ToList();
        //    PdfPTable table = new PdfPTable(10);
        //    table.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    table.WidthPercentage = 100;
        //    string[] col = { "المدة", "إغلاق أمر الشغل", "إغلاق البلاغ", "المدة", "نهاية أمر الشغل", "بداية أمر الشغل", "المدة", "إنشاء أمر الشغل", "تاريخ البلاغ", "رقم الأمر" };
        //    //for (int i = 0; i < col.Length; ++i)
        //    //{

        //    //    PdfPCell cell = new PdfPCell(new Phrase(col[i], new Font(Font.NORMAL,12)));
        //    //    //cell.Headers.Add(cell);// = true;
        //    //    // cell.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);
        //    //    table.AddCell(cell);
        //    //}

        //    // table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;


        //    foreach (var item in lstData)
        //    {
        //        //HorizontalAlignment = Element.ALIGN_RIGHT
        //        table.AddCell(new PdfPCell(new Phrase(item.DurationTillCloseDate, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.ClosedWorkOrderDate, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.CloseRequestDate, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.DurationBetweenWorkOrders, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.LastStepInTrackWorkOrderInProgress, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.FirstStepInTrackWorkOrderInProgress, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.DurationBetweenStartRequestWorkOrder, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.InitialWorkOrderDate, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.StartRequestDate, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //        table.AddCell(new PdfPCell(new Phrase(item.RequestNumber, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
        //    }
        //    return table;
        //}


        public PdfPCell createImageCell(String path)
        {
            Image img = Image.GetInstance(path);
            //img.ScaleToFit(50, 20);
            img.SpacingBefore = 10f;
            img.SpacingAfter = 1f;
            img.Alignment = Element.ALIGN_RIGHT;
            PdfPCell cell = new PdfPCell(img, true);
            return cell;
        }

        public PdfPCell createTextCell(string text)
        {

            //  return  table.AddCell(new PdfPCell(new Phrase(text, FontFactory.GetFont(FontFactory.TIMES, 10))) { Border = Rectangle.NO_BORDER, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

            PdfPCell cell = new PdfPCell(new Phrase(text));
            //Paragraph p = new Paragraph(text);
            //p.Alignment = Element.ALIGN_MIDDLE;
            //cell.AddElement(p);
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 0;
            return cell;
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

    }
}
