using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.ScrapVM;
using Microsoft.EntityFrameworkCore;
using Asset.ViewModels.PagingParameter;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Asset.API.Helpers;
using System.Globalization;
using System.Net.Http.Headers;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapController : Controller
    {
        IWebHostEnvironment _webHostingEnvironment;
        IScrapService _scrapService;
        ISettingService _settingService;
        private IPagingService _pagingService;

        string strInsitute, strInsituteAr, strLogo = "";

        public ScrapController(ISettingService settingService, IScrapService scrapService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _settingService = settingService;
            _scrapService = scrapService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;
        }

        [HttpGet]
        [Route("GetAllScraps")]
        public List<IndexScrapVM.GetData> GetAll()
        {
            return _scrapService.GetAll().ToList();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public Scrap GetById(int id)
        {
            return _scrapService.GetById(id);
        }

        [HttpGet]
        [Route("GetById2/{id}")]
        public IndexScrapVM.GetData GetById2(int id)
        {
            return _scrapService.GetById2(id);
        }



        [HttpGet]
        [Route("ViewScrapById/{id}")]
        public ViewScrapVM ViewScrapById(int id)
        {
            return _scrapService.ViewScrapById(id);
        }


        [HttpGet]
        [Route("GenerateScrapNumber")]
        public GeneratedScrapNumberVM GenerateScrapNumber()
        {
            return _scrapService.GenerateScrapNumber();
        }
        [HttpPut]
        [Route("ListScrapsWithPaging")]
        public IEnumerable<IndexScrapVM.GetData> ListScrapsWithPaging(PagingParameter pageInfo)
        {
            var scraps = _scrapService.GetAll().ToList();
            return _pagingService.GetAll<IndexScrapVM.GetData>(pageInfo, scraps);
        }


        [HttpPost]
        [Route("GetAllScrapsWithPaging/{pagenumber}/{pagesize}")]
        public IndexScrapVM GetAllScraps(int pageNumber, int pageSize)
        {
            return _scrapService.GetAllScraps(pageNumber, pageSize);
        }

        [HttpDelete]
        [Route("DeleteScrap/{id}")]
        public ActionResult<Scrap> Delete(int id)
        {
            try
            {

                int deletedRow = _scrapService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Scrap>();
        }

        [HttpPost]
        [Route("AddScrap")]
        public IActionResult AddScrap(CreateScrapVM scrapVM)
        {
            var lstScraps = _scrapService.GetAll().Where(a => a.AssetId == scrapVM.AssetDetailId).ToList();
            if (lstScraps.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "assetId", Message = "Asset is already scrapped", MessageAr = "هذا الجهاز مكهن" });
            }
            else
            {
                return Ok( _scrapService.Add(scrapVM));
            }
        }


        [HttpPost]
        [Route("SearchInScraps/{pagenumber}/{pagesize}")]
        public IndexScrapVM SearchInScraps(SearchScrapVM searchObj, int pageNumber, int pageSize)
        {
            return _scrapService.SearchInScraps(searchObj, pageNumber, pageSize);

        }

        [HttpPost]
        [Route("SortScraps/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexScrapVM.GetData> SortScraps(int pagenumber, int pagesize, SortScrapVM sortObj, int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _scrapService.SortScraps(sortObj, statusId).ToList();
            return _pagingService.GetAll<IndexScrapVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("CreateScrapAttachments")]
        public int CreateScrapAttachments(ScrapAttachment scrapAttachment)
        {
            return _scrapService.CreateScrapAttachments(scrapAttachment);
        }

        [HttpPost]
        [Route("UploadScrapFiles")]
        public ActionResult UploadScrapFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapFiles/";
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
        [Route("GetScrapAttachmentByScrapId/{scrapId}")]
        public IEnumerable<ScrapAttachment> GetScrapAttachmentByScrapId(int scrapId)
        {
            return _scrapService.GetScrapAttachmentByScrapId(scrapId);
        }
        [HttpGet]
        [Route("GetScrapReasonByScrapId/{scrapId}")]
        public IEnumerable<ViewScrapVM> GetScrapReasonByScrapId(int scrapId)
        {
            return _scrapService.GetScrapReasonByScrapId(scrapId);
        }






        [HttpPost]
        [Route("CreateScrapPDF")]
        public void CreateScrapPDF(SearchScrapVM searchObj)
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

            PdfPTable bodytable = CreateScrapTable(searchObj);
            int countnewpages = bodytable.Rows.Count / 13;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports/ScrapReportPDF.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + searchObj.PrintedBy, font), 200f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    // PdfPCell cell = new PdfPCell(new Phrase(" "));

                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 15;
                    headertable.AddCell(cell);
                    if (searchObj.Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + searchObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + searchObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


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
                    titleTable.AddCell(new PdfPCell(new Phrase("الأجهزة المكهنة", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    //     titleTable.AddCell(new PdfPCell(new Phrase("تحت التنفيذ", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



                    //DateTime sDate = new DateTime();
                    //DateTime eDate = new DateTime();
                    //if (searchObj.StrStartDate == "")
                    //    sDate = DateTime.Parse("01/01/1900");
                    //else
                    //    sDate = DateTime.Parse(searchObj.StrStartDate);

                    //var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    //var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    //var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    //var strStart = sday + "/" + smonth + "/" + syear;

                    //if (searchObj.StrEndDate == "")
                    //    eDate = DateTime.Today.Date;
                    //else
                    //    eDate = DateTime.Parse(searchObj.StrEndDate);


                    //var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    //var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    //var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    //var strEnd = eday + "/" + emonth + "/" + eyear;

                    //    if (sDate == DateTime.Parse("01/01/1900"))

                    // titleTable.AddCell(new PdfPCell(new Phrase("الأجهزة المكهنة", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    //else
                    //    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });


                    titleTable.WriteSelectedRows(0, -1, 5, 550, stamper.GetOverContent(i));
                }


                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(8);
                    bodytable2.SetTotalWidth(new float[] { 120f, 110f, 110f, 120f, 120f, 110f, 110f, 20f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 80, 80, 80, 80, 80, 80, 80, 20 });

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
                    bodytable2.WriteSelectedRows(0, -1, 10, 520, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports/ScrapReportPDF.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }
        public PdfPTable CreateScrapTable(SearchScrapVM searchObj)
        {
            var lstData = _scrapService.SearchInScraps(searchObj).ToList();
            PdfPTable table = new PdfPTable(8);
            table.SetTotalWidth(new float[] { 120f, 110f, 110f, 120f, 120f, 110f, 110f, 20f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 80, 80, 80, 80, 80, 80, 80, 20 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);

            if (searchObj.Lang == "ar")
            {
                string[] col = { "القسم", "الموديل", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم التكهين", "م" };
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
                string[] col = { "No.", "Scrap No", "Date", "Asset Name", "Barcode", "Serial", "Model", "Department" };
                for (int i = 0; i <= col.Length - 1; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
            }




            int index = 0;
            if (searchObj.Lang == "ar")
            {
                foreach (var item in lstData)
                {
                    ++index;

                    if (searchObj.Lang == "ar")
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });

                    if (item.ScrapNo != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapNo, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ScrapDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ScrapDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
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


                    if (item.Model != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });




                    if (item.DepartmentNameAr != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.DepartmentNameAr, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                }
            }

            else if (searchObj.Lang == "en")
            {
                foreach (var item in lstData)
                {
                    ++index;

                    if (searchObj.Lang == "ar")
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });

                    if (item.ScrapNo != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapNo, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ScrapDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapDate.ToString(), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetName, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.Model != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.DepartmentName != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.DepartmentName, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                }




            }

            return table;
        }








        [HttpPost]
        [Route("CreateScrapCheckedPDF")]
        public void CreateScrapCheckedPDF(List<IndexScrapVM.GetData> checkedItems)
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

            PdfPTable bodytable = CreateCheckedScrapItems(checkedItems);
            int countnewpages = bodytable.Rows.Count / 12;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports/CreateScrapCheckedReport.pdf", bytes);
            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + checkedItems[0].PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
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
                    if (checkedItems[0].Lang == "ar")
                        headertable.AddCell(new PdfPCell(new Phrase(strInsituteAr + "\n" + checkedItems[0].HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 15 });
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(strInsitute + "\n" + checkedItems[0].HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });


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
                    titleTable.AddCell(new PdfPCell(new Phrase("الأجهزة المكهنة", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



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
                    PdfPTable bodytable2 = new PdfPTable(8);
                    bodytable2.SetTotalWidth(new float[] { 120f, 110f, 110f, 120f, 120f, 110f, 110f, 20f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;
                    bodytable2.SetWidths(new int[] { 80, 80, 80, 80, 80, 80, 80, 20 });

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
                    bodytable2.WriteSelectedRows(0, -1, 10, 500, stamper.GetUnderContent(i));

                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports/CreateScrapCheckedReport.pdf", bytes);

            memoryStream.Close();
            document.Close();

        }
        public PdfPTable CreateCheckedScrapItems(List<IndexScrapVM.GetData> checkedItems)
        {
            var lstData = _scrapService.PrintListOfScraps(checkedItems).ToList();
            PdfPTable table = new PdfPTable(8);
            table.SetTotalWidth(new float[] { 120f, 110f, 110f, 120f, 120f, 110f, 110f, 20f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 80, 80, 80, 80, 80, 80, 80, 20 });


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 12);

            if (checkedItems[0].Lang == "ar")
            {
                string[] col = { "القسم", "الموديل", "الرقم المسلسل", "الباركود", "اسم الأصل", "التاريخ", "رقم التكهين", "م" };
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
                string[] col = { "No.", "Scrap No", "Date", "Asset Name", "Barcode", "Serial", "Model", "Department" };
                for (int i = 0; i <= col.Length - 1; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col[i], font));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                }
            }



            int index = 0;
            if (checkedItems[0].Lang == "ar")
            {
                foreach (var item in lstData)
                {
                    ++index;

                    if (checkedItems[0].Lang == "ar")
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });

                    if (item.ScrapNo != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapNo, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ScrapDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.ScrapDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
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


                    if (item.Model != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });




                    if (item.DepartmentNameAr != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.DepartmentNameAr, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                }
            }

            else if (checkedItems[0].Lang == "en")
            {
                foreach (var item in lstData)
                {
                    ++index;

                    if (checkedItems[0].Lang == "ar")
                        table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });

                    if (item.ScrapNo != null)
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapNo, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.ScrapDate.ToString() != "")
                        table.AddCell(new PdfPCell(new Phrase(item.ScrapDate.ToString(), font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.AssetNameAr != null)
                        table.AddCell(new PdfPCell(new Phrase(item.AssetName, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.Barcode != null)
                        table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.SerialNumber != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });


                    if (item.Model != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                    if (item.DepartmentName != null)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.DepartmentName, font)) { PaddingBottom = 5 });
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });

                }




            }


            return table;
        }































        [HttpGet]
        [Route("DownloadSRReportInProgressPDF/{fileName}")]
        public HttpResponseMessage DownloadSRReportWithInProgressPDF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
            {
                // return new HttpResponseMessage(HttpStatusCode.Gone);
                var folder = Directory.CreateDirectory(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/ScrapReports");
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
