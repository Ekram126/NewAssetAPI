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
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;


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
        private readonly UserManager<ApplicationUser> _userManager;
        private IEngineerService _EngineerService;

        public MVisitController(IVisitService visitService, IWebHostEnvironment webHostingEnvironment, UserManager<ApplicationUser> userManager, IEngineerService EngineerService)
        {
            _visitService = visitService;
            _webHostingEnvironment = webHostingEnvironment;
            _userManager = userManager;
            _EngineerService = EngineerService;
        }



        [HttpGet]
        [Route("GenerateVisitCode")]
        public GeneratedVisitCodeVM GenerateVisitCode()
        {
            return _visitService.GenerateVisitCode();
        }




        [HttpPost]
        [Route("AddVisit")]
        public async Task<ActionResult> AddVisit([FromForm] CreateVisitVM createVisitVM, [FromForm] List<IFormFile> ListAttachments)
        {
            if (createVisitVM != null)
            {
                var userObj = await _userManager.FindByIdAsync(createVisitVM.UserId);

                if (userObj == null)
                {
                    return Ok(new { data = "", msg = "No User Exist", status = '3' });
                }
                else
                {
                    var lstEngs = _EngineerService.GetAll().Where(a => a.Email == userObj.Email).ToList();
                    if (lstEngs.Count == 0)
                    {
                        return Ok(new { data = "", msg = "No Engineer Exist", status = '2' });
                    }
                    else
                    {
                        try
                        {
                            var visitId = _visitService.Add(createVisitVM);
                            if (ListAttachments.Count > 0)
                            {
                                for (int item = 0; item < ListAttachments.Count; item++)
                                {
                                    VisitAttachment attachmentObj = new VisitAttachment();
                                    attachmentObj.VisitId = visitId;
                                    attachmentObj.FileName = ListAttachments[item].FileName;
                                    attachmentObj.Title = Path.GetFileNameWithoutExtension(ListAttachments[item].FileName);
                                    var attachId = _visitService.CreateVisitAttachments(attachmentObj);
                                    var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/VisitFiles/";
                                    bool exists = System.IO.Directory.Exists(folderPath);
                                    if (!exists)
                                        System.IO.Directory.CreateDirectory(folderPath);
                                    string filePath = folderPath + "/" + attachmentObj.FileName;
                                    if (System.IO.File.Exists(filePath))
                                    {
                                    }
                                    else
                                    {
                                        Stream stream = new FileStream(filePath, FileMode.Create);
                                        ListAttachments[item].CopyTo(stream);
                                        stream.Close();
                                    }
                                }
                                return Ok(new { data = visitId, msg = "Success", status = '1' });
                            }
                            return Ok(new { data = visitId, msg = "Success", status = '1' });
                        }
                        catch(SqlException ex)
                        {
                          string str =  ex.Message;
                        }
                       
                    }
                }
            }
            return Ok();
        }
    }
}
