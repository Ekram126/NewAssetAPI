using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.VisitVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asset.ViewModels.PagingParameter;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asset.API.Controllers
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class MVisitController : ControllerBase
    {
        IWebHostEnvironment _webHostingEnvironment;

        IVisitService _visitService;
        private IPagingService _pagingService;


        public MVisitController(IVisitService visitService, IWebHostEnvironment webHostingEnvironment)
        {
            _visitService = visitService;
            _webHostingEnvironment = webHostingEnvironment;
        }



        [HttpGet]
        [Route("GenerateVisitCode")]
        public GeneratedVisitCodeVM GenerateVisitCode()
        {
            return _visitService.GenerateVisitCode();
        }




        [HttpPost]
        [Route("AddVisit")]
        public ActionResult AddVisit(CreateVisitVM createVisitVM)
        {
            if (createVisitVM != null)
            {
                var visitId = _visitService.Add(createVisitVM);
                return Ok(new { data = visitId, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = "", msg = "No Data", status = '0' });
        }


        [HttpPost]
        [Route("CreateVisitAttachments")]
        public ActionResult CreateVisitAttachments(VisitAttachment visitAttachment)
        {
            if (visitAttachment != null)
            {
                var createVisitAttachmentObj = _visitService.CreateVisitAttachments(visitAttachment);
                return Ok(new { data = createVisitAttachmentObj, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = "", msg = "No Data", status = '0' });
        }


        [HttpPost]
        [Route("UploadVisitFiles")]
        public ActionResult UploadVisitFiles(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/VisitFiles/";
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
            var lstUploadVisitFiles = StatusCode(StatusCodes.Status201Created);
            if (lstUploadVisitFiles != null)
            {
                return Ok(new { data = lstUploadVisitFiles, msg = "Success", status = '1' });
            }
            else
                return Ok(new { data = lstUploadVisitFiles, msg = "No Data", status = '0' });
        }

    }
}
