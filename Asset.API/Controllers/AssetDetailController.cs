﻿using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using Asset.ViewModels.BrandVM;
using Asset.ViewModels.CityVM;
using Asset.ViewModels.GovernorateVM;
using Asset.ViewModels.HospitalVM;
using Asset.ViewModels.OrganizationVM;
using Asset.ViewModels.PagingParameter;
using Asset.ViewModels.PMAssetTaskScheduleVM;
using Asset.ViewModels.PmAssetTimeVM;
using Asset.ViewModels.SupplierVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Asset.API.Helpers;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Rectangle = iTextSharp.text.Rectangle;
using System.Drawing.Imaging;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using iTextSharp.tool.xml;
using iTextSharp.text.html;
using Font = iTextSharp.text.Font;


namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetDetailController : ControllerBase
    {

        private const int PageSize = 10;
        private IAssetDetailService _AssetDetailService;
        private IWorkOrderService _workOrderService;
        private IAssetOwnerService _assetOwnerService;
        private IAssetStatusTransactionService _assetStatusTransactionService;
        private IAssetMovementService _assetMovementService;
        private IRequestService _requestService;
        private IPMAssetTimeService _pMAssetTimeService;
        private IPagingService _pagingService;
        private QrController _qrController;
        IWebHostEnvironment _webHostingEnvironment;
        string strInsitute, strInsituteAr, strLogo = "";
        bool isAgency, isScrap, isVisit, isExternalFix, isOpenRequest, canAdd;
        private readonly ISettingService _settingService;
        IHttpContextAccessor _httpContextAccessor;
        int i = 1;


        [Obsolete]
        public AssetDetailController(IAssetDetailService AssetDetailService, IAssetOwnerService assetOwnerService, IWorkOrderService workOrderService,
            IPMAssetTimeService pMAssetTimeService, IPagingService pagingService, IAssetMovementService assetMovementService, IAssetStatusTransactionService assetStatusTransactionService,
            QrController qrController, IRequestService requestService, IWebHostEnvironment webHostingEnvironment, ISettingService settingService, IHttpContextAccessor httpContextAccessor)
        {
            _AssetDetailService = AssetDetailService;
            _assetMovementService = assetMovementService;
            _requestService = requestService;
            _webHostingEnvironment = webHostingEnvironment;
            _assetOwnerService = assetOwnerService;
            _pMAssetTimeService = pMAssetTimeService;
            _pagingService = pagingService;
            _qrController = qrController;
            _settingService = settingService;
            _workOrderService = workOrderService;
            _assetStatusTransactionService = assetStatusTransactionService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("ListAssetDetails")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            return _AssetDetailService.GetAll();
        }

        [HttpPost]
        [Route("GetHospitalAssets/{hospitalId}/{statusId}/{userId}/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            return _AssetDetailService.GetHospitalAssets(hospitalId, statusId, userId, page, pageSize, sortObj);
        }
        [HttpPost]
        [Route("CountHospitalAssets/{hospitalId}/{statusId}/{userId}/{pagenumber}/{pagesize}")]
        public int CountHospitalAssets(int hospitalId, int statusId, string userId, int page, int pageSize, Sort sortObj)
        {
            return _AssetDetailService.GetHospitalAssets(hospitalId, statusId, userId, page, pageSize, sortObj).Count();
        }

        [HttpPut]
        [Route("ListAssetDetailsWithPaging")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAllWithPaging(PagingParameter pageInfo)
        {
            var HospitalAssets = _AssetDetailService.GetAll().ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, HospitalAssets);
        }
        [HttpGet]
        [Route("ListAssetDetailCarouselByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> ListAssetDetailCarouselByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetDetailsByUserId(userId);
        }
        [HttpGet]
        [Route("AutoCompleteAssetBarCode/{barcode}/{hospitalId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {
            return _AssetDetailService.AutoCompleteAssetBarCode(barcode, hospitalId);
        }

        [HttpGet]
        [Route("AutoCompleteAssetSerial/{serial}/{hospitalId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            return _AssetDetailService.AutoCompleteAssetSerial(serial, hospitalId);
        }


        [HttpPost]
        [Route("LoadAssetDetailsByUserId/{pagenumber}/{pagesize}/{userId}")]
        public async Task<IndexAssetDetailVM> LoadAssetDetailsByUserId(int pageNumber, int pageSize, string userId)
        {
            var lstAssetDetails = await _AssetDetailService.LoadAssetDetailsByUserId(pageNumber, pageSize, userId);
            return lstAssetDetails;
        }

        [HttpGet]
        [Route("GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId/{barcode}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(string barcode, int hospitalId)
        {
            return _AssetDetailService.GetAutoCompleteSupplierNoneExcludedAssetsByHospitalId(barcode, hospitalId);
        }



        [HttpGet]
        [Route("GetAutoCompleteSupplierExcludedAssetsByHospitalId/{barcode}/{hospitalId}")]
        public ActionResult<IEnumerable<ViewAssetDetailVM>> GetAutoCompleteSupplierExcludedAssetsByHospitalId(string barcode, int hospitalId)
        {
            //var lstExcludes = _AssetDetailService.GetAutoCompleteSupplierExcludedAssetsByHospitalId(barcode, hospitalId).ToList();
            //if (lstExcludes.Count > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "assetId", Message = "Asset already exist", MessageAr = "هذا الجهاز مسجل سابقاً" });
            //}
            //else
            //{
            return _AssetDetailService.GetAutoCompleteSupplierExcludedAssetsByHospitalId(barcode, hospitalId).ToList();
            //  }
        }









        [HttpGet]
        [Route("GetAssetHistoryById/{assetId}")]
        public ViewAssetDetailVM GetAssetHistoryById(int assetId)
        {
            return _AssetDetailService.GetAssetHistoryById(assetId);
        }



        [HttpGet]
        [Route("getcount/{userId}")]
        public int count(string userId)
        {
            return _AssetDetailService.GetAssetDetailsByUserId(userId).Result.ToList().Count;// _AssetDetailService.GetAll().ToList().Count();
        }
        [HttpGet]
        [Route("GetAllSerialsByMasterAssetIdAndHospitalId/{masterAssetId}/{hospitalId}")]
        public IEnumerable<AssetDetail> GetAllSerialsByMasterAssetIdAndHospitalId(int masterAssetId, int hospitalId)
        {
            return _AssetDetailService.GetAllSerialsByMasterAssetIdAndHospitalId(masterAssetId, hospitalId);
        }
        [HttpGet]
        [Route("GetAllAssetDetailsByHospitalId/{hospitalId}")]
        public IEnumerable<AssetDetail> GetAllAssetDetailsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetAllAssetDetailsByHospitalId(hospitalId);
        }
        [HttpPost]
        [Route("GetAllAssetsByStatusId/{statusId}/{userId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAllRequestsByStatusId(int statusId, string userId, PagingParameter pageInfo)
        {
            var lstAssets = _AssetDetailService.GetAllAssetsByStatusId(statusId, userId).ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, lstAssets);
        }


        [HttpPost]
        [Route("ExportAssetsByStatusId/{statusId}/{userId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> ExportAssetsByStatusId(int statusId, string userId)
        {
            var lstAssets = _AssetDetailService.GetAllAssetsByStatusId(statusId, userId).ToList();
            return lstAssets;
        }



        [HttpPost]
        [Route("GetAllAssetsCountByStatusId/{statusId}/{userId}")]
        public int GetCountByStatusId(int statusId, string userId)
        {
            return _AssetDetailService.GetAllAssetsByStatusId(statusId, userId).ToList().Count;
        }



        [HttpPost]
        [Route("GetAllAssetsByStatusId2/{pagenumber}/{pagesize}/{statusId}/{userId}")]
        public IndexAssetDetailVM GetAllRequestsByStatusId(int pageNumber, int pageSize, int statusId, string userId)
        {
            var lstAssets = _AssetDetailService.GetAllAssetsByStatusId(pageNumber, pageSize, statusId, userId);
            return lstAssets;
        }

        [HttpPost]
        [Route("SearchHospitalAssetsByDepartmentId/{departmentId}/{userId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SearchHospitalAssetsByDepartmentId(int departmentId, string userId, int pageNumber, int pageSize)
        {
            var lstAssets = _AssetDetailService.SearchHospitalAssetsByDepartmentId(departmentId, userId, pageNumber, pageSize);
            return lstAssets;
        }

        [HttpPost]
        [Route("ListHospitalAssets/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM ListHospitalAssets(SortAndFilterVM data, int pageNumber, int pageSize)
        {
            return _AssetDetailService.ListHospitalAssets(data, pageNumber, pageSize);
        }


        [HttpPost]
        [Route("SearchAssetDetails/{pagenumber}/{pagesize}")]
        public IndexAssetDetailVM SearchInMasterAssets(int pagenumber, int pagesize, SearchMasterAssetVM searchObj)
        {
            var list = _AssetDetailService.SearchAssetInHospital(pagenumber, pagesize, searchObj);
            return list;// _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list);
        }
        //[HttpPost]
        //[Route("FilterDataByDepartmentBrandSupplierId")]
        //public List<IndexAssetDetailVM.GetData> FilterDataByDepartmentBrandSupplierId(FilterHospitalAsset data)
        //{
        //    var list = _AssetDetailService.FilterDataByDepartmentBrandSupplierId(data);
        //    return list;
        //}


        [HttpGet]
        [Route("GetAssetDetailsByAssetId/{assetId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByAssetId(int assetId)
        {
            return _AssetDetailService.GetAssetDetailsByAssetId(assetId);
        }
        [HttpGet]
        [Route("GetDateByAssetDetailId/{assetDetailId}")]
        public IEnumerable<PMAssetTime> GetDateByAssetDetailId(int assetDetailId)
        {
            return _pMAssetTimeService.GetDateByAssetDetailId(assetDetailId);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public EditAssetDetailVM GetById(int id)
        {
            return _AssetDetailService.GetById(id);
        }

        [HttpGet]
        [Route("GenerateAssetDetailBarcode")]
        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            return _AssetDetailService.GenerateAssetDetailBarcode();
        }




        [HttpGet]
        [Route("ViewAssetDetailByMasterId/{masterId}")]
        public ActionResult<ViewAssetDetailVM> ViewAssetDetailByMasterId(int masterId)
        {
            return _AssetDetailService.ViewAssetDetailByMasterId(masterId);
        }





        [HttpGet]
        [Route("DownloadAssetHistory/{fileName}")]
        public HttpResponseMessage DownloadSRReportWithInProgressPDF(string fileName)
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/" + fileName;
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
            {
                // return new HttpResponseMessage(HttpStatusCode.Gone);
                var folder = Directory.CreateDirectory(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/");
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




        [HttpGet]
        [Route("PrintAssetHistory/{assetId}/{lang}")]
        public void PrintAssetHistory(int assetId, string lang)
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
            Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 20f, 20f, 30f, 20f);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.NewPage();
            document.Open();

            string adobearabic = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfUniCode = BaseFont.CreateFont(adobearabic, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfUniCode, 14);
            iTextSharp.text.Font headerfont = new iTextSharp.text.Font(bfUniCode, 22);
            iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCode, 16);



            var assetDetailObj = _AssetDetailService.QueryAssetDetailById(assetId);

            string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/" + strLogo;
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScaleAbsolute(70f, 50f);
            PdfPTable headertable = new PdfPTable(3);
       
            if (lang == "ar")
            {

                headertable.SetTotalWidth(new float[] { 40f, 150f ,40f});
                headertable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                headertable.WidthPercentage = 100;
                PdfPCell headerCell = new PdfPCell(new PdfPCell(jpg));
                headerCell.PaddingTop = 5;
                headerCell.Border = Rectangle.NO_BORDER;
                headerCell.PaddingRight = 30;
                headerCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                headertable.AddCell(headerCell);

            }
            else
            {
                headertable.SetTotalWidth(new float[] {  50f,150f, 50f });
                headertable.WidthPercentage = 100;
                PdfPCell headerCell = new PdfPCell(new PdfPCell(jpg));
                headerCell.PaddingTop = 5;
                headerCell.Border = Rectangle.NO_BORDER;
                headerCell.PaddingLeft = 60;
                headerCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                headertable.AddCell(headerCell);
            }
            if (lang == "ar")
                headertable.AddCell(new PdfPCell(new Phrase("\t\t\t\t " + strInsituteAr + "\n" + assetDetailObj.Hospital.NameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 5 });
            else
                headertable.AddCell(new PdfPCell(new Phrase(" " + strInsitute + "\n" + assetDetailObj.Hospital.Name + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });

            if(!string.IsNullOrEmpty(assetDetailObj.MasterAsset.AssetImg))
            {
                string assetImagePath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage/" + assetDetailObj.MasterAsset.AssetImg;
                iTextSharp.text.Image assetImage = iTextSharp.text.Image.GetInstance(assetImagePath);
                assetImage.ScaleAbsolute(50f, 50f);
                headertable.AddCell(new PdfPCell(new PdfPCell(assetImage) { Border = Rectangle.NO_BORDER }));
            }
            else
            {
                string assetNullImagePath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage/UnknownAsset.png";
                iTextSharp.text.Image assetImage = iTextSharp.text.Image.GetInstance(assetNullImagePath);
                assetImage.ScaleAbsolute(50f, 50f);
                headertable.AddCell(new PdfPCell(new PdfPCell(assetImage){ Border = Rectangle.NO_BORDER }));


            }



           
            document.Add(headertable);



            PdfPTable titleTable = new PdfPTable(1);
            titleTable.SetTotalWidth(new float[] { 800f });
            titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            titleTable.WidthPercentage = 100;
            if (lang == "ar")
                titleTable.AddCell(new PdfPCell(new Phrase("تاريخ الأصل", headerfont)) { PaddingBottom = 10, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

            else
                titleTable.AddCell(new PdfPCell(new Phrase("Asset History")) { PaddingBottom = 10, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });



            document.Add(titleTable);
            if (lang == "ar")
            {

                PdfPTable table = new PdfPTable(4);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell cell = new PdfPCell(new Phrase("البيانات الرئيسة", titlefont));
                cell.BackgroundColor = new BaseColor(153, 204, 255);
                cell.PaddingBottom = 10;
                cell.Border = 0;
                cell.Colspan = 4;
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell(new Phrase("المحافظة", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.Governorate.NameAr, font));
                table.AddCell(new Phrase("المدينة", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.City.NameAr, font));
                table.AddCell(new Phrase("الهيئة", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.Organization.NameAr, font));
                table.AddCell(new Phrase("الهيئة الفرعية", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.SubOrganization.NameAr, font));
                table.AddCell(new Phrase("الفئة", font));
                if (assetDetailObj.MasterAsset.Category != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.Category.NameAr, font));
                else
                    table.AddCell(new Phrase(" ", font));
                table.AddCell(new Phrase("الفئة الفرعية", font));
                if (assetDetailObj.MasterAsset.SubCategory != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.SubCategory.NameAr, font));
                else
                    table.AddCell(new Phrase(" ", font));
                table.AddCell(new Phrase("الأولوية", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.AssetPeriority.NameAr, font));
                table.AddCell(new Phrase("القسم", font));
                table.AddCell(new Phrase(assetDetailObj.Department.NameAr, font));
                table.AddCell(new Phrase("الباركود", font));
                table.AddCell(new Phrase(assetDetailObj.Barcode, font));
                table.AddCell(new Phrase("السيريال", font));
                table.AddCell(new Phrase(assetDetailObj.SerialNumber, font));
                table.AddCell(new Phrase("الموديل", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.ModelNumber, font));
                table.AddCell(new Phrase("الماركة", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.brand.NameAr, font));
                table.AddCell(new Phrase("المورد", font));
                table.AddCell(new Phrase(assetDetailObj.Supplier.NameAr, font));
                table.AddCell(new Phrase("بلد المنشأ", font));

                if (assetDetailObj.MasterAsset.Origin != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.Origin.NameAr, font));
                else
                    table.AddCell(new Phrase(" ", font));

                document.Add(table);


                Phrase ph = new Phrase(" ", font);
                ph.Leading = 15;
                document.Add(ph);


                PdfPTable assetLocationTable = new PdfPTable(6);
                assetLocationTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetLocationTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell assetLocationCell = new PdfPCell(new Phrase("الموقع", titlefont));
                assetLocationCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetLocationCell.Border = 0;
                assetLocationCell.Colspan = 6;
                assetLocationCell.PaddingBottom = 10;
                assetLocationCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                assetLocationTable.AddCell(assetLocationCell);

                assetLocationTable.AddCell(new Phrase("المبنى", font));
                if (assetDetailObj.Building != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Building.NameAr, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));

                assetLocationTable.AddCell(new Phrase("الدور", font));
                if (assetDetailObj.Floor != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Floor.NameAr, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));

                assetLocationTable.AddCell(new Phrase("الغرفة", font));
                if (assetDetailObj.Room != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Room.NameAr, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));
                document.Add(assetLocationTable);


                Phrase ph2 = new Phrase(" ", font);
                ph2.Leading = 15;
                document.Add(ph2);



                PdfPTable assetWarrantyTable = new PdfPTable(6);
                assetWarrantyTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetWarrantyTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell assetWarrantyCell = new PdfPCell(new Phrase("الضمان", titlefont));
                assetWarrantyCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetWarrantyCell.Border = 0;
                assetWarrantyCell.Colspan = 6;
                assetWarrantyCell.PaddingBottom = 10;
                assetWarrantyCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                assetWarrantyTable.AddCell(assetWarrantyCell);

                assetWarrantyTable.AddCell(new Phrase("يبدأ الضمان", font));
                if (assetDetailObj.WarrantyStart != null)
                {
                    if (assetDetailObj.WarrantyStart.Value.Date.Year != 1900)
                        assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyStart.Value.ToShortDateString(), font));
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));
                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));


                assetWarrantyTable.AddCell(new Phrase("ينتهي الضمان", font));
                if (assetDetailObj.WarrantyEnd != null)
                {
                    if (assetDetailObj.WarrantyEnd.Value.Date.Year != 1900)
                        assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyEnd.Value.ToShortDateString(), font));
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));
                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));



                assetWarrantyTable.AddCell(new Phrase("مدة الضمان", font));
                if (assetDetailObj.WarrantyExpires != "")
                    assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyExpires + "  شهر", font));
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));


                assetWarrantyTable.AddCell(new Phrase("الضمان ينتهي بعد", font));
                if (assetDetailObj.WarrantyEnd != null)
                {
                    var resultAr = Asset.Core.Helpers.DateTimeExtensions.ToDateStringAr(DateTime.Today.Date, DateTime.Parse(assetDetailObj.WarrantyEnd.Value.Date.ToString()));
                    if (assetDetailObj.WarrantyEnd.Value.Date.Year != 1900)
                    {
                        assetWarrantyTable.AddCell(new Phrase(Helpers.ArabicNumeralHelper.ConvertNumerals(resultAr.ToString()), font));
                    }
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));

                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));
                document.Add(assetWarrantyTable);




                Phrase ph3 = new Phrase(" ", font);
                ph3.Leading = 15;
                document.Add(ph3);


                PdfPTable assetPurchaseTable = new PdfPTable(8);
                assetPurchaseTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                assetPurchaseTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell assetPurchaseCell = new PdfPCell(new Phrase("الشراء", titlefont));
                assetPurchaseCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetPurchaseCell.Border = 0;
                assetPurchaseCell.Colspan = 8;
                assetPurchaseCell.PaddingBottom = 10;
                assetPurchaseCell.HorizontalAlignment = 0;
                assetPurchaseTable.AddCell(assetPurchaseCell);


                assetPurchaseTable.AddCell(new Phrase("الشراء", font));
                if (assetDetailObj.PurchaseDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.PurchaseDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("تاريخ توريد", font));
                if (assetDetailObj.ReceivingDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.ReceivingDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("تاريخ التركيب", font));
                if (assetDetailObj.InstallationDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.InstallationDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));


                assetPurchaseTable.AddCell(new Phrase("تاريخ التشغيل", font));
                if (assetDetailObj.OperationDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.OperationDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("رقم أمر الشراء", font));
                if (assetDetailObj.PONumber != "")
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.PONumber, font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("السعر", font));
                if (assetDetailObj.Price != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.Price.ToString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));


                assetPurchaseTable.AddCell(new Phrase("رقم الحساب", font));
                if (assetDetailObj.CostCenter != "")
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.CostCenter, font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));

                assetPurchaseTable.AddCell(new Phrase(" ", font));
                assetPurchaseTable.AddCell(new Phrase(" ", font));

                document.Add(assetPurchaseTable);



                Phrase ph4 = new Phrase(" ", font);
                ph4.Leading = 15;
                document.Add(ph4);

                var lstRequests = _requestService.GetAllRequestsByAssetId(assetId, int.Parse(assetDetailObj.HospitalId.ToString())).ToList();
                PdfPTable assetRequestTable = new PdfPTable(7);
                assetRequestTable.SetTotalWidth(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f });
                assetRequestTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetRequestTable.HorizontalAlignment = Element.ALIGN_CENTER;
                assetRequestTable.WidthPercentage = 100;
                assetRequestTable.PaddingTop = 200;
                assetRequestTable.HeaderRows = 1;
                assetRequestTable.SetWidths(new int[] { 10, 10, 10, 10, 10, 10, 10 });

                string[] col = { "الحالة", "الموضوع", "التاريخ", "رقم أمر الشغل", "الموضوع", "التاريخ", "رقم بلاغ العطل" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell reqCell = new PdfPCell(new Phrase(col[i], font));
                    reqCell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    reqCell.PaddingBottom = 10;
                    assetRequestTable.AddCell(reqCell);
                }

                int index = 0;
                foreach (var item in lstRequests)
                {
                    ++index;

                    if (item.RequestCode != "")
                        assetRequestTable.AddCell(new Phrase(item.RequestCode, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));

                    if (item.Subject != "")
                        assetRequestTable.AddCell(new Phrase(item.Subject, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));


                    if (item.RequestDate != null)
                        assetRequestTable.AddCell(new Phrase(item.RequestDate.ToShortDateString(), font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));




                    if (item.WorkOrderNumber != "")
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderNumber, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));



                    if (item.WorkOrderDate != null)
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderDate.Value.ToShortDateString(), font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));


                    if (item.WorkOrderSubject != "")
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderSubject, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));




                    if (item.StatusNameAr != "")
                        assetRequestTable.AddCell(new Phrase(item.StatusNameAr, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));
                }
                document.Add(assetRequestTable);
            }


            else
            {

                PdfPTable table = new PdfPTable(4);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell cell = new PdfPCell(new Phrase("Main Data", titlefont));
                cell.BackgroundColor = new BaseColor(153, 204, 255);
                cell.PaddingBottom = 10;
                cell.Border = 0;
                cell.Colspan = 4;
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell(new Phrase("Governorate", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.Governorate.Name, font));
                table.AddCell(new Phrase("City", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.City.Name, font));
                table.AddCell(new Phrase("Organization", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.Organization.Name, font));
                table.AddCell(new Phrase("Sub Organization", font));
                table.AddCell(new Phrase(assetDetailObj.Hospital.SubOrganization.Name, font));
                table.AddCell(new Phrase("Category", font));
                if (assetDetailObj.MasterAsset.Category != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.Category.Name, font));
                else
                    table.AddCell(new Phrase(" ", font));
                table.AddCell(new Phrase("Sub Category", font));
                if (assetDetailObj.MasterAsset.SubCategory != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.SubCategory.Name, font));
                else
                    table.AddCell(new Phrase(" ", font));
                table.AddCell(new Phrase("Periority", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.AssetPeriority.Name, font));
                table.AddCell(new Phrase("Department", font));
                table.AddCell(new Phrase(assetDetailObj.Department.Name, font));
                table.AddCell(new Phrase("BarCode", font));
                table.AddCell(new Phrase(assetDetailObj.Barcode, font));
                table.AddCell(new Phrase("Serial", font));
                table.AddCell(new Phrase(assetDetailObj.SerialNumber, font));
                table.AddCell(new Phrase("Model", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.ModelNumber, font));
                table.AddCell(new Phrase("Brand", font));
                table.AddCell(new Phrase(assetDetailObj.MasterAsset.brand.Name, font));
                table.AddCell(new Phrase("Supplier", font));
                table.AddCell(new Phrase(assetDetailObj.Supplier.Name, font));
                table.AddCell(new Phrase("Origin", font));
                if (assetDetailObj.MasterAsset.Origin != null)
                    table.AddCell(new Phrase(assetDetailObj.MasterAsset.Origin.Name, font));
                else
                    table.AddCell(new Phrase(" ", font));

                document.Add(table);


                Phrase ph = new Phrase(" ", font);
                ph.Leading = 15;
                document.Add(ph);


                PdfPTable assetLocationTable = new PdfPTable(6);
               // assetLocationTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetLocationTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell assetLocationCell = new PdfPCell(new Phrase("Location", titlefont));
                assetLocationCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetLocationCell.Border = 0;
                assetLocationCell.Colspan = 6;
                assetLocationCell.PaddingBottom = 10;
                assetLocationCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                assetLocationTable.AddCell(assetLocationCell);

                assetLocationTable.AddCell(new Phrase("Building", font));
                if (assetDetailObj.Building != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Building.Name, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));

                assetLocationTable.AddCell(new Phrase("Floor", font));
                if (assetDetailObj.Floor != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Floor.Name, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));

                assetLocationTable.AddCell(new Phrase("Room", font));
                if (assetDetailObj.Room != null)
                    assetLocationTable.AddCell(new Phrase(assetDetailObj.Room.Name, font));
                else
                    assetLocationTable.AddCell(new Phrase(" ", font));
                document.Add(assetLocationTable);


                Phrase ph2 = new Phrase(" ", font);
                ph2.Leading = 15;
                document.Add(ph2);



                PdfPTable assetWarrantyTable = new PdfPTable(6);
               // assetWarrantyTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetWarrantyTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell assetWarrantyCell = new PdfPCell(new Phrase("Warranty", titlefont));
                assetWarrantyCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetWarrantyCell.Border = 0;
                assetWarrantyCell.Colspan = 6;
                assetWarrantyCell.PaddingBottom = 10;
                assetWarrantyCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                assetWarrantyTable.AddCell(assetWarrantyCell);

                assetWarrantyTable.AddCell(new Phrase("Warranty Start", font));
                if (assetDetailObj.WarrantyStart != null)
                {
                    if (assetDetailObj.WarrantyStart.Value.Date.Year != 1900)
                        assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyStart.Value.ToShortDateString(), font));
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));
                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));


                assetWarrantyTable.AddCell(new Phrase("Warranty End", font));
                if (assetDetailObj.WarrantyEnd != null)
                {
                    if (assetDetailObj.WarrantyEnd.Value.Date.Year != 1900)
                        assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyEnd.Value.ToShortDateString(), font));
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));
                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));



                assetWarrantyTable.AddCell(new Phrase("Warranty Expires", font));
                if (assetDetailObj.WarrantyExpires != "")
                    assetWarrantyTable.AddCell(new Phrase(assetDetailObj.WarrantyExpires + "  Months", font));
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));


                assetWarrantyTable.AddCell(new Phrase("Warranty End After", font));
                if (assetDetailObj.WarrantyEnd != null)
                {
                    var result = Asset.Core.Helpers.DateTimeExtensions.ToDateString(DateTime.Parse(assetDetailObj.WarrantyEnd.Value.Date.ToString()), DateTime.Today.Date);
                    if (assetDetailObj.WarrantyEnd.Value.Date.Year != 1900)
                    {
                        assetWarrantyTable.AddCell(new Phrase(result.ToString()));
                    }
                    else
                        assetWarrantyTable.AddCell(new Phrase(" ", font));

                }
                else
                    assetWarrantyTable.AddCell(new Phrase(" ", font));
                document.Add(assetWarrantyTable);




                Phrase ph3 = new Phrase(" ", font);
                ph3.Leading = 15;
                document.Add(ph3);


                PdfPTable assetPurchaseTable = new PdfPTable(8);
                assetPurchaseTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
               // assetPurchaseTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                PdfPCell assetPurchaseCell = new PdfPCell(new Phrase("Purchase", titlefont));
                assetPurchaseCell.BackgroundColor = new BaseColor(153, 204, 255);
                assetPurchaseCell.Border = 0;
                assetPurchaseCell.Colspan = 8;
                assetPurchaseCell.PaddingBottom = 10;
                assetPurchaseCell.HorizontalAlignment = 0;
                assetPurchaseTable.AddCell(assetPurchaseCell);


                assetPurchaseTable.AddCell(new Phrase("Purchase", font));
                if (assetDetailObj.PurchaseDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.PurchaseDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("Receiving Date", font));
                if (assetDetailObj.ReceivingDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.ReceivingDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("Installation Date", font));
                if (assetDetailObj.InstallationDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.InstallationDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));


                assetPurchaseTable.AddCell(new Phrase("Operation Date", font));
                if (assetDetailObj.OperationDate != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.OperationDate.Value.ToShortDateString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("PONumber", font));
                if (assetDetailObj.PONumber != "")
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.PONumber, font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));



                assetPurchaseTable.AddCell(new Phrase("Price", font));
                if (assetDetailObj.Price != null)
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.Price.ToString(), font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));


                assetPurchaseTable.AddCell(new Phrase("CostCenter", font));
                if (assetDetailObj.CostCenter != "")
                    assetPurchaseTable.AddCell(new Phrase(assetDetailObj.CostCenter, font));
                else
                    assetPurchaseTable.AddCell(new Phrase(" ", font));

                assetPurchaseTable.AddCell(new Phrase(" ", font));
                assetPurchaseTable.AddCell(new Phrase(" ", font));

                document.Add(assetPurchaseTable);



                Phrase ph4 = new Phrase(" ", font);
                ph4.Leading = 15;
                document.Add(ph4);

                var lstRequests = _requestService.GetAllRequestsByAssetId(assetId, int.Parse(assetDetailObj.HospitalId.ToString())).ToList();
                PdfPTable assetRequestTable = new PdfPTable(7);
                assetRequestTable.SetTotalWidth(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f });
              //  assetRequestTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                assetRequestTable.HorizontalAlignment = Element.ALIGN_CENTER;
                assetRequestTable.WidthPercentage = 100;
                assetRequestTable.PaddingTop = 200;
                assetRequestTable.HeaderRows = 1;
                assetRequestTable.SetWidths(new int[] { 10, 10, 10, 10, 10, 10, 10 });
                string[] col = { "Req No", "Req Date", "Req Subject", "WorkOrder No.", "WO. Date", "Wo. Subject", "Status" };
                for (int i = col.Length - 1; i >= 0; i--)
                {
                    PdfPCell reqCell = new PdfPCell(new Phrase(col[i], font));
                    reqCell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                    reqCell.PaddingBottom = 10;
                    assetRequestTable.AddCell(reqCell);
                }

                int index = 0;
                foreach (var item in lstRequests)
                {
                    ++index;

                    if (item.RequestCode != "")
                        assetRequestTable.AddCell(new Phrase(item.RequestCode, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));

                    if (item.Subject != "")
                        assetRequestTable.AddCell(new Phrase(item.Subject, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));


                    if (item.RequestDate != null)
                        assetRequestTable.AddCell(new Phrase(item.RequestDate.ToShortDateString(), font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));




                    if (item.WorkOrderNumber != "")
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderNumber, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));



                    if (item.WorkOrderDate != null)
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderDate.Value.ToShortDateString(), font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));


                    if (item.WorkOrderSubject != "")
                        assetRequestTable.AddCell(new Phrase(item.WorkOrderSubject, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));




                    if (item.StatusNameAr != "")
                        assetRequestTable.AddCell(new Phrase(item.StatusName, font));
                    else
                        assetRequestTable.AddCell(new Phrase(" ", font));
                }
                document.Add(assetRequestTable);
            }


            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/AssetHistory.pdf", bytes);
            memoryStream.Close();
            document.Close();



        }



        [HttpGet]
        [Route("AlertAssetsBefore3Monthes")]
        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes()
        {
            return _AssetDetailService.AlertAssetsBefore3Monthes();
        }
        [HttpGet]
        [Route("AlertAssetsBefore3MonthesWithDuration/{duration}")]
        public IEnumerable<IndexAssetDetailVM.GetData> AlertAssetsBefore3Monthes(int duration)
        {
            return _AssetDetailService.AlertAssetsBefore3Monthes(duration);
        }

        [HttpPost]
        [Route("AlertAssetsBefore3MonthesWithDuration2/{duration}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM AlertAssetsBefore3Monthes2(int duration, int pageNumber, int pageSize)
        {
            return _AssetDetailService.AlertAssetsBefore3Monthes(duration, pageNumber, pageSize);
        }






        [HttpGet]
        [Route("ViewAllAssetDetailByMasterId/{MasterAssetId}")]
        public IEnumerable<AssetDetail> ViewAllAssetDetailByMasterId(int MasterAssetId)
        {
            return _AssetDetailService.ViewAllAssetDetailByMasterId(MasterAssetId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalNotInContract/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract(int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContract(hospitalId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalNotInContract2/{barcode}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContract2(string barcode, int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContract(barcode, hospitalId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalNotInContractBySerialNumber/{serialNumber}/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(string serialNumber, int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalNotInContractBySerialNumber(serialNumber, hospitalId);
        }
        [HttpGet]
        [Route("GetListOfAssetDetailsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetListOfAssetDetailsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetListOfAssetDetailsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetNoneExcludedAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetNoneExcludedAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetSupplierNoneExcludedAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<ViewAssetDetailVM> GetSupplierNoneExcludedAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.GetSupplierNoneExcludedAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("GetAssetDetailsByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetDetailsByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetDetailsByUserId(userId);
        }
        [HttpGet]
        [Route("GetAssetsByUserId/{userId}")]
        public async Task<IEnumerable<IndexAssetDetailVM.GetData>> GetAssetsByUserId(string userId)
        {
            return await _AssetDetailService.GetAssetsByUserId(userId);
        }

        [HttpPost]
        [Route("GetAssetsByUserIdAndPaging/{userId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM GetAssetsByUserIdAndPaging(string userId, int pageNumber, int pageSize)
        {
            // return _AssetDetailService.GetAssetsByUserId(userId, pageNumber, pageSize);
            return _AssetDetailService.GetAssetsByUserIdAndPaging(userId, pageNumber, pageSize);
        }

        //[HttpPost]
        //[Route("GetAssetsByUserIdAndPaging/{userId}/{pageNumber}/{pageSize}")]
        //public IndexAssetDetailVM GetAssetsByUserIdAndPaging(string userId, int pageNumber, int pageSize)
        //{
        //    //return _AssetDetailService.GetAssetsByUserId(userId, pageNumber, pageSize);
        //    return _AssetDetailService.GetAssetsByUserIdAndPaging(userId, pageNumber, pageSize);
        //}



        //[HttpPost]
        //[Route("GetAssetsByUserIdAndPaging/{userId}/{pageNumber}/{pageSize}")]
        //public IndexAssetDetailVM GetAssetsByUserIdAndPaging(string userId, int pageNumber, int pageSize)
        //{
        //    //return _AssetDetailService.GetAssetsByUserId(userId, pageNumber, pageSize);
        //    return _AssetDetailService.GetAssetsByUserIdAndPaging(userId, pageNumber, pageSize);
        //}






        [HttpPut]
        [Route("GetAssetDetailsByUserIdWithPaging/{userId}")]
        public IEnumerable<IndexAssetDetailVM.GetData> GetAssetDetailsByUserId(string userId, PagingParameter pageInfo)
        {
            var AssetDetail = _AssetDetailService.GetAssetDetailsByUserId(userId).Result.ToList();
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, AssetDetail);
        }
        [HttpPost]
        [Route("GetAssetDetailsByUserIdWithPaging2/{pagenumber}/{pagesize}/{userId}")]
        public async Task<IndexAssetDetailVM> GetAssetDetailsByUserId2(int pageNumber, int pageSize, string userId)
        {
            var lstAssetDetails = await _AssetDetailService.GetAssetDetailsByUserId2(pageNumber, pageSize, userId);
            return lstAssetDetails;
        }



        [HttpGet]
        [Route("GetAllPMAssetTaskSchedules/{hospitalId}")]
        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskSchedules(int? hospitalId)
        {
            return _AssetDetailService.GetAllPMAssetTaskSchedules(hospitalId);
        }



        [HttpGet]
        [Route("GetAllPMAssetTaskScheduleByAssetId/{assetId}")]
        public IEnumerable<IndexPMAssetTaskScheduleVM.GetData> GetAllPMAssetTaskScheduleByAssetId(int? assetId)
        {
            return _AssetDetailService.GetAllPMAssetTaskScheduleByAssetId(assetId);
        }



        [HttpPut]
        [Route("UpdateAssetDetail")]
        public IActionResult Update(EditAssetDetailVM AssetDetailVM)
        {
            try
            {
                //a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber
                int id = AssetDetailVM.Id;
                if (!string.IsNullOrEmpty(AssetDetailVM.Code))
                {
                    var lstCode = _AssetDetailService.GetAll().Where(a => a.Code == AssetDetailVM.Code && a.Id != id).ToList();
                    if (lstCode.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                    }
                }
                var lstNames = _AssetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "serial", Message = "Asset serial already exist", MessageAr = "هذا السيريال مسجل سابقاً" });
                }

                else
                {
                    // var domainName = "http://" + HttpContext.Request.Host.Value;
                    var domainName = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                    AssetDetailVM.DomainName = domainName;
                    int updatedRow = _AssetDetailService.Update(AssetDetailVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
            return Ok();
        }
        [HttpPost]
        [Route("AddAssetDetail")]
        public ActionResult Add(CreateAssetDetailVM AssetDetailVM)
        {
            var lstCode = _AssetDetailService.GetAll().ToList().Where(a => a.Code == AssetDetailVM.Code).ToList();
            if (lstCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _AssetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "name", Message = "Asset already exist with this data", MessageAr = "هذا الجهاز مسجل سابقاً" });
            }
            else
            {
                var savedId = _AssetDetailService.Add(AssetDetailVM);
                _qrController.Index(AssetDetailVM.Id);
                CreateAssetDetailAttachmentVM qrAttach = new CreateAssetDetailAttachmentVM();
                qrAttach.AssetDetailId = AssetDetailVM.Id;
                qrAttach.FileName = "asset-" + AssetDetailVM.Id + ".png";
                CreateAssetDetailAttachments(qrAttach);
                return Ok(savedId);
            }
        }

        [HttpDelete]
        [Route("DeleteAssetDetail/{id}")]
        public ActionResult<AssetDetail> Delete(int id)
        {
            try
            {
                var assetObj = _AssetDetailService.GetById(id);
                var lstMovements = _assetMovementService.GetMovementByAssetDetailId(id).ToList();
                if (lstMovements.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "move", Message = "You cannot delete this asset it has movement", MessageAr = "لا يمكن مسح هذا الأصل لأن له حركات في المستشفى" });
                }
                var lstRequests = _requestService.GetAllRequestsByAssetId(id, int.Parse(assetObj.HospitalId.ToString())).ToList();
                if (lstRequests.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "request", Message = "You cannot delete this asset it has requests", MessageAr = "لا يمكن مسح هذا الأصل لأن له بلاغات أعطال " });
                }
                var lstWO = _workOrderService.GetLastRequestAndWorkOrderByAssetId(id).ToList();
                if (lstWO.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "wo", Message = "You cannot delete this asset it has workorders", MessageAr = "لا يمكن مسح هذا الأصل لأن له  أوامر شغل" });
                }
                else
                {

                    var lstOwners = _assetOwnerService.GetOwnersByAssetDetailId(id).ToList();
                    if (lstOwners.Count > 0)
                    {
                        foreach (var item in lstOwners)
                        {
                            _assetOwnerService.Delete(item.Id);
                        }
                    }

                    var lstAssetTransactions = _assetStatusTransactionService.GetAssetStatusByAssetDetailId(id).ToList();
                    if (lstAssetTransactions.Count > 0)
                    {
                        foreach (var item in lstAssetTransactions)
                        {
                            _assetStatusTransactionService.Delete(item.Id);
                        }
                    }


                    int deletedRow = _AssetDetailService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("CreateAssetDetailAttachments")]
        public int CreateAssetDetailAttachments(CreateAssetDetailAttachmentVM attachObj)
        {
            return _AssetDetailService.CreateAssetDetailDocuments(attachObj);
        }
        [HttpPost]
        [Route("UploadAssetDetailFiles")]
        [Obsolete]
        public ActionResult UploadInFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/" + file.FileName;
            if (!System.IO.File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet]
        [Route("GetOwnersByAssetDetailId/{assetDetailId}")]
        public List<AssetOwner> GetOwnersByAssetDetailId(int assetDetailId)
        {
            return _assetOwnerService.GetOwnersByAssetDetailId(assetDetailId).ToList();
        }

        [HttpGet]
        [Route("GetAttachmentByAssetDetailId/{assetId}")]
        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _AssetDetailService.GetAttachmentByAssetDetailId(assetId);
        }
        [HttpDelete]
        [Route("DeleteAssetDetailAttachment/{id}")]
        public int DeleteAssetDetailAttachment(int id)
        {
            return _AssetDetailService.DeleteAssetDetailAttachment(id);
        }
        [HttpGet]
        [Route("CountAssetsByHospital")]
        public IEnumerable<CountAssetVM> CountAssetsByHospital()
        {
            return _AssetDetailService.CountAssetsByHospital();
        }
        [HttpGet]
        [Route("ListTopAssetsByHospitalId/{hospitalId}")]
        public IEnumerable<CountAssetVM> ListTopAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.ListTopAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("ListAssetsByGovernorateIds")]
        public IEnumerable<CountAssetVM> ListAssetsByGovernorateIds()
        {
            return _AssetDetailService.ListAssetsByGovernorateIds();
        }
        [HttpGet]
        [Route("ListAssetsByCityIds")]
        public IEnumerable<CountAssetVM> ListAssetsByCityIds()
        {
            return _AssetDetailService.ListAssetsByCityIds();
        }
        [HttpGet]
        [Route("CountAssetsInHospitalByHospitalId/{hospitalId}")]
        public IEnumerable<CountAssetVM> CountAssetsInHospitalByHospitalId(int hospitalId)
        {
            return _AssetDetailService.CountAssetsInHospitalByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("CountAssetsByHospitalId/{hospitalId}")]
        public int CountAssetsByHospitalId(int hospitalId)
        {
            return _AssetDetailService.CountAssetsByHospitalId(hospitalId);
        }
        [HttpGet]
        [Route("Group/{assetId}")]
        public IEnumerable<PmDateGroupVM> GetEquimentswithgrouping(int assetId)
        {
            return _AssetDetailService.GetAllwithgrouping(assetId);
        }
        [HttpGet]
        [Route("MonthDiff/{d1}/{d2}")]
        public int MonthDiff(DateTime d1, DateTime d2)
        {
            int m1;
            int m2;
            if (d1 < d2)
            {
                m1 = (d2.Month - d1.Month);//for years
                m2 = (d2.Year - d1.Year) * 12; //for months
            }
            else
            {
                m1 = (d1.Month - d2.Month);//for years
                m2 = (d1.Year - d2.Year) * 12; //for months
            }

            return m1 + m2;
        }
        [Route("FilterAsset")]
        [HttpPost]
        public ActionResult<List<IndexAssetDetailVM.GetData>> FilterAsset(filterDto data)
        {
            return _AssetDetailService.FilterAsset(data);
        }
        [HttpPost]
        [Route("GetAssetByDepartment")]
        public ActionResult<List<DepartmentGroupVM>> GetAssetByDepartment(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByDepartment(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByBrands")]
        public ActionResult<List<BrandGroupVM>> GetAssetByBrands(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByBrands(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByHospital")]
        public ActionResult<List<GroupHospitalVM>> GetAssetByHospital(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByHospital(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByGovernorate")]
        public ActionResult<List<GroupGovernorateVM>> GetAssetByGovernorate(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByGovernorate(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByCity")]
        public ActionResult<List<GroupCityVM>> GetAssetByCity(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByCity(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetBySupplier")]
        public ActionResult<List<GroupSupplierVM>> GetAssetBySupplier(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetBySupplier(AssetModel);
        }
        [HttpPost]
        [Route("GetAssetByOrganization")]
        public ActionResult<List<GroupOrganizationVM>> GetAssetByOrganization(List<IndexAssetDetailVM.GetData> AssetModel)
        {
            return _AssetDetailService.GetAssetByOrganization(AssetModel);
        }
        [HttpPost]
        [Route("SortAssets/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexAssetDetailVM.GetData> SortAssets(int pagenumber, int pagesize, Sort sortObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _AssetDetailService.SortAssets(sortObj);
            return _pagingService.GetAll<IndexAssetDetailVM.GetData>(pageInfo, list.ToList());
        }

        [HttpPost]
        [Route("SortAssets2/{statusId}/{userId}")]
        public IndexAssetDetailVM SortAssets(Sort sortObj, int statusId, string userId)
        {
            var assetDetailData = _AssetDetailService.SortAssets(sortObj, statusId, userId);
            return assetDetailData;
        }

        [HttpPost]
        [Route("SortAssetsByPaging/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssets2(Sort sortObj, int pageNumber, int pageSize)
        {
            var assetDetailData = _AssetDetailService.SortAssets2(sortObj, pageNumber, pageSize);
            return assetDetailData;
        }




        [HttpPost]
        [Route("SortHospitalAssetsBySupplierId/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortHospitalAssetsBySupplierId(Sort sortObj, int pageNumber, int pageSize)
        {
            var assetDetailData = _AssetDetailService.SortHospitalAssetsBySupplierId(sortObj, pageNumber, pageSize);
            return assetDetailData;
        }

        [HttpPost]
        [Route("SortAssetsCount")]
        public int SortAssets(Sort sortObj)
        {
            var list = _AssetDetailService.SortAssets(sortObj);
            // var list = await Task.Run(() => _AssetDetailService.SortAssets(sortObj));

            var count = list.Count();
            return count;
        }
        [HttpGet]
        [Route("GetAssetsByAgeGroup/{hospitalId}")]
        public List<HospitalAssetAge> GetAssetsByAgeGroup(int hospitalId)
        {
            var list = _AssetDetailService.GetAssetsByAgeGroup(hospitalId);
            return list;
        }
        [HttpPost]
        [Route("GetGeneralAssetsByAgeGroup")]
        public List<HospitalAssetAge> GetGeneralAssetsByAgeGroup(FilterHospitalAssetAge model)
        {
            var list = _AssetDetailService.GetGeneralAssetsByAgeGroup(model);
            return list;
        }

        [Route("GetLastDocumentForAssetDetailId/{assetDetailId}")]
        public AssetDetailAttachment GetLastDocumentForWorkOrderTrackingId(int assetDetailId)
        {
            return _AssetDetailService.GetLastDocumentForAssetDetailId(assetDetailId);
        }

        [HttpPost]
        [Route("CreateAssetDepartmentBrandSupplierPDF")]
        public void CreateAssetDepartmentBrandSupplierPDF(FilterHospitalAsset filterHospitalAssetObj)
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
                    if (item.KeyName == "CanAdd")
                        canAdd = Convert.ToBoolean(item.KeyValue);
                }
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
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

            PdfPTable bodytable = AssetDepartmentBrandSupplier(filterHospitalAssetObj);
            int countnewpages = bodytable.Rows.Count / 25;
            for (int i = 1; i <= countnewpages; i++)
            {
                document.NewPage();
                writer.PageEmpty = false;
            }

            document.Close();
            byte[] bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf", bytes);


            memoryStream = new MemoryStream();
            PdfReader reader = new PdfReader(bytes);
            using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
            {
                int pages = reader.NumberOfPages;
                //Footer
                for (int i = 1; i <= pages; i++)
                {
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(ArabicNumeralHelper.toArabicNumber(pages.ToString()) + "/" + ArabicNumeralHelper.toArabicNumber(i.ToString()), font), 800f, 15f, 0);
                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("تمت الطباعة بواسطة  " + filterHospitalAssetObj.PrintedBy, font), 150f, 15f, 0, PdfWriter.RUN_DIRECTION_RTL, ColumnText.AR_LIG);
                }
                //Header
                for (int i = 1; i <= pages; i++)
                {
                    string imageURL = _webHostingEnvironment.ContentRootPath + "/Images/" + strLogo;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                    jpg.ScaleAbsolute(70f, 50f);
                    PdfPTable headertable = new PdfPTable(2);
                    headertable.SetTotalWidth(new float[] { 250f, 50f });
                    headertable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    headertable.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new PdfPCell(jpg));
                    //cell.Rowspan = 2;
                    cell.PaddingTop = 5;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.PaddingRight = 10;
                    //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    headertable.AddCell(cell);

                    if (filterHospitalAssetObj.Lang == "ar")
                    {
                        headertable.AddCell(new PdfPCell(new Phrase("\t\t\t\t " + strInsituteAr + "\n" + filterHospitalAssetObj.HospitalNameAr + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    }
                    else
                        headertable.AddCell(new PdfPCell(new Phrase(" " + strInsitute + "\n" + filterHospitalAssetObj.HospitalName + "", font)) { Border = Rectangle.NO_BORDER, PaddingTop = 10 });
                    headertable.WriteSelectedRows(0, -1, 270, 830, stamper.GetOverContent(i));

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    string adobearabicheaderTitle = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
                    BaseFont bfUniCodeheaderTitle = BaseFont.CreateFont(adobearabicheaderTitle, BaseFont.IDENTITY_H, true);
                    iTextSharp.text.Font titlefont = new iTextSharp.text.Font(bfUniCodeheaderTitle, 13);
                    titlefont.SetStyle("bold");


                    PdfPTable titleTable = new PdfPTable(1);
                    titleTable.SetTotalWidth(new float[] { 600f });
                    titleTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    titleTable.WidthPercentage = 100;
                    titleTable.AddCell(new PdfPCell(new Phrase("تقرير الأجهزة بالأقسام والموردين والماركات", titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    if (filterHospitalAssetObj.Start == "")
                        filterHospitalAssetObj.Start = "01/01/1900";

                    var sDate = DateTime.Parse(filterHospitalAssetObj.Start);
                    var sday = ArabicNumeralHelper.toArabicNumber(sDate.Day.ToString());
                    var smonth = ArabicNumeralHelper.toArabicNumber(sDate.Month.ToString());
                    var syear = ArabicNumeralHelper.toArabicNumber(sDate.Year.ToString());
                    var strStart = sday + "/" + smonth + "/" + syear;

                    if (filterHospitalAssetObj.End == "")
                        filterHospitalAssetObj.End = DateTime.Today.Date.ToShortDateString();

                    var eDate = DateTime.Parse(filterHospitalAssetObj.End);
                    var eday = ArabicNumeralHelper.toArabicNumber(eDate.Day.ToString());
                    var emonth = ArabicNumeralHelper.toArabicNumber(eDate.Month.ToString());
                    var eyear = ArabicNumeralHelper.toArabicNumber(eDate.Year.ToString());
                    var strEnd = eday + "/" + emonth + "/" + eyear;

                    titleTable.AddCell(new PdfPCell(new Phrase("خلال الفترة من" + strStart + " إلى " + strEnd, titlefont)) { PaddingBottom = 5, Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    titleTable.WriteSelectedRows(0, -1, 0, 760, stamper.GetOverContent(i));
                }
                for (int i = 1; i <= pages; i++)
                {
                    PdfPTable bodytable2 = new PdfPTable(8);
                    bodytable2.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
                    bodytable2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    bodytable2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    bodytable2.WidthPercentage = 100;
                    bodytable2.PaddingTop = 200;
                    bodytable2.HeaderRows = 1;

                    bodytable2.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 7 });
                    int countRows = bodytable.Rows.Count;
                    if (countRows > 25)
                    {
                        countRows = 25;
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
                    bodytable2.WriteSelectedRows(0, -1, 10, 700, stamper.GetUnderContent(i));
                }
            }
            bytes = memoryStream.ToArray();
            System.IO.File.WriteAllBytes(_webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf", bytes);
            memoryStream.Close();
            document.Close();

        }
        public PdfPTable AssetDepartmentBrandSupplier(FilterHospitalAsset filterHospitalAssetObj)
        {

            var lstData = _AssetDetailService.FilterDataByDepartmentBrandSupplierId(filterHospitalAssetObj).ToList();
            PdfPTable table = new PdfPTable(8);
            table.SetTotalWidth(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.WidthPercentage = 100;
            table.PaddingTop = 200;
            table.HeaderRows = 1;
            table.SetWidths(new int[] { 25, 25, 25, 25, 25, 25, 25, 7 });
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string ARIALUNI_TFF = _webHostingEnvironment.ContentRootPath + "/Font/adobearabic.ttf";
            BaseFont bfArialUniCode = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(bfArialUniCode, 10);


            if (filterHospitalAssetObj.selectedElement == "supplier" || filterHospitalAssetObj.selectedElement == "المورد")
            {
                var lstAssetsByBrand = _AssetDetailService.GetAssetBySupplier(lstData).ToList();
                foreach (var item in lstAssetsByBrand)
                {
                    // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

                    PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
                    c1.Colspan = 8;
                    table.AddCell(c1);



                    string[] col = { "المورد", "الماركة", "القسم", "الموديل", "السيريال", "الباركود", "الاسم", "م" };
                    string[] encol = { "No.", "Name", "Barcode", "Serial", "Model", "Department", "Brand", "Supplier" };
                    if (filterHospitalAssetObj.Lang == "ar")
                    {
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
                        for (int i = 0; i <= encol.Length - 1; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(encol[i]));
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(153, 204, 255);
                            cell.PaddingBottom = 10;
                            table.AddCell(cell);
                        }
                    }
                    if (item.AssetList.Count > 0)
                    {
                        int index = 0;
                        foreach (var groupItems in item.AssetList)
                        {
                            ++index;
                            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
                            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
                            if (groupItems.PurchaseDate != null)
                                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
                            else
                                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
                        }
                    }
                }
            }
            //if (filterHospitalAssetObj.selectedElement == "brand" || filterHospitalAssetObj.selectedElement == "الصانع")
            //{
            //    var lstAssetsByBrand = _AssetDetailService.GetAssetByBrands(lstData).ToList();
            //    foreach (var item in lstAssetsByBrand)
            //    {
            //        // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

            //        PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
            //        c1.Colspan = 8;
            //        table.AddCell(c1);

            //        foreach (var groupItems in item.AssetList)
            //        {
            //           // ++index;
            //           // table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (groupItems.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}
            //if (filterHospitalAssetObj.selectedElement == "Department" || filterHospitalAssetObj.selectedElement == "القسم")
            //{
            //    var lstAssetsByBrand = _AssetDetailService.GetAssetByDepartment(lstData).ToList();
            //    foreach (var item in lstAssetsByBrand)
            //    {
            //        // table.AddCell(new PdfPCell(new Phrase(item.NameAr, font)) { PaddingBottom = 5, Colspan = 8 });

            //        PdfPCell c1 = new PdfPCell(new Phrase(item.NameAr, font));
            //        c1.Colspan = 8;
            //        table.AddCell(c1);

            //        foreach (var groupItems in item.AssetList)
            //        {
            //            ++index;
            //            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(groupItems.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (groupItems.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(groupItems.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}
            //else
            //{

            //    foreach (var item in lstData)
            //    {
            //        //  table.AddCell(new PdfPCell(new Phrase("R3C1-4")) { Colspan = 8 });
            //        ++index;
            //        if (filterHospitalAssetObj.Lang == "ar")
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(ArabicNumeralHelper.toArabicNumber(index.ToString()), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.AssetNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.DepartmentNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.BrandNameAr, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SupplierNameAr, font)) { PaddingBottom = 5 });
            //            if (item.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(ConvertDateTimeToArabicNumerals.ConvertToArabicNumerals(DateTime.Parse(item.PurchaseDate.ToString()).ToString("g", new CultureInfo("ar-AE"))), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //        else
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(index.ToString(), font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.AssetName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Barcode, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SerialNumber, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.Model, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.DepartmentName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.BrandName, font)) { PaddingBottom = 5 });
            //            table.AddCell(new PdfPCell(new Phrase(item.SupplierName, font)) { PaddingBottom = 5 });
            //            if (item.PurchaseDate != null)
            //                table.AddCell(new PdfPCell(new Phrase(DateTime.Parse(item.PurchaseDate.ToString()).ToString("g", new CultureInfo("en-US")), font)) { PaddingBottom = 5 });
            //            else
            //                table.AddCell(new PdfPCell(new Phrase(" ")) { PaddingBottom = 5 });
            //        }
            //    }
            //}

            return table;
        }

        [HttpGet]
        [Route("DownloadAssetDepartmentBrandSupplierPDF")]
        public HttpResponseMessage DownloadFile()
        {
            var file = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/FilterAssetDetails/FilterAssetDetails.pdf";
            HttpResponseMessage response = null;
            if (!System.IO.File.Exists(file))
                System.IO.Directory.CreateDirectory(file);
            //return new HttpResponseMessage(HttpStatusCode.Gone);
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




        /////////////////////////////////////////////////////
        /// PoliceQr - 2
        /////////////////////////////////////////
        [HttpPost]
        [Route("GenerateQrCodeForAllAssets")]
        public bool GenerateQrCodeForAllAssets(string domainName)
        {
            domainName = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            return _AssetDetailService.GenerateQrCodeForAllAssets(domainName);
        }
        [Route("GenerateWordForQrCodeForPoliceAssets")]
        public ActionResult GenerateWordForQrCodeForAllAssets()
        {

            using (WordDocument document = new WordDocument())
            {
                //Opens the Word template document
                string strTemplateFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\PoliceCardTemplate.dotx";

                Stream docStream = System.IO.File.OpenRead(strTemplateFile);
                document.Open(docStream, FormatType.Docx);
                docStream.Dispose();


                var allAssets = ListAssets().ToList();
                // DataTable dtAssets = GetAssetsAsDataTable();
                MailMergeDataTable dataTable = new MailMergeDataTable("Asset_QrCode", allAssets);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField_InsertPageBreak);
                document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(InsertQRBarcode);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField_Event);
                document.MailMerge.RemoveEmptyGroup = true;

                document.MailMerge.ExecuteGroup(dataTable);


                //Saves the file in the given path
                string strExportFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\PoliceCards.docx";
                docStream = System.IO.File.Create(strExportFile);
                document.Save(docStream, FormatType.Docx);
                docStream.Dispose();
                document.Close();

            }
            return Ok();
        }
        private static void MergeField_Event(object sender, MergeFieldEventArgs args)
        {
            string fieldValue = args.FieldValue.ToString();
            //When field value is Null or empty, then remove the field owner paragraph.
            if (string.IsNullOrEmpty(fieldValue))
            {
                //Get the merge field owner paragraph and remove it from its owner text body.
                WParagraph ownerParagraph = args.CurrentMergeField.OwnerParagraph;
                WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
                ownerTextBody.ChildEntities.Remove(ownerParagraph);
            }
        }
        private void MergeField_InsertPageBreak(object sender, MergeFieldEventArgs args)
        {
            if (args.FieldName == "DepartmentName")
            {
                //Gets the owner paragraph 
                WParagraph paragraph = args.CurrentMergeField.OwnerParagraph;
                //Appends the page break 
                paragraph.AppendBreak(BreakType.PageBreak);
                i++;
            }

        }
        private void InsertQRBarcode(object sender, MergeImageFieldEventArgs args)
        {
            if (args.FieldName == "QrFilePath")
            {
                ////Generates barcode image for field value.
                System.Drawing.Image barcodeImage = GenerateQRBarcodeImage(args.FieldValue.ToString());
                var stream = FormatImage.ToStream(barcodeImage, ImageFormat.Png);
                args.ImageStream = stream;
            }
        }
        private System.Drawing.Image GenerateQRBarcodeImage(string qrBarcodeText)
        {
            //Drawing QR Barcode
            PdfQRBarcode barcode = new PdfQRBarcode();
            //Set Error Correction Level
            barcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.Low;
            //Set XDimension
            barcode.XDimension = 4;
            barcode.Text = qrBarcodeText;
            PdfColor pdfColor = new PdfColor();
            //pdfColor.
            barcode.ForeColor = pdfColor;


            //Convert the barcode to image
            System.Drawing.Image barcodeImage = barcode.ToImage(new SizeF(88f, 88f));
            return barcodeImage;
        }
        private List<IndexAssetDetailVM.GetData> ListAssets()
        {

            var allAssets = _AssetDetailService.GetAll().OrderBy(a => a.Barcode).ToList();
            if (allAssets.Count > 0)
            {
                return allAssets;
            }
            return new List<IndexAssetDetailVM.GetData>();
        }


        /////////////////////////////////////////////////////
        /// UniversityQr - 3
        /////////////////////////////////////////
        [HttpPost]
        [Route("GenerateQrCodeForUniversityAssets")]
        public bool GenerateQrCodeForUniversityAssets(string domainName)
        {
            // domainName = "http://" + HttpContext.Request.Host.Value;
            domainName = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;

            return _AssetDetailService.GenerateQrCodeForAllAssets(domainName);
        }
        [Route("GenerateWordForQrCodeForUniversityAssets")]
        public ActionResult GenerateWordForQrCodeForUniversityAssets()
        {

            using (WordDocument document = new WordDocument())
            {
                //Opens the Word template document
                string strTemplateFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\UniversityCardTemplate.dotx";

                Stream docStream = System.IO.File.OpenRead(strTemplateFile);
                document.Open(docStream, FormatType.Docx);
                docStream.Dispose();


                var allAssets = ListAssets().ToList();
                MailMergeDataTable dataTable = new MailMergeDataTable("Asset_QrCode", allAssets);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField1_InsertPageBreak);
                document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(InsertQRBarcode);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField1_Event);
                document.MailMerge.RemoveEmptyGroup = true;

                document.MailMerge.ExecuteGroup(dataTable);


                //Saves the file in the given path
                string strExportFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\UniversityCards.docx";
                docStream = System.IO.File.Create(strExportFile);
                document.Save(docStream, FormatType.Docx);
                docStream.Dispose();
                document.Close();

            }
            return Ok();
        }
        private static void MergeField1_Event(object sender, MergeFieldEventArgs args)
        {
            string fieldValue = args.FieldValue.ToString();
            //When field value is Null or empty, then remove the field owner paragraph.
            if (string.IsNullOrEmpty(fieldValue))
            {
                //Get the merge field owner paragraph and remove it from its owner text body.
                WParagraph ownerParagraph = args.CurrentMergeField.OwnerParagraph;
                WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
                ownerTextBody.ChildEntities.Remove(ownerParagraph);
            }
        }
        private void MergeField1_InsertPageBreak(object sender, MergeFieldEventArgs args)
        {


            if (args.FieldName == "DepartmentName")
            {
                //Gets the owner paragraph 
                WParagraph paragraph = args.CurrentMergeField.OwnerParagraph;
                //Appends the page break 
                paragraph.AppendBreak(BreakType.PageBreak);
                i++;
            }

        }


        /////////////////////////////////////////////////////
        /// HospitalQr - 1
        /////////////////////////////////////////
        [HttpPost]
        [Route("GenerateQrCodeForHospitalAssets")]
        public bool GenerateQrCodeForHospitalAssets(string domainName)
        {
            // domainName = "http://" + HttpContext.Request.Host.Value;
            domainName = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            return _AssetDetailService.GenerateQrCodeForAllAssets(domainName);
        }


        [HttpPost]
        [Route("GenerateWordForQrCodeForHospitalAssets")]
        public ActionResult GenerateWordForQrCodeForHospitalAssets()
        {

            using (WordDocument document = new WordDocument())
            {
                //Opens the Word template document
                string strTemplateFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\HospitalCardTemplate.dotx";

                Stream docStream = System.IO.File.OpenRead(strTemplateFile);
                document.Open(docStream, FormatType.Docx);
                docStream.Dispose();


                var allAssets = ListAssets().ToList();
                MailMergeDataTable dataTable = new MailMergeDataTable("Asset_QrCode", allAssets);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField2_InsertPageBreak);
                document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(InsertQRBarcode);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField2_Event);
                document.MailMerge.RemoveEmptyGroup = true;

                document.MailMerge.ExecuteGroup(dataTable);


                //Saves the file in the given path
                string strExportFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\HospitalCards.docx";
                docStream = System.IO.File.Create(strExportFile);
                document.Save(docStream, FormatType.Docx);
                docStream.Dispose();
                document.Close();

            }
            return Ok();
        }
        private static void MergeField2_Event(object sender, MergeFieldEventArgs args)
        {
            string fieldValue = args.FieldValue.ToString();
            //When field value is Null or empty, then remove the field owner paragraph.
            if (string.IsNullOrEmpty(fieldValue))
            {
                //Get the merge field owner paragraph and remove it from its owner text body.
                WParagraph ownerParagraph = args.CurrentMergeField.OwnerParagraph;
                WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
                ownerTextBody.ChildEntities.Remove(ownerParagraph);
            }
        }
        private void MergeField2_InsertPageBreak(object sender, MergeFieldEventArgs args)
        {

            //List<IndexAssetDetailVM.GetData> allAssets = ListAssets().ToList();
            //if (allAssets.Count > 0)
            //{
            if (args.FieldName == "DepartmentName")
            {
                //Gets the owner paragraph 
                WParagraph paragraph = args.CurrentMergeField.OwnerParagraph;
                //Appends the page break 
                paragraph.AppendBreak(BreakType.PageBreak);
                i++;
            }
            // }
        }


        [HttpPost]
        [Route("GetHospitalAssetsByGovIdAndDeptIdAndHospitalId/{departmentId}/{govId}/{hospitalId}/{userId}/{pageNumber}/{pageSize}")]
        public ActionResult<IndexAssetDetailVM> GetHospitalAssetsByGovIdAndDeptIdAndHospitalId2(int departmentId, int govId, int hospitalId, string userId, int pageNumber, int pageSize)
        {
            return _AssetDetailService.GetHospitalAssetsByGovIdAndDeptIdAndHospitalId(departmentId, govId, hospitalId, userId, pageNumber, pageSize);
        }






        [HttpPost]
        [Route("SortAssetsWithoutSearch/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetsWithoutSearch(Sort sortObj, int pageNumber, int pageSize)
        {
            return _AssetDetailService.SortAssetsWithoutSearch(sortObj, pageNumber, pageSize);
        }


        [HttpPost]
        [Route("GetHospitalAssetsBySupplierId/{supplierId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM GetHospitalAssetsBySupplierId(int supplierId, int pageNumber, int pageSize)
        {
            return _AssetDetailService.GetHospitalAssetsBySupplierId(supplierId, pageNumber, pageSize);
        }


        [HttpPost]
        [Route("SearchHospitalAssetsBySupplierId/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SearchHospitalAssetsBySupplierId(SearchAssetDetailVM searchObj, int pageNumber, int pageSize)
        {
            return _AssetDetailService.SearchHospitalAssetsBySupplierId(searchObj, pageNumber, pageSize);
        }













        [HttpGet]
        [Route("GetAssetsByBrandId/{brandId}")]
        public IndexAssetDetailVM GetAssetsByBrandId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _AssetDetailService.GetAssetsByBrandId(brandId);
            return result;
        }
        [HttpGet]
        [Route("GetAssetsByDepartmentId/{departmentId}")]
        public IndexAssetDetailVM GetAssetsByDepartmentId(int departmentId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _AssetDetailService.GetAssetsByDepartmentId(departmentId);
            return result;

        }

        [HttpGet]
        [Route("GetAssetsBySupplierId/{supplierId}")]

        public List<IndexAssetDetailVM.GetData> GetAssetsBySupplierId(int supplierId)
        {
            List<IndexAssetDetailVM.GetData> result = new List<IndexAssetDetailVM.GetData>();
            result = _AssetDetailService.GetAssetsBySupplierId(supplierId);
            return result;
        }

        [HttpGet]
        [Route("GetAssetsBySupplierIdWithPaging/{supplierId}/{pageNumber}/{pageSize}")]

        public IndexAssetDetailVM GetAssetsBySupplierIdWithPaging(int supplierId, int pageNumber, int pageSize)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _AssetDetailService.GetAssetsBySupplierIdWithPaging(supplierId, pageNumber, pageSize);
            return result;
        }

        [HttpPost]
        [Route("SortAssetDetail/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetDetail(SortAssetDetail sortObject, int pageNumber, int pageSize)
        {
            var result = new IndexAssetDetailVM();
            result = _AssetDetailService.SortAssetDetail(sortObject, pageNumber, pageSize);
            return result;
        }

        [HttpPost]
        [Route("SortAssetDetailAfterSearch/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetDetailAfterSearch(SortAndFilterDataModel data, int pageNumber, int pageSize)
        {
            return _AssetDetailService.SortAssetDetailAfterSearch(data, pageNumber, pageSize);
        }

        [HttpPost]
        [Route("GroupAssetDetailsByBrand")]
        public List<BrandGroupVM> GroupAssetDetailsByBrand(FilterHospitalAsset data)
        {
            return _AssetDetailService.GroupAssetDetailsByBrand(data);
        }

        [HttpPost]
        [Route("GroupAssetDetailsBySupplier")]
        public List<SupplierGroupVM> GroupAssetDetailsBySupplier(FilterHospitalAsset data)
        {
            return _AssetDetailService.GroupAssetDetailsBySupplier(data);
        }

        [HttpPost]
        [Route("GroupAssetDetailsByDepartment")]
        public List<DepartmentGroupVM> GroupAssetDetailsByDepartment(FilterHospitalAsset data)
        {
            return _AssetDetailService.GroupAssetDetailsByDepartment(data);
        }


        [HttpPost]
        [Route("FilterDataByDepartmentBrandSupplierIdAndPaging/{userId}/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM FilterDataByDepartmentBrandSupplierIdAndPaging(FilterHospitalAsset data, string userId, int pageNumber, int pageSize)
        {
            var list = _AssetDetailService.FilterDataByDepartmentBrandSupplierIdAndPaging(data, userId, pageNumber, pageSize);
            return list;
        }



        [HttpGet]
        [Route("DrawingChart")]
        public List<DrawChart> DrawingChart()
        {
            var list = _AssetDetailService.DrawingChart();
            return list;
        }

    }
}
