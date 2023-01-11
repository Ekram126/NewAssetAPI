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
using System.Threading.Tasks;

using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Common;


namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private IAssetDetailService _assetDetailService;

        public QrController(IAssetDetailService assetDetailService,
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager)
        {
            _assetDetailService = assetDetailService;
            _context = context;

            this.userManager = userManager;
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("Index/{eqId}")]
        public IActionResult Index(int eqId)
        {
            int assetId = eqId;

            // string url = "http://192.168.0.102:2020/#/dash/hospitalassets/edithospitalasset/" + assetId;


            var domainName = "http://" + HttpContext.Request.Host.Value;
            string url = domainName + "#/dash/hospitalassets/detail/" + eqId;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

            //QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(15);
            //var bitmapFiles = BitmapToBytes(qrCodeImage, assetId);

            var asset = _context.AssetDetails.Where(e => e.Id == assetId).FirstOrDefault();
            asset.QrFilePath = url;
            _context.Entry(asset).State = EntityState.Modified;
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
           // var lstHospitalAssets = _assetDetailService.GetAll().Take(1).ToList();
            var lstHospitalAssets = _assetDetailService.GetAll().ToList();
            foreach (var item in lstHospitalAssets)
            {
                var assetObj = _assetDetailService.GetById(item.Id);
                //  var domainName = "http://" + HttpContext.Request.Host.Value;
                // string url = domainName + "/#/dash/hospitalassets/detail/" + assetId + ";" + assetObj.AssetName + ";" + assetObj.BrandName + ";" + assetObj.Model + ";" + assetObj.SerialNumber + ";" + assetObj.BarCode;

                string assetObjData = "AssetName " + assetObj.AssetName + ";\nManufacture " + assetObj.BrandName + ";\nModel " + assetObj.Model + ";\nSerialNumber " + assetObj.SerialNumber + ";\nBarcode " + assetObj.BarCode;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(assetObjData, QRCodeGenerator.ECCLevel.Q,true);
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
    }
}

