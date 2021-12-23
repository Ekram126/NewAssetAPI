using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.AssetDetailAttachmentVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        private IAssetDetailService _AssetDetailService;
      
        public QrController(IAssetDetailService AssetDetailService,
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager)
        {
            _AssetDetailService = AssetDetailService;
            _context = context;
           
            this.userManager = userManager;
        }
            [HttpPost]
            [DisableRequestSizeLimit]
            [Route("Index/{eqId}")]
            public IActionResult Index(int eqId)
            {
                int assetId = eqId;

                string url = "http://biomedicalupd-001-site1.itempurl.com/#/home/EquipmentDetails/" + assetId; 
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(15);
                var bitmapFiles = BitmapToBytes(qrCodeImage, assetId);
                
                var asset = _context.AssetDetails.Where(e => e.Id == assetId).FirstOrDefault();
                asset.QrFilePath = url;
               _context.Entry(asset).State = EntityState.Modified;
               _context.SaveChanges();



            return Ok(url);
            }

            private static Byte[] BitmapToBytes(Bitmap img,int assetId)
            {
                //var eq= _context.Equipments.Where(e => e.Id == qrText).FirstOrDefault();
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    img.Save(Directory.GetCurrentDirectory() + "/UploadedAttachments/qrFiles/equipment-" + assetId + ".png", System.Drawing.Imaging.ImageFormat.Png);
              

                return stream.ToArray();
                }
            }


        }
    }

