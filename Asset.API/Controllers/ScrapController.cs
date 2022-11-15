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

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapController : Controller
    {
        IWebHostEnvironment _webHostingEnvironment;
        IScrapService _scrapService;
        private IPagingService _pagingService;

        public ScrapController(IScrapService scrapService, IPagingService pagingService, IWebHostEnvironment webHostingEnvironment)
        {
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
        public int AddScrap(CreateScrapVM scrapVM)
        {
            return _scrapService.Add(scrapVM);
        }

        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        
        [HttpPost]
        [Route("SearchInScraps/{pagenumber}/{pagesize}")]
        public IEnumerable<IndexScrapVM.GetData> SearchInScraps(int pagenumber, int pagesize, SearchScrapVM searchObj)
        {
            PagingParameter pageInfo = new PagingParameter();
            pageInfo.PageNumber = pagenumber;
            pageInfo.PageSize = pagesize;
            var list = _scrapService.SearchInScraps(searchObj).ToList();
            return _pagingService.GetAll<IndexScrapVM.GetData>(pageInfo, list);
        }

        [HttpPost]
        [Route("SearchInScrapsCount")]
        public int SearchInScrapsCount(SearchScrapVM searchObj)
        {
            int c = _scrapService.SearchInScraps(searchObj).ToList().Count();
            return c;
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


    }
}
