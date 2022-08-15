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
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        IWebHostEnvironment _webHostingEnvironment;

        IVisitService _visitService;
        private IPagingService _pagingService;


        public VisitController(IVisitService visitService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
            _visitService = visitService;
            _pagingService = pagingService;
            _webHostingEnvironment = webHostingEnvironment;

        }



        // GET: api/<VisitController>
        [HttpGet]
        [Route("GetAllVisits")]
        public List<IndexVisitVM.GetData> Get()
        {
            return _visitService.GetAll().ToList();
        }

        // GET api/<VisitController>/5
        [HttpGet]
        [Route("GetById/{id}")]
        public Visit GetById(int id)
        {
            return _visitService.GetById(id);
        }

        [HttpGet]
        [Route("ViewVisitById/{id}")]
        public ViewVisitVM ViewVisitById(int id)
        {
            return _visitService.ViewVisitById(id);
        }
      
        [HttpPut]
        [Route("ListVisitsWithPaging")]
        public IEnumerable<IndexVisitVM.GetData> ListVisitsWithPaging(PagingParameter pageInfo)
        {
            var visits = _visitService.GetAll().ToList();
            return _pagingService.GetAll<IndexVisitVM.GetData>(pageInfo, visits);
        }
       
        
        [HttpGet]
        [Route("getcount")]
        public int count()
        {
            return _pagingService.Count<Visit>();
        }

        // POST api/<VisitController>
        [HttpPost]
        [Route("AddVisit")]
        public int AddVisit(CreateVisitVM createVisitVM)
        {
            return _visitService.Add(createVisitVM);
        }

        // PUT api/<VisitController>
        [HttpPut]
        [Route("UpdateVisit")]
        public int UpdateVisit(EditVisitVM editVisitVM)
        {
            return _visitService.Update(editVisitVM);
        }
        // PUT api/<VisitController>
        [HttpPut]
        [Route("UpdateVer")]
        public int UpdateVer(EditVisitVM editVisitVM)
        {
            return _visitService.UpdateVer(editVisitVM);
        }

        // PUT api/<VisitController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VisitController>/5

        [HttpDelete]
        [Route("DeleteVisit/{id}")]
        public ActionResult<Visit> Delete(int id)
        {
            try
            {

                int deletedRow = _visitService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }

        [HttpPost]
        [Route("SearchInVisits/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexVisitVM.GetData> SearchInVisits(int pagenumber, int pagesize, SearchVisitVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _visitService.SearchInVisits(searchObj).ToList();
            return _pagingService.GetAll<IndexVisitVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInVisitsCount")]
        public int SearchInVisitsCount(SearchVisitVM searchObj)
        {
            int c = _visitService.SearchInVisits(searchObj).ToList().Count();
            return c;
        }

        [HttpPost]
        [Route("SortVisits/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexVisitVM.GetData> SortVisits(int pagenumber, int pagesize, SortVisitVM sortObj, int statusId)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _visitService.SortVisits(sortObj, statusId).ToList();
            return _pagingService.GetAll<IndexVisitVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("CreateVisitAttachments")]
        public int CreateVisitAttachments(VisitAttachment visitAttachment)
        {
            return _visitService.CreateVisitAttachments(visitAttachment);
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
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("GetVisitAttachmentByVisitId/{visitId}")]
        public IEnumerable<VisitAttachment> GetVisitAttachmentByVisitId(int visitId)
        {
            return _visitService.GetVisitAttachmentByVisitId(visitId);
        }



        [HttpGet]
        [Route("GenerateVisitCode")]
        public GeneratedVisitCodeVM GenerateVisitCode()
        {
            return _visitService.GenerateVisitCode();
        }


    }
}
