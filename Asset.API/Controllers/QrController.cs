using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Asset.ViewModels.AssetDetailVM;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Asset.API.Helpers;
using System.Drawing.Imaging;

using Microsoft.AspNetCore.Hosting;
using Syncfusion.Pdf;


namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private IAssetDetailService _assetDetailService;
        IWebHostEnvironment _webHostingEnvironment;
        IHttpContextAccessor _httpContextAccessor;
        int i = 1;

        public QrController(IAssetDetailService assetDetailService,
          ApplicationDbContext context, IWebHostEnvironment webHostingEnvironment,
          UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _assetDetailService = assetDetailService;
            _context = context;
            _webHostingEnvironment = webHostingEnvironment;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("Index/{eqId}")]
        public IActionResult Index(int id)
        {
            int assetId = id;

            // string url = "http://192.168.0.102:2020/#/dash/hospitalassets/edithospitalasset/" + assetId;
            // var x = _webHostingEnvironment.ContentRootPath;

            var domainName = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value;

            //  var domainName = "http://" + HttpContext.Request.Host.Value;
            string url = domainName + "#/dash/hospitalassets/detail/" + id;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var asset = _context.AssetDetails.Where(e => e.Id == assetId).FirstOrDefault();
            asset.QrFilePath = url;
            _context.Entry(asset).State = EntityState.Modified;
            _context.SaveChanges();




            var assetObj = _assetDetailService.GetById(id);
            string assetObjData = "AssetName " + assetObj.AssetName + ";\nManufacture " + assetObj.BrandName + ";\nModel " + assetObj.Model + ";\nSerialNumber " + assetObj.SerialNumber + ";\nBarcode " + assetObj.BarCode;
            QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
            QRCodeData qrCodeData1 = qrGenerator1.CreateQrCode(assetObjData, QRCodeGenerator.ECCLevel.L, true);
            var assetItem = _context.AssetDetails.Where(e => e.Id == id).FirstOrDefault();
            asset.QrData = assetObjData;
            _context.Entry(assetItem).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(url);
        }





        private static Byte[] BitmapToBytes(Bitmap img, int assetId)
        {
            //var eq= _context.Equipments.Where(e => e.Id == qrText).FirstOrDefault();
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                img.Save(Directory.GetCurrentDirectory() + "/UploadedAttachments/qrFiles/equipment-" + assetId + ".png", System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }



        /// <summary>
        /// Police Hospitals
        /// </summary>
        /// <param name="eqId"></param>
        /// <returns></returns>
        [HttpPost]
        // [DisableRequestSizeLimit]
        [Route("GenerateQrCodeWithAssetData")]
        public IActionResult GenerateQrCodeWithAssetData()
        {
            var lstHospitalAssets = _assetDetailService.GetAll().ToList();
            foreach (var item in lstHospitalAssets)
            {
                var assetObj = _assetDetailService.GetById(item.Id);
                string assetObjData = "AssetName " + assetObj.AssetName + ";\nManufacture " + assetObj.BrandName + ";\nModel " + assetObj.Model + ";\nSerialNumber " + assetObj.SerialNumber + ";\nBarcode " + assetObj.BarCode;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(assetObjData, QRCodeGenerator.ECCLevel.L, true);
                var asset = _context.AssetDetails.Where(e => e.Id == item.Id).FirstOrDefault();
                asset.QrData = assetObjData;
                _context.Entry(asset).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpPost]
        [Route("GenerateSelectedQrCodeWithAssetData")]
        public IActionResult GenerateSelectedQrCodeWithAssetData(List<IndexAssetDetailVM.GetData> selectedAssets)
        {
            //  var lstHospitalAssets = _assetDetailService.GetAll().ToList();
            foreach (var item in selectedAssets)
            {
                var assetObj = _assetDetailService.GetById(item.Id);
                string assetObjData = "AssetName " + assetObj.AssetName + ";\nManufacture " + assetObj.BrandName + ";\nModel " + assetObj.Model + ";\nSerialNumber " + assetObj.SerialNumber + ";\nBarcode " + assetObj.BarCode;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(assetObjData, QRCodeGenerator.ECCLevel.L, true);
                var asset = _context.AssetDetails.Where(e => e.Id == item.Id).FirstOrDefault();
                asset.QrFilePath = assetObjData;
                _context.Entry(asset).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpGet]
        [Route("DisplayQrCodeWithAssetData/{assetId}")]
        public IActionResult DisplayQrCodeWithAssetData(int assetId = 1309)
        {
            var assetObj = _assetDetailService.GetById(assetId);
            var QrPathToImage = assetObj.QrFilePath;
            var result = QrPathToImage.Split(new String[] { ";" }, StringSplitOptions.None);
            return Ok(result);
        }



        [Route("GenerateWordForSelectedQrCodeForAllAssets")]
        public ActionResult GenerateWordForQrCodeForAllAssets(List<IndexAssetDetailVM.GetData> selectedAssets)
        {

            using (WordDocument document = new WordDocument())
            {
                //Opens the Word template document
                string strTemplateFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\CardTemplate1.dotx";

                Stream docStream = System.IO.File.OpenRead(strTemplateFile);
                document.Open(docStream, FormatType.Docx);
                docStream.Dispose();


                var allAssets = ListAssets(selectedAssets).ToList();
                MailMergeDataTable dataTable = new MailMergeDataTable("Asset_QrCode", allAssets);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField_InsertPageBreak);
                document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(InsertQRBarcode);
                document.MailMerge.MergeField += new MergeFieldEventHandler(MergeField_Event);
                document.MailMerge.RemoveEmptyGroup = true;
                document.MailMerge.ExecuteGroup(dataTable);


                //Saves the file in the given path
                string strExportFile = _webHostingEnvironment.ContentRootPath + @"\UploadedAttachments\QrTemplates\Cards1.docx";
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
            barcode.XDimension = 3;
            barcode.Text = qrBarcodeText;
            //Convert the barcode to image
            System.Drawing.Image barcodeImage = barcode.ToImage(new SizeF(90f, 90f));
            return barcodeImage;
        }
        private List<IndexAssetDetailVM.GetData> ListAssets(List<IndexAssetDetailVM.GetData> selectedAssets)
        {
            return selectedAssets;
        }

    }
}

