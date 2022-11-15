using Asset.API.Helpers;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.WorkOrderVM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private IWorkOrderService _workOrderService;

        private IPagingService _pagingService;
        IWebHostEnvironment _webHostingEnvironment;
        private IWorkOrderTrackingService _workOrderTackingService;

        string strInsitute, strInsituteAr, strLogo = "";
        private readonly ISettingService _settingService;


        public WorkOrderController(IWorkOrderService workOrderService, IWorkOrderTrackingService workOrderTackingService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment, ISettingService settingService)
        {
            _workOrderService = workOrderService;
            _workOrderTackingService = workOrderTackingService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
            _settingService = settingService;
        }
        // GET: api/<WorkOrderController>
        [HttpGet]
        public IEnumerable<IndexWorkOrderVM> Get()
        {
            return _workOrderService.GetAllWorkOrders();
        }


        [HttpGet]
        [Route("GetworkOrderByUserId/{requestId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrderByUserId(int requestId, string userId)
        {
            return _workOrderService.GetworkOrderByUserId(requestId, userId);
        }

        [HttpGet]
        [Route("CountWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public int CountWorkOrdersByHospitalId(int hospitalId, string userId)
        {
            return _workOrderService.CountWorkOrdersByHospitalId(hospitalId, userId);
        }


        [HttpGet]
        [Route("GenerateWorOrderNumber")]
        public GeneratedWorkOrderNumberVM GenerateWorOrderNumber()
        {
            return _workOrderService.GenerateWorOrderNumber();
        }

        [HttpGet]
        [Route("GetLastRequestAndWorkOrderByAssetId/{assetId}")]
        public IEnumerable<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetId(int assetId)
        {
            return _workOrderService.GetLastRequestAndWorkOrderByAssetId(assetId);
        }

        [HttpGet]
        [Route("GetLastRequestAndWorkOrderByAssetIdAndRequestId/{assetId}/{requestId}")]
        public IEnumerable<IndexWorkOrderVM> GetLastRequestAndWorkOrderByAssetIdAndRequestId(int assetId, int requestId)
        {
            return _workOrderService.GetLastRequestAndWorkOrderByAssetId(assetId, requestId);
        }

        [HttpGet]
        [Route("GetworkOrder/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetworkOrder(string userId)
        {
            return _workOrderService.GetworkOrder(userId);
        }

        [HttpGet("GetTotalWorkOrdersForAssetInHospital/{assetDetailId}")]
        public int GetTotalWorkOrdersForAssetInHospital(int assetDetailId)
        {
            return _workOrderService.GetTotalWorkOrdersForAssetInHospital(assetDetailId);

        }

        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(int? hospitalId, string userId, PagingParameter pageInfo)
        {
            var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders.ToList());
        }




        [HttpGet]
        [Route("GetWorkOrdersByHospitalId/{hospitalId}/{userId}")]
        public IEnumerable<IndexWorkOrderVM> GetAWorkOrdersByHospitalId(int? hospitalId, string userId)
        {
            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId);

        }

        [HttpPost]
        [Route("GetAllWorkOrdersByDate")]
        public IEnumerable<IndexWorkOrderVM> GetRequestsByDate(SearchWorkOrderByDateVM woDateObj)
        {
            return _workOrderService.GetWorkOrdersByDate(woDateObj).ToList();
        }

        [HttpGet]
        [Route("ExportWorkOrdersByStatusId/{hospitalId}/{userId}/{statusId}")]
        public List<IndexWorkOrderVM> ExportWorkOrdersByStatusId(int hospitalId, string userId, int statusId)
        {
            var workOrders = _workOrderService.ExportWorkOrdersByStatusId(hospitalId, userId, statusId).ToList();
            return workOrders;
        }

        [HttpPost]
        [Route("GetAllWorkOrdersByHospitalIdAndPaging/{hospitalId}/{userId}/{statusId}/{pageNumber}/{pageSize}")]
        public List<IndexWorkOrderVM> GetAllWorkOrdersByHospitalIdAndPaging(int hospitalId, string userId, int statusId, int pageNumber, int pageSize)
        {
            var workOrders = _workOrderService.GetAllWorkOrdersByHospitalIdAndPaging(hospitalId, userId, statusId, pageNumber, pageSize).ToList();
            return workOrders;
        }

        [HttpGet]
        [Route("GetWorkOrdersCountByStatusIdAndPaging/{hospitalId}/{userId}/{statusId}")]
        public int GetWorkOrdersCountByStatusIdAndPaging(int hospitalId, string userId, int statusId)
        {
            return _workOrderService.GetWorkOrdersCountByStatusIdAndPaging(hospitalId, userId, statusId);
        }





        [HttpPost]
        [Route("GetWorkOrdersByDate/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexWorkOrderVM> GetRequestsByDate(int pagenumber, int pagesize, SearchWorkOrderByDateVM woDateObj)
        {

            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var lstRequests = _workOrderService.GetWorkOrdersByDate(woDateObj).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstRequests);
        }

        [HttpPost]
        [Route("GetWorkOrdersByDateAndStatus/{pagenumber}/{pagesize}")]
        public IndexWorkOrderVM2 GetWorkOrdersByDateAndStatus(SearchWorkOrderByDateVM woDateObj, int pageNumber, int pageSize)
        {
            return _workOrderService.GetWorkOrdersByDateAndStatus(woDateObj, pageNumber, pageSize);
        }



        [HttpPost]
        [Route("CountGetWorkOrdersByDate")]
        public int CountGetRequestsByDate(SearchWorkOrderByDateVM woDateObj)
        {
            return _workOrderService.GetWorkOrdersByDate(woDateObj).ToList().Count;

        }


        [HttpGet]
        [Route("getcount/{hospitalId}/{userId}")]
        public int count(int hospitalId, string userId)
        {
            var count = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId).ToList().Count;
            return count;
        }
        [HttpPut]
        [Route("GetAllWorkOrdersByHospitalStatusId/{hospitalId}/{userId}/{statusId}")]
        public IEnumerable<IndexWorkOrderVM> GetAllWorkOrdersByHospitalId(PagingParameter pageInfo, int? hospitalId, string userId, int statusId)
        {


            //PagingParameter pageInfo = new PagingParameter();
            //pageInfo.PageNumber = pagenumber;
            //pageInfo.PageSize = pagesize;
            var lstWorkOrders = _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, lstWorkOrders);
        }


        [HttpGet]
        [Route("GetCountByStatus/{hospitalId}/{userId}/{statusId}")]
        public int GetCountByStatus(int? hospitalId, string userId, int statusId)
        {
            //  return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId).Count();

            return _workOrderService.GetAllWorkOrdersByHospitalId(hospitalId, userId, statusId).ToList().Count;
        }



        [HttpPost]
        [Route("SearchInWorkOrders/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexWorkOrderVM> SearchInWorkOrders(int pagenumber, int pagesize, SearchWorkOrderVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _workOrderService.SearchWorkOrders(searchObj).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInWorkOrdersCount")]
        public int SearchInWorkOrderssCount(SearchWorkOrderVM searchObj)
        {
            int count = _workOrderService.SearchWorkOrders(searchObj).ToList().Count();
            return count;
        }





        // GET api/<WorkOrderController>/5
        [HttpGet("{id}")]
        public ActionResult<IndexWorkOrderVM> Get(int id)
        {
            return _workOrderService.GetWorkOrderById(id);
        }


        [HttpGet("GetWorkOrderByRequestId/{requestId}")]
        public ActionResult<IndexWorkOrderVM> GetWorkOrderByRequestId(int requestId)
        {
            return _workOrderService.GetWorkOrderByRequestId(requestId);
        }



        [HttpGet("PrintWorkOrderById/{id}")]
        public ActionResult<PrintWorkOrderVM> PrintWorkOrderById(int id)
        {
            return _workOrderService.PrintWorkOrderById(id);
        }





        [HttpPost]
        [Route("CreateWorkOrderAttachments")]
        public int CreateWorkOrderAttachments(WorkOrderAttachment attachObj)
        {
            return _workOrderService.CreateWorkOrderAttachments(attachObj);
        }

        [HttpPost]
        [Route("UploadWorkOrderFiles")]
        public ActionResult UploadWorkOrerFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/WorkOrderFiles";
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




        // POST api/<WorkOrderController>
        [HttpPost]
        public int Post(CreateWorkOrderVM createWorkOrderVM)
        {
            return _workOrderService.AddWorkOrder(createWorkOrderVM);
        }

        // PUT api/<WorkOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, EditWorkOrderVM editWorkOrderVM)
        {
            _workOrderService.UpdateWorkOrder(id, editWorkOrderVM);
        }

        // DELETE api/<WorkOrderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var lstTracks = _workOrderTackingService.GetAllWorkOrderTrackingByWorkOrderId(id).ToList();
            if (lstTracks.Count > 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "workorder", Message = "Work Order has many tracks you cannot delete it", MessageAr = "لا يمكنك مسح هذا الطلب لوجود العديد من الأوامر" });
            }
            else
            {
                _workOrderService.DeleteWorkOrder(id);
            }

            return Ok();
        }
        [HttpPost]
        [Route("SortWorkOrders/{hosId}/{userId}/{pagenumber}/{pagesize}/{statusId}")]
        public IEnumerable<IndexWorkOrderVM> SortWorkOrders(int hosId, string userId, int pagenumber, int pagesize, SortWorkOrderVM sortObj, int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _workOrderService.SortWorkOrders(hosId, userId, sortObj, statusId).ToList();
            return _pagingService.GetAll<IndexWorkOrderVM>(pageInfo, list);
        }


        [HttpPost]
        [Route("CountSortWorkOrders/{hosId}/{userId}/{statusId}")]
        public int CountSortWorkOrders(int hosId, string userId, SortWorkOrderVM sortObj, int statusId)
        {
            return _workOrderService.SortWorkOrders(hosId, userId, sortObj, statusId).ToList().Count;
        }





        [HttpPost]
        [Route("CreateWOReportWithinDatePDF")]
        public void CreateWOReportWithinDatePDF(SearchWorkOrderByDateVM searchWorkOrderObj)
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
            PdfPTable bodytable = createWOReportWithinDateTable(searchWorkOrderObj);
            int countnewpages = bodytable.Rows.Count / 17;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/WOReports/WOReport.pdf", bytes);


            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                // Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchWorkOrderObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);
                    if (searchWorkOrderObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchWorkOrderObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchWorkOrderObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });

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
                    titleTable.AddCell(new PdfPCell(new Phrase("أوامر الشغل", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    if (searchWorkOrderObj.StrStartDate == "")
                        sDate = DateTime.Parse("01/01/1900");
                    else
                        sDate = DateTime.Parse(searchWorkOrderObj.StrStartDate);


                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;


                    if (searchWorkOrderObj.StrEndDate == "")
                        eDate = DateTime.Today.Date;
                    else
                        eDate = DateTime.Parse(searchWorkOrderObj.StrEndDate);


                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    //   titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    titleTable.AddCell(new PdfPCell(new Phrase(searchWorkOrderObj.StatusName, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    if (sDate == DateTime.Parse("01/01/1900"))
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من بداية أوامر الشغل إلى  " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    else
                        titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });




                    titleTable.WriteSelectedRows(0, -1, 5, 520, stamper.GetOverContent(i));
                }


                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(11);
                    bodytable2.SetTotalWidth(new float[] { 80f, 70f, 90f, 90f, 60f, 90f, 60f, 60f, 80f, 80f, 65f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 15, 12, 12, 12, 8, 10, 12, 10, 20, 12, 7 });

                    int countRows = bodytable.Rows.Count;
                    if (countRows > 17)
                    {
                        countRows = 17;
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
                    bodytable2.WriteSelectedRows(0, -1, 10, 445, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/WOReports/WOReport.pdf", bytes);


            memoryStream.Close();
            document.Close();
        }



        public PdfPTable createWOReportWithinDateTable(SearchWorkOrderByDateVM searchWorkOrderObj)
        {
            var lstData = _workOrderService.GetWorkOrdersByDate(searchWorkOrderObj).ToList();
            PdfPTable table = new PdfPTable(11);
            table.SetTotalWidth(new float[] { 80f, 70f, 90f, 90f, 60f, 90f, 60f, 60f, 80f, 80f, 65f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 15, 12, 12, 12, 8, 10, 12, 10, 20, 12, 7 });
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);
            string[] col = { "ملاحظات", "حالة أمر الشغل", "تاريخ إغلاق أمر الشغل", "تاريخ أمر الشغل", "أنشأ  بواسطة", "الموضوع", "السيريال", "الباركود", "اسم الأصل", "رقم أمر الشغل", "م" };




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
                //TimeSpan diff = DateTime.Now - DateTime.Parse(item.CreationDate.ToString());
                //var days = diff.Days;
                //var hours = diff.Hours;
                //int minutes = diff.Minutes;
                //int seconds = diff.Seconds;
                //if (searchWorkOrderObj.Lang == "en")
                //{
                //    var elapsedTime = days + " days " + hours + " hours " + minutes + " minutes " + seconds + " seconds";
                //    item.ElapsedTime = elapsedTime;
                //}
                //else
                //{
                //    var elapsedTime = days + " يوم " + hours + " ساعة " + minutes + " دقيقة " + seconds + " ثانية";
                //    item.ElapsedTime = elapsedTime;
                //}

                ++index;

                table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });


                if (item.WorkOrderNumber != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.WorkOrderNumber), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.AssetNameAr != null)
                {
                    if (searchWorkOrderObj.Lang == "ar")
                        table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(item.AssetName, font)) { PaddingBottom = 5 });
                }
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.BarCode != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.BarCode), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.SerialNumber != null)
                    table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(item.SerialNumber), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.Subject != null)
                    table.AddCell(new PdfPCell(new Phrase(item.Subject, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                if (item.CreatedBy != null)
                    table.AddCell(new PdfPCell(new Phrase(item.CreatedBy, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                if (item.TrackCreationDate.ToString() != "")
                    table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.TrackCreationDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                if (item.StatusId == 12)
                {
                    if (item.ClosedDate != null)
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ClosedDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                }
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



                if (item.StatusNameAr != null)
                    table.AddCell(new PdfPCell(new Phrase(item.StatusNameAr, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                if (item.Note != null)
                    table.AddCell(new PdfPCell(new Phrase(item.Note, font)) { PaddingBottom = 5 });
                else
                    table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });



            }

            return table;
        }


    }
}
